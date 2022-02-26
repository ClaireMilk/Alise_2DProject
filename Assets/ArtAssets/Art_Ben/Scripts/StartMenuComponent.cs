using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenuComponent : MonoBehaviour
{
    int index = 0;
    bool isCloseBGM = false;

    [SerializeField]
    List<GameObject> m_Buttons;
    public GameObject MusicOn;
    public GameObject MusicOff;

    public AudioSource StartMusic;

    private void Awake()
    {
        int x = SceneManager.GetActiveScene().buildIndex;
        isCloseBGM = PlayerPrefs.GetInt("IsCloseBGM") == 1 ? false : true;
        if (x == 0)
            isCloseBGM = false;
        
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        SelectButton(m_Buttons[0]);
        BGMButtonStartCheck();
    }
    private void Update()
    {
        if (Input.GetAxis("Vertical") == -1 && index < 2)
        {
            SelectButton(m_Buttons[++index]);
        }

    }

    private void SelectButton(GameObject button)
    {
        if (button == null) return;
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(button);
    }
    public void OnClickPlayButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("Play");
    }
    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    #region SoundFunction
    void BGMButtonStartCheck()
    {
        if (!isCloseBGM)
        {
            MusicOn.SetActive(true);
            MusicOff.SetActive(false);
            StartMusic.Play();
        }
        else if (isCloseBGM)
        {
            MusicOn.SetActive(false);
            MusicOff.SetActive(true);
            StartMusic.Pause();
        }
    }

    public void SoundChange()
    {
        if (MusicOn.activeSelf)
        {
            MusicOn.SetActive(false);
            MusicOff.SetActive(true);
            if (!isCloseBGM)
            {
                StartMusic.Pause();
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
                StartMusic.Play();
            }
            EventSystem.current.SetSelectedGameObject(MusicOn);
            isCloseBGM = false;
        }
    }

    private void OnDestroy()
    {
        int x = isCloseBGM ? 0 : 1;
        PlayerPrefs.SetInt("IsCloseBGM", x);
    }
    #endregion
}
