using UnityEngine;

public class PlayerCharacterStamina : MonoBehaviour
{
    //Character movement reference
    [SerializeField]
    private PlayerCharacterMovement _playerCharacterMovement;

    //Stats variables
    [SerializeField]
    private float _maxStamina = 100;
    [SerializeField]
    private float _sprintStaminaCost = 20;
    [SerializeField]
    private float _staminaRegenvalue = 20;
    private float _currentStamina;

    private void Awake() {
        _currentStamina = _maxStamina;
    }

    private void Update() {
        CalculateStamina();
    }

    private void CalculateStamina() {
        if (_playerCharacterMovement.IsSprinting) {
            if (_currentStamina > 0) {
                _currentStamina = _currentStamina - _sprintStaminaCost * Time.deltaTime;
            }
            else {
                _playerCharacterMovement.SetSprint(false);
            }
        }
        else {
            _currentStamina = _currentStamina + _staminaRegenvalue * Time.deltaTime;
        }
        Mathf.Clamp(_currentStamina, 0, _maxStamina);
    }
}
