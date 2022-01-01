using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    #region INITIALIZE INPUT
        
    private InputActions _inputActions;
    private void Awake() => _inputActions = new InputActions();
    private void OnEnable() => _inputActions.Enable();
    private void OnDisable() => _inputActions.Disable();

    #endregion

    #region INPUT VARIABLES
        
    public Vector2 MoveVector { get; private set; }
    public Vector2 LookVector { get; private set; }
        
    public float JumpValue { get; private set; }
        
    public bool JumpHeld { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool JumpPressed { get; private set; }

    public InputAction Jump { get; private set; }
        
    public float DashValue { get; private set; }
    public bool Dash { get; private set; }

    public float InteractValue { get; private set; }
    public bool Interact { get; private set; }

    #endregion
        
    private void Update()
    {
        MoveVector = _inputActions.Player.Move.ReadValue<Vector2>();
        LookVector = _inputActions.Player.Look.ReadValue<Vector2>();
            
        JumpValue = _inputActions.Player.Jump.ReadValue<float>();
        JumpHeld = _inputActions.Player.Jump.IsPressed();
        JumpPressed = _inputActions.Player.Jump.WasPressedThisFrame();
        JumpReleased = _inputActions.Player.Jump.WasReleasedThisFrame();

        Jump = _inputActions.Player.Jump;

        DashValue = _inputActions.Player.Dash.ReadValue<float>();
        Dash = _inputActions.Player.Dash.WasPressedThisFrame();
            
        InteractValue = _inputActions.Player.Interact.ReadValue<float>();
        Interact = _inputActions.Player.Interact.WasPressedThisFrame();
            
    }
}