using UnityEngine;
using System.Collections;
using System;
using System.Runtime.CompilerServices;

public abstract class PacmanBase: MonoBehaviour
{
    // Responsibility: Serves as the base class for any version of Pacman controller to inherit from

    protected SpriteRenderer m_SpriteRenderer;
    protected CircleCollider2D m_CircleCollider;
    protected Movement m_Movement;
    [SerializeField] protected AnimatedSprite m_DeathSequence;

    protected virtual void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_CircleCollider = GetComponent<CircleCollider2D>();
        m_Movement = GetComponent<Movement>();
    }
    
    protected virtual void Update()
    {
        UpdateRotation();
    }

    protected void OnEnable(){
        // Subscribe to Input Events
        InputManager.Instance.OnMoveUp += InputManager_OnMoveUp;
        InputManager.Instance.OnMoveDown += InputManager_OnMoveDown;
        InputManager.Instance.OnMoveLeft += InputManager_OnMoveLeft;
        InputManager.Instance.OnMoveRight += InputManager_OnMoveRight;
    }

    protected void OnDisable()
    {
        // Unsubscribe from Input Events
        InputManager.Instance.OnMoveUp -= InputManager_OnMoveUp;
        InputManager.Instance.OnMoveDown -= InputManager_OnMoveDown;
        InputManager.Instance.OnMoveLeft -= InputManager_OnMoveLeft;
        InputManager.Instance.OnMoveRight -= InputManager_OnMoveRight;
    }
    
    // INPUT HANDLING
    protected void InputManager_OnMoveUp(){
        if (GameManager.gameState != GameState.Gameplay) return;
        m_Movement.SetDirection(Vector2.up);
    }

    protected void InputManager_OnMoveDown(){
        if (GameManager.gameState != GameState.Gameplay) return;
        m_Movement.SetDirection(Vector2.down);
    }

    protected void InputManager_OnMoveLeft(){
        if (GameManager.gameState != GameState.Gameplay) return;
        m_Movement.SetDirection(Vector2.left);
    }

    protected void InputManager_OnMoveRight()
    {
        if (GameManager.gameState != GameState.Gameplay) return;
        m_Movement.SetDirection(Vector2.right);
    }

    // CONFIG
    public virtual void ResetState()
    {
        enabled = true;
        m_SpriteRenderer.enabled = true;
        m_CircleCollider.enabled = true;
        m_DeathSequence.enabled = false;
        m_Movement.ResetState();
        gameObject.SetActive(true);
    }

    public virtual void DeathSequence()
    {
        AudioManager.Instance.PlaySound("PacManDeath");
        enabled = false;
        m_SpriteRenderer.enabled = false;
        m_CircleCollider.enabled = false;
        m_Movement.enabled = false;
        //m_SpriteRenderer.enabled = true;
        m_DeathSequence.enabled = true;
        m_DeathSequence.Restart();
    }

    protected virtual void UpdateRotation()
    {
        if (GameManager.gameState != GameState.Gameplay) return;

        // Keep Pac-Man upright and just flip or rotate 90° increments
        Vector2 dir = m_Movement.direction;
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
}
