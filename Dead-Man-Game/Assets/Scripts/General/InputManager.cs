using UnityEngine;
using System;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-75)]
public class InputManager : MonoBehaviour
{
    //static ref for singleton
    public static InputManager Instance { get; private set; }

    [Header("Components"), Space(5)]
    [SerializeField] private InputActionAsset m_InputActions;

    // INPUT ACTIONS
    private InputAction m_MoveUp;
    private InputAction m_MoveDown;
    private InputAction m_MoveLeft;
    private InputAction m_MoveRight;
    private InputAction m_AnyKey;

    // INPUT EVENTS
    public event Action OnMoveUp;
    public event Action OnMoveDown;
    public event Action OnMoveLeft;
    public event Action OnMoveRight;
    public event Action OnAnyKey;

    private void OnEnable()
    {
        // Enable the input actions when the object is enabled
        m_MoveUp?.Enable();
        m_MoveDown?.Enable();
        m_MoveLeft?.Enable();
        m_MoveRight?.Enable();
        m_AnyKey?.Enable();
    }

    private void OnDisable()
    {
        // Disable the input actions when the object is disabled
        m_MoveUp?.Disable();
        m_MoveDown?.Disable();
        m_MoveLeft?.Disable();
        m_MoveRight?.Disable();
        m_AnyKey?.Disable();
    }

    private void Awake(){
        // Check singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.LogWarning("Cannot have more than one instance of InputManager");
            Destroy(gameObject);
            return;
        }

        // Check if Input Action Asset is assigned
        if (m_InputActions == null){
            Debug.LogError("m_InputActions not assined, input will not register. Assign in Inspector.");
            return;
        }

        // Initialize Player Actions & subscribe internal handler methods to the trigger of those actions
        if (m_MoveUp == null) m_MoveUp = m_InputActions.FindAction("Player/Move Up");
        if (m_MoveUp != null) m_MoveUp.performed += OnPlayerMoveUp;
        else Debug.LogError("Could not find Move Up action on action map 'Player'");

        if (m_MoveDown == null) m_MoveDown = m_InputActions.FindAction("Player/Move Down");
        if (m_MoveDown != null) m_MoveDown.performed += OnPlayerMoveDown;
        else Debug.LogError("Could not find Move Down action on action map 'Player'");

        if (m_MoveLeft == null) m_MoveLeft = m_InputActions.FindAction("Player/Move Left");
        if (m_MoveLeft != null) m_MoveLeft.performed += OnPlayerMoveLeft;
        else Debug.LogError("Could not find Move Left action on action map 'Player");

        if (m_MoveRight == null) m_MoveRight = m_InputActions.FindAction("Player/Move Right");
        if (m_MoveRight != null) m_MoveRight.performed += OnPlayerMoveRight;
        else Debug.LogError("Could not find Move Right action on action map 'Player");

        if (m_AnyKey == null) m_AnyKey = m_InputActions.FindAction("Player/Any Key");
        if (m_AnyKey != null) m_AnyKey.performed += OnPlayerAnyKey;
        else Debug.LogError("Could not find Move Right action on action map 'Player");

    }

    // METHODS
    private void OnPlayerMoveUp(InputAction.CallbackContext cxt){
        OnMoveUp?.Invoke();
    }

    private void OnPlayerMoveDown(InputAction.CallbackContext cxt){
        OnMoveDown?.Invoke();
    }

    private void OnPlayerMoveLeft(InputAction.CallbackContext cxt){
        OnMoveLeft?.Invoke();
    }

    private void OnPlayerMoveRight(InputAction.CallbackContext cxt){
        OnMoveRight?.Invoke();
    }

    private void OnPlayerAnyKey(InputAction.CallbackContext cxt){
        OnAnyKey?.Invoke();
    }
}
