using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI_PauseMenu : MonoBehaviour
{
    public AudioSource Level1And2;
    public AudioSource Level3And4;
    public AudioSource Level5;
    public AudioSource BossFightMusic;
    bool canPlayBossBGM = false;
    bool isCloseBGM = false;
    bool doOnce = true;
    bool doOnceBoss = true;

    public GameObject PauseMenu;
    public GameObject PauseFirstButton;

    public GameObject MusicOn;
    public GameObject MusicOff;
    public GameObject Boss;
    private void Awake()
    {
        isCloseBGM = PlayerPrefs.GetInt("IsCloseBGM") == 1? false : true;
        DontDestroyOnLoad(Level1And2);
    }
    void Start()
    {
        if(Boss != null)
        Boss.SetActive(false);
        PauseMenu.SetActive(false);
        BGMButtonStartCheck();
        StartBGMCheck();

        //BackgroundMusic.Play();
        Time.timeScale = 1;

    }

    private void Update()
    {
        OpenPauseMenu();

    }

    #region PauseMenuFunction
    public void GamePause()
    {
        if (PauseMenu.activeSelf)
        {
            PauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        else if (!PauseMenu.activeSelf)
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(PauseFirstButton);
        }
    }

    public void Restart()
    {
        int x = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(x);
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("StartScene");
    }

    void OpenPauseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            GamePause();
        }
    }
    #endregion

    #region Sound
    public void SoundChange()
    {
        if (MusicOn.activeSelf)
        {
            MusicOn.SetActive(false);
            MusicOff.SetActive(true);
            if (!isCloseBGM)
            {
                PauseBGM();
            }
            EventSystem.current.SetSelectedGameObject(MusicOff);
            isCloseBGM = true;
        }
        else if (!MusicOn.activeSelf)
        {
            MusicOn.SetActive(true);
            MusicOff.SetActive(false);
            if (isCloseBGM)
            {
                RestartBGM();
            }
            EventSystem.current.SetSelectedGameObject(MusicOn);
            isCloseBGM = false;
        }
    }

    void StartBGMCheck()
    {
        int x = SceneManager.GetActiveScene().buildIndex;
        if (!isCloseBGM)
        {
            if (x == 1 || x == 2)
            {
                Level1And2.Play();
            }
            else if (x == 3 || x == 4)
            {
                Level3And4.Play();
            }
            else if (x == 5)
            {
                Level5.Play();
            }
        }
    }

    void PauseBGM()
    {
        int x = SceneManager.GetActiveScene().buildIndex;
        if (x == 1 || x == 2)
        {
            Level1And2.Pause();
        }
        else if (x == 3 || x == 4)
        {
            Level3And4.Pause();
        }
        else if (x == 5)
        {
            Level5.Pause();
        }
        else if (x == 6)
        {
            BossFightMusic.Pause();
        }
    }

    void RestartBGM()
    {
        int x = SceneManager.GetActiveScene().buildIndex;
        if (x == 1 || x == 2)
        {
            Level1And2.Play();
        }
        else if (x == 3 || x == 4)
        {
            Level3And4.Play();
        }
        else if (x == 5)
        {
            Level5.Play();
        }

        else if (x == 6)
        {
            if (canPlayBossBGM)
            {
                BossFightMusic.Play();
            }
        }
    }

    void BGMButtonStartCheck()
    {
        if (!isCloseBGM)
        {
            MusicOn.SetActive(true);
            MusicOff.SetActive(false);
        }
        else if (isCloseBGM)
        {
            MusicOn.SetActive(false);
            MusicOff.SetActive(true);
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int x = SceneManager.GetActiveScene().buildIndex;

        if (x == 6)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (doOnceBoss)
                {
                    Boss.SetActive(true);
                    Boss.GetComponentInChildren<Enemy>().ShowTime();
                    doOnceBoss = false;
                }
                if (!isCloseBGM)
                {
                    if (doOnce)
                    {
                        
                        BossFightMusic.Play();
                    }
                }
                doOnce = false;
                canPlayBossBGM = true;
            }
        }
    }
    private void OnDestroy()
    {
        int x = isCloseBGM ? 0 : 1;
        PlayerPrefs.SetInt("IsCloseBGM", x);
    }
}
