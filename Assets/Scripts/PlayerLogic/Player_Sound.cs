using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    #region Variable Declaration
    // ---- AudioSource Component ----
    private AudioSource m_audioSource;


    // ---- Sound Effects ----
    #region Sound Effects

    private AudioClip m_jumpSFX;
    private AudioClip m_attackHitSFX_1;
    private AudioClip m_attackHitSFX_2;
    private AudioClip m_attackHitSFX_3;
    private AudioClip m_attackMissSFX;
    private AudioClip m_perfectParrySFX_1;
    private AudioClip m_perfectParrySFX_2;
    private AudioClip m_imperfectParrySFX_1;
    private AudioClip m_hurtSFX_1;
    private AudioClip m_execute;
    #endregion
    #endregion

    // Can Load SFX From "Resources" Folder
    private void InitializeAudioSources()
    {
        m_audioSource = GetComponent<AudioSource>();

        m_jumpSFX = Resources.Load<AudioClip>("Player_Jump");
        m_attackHitSFX_1 = Resources.Load<AudioClip>("Player_AttackHit_1");
        m_attackHitSFX_2 = Resources.Load<AudioClip>("Player_AttackHit_2");
        m_attackHitSFX_3 = Resources.Load<AudioClip>("Player_AttackHit_3");
        m_attackMissSFX = Resources.Load<AudioClip>("Player_AttackMiss");
        m_perfectParrySFX_1 = Resources.Load<AudioClip>("Player_PerfectParry_1");
        m_perfectParrySFX_2 = Resources.Load<AudioClip>("Player_PerfectParry_2");
        m_imperfectParrySFX_1 = Resources.Load<AudioClip>("Player_ImperfectParry_1");
        m_hurtSFX_1 = Resources.Load<AudioClip>("Player_Hurt_1");
        m_execute = Resources.Load<AudioClip>("Execute_2");
    }

    // Play SoundEffect
    public void PlaySoundEffect(PlayerAction playerAction)
    {
        switch (playerAction)
        {
            case PlayerAction.Jump:
                PlaySoundName(m_jumpSFX);
                break;
            case PlayerAction.AttackHit_1:
                PlaySoundName(m_attackHitSFX_1);
                break;
            case PlayerAction.AttackHit_2:
                PlaySoundName(m_attackHitSFX_2);
                break;
            case PlayerAction.AttackHit_3:
                PlaySoundName(m_attackHitSFX_3);
                break;
            case PlayerAction.AttackMiss:
                PlaySoundName(m_attackMissSFX);
                break;
            case PlayerAction.ParryPerfect_1:
                PlaySoundName(m_perfectParrySFX_1);
                break;
            case PlayerAction.ParryPerfect_2:
                PlaySoundName(m_perfectParrySFX_2);
                break;
            case PlayerAction.ParryImperfect_1:
                PlaySoundName(m_imperfectParrySFX_1);
                break;
            case PlayerAction.Hurt_1:
                PlaySoundName(m_hurtSFX_1);
                break;
            case PlayerAction.m_execute:
                PlaySoundName(m_execute);
                break;
            default:
                break;
        }
    }

    // Don't Change it
    #region Packaged Methods
    // Enum PlayerAction
   public  enum PlayerAction
    {
        Jump,
        AttackHit_1,
        AttackHit_2,
        AttackHit_3,
        AttackMiss,
        ParryPerfect_1,
        ParryPerfect_2,
        ParryImperfect_1,
        Hurt_1,
        m_execute
    }
    // Play SFX
    private void PlaySoundName(AudioClip audioClip)
    {
        m_audioSource.clip = audioClip;
        m_audioSource.Play();
    }
    #endregion
}
