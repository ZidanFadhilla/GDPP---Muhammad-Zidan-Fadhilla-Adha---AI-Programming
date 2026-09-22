using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerCharacterMovement : MonoBehaviour
{
    //Input manager reference
    [SerializeField]
    private InputManager _inputManager;

    //Character controller reference
    [SerializeField]
    private CharacterController _characterController;

    //Movement variables
    private Vector3 _movementDirection;
    [SerializeField]
    private float _currentSpeed;
    [SerializeField]
    private float _walkSpeed = 1;
    [SerializeField]
    private float _sprintSpeed = 2;
    [SerializeField]
    private float _acceleration = 0.5f;
    private Vector3 _velocityXZ;
    private bool _isSprinting;

    //Physics Variables
    [SerializeField]
    private float _gravityScale = 1;
    private float _velocityY;
    private bool _isGrounded;

    //Properties
    public bool IsSprinting => _isSprinting;

    private void OnEnable() {
        _inputManager.MoveEvent += SetMoveDirection;
        _inputManager.SprintEvent += SetSprint;
    }

    private void OnDisable() {
        _inputManager.MoveEvent -= SetMoveDirection;
        _inputManager.SprintEvent -= SetSprint;
    }

    private void SetMoveDirection(Vector2 inputDirection) {
        _movementDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    private void Update() {
        CheckIsGrounded();
        ResetVelocityY();
        Move();
    }

    private void CalculateVelocityXZ() {
        Transform cameraTransform = Camera.main.transform;
        Vector3 xDirection = _movementDirection.x * cameraTransform.right;
        Vector3 zDirection = _movementDirection.z * cameraTransform.forward;
        Vector3 direction = xDirection + zDirection;
        direction.y = 0;
        if (_movementDirection.magnitude >= 0.01f) {
            _velocityXZ = direction.normalized * _currentSpeed * Time.deltaTime;
        }
        else {
            _velocityXZ = Vector3.zero;
        }
    }

    private void CalculateVelocityY() {
        _velocityY = _velocityY + Physics.gravity.y * _gravityScale * Time.deltaTime;
    }

    private void CheckIsGrounded() {
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        _isGrounded = Physics.CheckSphere(transform.position, 0.5f, groundLayer);
    }

    private void ResetVelocityY() {
        if(_isGrounded && _velocityY < 0) {
            _velocityY = -2;
        }
    }

    public void SetSprint(bool isSprinting) {
        _isSprinting = isSprinting;
    }

    private void CalculateAcceleration() {
        if (_movementDirection.magnitude >= 0.01f) {
            if (_isSprinting) {
                _currentSpeed = _currentSpeed + _acceleration * Time.deltaTime;
            }
            else {
                _currentSpeed = _currentSpeed - _acceleration * Time.deltaTime;
            }
        }
        else {
            _currentSpeed = 0;
        }
        _currentSpeed = Mathf.Clamp(_currentSpeed,_walkSpeed, _sprintSpeed);
    }

    public void Move() {
        CalculateVelocityXZ();
        CalculateAcceleration();
        CalculateVelocityY();
        Vector3 velocity = new Vector3(_velocityXZ.x, _velocityY, _velocityXZ.z);
        _characterController.Move(velocity);
    }
}
