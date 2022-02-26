using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBottleLogic : MonoBehaviour
{
    SpriteRenderer sr;
    Player m_playerLogic;
    private AudioSource m_audioSource;
    public AudioClip m_pickup;

    public int healNum;
    void Start()
    {
        m_audioSource = GetComponentInParent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        m_playerLogic = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInChildren<Player>() != null)
        {
            collision.GetComponentInChildren<Player>().HealthBottleHeal(healNum, this.gameObject, m_pickup);
        }
    }
    private void PlaySoundName(AudioClip audioClip)
    {
        m_audioSource.clip = audioClip;
        m_audioSource.Play();
    }
}
