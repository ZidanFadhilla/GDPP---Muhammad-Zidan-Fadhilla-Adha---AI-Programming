using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputManager", menuName = "Scriptable Objects/InputManager")]
public class InputManager : ScriptableObject, PlayerMovement.IPlayerActions
{
    public event UnityAction<Vector2> MoveEvent;
    public event UnityAction InteractEvent;
    public event UnityAction<bool> SprintEvent;


    private PlayerMovement _playerMovement;

    private void OnEnable() {
        if (_playerMovement == null) {
            _playerMovement = new PlayerMovement();

            _playerMovement.Player.SetCallbacks(this);
        }
        _playerMovement.Enable();
    }

    private void OnDisable() {
        _playerMovement.Player.Disable();
    }

    public void OnMove(InputAction.CallbackContext context) {
        Debug.Log(context.ReadValue<Vector2>());
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            InteractEvent?.Invoke();
        }
    }

    public void OnSprint(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            SprintEvent?.Invoke(true);
        }
        else if (context.phase == InputActionPhase.Canceled) {
            SprintEvent?.Invoke(false);
        }
    }
}
