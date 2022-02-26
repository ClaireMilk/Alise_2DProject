using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BGMController : MonoBehaviour
{
    [SerializeField] Image soundOnIcon;
    [SerializeField] Image soundOffIcon;
    private bool muted = false;
    public AudioSource BackgroundMusic;
    // Start is called before the first frame update
    void Start()
    {
        soundOnIcon.enabled = true;
        soundOffIcon.enabled = false;
        if(!PlayerPrefs.HasKey("muted"))
        {
            PlayerPrefs.SetInt("muted", 0);
            //load();
        }
        else
        {
            load();
        }
        UpdateButtonIcon();
        BackgroundMusic.Play();
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnButtonPressed()
    {
        if(muted==false)
        {
            muted = true;
            BackgroundMusic.Pause();
        }
        else
        {
            muted = false;
            BackgroundMusic.Play();
        }
        save();
        UpdateButtonIcon();
    }
    private void UpdateButtonIcon()
    {
        if(muted==false)
        {
            soundOnIcon.enabled = true;
            soundOffIcon.enabled = false;
        }
        else
        {
            soundOnIcon.enabled = false;
            soundOffIcon.enabled = true;
        }
    }
    private void load()
    {
        muted = PlayerPrefs.GetInt("muted") == 1;
    }
    private void save()
    {
        PlayerPrefs.SetInt("muted", muted ? 1 : 0); //if muted is true, we will set it as 1, if it is false,it will be zero.
    }
}
