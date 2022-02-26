using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    #region variable
    [Header("Movement")]
    public float xOffSet;//射线发射点横向偏移
    public float moveVelocity = 8f;
    [Range(0, 1)]
    public float velPower = 0.96f;
    [Range(0, 1)]
    public float frictionAmount = 0.22f;
    public float rayLength = 0.5f;

    private Transform originalTransform;
    private float direction = 1f;
    #endregion


    /// <summary>
    /// AddForce to the player to make it move
    /// </summary>
    private void PlayerMove(float direction)
    {
        #region Acceleration and inertia of the character
        // This is Inertia and drag, We abolished it
        //public float acceleration = 13f;
        //public float deceleration = 13f;
        //public float airAccel = 9f;
        //public float airDecel = 9f;

        //float targetSpeed = direction * moveVelocity; // Max Speed
        //float speedDif = targetSpeed - selfRigidbody.velocity.x; // Max Speed - current Speed
        //float accelRate;
        //if (IsGround(xOffSet, rayLength) || IsGround(-xOffSet, rayLength))
        //{
        //    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        //}
        //else
        //{
        //    accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? airAccel : airDecel;
        //}

        //float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif); //speedDif>0 return=1
        //                                                                                              // Debug.Log(movement);
        //selfRigidbody.AddForce(movement * Vector2.right);
        #endregion

        selfRigidbody.velocity = new Vector2(moveVelocity * direction, selfRigidbody.velocity.y);

        if (direction > 0)
        {
            trans.localScale = new Vector3(1, 1, 1);
        }
        if (direction < 0)
        {
            trans.localScale = new Vector3(-1, 1, 1);
        }
        animator.SetFloat("Speed", Mathf.Abs(direction));
    }
    /// <summary>
    /// add fraction to move
    /// </summary>
    protected void Fraction()
    {
        if ((IsGround(xOffSet, rayLength) || IsGround(xOffSet, rayLength)) && direction < 0.01f)
        {
            float fraction = Mathf.Min(Mathf.Abs(selfRigidbody.velocity.x), Mathf.Abs(frictionAmount));
            fraction *= Mathf.Sign(selfRigidbody.velocity.x);
            selfRigidbody.AddForce(Vector2.right * -fraction, ForceMode2D.Impulse);
        }
    }

    /// <summary>
    /// Detect whether the player is on the ground 
    /// </summary>
    /// <param name="offsetX"></param>
    /// <param name="rayLength"></param>
    /// <returns></returns>
    private bool IsGround(float offsetX, float rayLength)
    {
        Vector2 rayStart = new Vector2(groundPoint.transform.position.x + offsetX, groundPoint.transform.position.y);
        Debug.DrawRay(rayStart, Vector2.down * rayLength, Color.red);//将射线显示出来
        RaycastHit2D ray = Physics2D.Raycast(rayStart, Vector2.down, rayLength,1<<8);//only cast the Ground Layer
        //Debug.Log(ray.collider.gameObject.name);
        if (ray.collider != null)
        {
            //Debug.Log("True");
            return true;
        }
        else
        {
            //Debug.Log("false");
            return false;
        }

    }
}
