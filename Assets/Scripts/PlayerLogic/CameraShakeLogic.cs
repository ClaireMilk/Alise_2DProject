using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShakeLogic : MonoBehaviour
{
    public static CameraShakeLogic Instance = null;
    GameObject m_camera;
    Vector3 m_cameraPos;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        m_camera = GameObject.FindGameObjectWithTag("MainCamera");
        m_cameraPos = m_camera.transform.position;
    }
    public void AttackFreezeFrames(float endTimeSacle, float time)
    {
        //StopAllCoroutines();
        StartCoroutine(AttackFreeze(endTimeSacle, time));
    }
    public void ShakeCamera(float range, float time)
    {
        StopAllCoroutines();
        StartCoroutine(Shake(range, time));
    }
    public IEnumerator Shake(float range, float time)
    {
        Vector3 originalPos = transform.position;
        while (time >= .0f )
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                break;
            }
            Vector3 pos = transform.position;
            pos.x += Random.Range(-range, range);
            pos.y += Random.Range(-range, range);
            transform.position = pos;
            yield return null;
        }
        transform.position = m_cameraPos;
    }
    public IEnumerator AttackFreeze(float endingTimeScale, float seconds)
    {
        float originalTimeScale = 1f;
        Vector2 a = new Vector2(0, originalTimeScale);
        Vector2 b = new Vector2(1, endingTimeScale);
        Time.timeScale = 0f;
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / seconds;
            Time.timeScale = endingTimeScale;
            yield return null;
        }
        Time.timeScale = originalTimeScale;
    }
}
