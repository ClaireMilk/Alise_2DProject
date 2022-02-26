using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTriggerLogic : MonoBehaviour
{
    DoorTriggerLogic m_doorTriggerLogic;
    public GameObject m_door;
    private AudioSource m_audioSource;
    public AudioClip m_pickup;
    private void Start()
    {
        m_audioSource = GetComponentInParent<AudioSource>();
        m_doorTriggerLogic = m_door.GetComponent<DoorTriggerLogic>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.GetComponentInChildren<Player>()) return;
        m_doorTriggerLogic.CanEnterCastle = true;
        PlaySoundName(m_pickup);

        DestroySelf();
        
    }
    private void PlaySoundName(AudioClip audioClip)
    {
        m_audioSource.clip = audioClip;
        m_audioSource.Play();
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
