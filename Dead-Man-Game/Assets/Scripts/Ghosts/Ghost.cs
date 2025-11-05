using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[DefaultExecutionOrder(-10)]
[RequireComponent(typeof(Movement))]
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }
    public GhostHome home { get; private set; }
    public GhostScatter scatter { get; private set; }
    public GhostChase chase { get; private set; }
    public GhostFrightened frightened { get; private set; }
    public GhostBehavior initialBehavior;

    [SerializeField] private GameObject bodyContainer;
    [SerializeField] private GameObject textObject;
    [SerializeField] private Text pointText;

    [SerializeField] private GhostType ghostType;
    public Transform target;
    public int points = 200;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        home = GetComponent<GhostHome>();
        scatter = GetComponent<GhostScatter>();
        chase = GetComponent<GhostChase>();
        frightened = GetComponent<GhostFrightened>();

        textObject.SetActive(false);
        pointText.enabled = false;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        gameObject.SetActive(true);
        movement.ResetState();

        frightened.Disable();
        chase.Disable();
        scatter.Enable();

        if (home != initialBehavior) {
            home.Disable();
        }

        if (initialBehavior != null) {
            initialBehavior.Enable();
        }

        if (GameManager.gameState == GameState.Gameplay)
        {
            AudioManager.Instance.PlaySound("GhostMove");
            AudioManager.Instance.PlaySound("STARS Footsteps");
        }
        
    }

    public void SetPosition(Vector3 position)
    {
        // Keep the z-position the same since it determines draw depth
        position.z = transform.position.z;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (frightened.enabled) {
                GameManager.Instance.GhostEaten(this);
                AudioManager.Instance.PlaySound("GhostEaten");

            } else {
                GameManager.Instance.PacmanEaten();
            }
        }
    }

    public IEnumerator GhostEaten(int multiplier){
        // Pause Gameplay
        GameManager.Instance.ChangeGameState(GameState.GhostEaten);
        // Toggle Ghost body container
        bodyContainer.SetActive(false);
        // Calculate points to display
        int earnedPoints = points * multiplier;
        pointText.text = earnedPoints.ToString().PadLeft(2, '0');
        

        // Toggle world canvas text
        textObject.transform.position = transform.position;
        textObject.SetActive(true);
        pointText.enabled = true;
        // Wait 1 second
        yield return new WaitForSeconds(1.5f);
        // Resume gameplay
        textObject.SetActive(false);
        pointText.enabled = false;
        bodyContainer.SetActive(true);
        GameManager.Instance.ChangeGameState(GameState.Gameplay);
    }

}
