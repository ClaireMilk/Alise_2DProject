using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class TitleStringLogic : MonoBehaviour
{
    SpriteRenderer m_spriteRenderer;
    Material m_shaderGraphic;
    const float M_INITIAFADEVALUE = 0.0f;
    const float M_ENDFADEVALUE = 0.85f;
    const float M_FADETIME = 2.5f;
    float m_fadeValue;

    [SerializeField]
    GameObject m_dynamicLight;
    Light2D m_dynamicLightLogic;
    float m_lightIntensity = 0.0f;
    bool m_status = true;
    public float DynamicLightTime = 2.0f;

    private void Awake()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_dynamicLightLogic = m_dynamicLight.GetComponent<Light2D>();
        m_shaderGraphic = m_spriteRenderer.material;
    }
    private void Start()
    {
        Time.timeScale = 1;
        if (m_shaderGraphic)
        {
            m_shaderGraphic.SetFloat("_Fade", M_INITIAFADEVALUE);
        }
        StartCoroutine(GradualChange(M_INITIAFADEVALUE, M_ENDFADEVALUE, M_FADETIME));
    }
    private void Update()
    {
        if (m_lightIntensity > DynamicLightTime || m_lightIntensity < 0.0f)
        {
            m_status = !m_status;
        }
        if (m_dynamicLight && m_dynamicLightLogic)
        {
            m_dynamicLightLogic.intensity = m_lightIntensity;
            m_lightIntensity = m_status? m_lightIntensity + Time.deltaTime : m_lightIntensity - Time.deltaTime;
        }

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
            m_shaderGraphic.SetFloat("_Fade", m_fadeValue);
            yield return null;
        }
    }
}
