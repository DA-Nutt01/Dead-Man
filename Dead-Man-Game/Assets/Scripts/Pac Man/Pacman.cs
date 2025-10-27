using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    [SerializeField]
    private AnimatedSprite deathSequence;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private Movement movement;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (GameManager.gameState != GameState.Gameplay) return;

    if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
        movement.SetDirection(Vector2.up);
    }
    else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
        movement.SetDirection(Vector2.down);
    }
    else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
        movement.SetDirection(Vector2.left);
    }
    else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
        movement.SetDirection(Vector2.right);
    }

    // Keep Pac-Man upright and just flip or rotate 90° increments
    Vector2 dir = movement.direction;
    if (dir == Vector2.up) {
        transform.rotation = Quaternion.Euler(0, 0, 90);
    }
    else if (dir == Vector2.down) {
        transform.rotation = Quaternion.Euler(0, 0, -90);
    }
    else if (dir == Vector2.left) {
        transform.rotation = Quaternion.Euler(0, 180, 0); // flip horizontally
    }
    else if (dir == Vector2.right) {
        transform.rotation = Quaternion.identity;
    }
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;
        deathSequence.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
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

}
