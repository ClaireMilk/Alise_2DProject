using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    #region varible
    [Header("Jump")]
    public float jumpHeight;//jump height
    float xInput;
    public float fallMultiplier = 2.5f, upMultiplier, lowJumpMultiplier = 2.3f;
    public float criticalJumpSpeed = 0.5f;
    [HideInInspector]
    public bool isJumping = false;

    bool isJump;
    #endregion

    private void Jump()
    {
        if (isJumping)
        {
            // animator.Play("Jump");
            animator.SetTrigger("Jump");
            PlaySoundEffect(PlayerAction.Jump);

            currentState = PlayerState.Jump;
            selfRigidbody.velocity = new Vector2(selfRigidbody.velocity.x, jumpHeight);
            isJumping = false;
        }
    }
    private void BetterJumpFelling()
    {
            if (selfRigidbody.velocity.y < 0.1f && !IsGround(xOffSet, rayLength) && !IsGround(-xOffSet, rayLength))
            {
                animator.SetBool("Fall", true);
                selfRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (selfRigidbody.velocity.y > 0 && !Input.GetButton("Jump") && currentState != PlayerState.ParryJump)
            {
                selfRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
            else if (selfRigidbody.velocity.y > 0)
            {
                selfRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (upMultiplier - 1) * Time.deltaTime;
            }


        if (!IsGround(xOffSet, rayLength) && !IsGround(-xOffSet, rayLength))
        {
            if (0 < selfRigidbody.velocity.y && selfRigidbody.velocity.y < criticalJumpSpeed)
            {
                selfRigidbody.velocity = new Vector2(selfRigidbody.velocity.x, 0);
            }
        }
        else
            animator.SetBool("Fall", false);
    }
    private void SetJumpState()
    {
        if (!animator.GetBool("isGrounded"))
        {
            isJump = true;
        }
        if (isJump == true && animator.GetBool("isGrounded"))
        {
            if(currentState!=PlayerState.Execute)
                currentState = PlayerState.Idle;
            isJump = false;
           // animator.SetBool("ParryDown", false);
        }
        if(currentState==PlayerState.ParryJump&& animator.GetBool("isGrounded"))
        {
            currentState = PlayerState.Idle;
           // isDownParry = false;
        }
    }
}
