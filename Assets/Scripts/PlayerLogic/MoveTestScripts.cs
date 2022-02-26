using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTestScripts : MonoBehaviour
{
    #region variable
    [Header("Movement")]
    public float xOffSet;//射线发射点横向偏移
    public float moveVelocity = 8f;
    public float acceleration = 13f;
    public float deceleration = 13f;
    [Range(0, 1)]
    public float velPower = 0.96f;
    [Range(0, 1)]
    public float frictionAmount = 0.22f;
    public float airAccel = 9f;
    public float airDecel = 9f;
    public float rayLength = 0.5f;

    private Transform originalTransform;
    private float direction = 1f;
    #endregion
    private Rigidbody2D m_rigidbody;

    /// <summary>
    /// AddForce to the player to make it move
    /// </summary>
    /// 
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        direction = Input.GetAxisRaw("Horizontal");
    }
    private void FixedUpdate()
    {
        Time.timeScale = 1;
        PlayerMove(direction);
    }
    private void PlayerMove(float direction)
    {
        m_rigidbody.velocity = new Vector2(direction * moveVelocity*Time.deltaTime, m_rigidbody.velocity.y);
    }
}
