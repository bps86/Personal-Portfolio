using UnityEngine;
using UnityEngine.InputSystem;

public class Player : BaseUnit {

    public void OnMovementInput(InputActionPhase inputActionPhase, float value) {
        switch (inputActionPhase) {
            case InputActionPhase.Performed:
                TriggerMovement(value);
                break;
            case InputActionPhase.Canceled:
                TriggerMovement(0);
                break;
        }
    }

    public void OnAttackInput(InputActionPhase inputActionPhase) {
        if (inputActionPhase == InputActionPhase.Started) {
            TriggerAttack();
        }
    }

    public void OnJumpInput(InputActionPhase inputActionPhase) {
        if (inputActionPhase == InputActionPhase.Started) {
            TriggerJump();
        }
    }

    public void OnSlideInput(InputActionPhase inputActionPhase) {
        if (Mathf.Abs(inputDirection) > 0 && inputActionPhase == InputActionPhase.Started) {
            TriggerSliding();
        }
    }

    public void OnHurtInput(InputActionPhase inputActionPhase) {
        if (inputActionPhase == InputActionPhase.Started) {
            animator.SetTrigger("Hurt");
            isGliding = false;
            isSliding = false;
            isAttacking = false;
            isHurt = true;
        }
    }

    public void OnDieInput(InputActionPhase inputActionPhase) {
        if (inputActionPhase == InputActionPhase.Started) {
            animator.SetTrigger("Die");
            direction3dVelocity.x = 0;
            isDead = true;
        }
    }
}
