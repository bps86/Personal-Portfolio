using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public Action<InputActionPhase, float> MovementInputEvent;
    public Action<InputActionPhase> AttackInputEvent;
    public Action<InputActionPhase> JumpInputEvent;
    public Action<InputActionPhase> SlideInputEvent;
    public Action<InputActionPhase> HurtInputEvent;
    public Action<InputActionPhase> DieInputEvent;
    [SerializeField] private PlayerInput playerInput;

    public void Init() {
        playerInput.currentActionMap.actionTriggered += OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext callbackContext) {
        switch (callbackContext.action.name) {
            case "Movement":
                MovementInputEvent?.Invoke(callbackContext.action.phase, callbackContext.action.ReadValue<float>());
                break;
            case "Attack":
                AttackInputEvent?.Invoke(callbackContext.action.phase);
                break;
            case "Jump":
                JumpInputEvent?.Invoke(callbackContext.action.phase);
                break;
            case "Slide":
                SlideInputEvent?.Invoke(callbackContext.action.phase);
                break;
            case "Hurt":
                HurtInputEvent?.Invoke(callbackContext.action.phase);
                break;
            case "Die":
                DieInputEvent?.Invoke(callbackContext.action.phase);
                break;
        }
    }
}
