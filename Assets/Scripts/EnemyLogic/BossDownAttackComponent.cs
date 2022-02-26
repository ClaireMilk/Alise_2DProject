using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDownAttackComponent : MonoBehaviour
{
    public ParticleSystem DownAttackParticle;

    public void CreatDownParticle()
    {
        DownAttackParticle.Play();
    }

    public void DestroyEnemyParticle()
    {
        if (DownAttackParticle)
        {
            DownAttackParticle.Clear();
        }
    }
}
