using UnityEngine;

public class BaseUnit : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] protected Animator animator;
    [SerializeField] private Rigidbody unitRigidbody;
    [SerializeField] private float moveAcceleration;
    [SerializeField] private float moveDecceleration;
    [SerializeField] private float jumpMoveDecceleration;
    [SerializeField] private float moveMinSpeed;
    [SerializeField] private float moveMaxSpeed;
    [Range(0f, 1f)]
    [SerializeField] private float hurtSpeedMultiplier;
    [SerializeField] private float attackForce;
    [SerializeField] private float slideForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityAcceleration;
    [SerializeField] private float maxGravitySpeed;
    [SerializeField] private float OnGroundGravity;
    [Range(0f, 1f)]
    [SerializeField] private float glidingGravityMultiplier;
    [SerializeField] private float attackGravityMultiplier;
    protected Vector3 directionScale;
    protected Vector3 direction3dVelocity;
    protected float inputDirection;
    protected float lastDirection;
    protected bool isAttacking;
    protected bool isSliding;
    protected bool isGliding;
    protected bool isHurt;
    protected bool isDead;
    private bool onGround;

    public void Init() {
        directionScale = transform.localScale;
        lastDirection = 1;
    }

    private void FixedUpdate() {
        Movement();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == "Ground") {
            onGround = true;
            isGliding = false;
            isAttacking = false;
            animator.SetBool("OnGround", true);
        }
    }

    protected void TriggerMovement(float value) {
        if (isDead) return;

        if (value != 0) {
            directionScale.x = value;
        } else {
            lastDirection = inputDirection;
        }
        inputDirection = value;
        animator.SetBool("Running", Mathf.Abs(inputDirection) > 0);
    }

    protected void TriggerAttack() {
        if (isDead) return;
        if (isAttacking) return;
        if (isSliding) return;
        if (isGliding) return;

        if (onGround) {
            if (inputDirection != 0) {
                lastDirection = inputDirection;
            }
            direction3dVelocity.x = attackForce * lastDirection;
        }
        animator.SetTrigger("Attack");
        isAttacking = true;
    }

    protected void TriggerSliding() {
        if (isDead) return;
        if (isAttacking) return;
        if (isSliding) return;

        if (onGround) {
            if (inputDirection != 0) {
                lastDirection = inputDirection;
            }
            direction3dVelocity.x = slideForce * lastDirection;
            animator.SetTrigger("Slide");
            isSliding = true;
        }
    }

    protected void TriggerJump() {
        if (isDead) return;

        if (onGround) {
            direction3dVelocity.y = jumpForce;
            animator.SetTrigger("Jump");
            animator.SetBool("OnGround", false);
            onGround = false;
        } else if (!isGliding && direction3dVelocity.y < 1.5f) {
            isGliding = true;
            animator.SetTrigger("Glide");
        }
    }

    private void Movement() {
        if (Mathf.Abs(inputDirection) > 0 && !isAttacking) {
            transform.localScale = directionScale;
        }
        CalculateMoveSpeed();
        CalculateGravity();
        unitRigidbody.Move(transform.position + direction3dVelocity, Quaternion.identity);
    }

    private void CalculateMoveSpeed() {
        if (isDead) return;

        if (Mathf.Abs(inputDirection) != 0 && !isAttacking && !isSliding) {
            if (isHurt) {
                SetMoveSpeed(lastDirection, direction3dVelocity.x * hurtSpeedMultiplier);
                isHurt = false;
            } else if (Mathf.Abs(direction3dVelocity.x) > moveMaxSpeed * Time.fixedDeltaTime) {
                SetMoveSpeed(inputDirection, moveMaxSpeed);
            } else {
                // direction3dVelocity.x += inputDirection * moveAcceleration * Time.fixedDeltaTime;
                CheckMoveAcceleration(inputDirection, moveAcceleration);
            }
        } else {
            if (Mathf.Abs(direction3dVelocity.x) > 0) {
                if (onGround) {
                    CheckMoveDecceleration(moveDecceleration);
                } else {
                    CheckMoveDecceleration(jumpMoveDecceleration);
                }
                if (Mathf.Abs(direction3dVelocity.x) <= moveMinSpeed) {
                    direction3dVelocity.x = 0;
                    isAttacking = false;
                    isSliding = false;
                }
            }
        }

    }

    private void CalculateGravity() {
        if (onGround) {
            direction3dVelocity.y = -OnGroundGravity;
        } else if (isHurt || isDead) {
            direction3dVelocity.y = -maxGravitySpeed * Time.fixedDeltaTime;
        } else if (isGliding) {
            CheckGravity(glidingGravityMultiplier);
        } else if (isAttacking) {
            CheckGravity(attackGravityMultiplier);
        } else {
            CheckGravity();
        }
    }

    private void SetMoveSpeed(float direction, float moveSpeed) {
        direction3dVelocity.x = direction * moveSpeed * Time.fixedDeltaTime;
    }

    private void CheckMoveAcceleration(float direction, float accelerationSpeed) {
        direction3dVelocity.x += direction * accelerationSpeed * Time.fixedDeltaTime;
    }

    private void CheckMoveDecceleration(float deccelerationSpeed) {
        direction3dVelocity.x -= lastDirection * deccelerationSpeed * Time.fixedDeltaTime;
    }

    private void CheckGravity(float multiplier = 1) {
        if (direction3dVelocity.y <= -maxGravitySpeed * multiplier * Time.fixedDeltaTime) {
            direction3dVelocity.y = -maxGravitySpeed * multiplier * Time.fixedDeltaTime;
        } else {
            direction3dVelocity.y -= gravityAcceleration * multiplier * Time.fixedDeltaTime;
        }
    }
}
