using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_BaseUnit : MonoBehaviour
{
    //----------------------------------------- Variable Declaration -----------------------------------------
    #region Components
    protected Rigidbody2D rb;//角色自身刚体
    protected Animator anim;//角色Animator
    protected SpriteRenderer sr;//自身SpriteRenderer
    #endregion

    public GameObject groundPoint;//射线检测起点

    #region Movement Variables
    [Header("Movement")]
    public float xOffSet;//射线发射点横向偏移
    public float moveVelocity = 8f;
    public float acceleration = 13f;
    public float deceleration = 13f;
    private float direction = 1f;
    [Range(0, 1)]
    public float velPower = 0.96f;
    [Range(0, 1)]
    public float frictionAmount = 0.22f;
    public float airAccel = 9f;
    public float airDecel = 9f;
    public float rayLength = 0.5f;
    protected Transform originalTransform;
    #endregion

    //----------------------------------------- Methods/ Functions -----------------------------------------
    private void Awake()
    {
        //获取组件
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        originalTransform = transform;
    }

    #region Move
    protected void Move(float direction)
    {
        float targetSpeed = direction * moveVelocity; // Max Speed
        float speedDif = targetSpeed - rb.velocity.x; // Max Speed - current Speed
        float accelRate;
        //if (IsGround(xOffSet) || IsGround(-xOffSet))
        //{
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        //}
        //else
        //{
        //accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? airAccel : airDecel;
        //}
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif); //speedDif>0 return=1
        rb.AddForce(movement * Vector2.right);
        if (direction > 0)
        {
            //transform.localScale = originalTransform.localScale;
            sr.flipX = false;
        }
        if (direction < 0)
        {
            //    transform.localScale = new Vector3(-originalTransform.localScale.x, originalTransform.localScale.y, originalTransform.localScale.z);
            sr.flipX = true;   //}
        }
        //if (direction * Physics2D.gravity.y > 0)
        //    sr.flipX = true;
        //if (direction * Physics2D.gravity.y < 0)
        //    sr.flipX = false;
        //if (direction == 0)
        //    rb.velocity = new Vector2(0, rb.velocity.y);
        //rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
        //anim.SetFloat("Speed", Mathf.Abs(direction));
    }
    /// <summary>
    /// 判断角色是否触地
    /// </summary>
    /// <param name="offsetX"></param>
    /// <param name="rayLength"></param>
    /// <returns></returns>
    /// 

    #endregion

    #region Fraction
    protected void Fraction()
    {
        if ((IsGround(xOffSet, rayLength) || IsGround(xOffSet, rayLength)) && direction < 0.01f)
        {
            float fraction = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            fraction *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -fraction, ForceMode2D.Impulse);
        }
    }
    #endregion
    protected bool IsGround(float offsetX, float rayLength)
    {
        Vector2 rayStart = new Vector2(groundPoint.transform.position.x + offsetX, groundPoint.transform.position.y);
        Debug.DrawRay(rayStart, Vector2.down * rayLength, Color.red);//将射线显示出来
        RaycastHit2D ray = Physics2D.Raycast(rayStart, Vector2.down, rayLength);
        if (ray.collider != null)
        {
            return true;
        }
        else
            return false;
    }
}
