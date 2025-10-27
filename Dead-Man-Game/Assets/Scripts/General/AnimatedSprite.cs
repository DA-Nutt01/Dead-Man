using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedSprite : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[0];
    public float animationTime = 0.25f;
    public bool loop = true;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;
    [SerializeField, Tooltip("Flag to determine if this animation will play regardless of the game state")] 
    private bool ignoreGameState = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        spriteRenderer.enabled = true;
        // Restart the animation when the component is enabled
        //animationFrame = -1;
        //InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void OnDisable()
    {
        spriteRenderer.enabled = false;
        //CancelInvoke(nameof(Advance)); 
    }

    private void Start()
    {
        InvokeRepeating(nameof(Advance), animationTime, animationTime);
    }

    private void Advance()
    {
        if (ignoreGameState == false)
        {
            if (GameManager.gameState != GameState.Gameplay) return;
        }

        if (!spriteRenderer.enabled) return;

        

        animationFrame++;

        if (animationFrame >= sprites.Length && loop) {
            animationFrame = 0;
        }

        if (animationFrame >= 0 && animationFrame < sprites.Length) {
            spriteRenderer.sprite = sprites[animationFrame];
        }
    }

    public void Restart()
    {
        animationFrame = -1;

        Advance();
    }

}
