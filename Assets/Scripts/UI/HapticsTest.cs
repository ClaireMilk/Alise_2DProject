using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

public class HapticsTest : MonoBehaviour
//public class HapticsTest : InputDevice, IDualMotorRumble
{
    private void OnEnable()
    {
        InputSystem.ResumeHaptics();

        Gamepad.current.SetMotorSpeeds(999999f, 9999999f);
    }

    private void OnDisable()
    {
        InputSystem.PauseHaptics();
    }

    //private IDualMotorRumble m_Rumble;
    //public void SetMotorSpeeds(float lowFrequency, float highFrequency)
    //{
    //    m_Rumble.SetMotorSpeeds(lowFrequency, highFrequency);
    //}

    //public void PauseHaptics()
    //{
    //    m_Rumble.PauseHaptics();
    //}

    //public void ResumeHaptics()
    //{
    //    m_Rumble.ResumeHaptics();
    //}

    //public void ResetHaptics()
    //{
    //    m_Rumble.ResetHaptics();
    //}
}
