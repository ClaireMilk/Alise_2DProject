using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public partial class Enemy : MonoBehaviour
{
    [Header("hurt")]
    public GameObject bossState2;
    public GameObject environment_vfx;
    private void EnemyDies()
    {
        if(!isState2)
        {
            Invoke("State2",2.0f);

            this.gameObject.SetActive(false);
        }
        else
        {
            if (OnBossDie != null)
            {
                OnBossDie();
            }
            Destroy(this.transform.parent.gameObject);
        }
    }
    
    void State2()
    {
        bossState2.SetActive(true);
        bossState2.transform.position = this.transform.parent.position;
        environment_vfx.SetActive(true);
    }
    


}
