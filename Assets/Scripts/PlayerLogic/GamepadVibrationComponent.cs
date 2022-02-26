using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamepadVibrationComponent : MonoBehaviour
{

    public float SmallMinFrequency;
    public float SmallMaxFrequency;

    public float MediumMinFrequency;
    public float MediumMaxFrequency;

    public float LargeMinFrequency;
    public float LargeMaxFrequency;

    public float EndingTimeScale;
    public float FreezeTime;
    public float VibrationTime;

    private void Start()
    {
        StopVibration();
    }

    public void LargeVibration()
    {
        Vibration(LargeMinFrequency, LargeMaxFrequency);
    }

    public void MediumVibration()
    {
        Vibration(MediumMinFrequency, MediumMaxFrequency);
    }

    public void SmallVibration()
    {
        Vibration(SmallMinFrequency, SmallMaxFrequency);
    }
    public void ParrySmallVibration()
    {
        Gamepad.current.SetMotorSpeeds(SmallMinFrequency / EndingTimeScale, SmallMinFrequency / EndingTimeScale);
        Invoke("StopVibration", VibrationTime / EndingTimeScale);
    }
    public void ExecuteVibration()
    {
        Gamepad.current.SetMotorSpeeds(LargeMinFrequency / EndingTimeScale, LargeMinFrequency / EndingTimeScale);
        Invoke("StopVibration", VibrationTime * 3f / EndingTimeScale);
    }

    public void FreezeFrame()
    {
        StopAllCoroutines();
        StartCoroutine(AttackFreeze(EndingTimeScale, FreezeTime));
    }

    public void Vibration(float lowFrequency, float highFrequency)
    {
        Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
        Invoke("StopVibration", VibrationTime);
    }

    private void StopVibration()
    {
        if (Gamepad.current != null) Gamepad.current.ResetHaptics();
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
