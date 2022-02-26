using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLastSceneComponent : MonoBehaviour
{
    private void Start()
    {
        Enemy.OnBossDie += LoadLastScene;
    }
    public void LoadLastScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(++sceneIndex);
    }
}
