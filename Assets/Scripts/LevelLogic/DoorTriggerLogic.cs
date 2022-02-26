using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerLogic : MonoBehaviour
{
    public bool CanEnterCastle;
    public GameObject lock_sp;
    bool isOpen;

    private AudioSource m_audioSource;
    private AudioClip m_doorLocked;
    private AudioClip m_doorOpen;

    [SerializeField]
    GameObject m_doorAnim;
    private void Start()
    {
        CanEnterCastle = false;
        isOpen = false;
        m_doorAnim.SetActive(false);
        lock_sp.SetActive(false);
        InitializeAudioSources();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.GetComponentInChildren<Player>()) return;
       // Debug.Log("Right!");
       if(Input.GetAxisRaw("Vertical")==1f&&!isOpen)
        {
            if (!CanEnterCastle)
            {
                PlaySoundName(m_doorLocked);
                //show the lock
                lock_sp.SetActive(true);
                Invoke("LockDisappear", 1.0f);
                return;
            }
            //show the unlock
            lock_sp.SetActive(true);
            lock_sp.GetComponent<Animator>().SetTrigger("isUnlocked");
            PlaySoundName(m_doorOpen);
            Invoke("LockDisappear", 1.5f);
            isOpen = true;
        }
    }
    void LockDisappear()
    {
        lock_sp.SetActive(false);
        if(CanEnterCastle)
            m_doorAnim.SetActive(true);
    }

    private void InitializeAudioSources()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_doorLocked = Resources.Load<AudioClip>("Locked");
        m_doorOpen = Resources.Load<AudioClip>("OpenLocker");
    }
    private void PlaySoundName(AudioClip audioClip)
    {
        m_audioSource.clip = audioClip;
        m_audioSource.Play();
    }
}
