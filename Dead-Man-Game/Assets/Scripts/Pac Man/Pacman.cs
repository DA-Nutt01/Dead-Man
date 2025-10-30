// 10/30/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    [SerializeField] private AnimatedSprite deathSequence;
    [SerializeField] private GameObject NemesisSkin;
    [SerializeField] private AnimatedSprite NemesisMovement;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private Movement movement;
    private bool isTransformed = false;
    private Coroutine currentTransformationCoroutine; // Stores the active coroutine

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();

        NemesisSkin.SetActive(false);
        isTransformed = false;
    }

    private void Update()
    {
        if (GameManager.gameState != GameState.Gameplay) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            movement.SetDirection(Vector2.right);
        }

        // Do not rotate Deadman if he is transformed as Nemesis
        if (isTransformed) return;

        // Keep Pac-Man upright and just flip or rotate 90° increments
        Vector2 dir = movement.direction;
        if (dir == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (dir == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (dir == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // flip horizontally
        }
        else if (dir == Vector2.right)
        {
            transform.rotation = Quaternion.identity;
        }
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
        deathSequence.enabled = false;
        NemesisMovement.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
        NemesisSkin.SetActive(false);
    }

    public void DeathSequence()
    {
        AudioManager.Instance.PlaySound("PacManDeath");
        enabled = false;
        spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.Restart();
    }

    public void Transform(float buffer, float duration){
        isTransformed = true;

        // Stop the current transformation coroutine if it's running
        if (currentTransformationCoroutine != null)
        {
            StopCoroutine(currentTransformationCoroutine);
        }

        // Reset the rotation of NemesisSkin to ensure it's upright
        NemesisSkin.transform.rotation = Quaternion.identity;

        // Start a new transformation coroutine with the new duration
        currentTransformationCoroutine = StartCoroutine(NemesisTransformation(buffer, duration));
    }

    private IEnumerator NemesisTransformation(float audioBufferTime, float duration)
    {
        // Ensure NemesisSkin is enabled and spriteRenderer is disabled
        spriteRenderer.enabled = false;
        NemesisSkin.SetActive(true);

        // Wait for the audio buffer
        yield return new WaitForSeconds(audioBufferTime);

        // Ensure NemesisSkin is still active after the audio buffer
        if (!NemesisSkin.activeSelf)
        {
            NemesisSkin.SetActive(true);
        }

        // Enable the Nemesis moving animation set
        NemesisMovement.enabled = true;
        NemesisMovement.Restart();

        // Wait for the transformation duration
        yield return new WaitForSeconds(duration);

        // Transform back
        isTransformed = false;
        NemesisMovement.enabled = false;
        NemesisMovement.Restart();
        NemesisSkin.SetActive(false);
        spriteRenderer.enabled = true;

        // Clear the current coroutine reference
        currentTransformationCoroutine = null;
    }
}