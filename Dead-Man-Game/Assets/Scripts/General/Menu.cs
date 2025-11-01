using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField, Tooltip("The amount of time in seconds before the player can press a button to start the game after this scene is loaded.")] 
    private float inputDelay = 3f;
    [SerializeField] private Text HighScoreText;

    private bool canListenForInput = false; // Flag to track if input can be processed

    private void OnEnable(){
        InputManager.Instance.OnAnyKey += InputManager_OnAnyKey;
    }

    private void OnDisable(){
        InputManager.Instance.OnAnyKey -= InputManager_OnAnyKey;
    }

    void Start(){
        // Start Menu Music
        AudioManager.Instance.PlaySound("MenuMusic");

        // Start a delayed activation of input listening
        Invoke(nameof(EnableInputListening), inputDelay);

        // Display High Score
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        HighScoreText.text = highScore.ToString().PadLeft(2, '0'); 
     }

    private void InputManager_OnAnyKey(){
        if (!canListenForInput) return;
        AudioManager.Instance.StopSound("MenuMusic");
        AudioManager.Instance.PlaySound("StartGame");
        SceneManager.LoadScene(1);
    }

    private void EnableInputListening(){
        canListenForInput = true;
    }
}
