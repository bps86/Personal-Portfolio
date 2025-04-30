using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour {
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Player player;

    private void Awake() {
        playerController.MovementInputEvent += player.OnMovementInput;
        playerController.AttackInputEvent += player.OnAttackInput;
        playerController.JumpInputEvent += player.OnJumpInput;
        playerController.SlideInputEvent += player.OnSlideInput;
        playerController.HurtInputEvent += player.OnHurtInput;
        playerController.DieInputEvent += player.OnDieInput;
        playerController.Init();
        player.Init();
    }
}
