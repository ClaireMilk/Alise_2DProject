using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    public float m_moveSpeed;
    public float m_direction;
    private float time;
    public float timer;
    public int damage;
    public int maxHP;
    public int currentHP;
    [HideInInspector]
    public bool isHurt;

    SpriteRenderer[] m_spriteRenderer;
    List<Material> m_selfMaterialShader = new List<Material>();
    Collider2D m_coll;
    const float M_INITIAFADEVALUE = 1.0f;
    const float M_ENDFADEVALUE = 0.0f;
    const float M_FADETIME = 1f;
    float m_fadeValue;
    bool m_isDying;
    // Start is called before the first frame update
    private void Awake()
    {
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        m_coll = GetComponent<Collider2D>();
        currentHP = maxHP;
        m_spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        foreach (var spriteRender in m_spriteRenderer)
        {
            m_selfMaterialShader.Add(spriteRender.material);
        }
        m_isDying = false;
    }
    private void Update()
    {
        if (currentHP <= 0 && gameObject)
        {
            StartCoroutine(GradualChange(M_INITIAFADEVALUE, M_ENDFADEVALUE, M_FADETIME));
            m_coll.enabled = false;
            m_isDying = true;
            Invoke("DestroySelf", M_FADETIME);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isHurt && !m_isDying)
        {
            Move(m_direction);
            time += Time.deltaTime;
            if (time >= timer)
            {
                m_direction = -m_direction;
                time = 0;
            }
        }

    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
    public void Move(float direction)
    {
        if (direction < 0)
        {
            sr.flipX = true;
        }
        if (direction > 0)
        {
            sr.flipX = false;
        }

        rb.velocity = new Vector2(direction * m_moveSpeed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.GetComponentInChildren<Player>().BeHurt(this.gameObject, 5,9);
    }
    IEnumerator GradualChange(float initialValue, float endValue, float seconds)
    {
        Vector2 initialPos = new Vector2(0, initialValue);
        Vector2 endPos = new Vector2(seconds, endValue);
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / seconds;
            m_fadeValue = Vector2.Lerp(initialPos, endPos, t).y;
            foreach (var shaderMat in m_selfMaterialShader)
            {
                shaderMat.SetFloat("_Fade", m_fadeValue);
            }
            yield return null;
        }
    }
}
