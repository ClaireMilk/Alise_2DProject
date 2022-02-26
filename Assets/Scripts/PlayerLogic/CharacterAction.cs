using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAction : Player_BaseUnit
{
    public float jumpHeight;//跳跃高度
    private float xInput, fallMultiplier = 2.5f, lowJumpMultiplier = 2.3f;

    public bool isJumping = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //角色移动
        xInput = Input.GetAxisRaw("Horizontal");
        Move(xInput);
        Fraction();
        BetterJumpFelling();
        if (IsGround(xOffSet, rayLength) || IsGround(-xOffSet, rayLength))
        {
            anim.SetBool("isGround", true);
            //触地可以跳跃，以下为跳跃代码
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
            }
        }
        else
            anim.SetBool("isGround", false);
    }

    private void FixedUpdate()
    {
        Jump();
    }

    private void Jump()
    {
        if (isJumping)
        {
            anim.Play("Alise_Jump");
            rb.velocity = new Vector2(rb.velocity.x, -jumpHeight * Physics2D.gravity.y / Mathf.Abs(Physics2D.gravity.y));
            isJumping = false;
        }
    }
    private void BetterJumpFelling()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y>0&&!Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
