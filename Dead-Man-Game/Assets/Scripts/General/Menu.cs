using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField, Tooltip("The amount of time in seconds before the player can press a button to start the game after this scene is loaded.")] 
    private float inputDelay = 3f;
    private bool canListenForInput = false; // Flag to track if input can be processed

    void Start(){
            // Start a delayed activation of input listening
            Invoke(nameof(EnableInputListening), inputDelay);
        }

    void Update(){
        if (!canListenForInput) return;

        // Check if the "P" key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Load the "Pacman" scene (index 2)
            SceneManager.LoadScene(2);
            return;
        }

        // Check if any key is pressed
        if (Input.anyKeyDown)
        {
            // Load the "Deadman" scene (index 1)
            SceneManager.LoadScene(1);
            return;
        }
    }

    private void EnableInputListening(){
        canListenForInput = true;
    }
}
