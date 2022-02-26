using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public partial class Player : MonoBehaviour
{
    [HideInInspector]
    public bool canExecute;
    bool isExecuting;
    GameObject executeObj;
    //if the weapon is flying
    [HideInInspector]
    public  bool isFlying;
    //the duration from the place to the end position
    [HideInInspector]
    public float moveDuration;
    [Header("Execute")]
    public float sprintSpeed;
    public float sprintSpeed_enemy;
    public float slowMotionSpeed;
    public float slowMotionLength;
    public float executionForce;
    public float executionDistance;
    public float executionDistance_pocker;
    public Collider2D collider_execute;

    public GameObject execution_vfx;
    public GamepadVibrationComponent GamepadVibration;
  //  public GameObject execution_text;
    public int executeCondition;//the perfect times for execution 
    bool isTimer;
    void Execute()
    {
        if (perfectParryTimes == executeCondition)
        {
            execution_vfx.SetActive(true);
            executeObj = parryObj;
            if(!isTimer)
            {
                StartCoroutine(Timer());
                isTimer = true;
            }
            //Debug.Log(Vector2.Distance(this.gameObject.transform.position, executeObj.transform.position));
            if ((Vector2.Distance(this.gameObject.transform.position,executeObj.transform.position)<executionDistance&&(executeObj.GetComponent<Enemy>()||(executeObj.GetComponent<PuppetLogic>()&& executeObj.GetComponent<PuppetLogic>().isPuppet))) 
                ||(executeObj.GetComponent<PuppetLogic>()&&! executeObj.GetComponent<PuppetLogic > ().isPuppet&& Vector2.Distance(this.gameObject.transform.position, executeObj.transform.position) < executionDistance_pocker))
            {
               // Debug.Log("111");
                SlowMotion();
                GamepadVibration.LargeVibration();
            }
            //Play enemy stiff anim
        }
    }
    //Slow motion
    IEnumerator Timer()
    {  
        //add post processing
        yield return new WaitForSecondsRealtime(slowMotionLength);
        isTimer = false;
        perfectParryTimes = 0;
        Time.timeScale = 1;
        canExecute = false;
        execution_vfx.SetActive(false);
    }
    void SlowMotion()
    {
        isTimer = false;
        perfectParryTimes = 0;
        Time.timeScale = slowMotionSpeed;
        canExecute = true;
        //Debug.Log("can execute,Press X to execute");
    }
    public void PlayerExecute()
    {
        //play the execute anim
        if(executeObj)
        {
            if (executeObj.GetComponent<Enemy>())
                trans.localScale = new Vector3(-(trans.position.x - executeObj.GetComponent<Enemy>().transform.position.x) / Mathf.Abs(trans.position.x - executeObj.GetComponent<Enemy>().transform.position.x), 1, 1);
            else if (executeObj.GetComponent<PuppetLogic>())
                trans.localScale = new Vector3(-(trans.position.x - executeObj.GetComponent<PuppetLogic>().transform.position.x) / Mathf.Abs(trans.position.x - executeObj.GetComponent<PuppetLogic>().transform.position.x), 1, 1);
        }

        animator.SetTrigger("Execute");
        GamepadVibration.ExecuteVibration();
        isTimer = false;
        perfectParryTimes = 0;
        Time.timeScale = 1;
        execution_vfx.SetActive(false);
        if(executeObj)
        {
            if (executeObj.GetComponent<Enemy>())
                executeObj.GetComponent<Animator>().SetTrigger("BeforeExecuted");
            canInput = false;
            if (executeObj.GetComponent<Enemy>())
                executeObj.GetComponent<Enemy>().enemyState = EnemyState.BeExecuted;
            else if (executeObj.GetComponent<PuppetLogic>())
                executeObj.GetComponent<PuppetLogic>().enemyState = EnemyState.BeExecuted;
        }

        currentState = PlayerState.Execute;
        //xInput = 0;
        //do harm to enemies
        //sprint to the o
        //ther side of the enemy
        // Physics2D.IgnoreLayerCollision(6, 7, true);
        // selfRigidbody.AddForce(new Vector2(sprintSpeed, 0f), ForceMode2D.Impulse);
        //  executeObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(sprintSpeed_enemy, 0f), ForceMode2D.Impulse);
        // Invoke("RecoverCollision", 1.0f);
        //Debug.Log("is executing");
    }
    void RecoverCollision()
    {
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }
    #region AnimationEvent
    void ResetPosition()
    {
        if(trans.localScale.x>0)
            trans.position += new Vector3(16, 0, 0);
        else
            trans.position -= new Vector3(16, 0, 0);
    }
    void SetState()
    {
        currentState = PlayerState.Idle;
        if(executeObj)
             executeObj.GetComponent<Enemy>().enemyState = EnemyState.Chase;
        canInput = true;
    }
    void SetCollision_execute()
    {
        weaponCollider.GetComponent<PolygonCollider2D>().enabled = true;
    }
    void EnableExecuteCollision()
    {
        collider_execute.enabled = true;
    }
    #endregion
}
