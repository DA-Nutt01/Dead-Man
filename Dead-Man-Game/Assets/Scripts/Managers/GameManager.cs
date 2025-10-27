using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static GameState gameState = GameState.LevelStart;
    public int currentLevel {get; private set;} = 0;

    [Header("Configuration"), Space(5)]
    [SerializeField] private Ghost[] ghosts;
    [SerializeField] private Pacman pacman;
    [SerializeField] private Transform pellets;
    [Header("UI"), Space(5)]
    [SerializeField] private Text gameOverText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text HighScoreText;
    [SerializeField] private Text livesText;
    [SerializeField] private Text oneUpText;
    [SerializeField] private Text readyText;
    [SerializeField] private Text playerText;

    public int highScore {get; private set;}
    public int score { get; private set; } = 0;
    public int lives { get; private set; } = 3;

    private int ghostMultiplier = 1;
    private bool earnedExtraLife = false;

    // EVENTS
    public static event Action OnLevelStart;
    
    private void Awake()
    {
        if (Instance != null) {
            DestroyImmediate(gameObject);
        } else {
            Instance = this;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) {
            Instance = null;
        }
    }

    private void Start()
    {
        // Update Level
        currentLevel = 0;
        UIManager.Instance.UpdateLevelUI(1);

        // Update high score text from player pref
        score = 0;
        SetScore(score);
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        SetHighScore(highScore);

        // Play Start of Game Sequence
        gameState = GameState.LevelStart;
        StartCoroutine(PlayIntroSequence());
    }

    private void Update()
    {
        if (lives <= 0 && Input.anyKeyDown && gameState != GameState.LevelStart) {
            StartCoroutine(PlayIntroSequence());
        }
    }

    private void NewGame()
    {
        earnedExtraLife = false;
        SetScore(0);
        SetLives(3);
        NextLevel();
    }

    private void NextLevel()
    {
        // Fire Event
        OnLevelStart?.Invoke();

        currentLevel ++;
        UIManager.Instance.UpdateLevelUI(currentLevel);

        gameOverText.enabled = false;

        foreach (Transform pellet in pellets) {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
        gameState = GameState.Gameplay;
    }

    private void ResetState()
    {
        gameState = GameState.Gameplay;
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].ResetState();
        }

        pacman.ResetState();
    }

    private void EnableGhosts()
    {
         for (int counter = 0; counter < ghosts.Length; counter++) {
            ghosts[counter].gameObject.SetActive(true);
        }
    }

    private void DisableGhosts()
    {
         for (int counter = 0; counter < ghosts.Length; counter++) {
            ghosts[counter].gameObject.SetActive(false);
        }
    }

    private void EnablePacMan()
    {
        pacman.gameObject.SetActive(true);
    }

    private void DisablePacMan()
    {
        pacman.gameObject.SetActive(false);
    }

    private void GameOver()
    {
        gameOverText.enabled = true;

        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].gameObject.SetActive(false);
        }

        pacman.gameObject.SetActive(false);
        SaveHighScore();
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    private void SetScore(int newScore){
        score = newScore;
        scoreText.text = score.ToString().PadLeft(2, '0');

        // Check Highscore
        if (score > highScore) SetHighScore(score);

        // Check for Extra Life
        if (score > 10000 && !earnedExtraLife){
            earnedExtraLife = true;
            SetLives(lives + 1);
            AudioManager.Instance.PlaySound("ExtraLife");
        }
        
    }

    private void SetHighScore(int newHighScore){
        highScore = newHighScore;
        HighScoreText.text = highScore.ToString().PadLeft(2, '0'); 
    }

    private void SaveHighScore()
    {
        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.Save();
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;
        SetScore(score + points);

        ghostMultiplier++;
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);

        SetScore(score + pellet.points);

        if (!HasRemainingPellets())
        {
            pacman.gameObject.SetActive(false);
            Invoke(nameof(NextLevel), 3f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < ghosts.Length; i++) {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet);
        CancelInvoke(nameof(ResetGhostMultiplier));
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
        {
            if (pellet.gameObject.activeSelf) {
                return true;
            }
        }

        return false;
    }

    private void ResetGhostMultiplier()
    {
        ghostMultiplier = 1;
    }

    public void PacmanEaten()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {   
        // Set game state to pause movment & animations
        gameState = GameState.PlayerDeath;
        AudioManager.Instance.StopSound("GhostMove");
        // Wait a second
        yield return new WaitForSeconds(1f);
        DisableGhosts();
        pacman.DeathSequence();
        
        SetLives(lives - 1);

        if (lives > 0) {
            Invoke(nameof(ResetState), 3f);
        } else {
            GameOver();
        }
    }


    private IEnumerator PlayIntroSequence()
    {
        // Reset Score
        score = 0;
        SetScore(score);

        // Reset Level
        currentLevel = 0;

        highScore = PlayerPrefs.GetInt("HighScore", 0);

        gameState = GameState.LevelStart;
        // Play Intro music
        AudioManager.Instance.PlaySound("IntroMusic");

        // Get the Sound object for the intro music
        Sound introMusic = Array.Find(AudioManager.Instance.sounds, sound => sound.name == "IntroMusic");
        if (introMusic == null || introMusic.audioClip == null)
        {
            Debug.LogError("Intro music not found in AudioManager!");
            yield break;
        }

        // Phase 1: Display "1-UP" and "Player One" text
        DisableGhosts();
        DisablePacMan();

        oneUpText.enabled = true;
        playerText.enabled = true;
        readyText.enabled = true;
        gameOverText.enabled = false;

        // Wait until the audio reaches 45% of its duration
        float phase1Duration = introMusic.audioClip.length * 0.30f;
        yield return new WaitForSeconds(phase1Duration);

        // Phase 2: Hide "Player One" text, show "Ready!" text, enable Pac-Man and ghosts
        playerText.enabled = false;

        EnablePacMan();
        EnableGhosts();

        // Wait until the audio reaches 95% of its duration
        float phase2Duration = introMusic.audioClip.length * 0.80f - phase1Duration;
        yield return new WaitForSeconds(phase2Duration);

        // Phase 3: Intro music stops, "Ready!" text is disabled, and gameplay starts
        readyText.enabled = false;
        gameState = GameState.Gameplay;
        NewGame();
    }
}
