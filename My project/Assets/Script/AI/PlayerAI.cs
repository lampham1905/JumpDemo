using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spine.Unity;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class PlayerAI : MonoBehaviour
{
    public static PlayerAI instance;
    void Awake(){
            instance = this;
    }
    [Header("Animation")]
    [SpineAnimation] public string idleAnimationName;
    [SpineAnimation] public string dieAnimationName;
    [SpineAnimation] public string holdAnimationName;
    [SpineAnimation] public string jumpStartAnimationName;
    [SpineAnimation] public string jumpUpAnimationName;
    [SpineAnimation] public string jumpDownAnimationName;
    [SpineAnimation] public string jumpEndAnimationName;


    [Header("Movement")]
    public Vector2 jumpForce;
    public Vector2 jumpForceUp;
     public float minForceX;
    public float maxForceX;
    public float minForceY;
    public float maxForceY;
    public AnimationCurve curve;



    // Set power
    bool m_didJump;
    bool m_didJumpDown;
    bool m_powerSetted;
    bool maxPower;
   
    public bool jumpFull = true;

    //Parabola
    public float h = 25;
    public float gravity = -18;
    private Rigidbody2D rb;
    private Collider2D coll;
    public float currPowerBarVal = 0;
    public string nextScene;
    private LineRenderer lr;
    //private Renderer rend;
     public float displacementY;
     public float forceX;

     // Spine Region
     SkeletonAnimation skeletonAnimation;
     public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;
    public GameObject foot;
    bool isMouseDown = false;
    public bool isAIJumpHold =false;
    public bool isAIJumpStart = false;
    public GameObject PlatformPosition;
    public Transform[] platFormPositionList;
    public Transform platformPositionCur;
    public int indexPlaotformPositionCur = 0;
    public bool isCompelete = false;
    Transform currentPosition;
    public int ramdomAIDieValue;

    void Start()
    {
        platFormPositionList = GameManager.instance.platformPositionsList;
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        lr = GetComponent<LineRenderer>();
        // rend = GetComponent<Renderer>();
        // rend.material.mainTextureScale =
        //   new Vector2(Vector2.Distance(lr.GetPosition(0), lr.GetPosition(lr.positionCount - 1)) / lr.widthMultiplier,1);
        //Spine animation
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
        Time.timeScale = 1;
        // AI Jump Hold
        if(PlayerPrefs.GetInt("dokho") == 0){
            StartCoroutine(AIStartHold(1f));
        }
        else {
            StartCoroutine(AIStartHold(0.3f));
        }
       
    }
    IEnumerator AIStartHold(float timeWaitToHold){
        yield return new WaitForSeconds(timeWaitToHold);
        isAIJumpHold = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(ramdomAIDieValue);
        //Debug.Log(PlayerPrefs.GetInt("dokho"));
        if(indexPlaotformPositionCur <  platFormPositionList.Length){
            platformPositionCur = platFormPositionList[indexPlaotformPositionCur];
        }
        
         SetPower();
    if(isAIJumpHold && !isCompelete){
            SetPower(true);
            spineAnimationState.SetAnimation(0, holdAnimationName, false);
            isMouseDown = true;
        }
    if(isAIJumpStart){
            if(PlayerPrefs.GetInt("isSoundOn", 1) == 1){
                //BgSound.Instance.PlayJumpUp();
            }
            SetPower(false);
            lr.positionCount = 0;
            spineAnimationState.SetAnimation(0, jumpStartAnimationName, false);
            spineAnimationState.AddAnimation(0, jumpUpAnimationName, false, 0);
            isMouseDown = false;
            isAIJumpHold = false;
        }
        // rend.material.mainTextureScale =
        //   new Vector2(Vector2.Distance(lr.GetPosition(0), lr.GetPosition(lr.positionCount - 1)) / lr.widthMultiplier,1);
        lr.material.mainTextureScale = new Vector2(1f / lr.startWidth, 1.0f);
        checkAnimationJump();
        CheckRaycast();

    }
    private void CheckCompelete(){
        if(isCompelete){
            spineAnimationState.SetAnimation(0, idleAnimationName, true);
        }
    }
    void fixedUpdate(){

    }
    void SetPower()
    {
        if(jumpFull){
            minForceX = 2.5f;
            maxForceX = 4.5f;
            minForceY = 3f;
            maxForceY = 13f;
        }
        else{
            minForceX = .7f;
            maxForceX = 2.7f;
            minForceY = 1f;
            maxForceY = 11f;

        }
        forceX = jumpForce.x;
        if (m_powerSetted && !m_didJump)
        {
            //m_powerJumpX = jumpForce.x;
            if(jumpForce.x == maxForceX) maxPower = true;
            if(jumpForce.x == minForceX) maxPower = false;
            if(maxPower == false){
                //Effect by wind
                //jumpForce.x += jumpForceUp.x * Time.deltaTime + speedWind/300;

                // Not effect by wind
                jumpForce.x += jumpForceUp.x * Time.deltaTime/1.5f;
                jumpForce.y += jumpForceUp.y * Time.deltaTime/1.5f;
                jumpForce.x = Mathf.Clamp(jumpForce.x, minForceX, maxForceX);
                jumpForce.y = Mathf.Clamp(jumpForce.y, minForceY, maxForceY);
                
            }

            if(maxPower){
                // Effect by wind
                //jumpForce.x -= jumpForceUp.x * Time.deltaTime + speedWind/300;

                // Not effect by wind
                jumpForce.x -= jumpForceUp.x * Time.deltaTime/1.5f;
                jumpForce.y -= jumpForceUp.y * Time.deltaTime/1.5f;
                jumpForce.x = Mathf.Clamp(jumpForce.x, minForceX, maxForceX);
                jumpForce.y = Mathf.Clamp(jumpForce.y, minForceY, maxForceY);
            }





            // tranjectory line
            Vector2 StartTranjectory = new Vector2(transform.position.x, transform.position.y + .5f);
            Vector2[] tranjectory = Plot(rb, StartTranjectory, jumpForce, 450);
            lr.positionCount = tranjectory.Length;
            Vector3[] positions = new Vector3[tranjectory.Length];
            for(int i = 0; i < tranjectory.Length; i++)
            {
                positions[i] = tranjectory[i];
            }
            lr.SetPositions(positions);
            

            float width = lr.startWidth;
            //lr.material.mainTextureScale = new Vector2(1f / lr.startWidth, 1.0f);
            if(PlayerPrefs.GetInt("dokho") == 1){
                if(platformPositionCur != null){
                for (int i = 0; i < 450; i++){
                if(Math.Abs (tranjectory[i].x -  platformPositionCur.transform.position.x) < 0.02  && Math.Abs (tranjectory[i].y - platformPositionCur.transform.position.y) < 0.02 ){
                    Debug.Log(tranjectory[i].x);
                    isAIJumpStart = true;
                }
            }
          }
         }
         if(PlayerPrefs.GetInt("dokho") == 0){
             if(platformPositionCur != null){
                 int ramdomAIDie = Random.Range(0, 2);
                 ramdomAIDieValue = ramdomAIDie;
                 if(ramdomAIDie == 1){
                     for (int i = 0; i < 450; i++){
                        if(Math.Abs (tranjectory[i].x -  platformPositionCur.transform.position.x) < 0.02  && Math.Abs (tranjectory[i].y - platformPositionCur.transform.position.y) < 0.02 ){
                         //Debug.Log(tranjectory[i].x);
                        isAIJumpStart = true;
                }
            }
                 }
                 else{
                     for (int i = 0; i < 450; i++){
                        if(Math.Abs (tranjectory[i].x -  platformPositionCur.transform.position.x) < 0.1  && Math.Abs (tranjectory[i].y - platformPositionCur.transform.position.y) < 0.1 ){
                        //Debug.Log(tranjectory[i].x);
                        isAIJumpStart = true;
                }
            }
                 }
             }
         }
          

            

        }
    }
     public  Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps){
        Vector2[] results = new Vector2 [steps];

        float timestep = Time.fixedDeltaTime/ Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;

        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;
        for(int i = 0; i < steps; i++){
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }
        return results;
    }

      public void SetPower(bool isHoldingMosue)
    {
        m_powerSetted = isHoldingMosue;

        if(!m_powerSetted && !m_didJump)
        {
            Jump();
        }
    }
     void Jump()
    {
        if (!rb || jumpForce.x <= 0 || jumpForce.y <= 0) return;

        rb.velocity = jumpForce;

        m_didJump = true;

        // if (m_anim)
        // {
        //     anim.SetBool("didJump", true);
        // }
    }
    // check animation when jump
    private void checkAnimationJump(){
        if(m_didJump){
            if(rb.velocity.y < 0){
                spineAnimationState.SetAnimation(0, jumpDownAnimationName, false);
                m_didJumpDown = true;
            }
        }
        if(m_didJumpDown){
            if (!m_didJump){
                spineAnimationState.SetAnimation(0, jumpEndAnimationName, false);
                spineAnimationState.AddAnimation(0,idleAnimationName, true, 0);
                m_didJumpDown = false;
            }
        }
    }
    IEnumerator AnimationJumpDown(){
        yield return new WaitForSeconds(.233f);
        spineAnimationState.SetAnimation(0, idleAnimationName, true);

    }
    public void CheckRaycast(){
        int layerPlatform = 3;
        int layerMaskPLatformat = 1 << layerPlatform;
        RaycastHit2D hitBottom = Physics2D.Raycast(transform.position, Vector2.down, 1f, layerMaskPLatformat);
    //     if(GameManager.instance.isJumpingEnemy){
    //        if(hitBottom.collider != null && hitBottom.collider.CompareTag("Ground")){
    //             GameManager.instance.isJumpingEnemy = false;
    //             if(GameManager.instance.endPoint.Length > GameManager.instance.currEndPoint + 1){
    //                  GameManager.instance.currEndPoint++;
    //             }
    //             else{
    //                 return;
    //             }
    //         }
    // }
        // if(GameManager.instance.isShootingEnemyStop){
        //     if(hitBottom.collider != null && hitBottom.collider.CompareTag("Ground")){
        //         GameManager.instance.isShootingEnemyStop = false;
        // }}
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag  == "Ground"){
            if(PlayerPrefs.GetInt("isSoundOn", 1) == 1){
                //BgSound.Instance.PlayJumpDown();
            }
            if(m_didJump){
                m_didJump = false;
                if(rb){
                rb.velocity = Vector2.zero;
                }
                jumpForce = Vector2.zero;
                currPowerBarVal = 0;
                isAIJumpStart = false;
                if(PlayerPrefs.GetInt("dokho") == 0){
                    StartCoroutine(AIStartHold(1f));
                }
                else{
                    StartCoroutine(AIStartHold(0.3f));
                }
                indexPlaotformPositionCur++;
            }
        }
        if(other.gameObject.tag == "Die"){
           
            Die();
            //StartCoroutine(DieCouroutine());
        }

    //    if(other.gameObject.CompareTag("endPoint")){
    //         if(GameManager.instance.endPoint.Length > GameManager.instance.currEndPoint + 1){
    //             GameManager.instance.currEndPoint++;
    //         }
    //         else{
    //             return;
    //         }
    //     }

    }
    IEnumerator DieCouroutine(){
        //GameManager.instance.isAIDead = true;
        GameManager.instance.SpawnAIPlayer();
        yield return new WaitForSeconds(.5f);
        Destroy(gameObject);
        }
     private void OnTriggerEnter2D(Collider2D other)
    {
        //  if(other.gameObject.CompareTag("nextPlatform")){
        //     CamController.instance.LerpTrigeer(transform.position.x + 1.8f);
        // }
        if(other.gameObject.CompareTag("jumpHalf")){
            jumpFull = false;
            GameUIManager.instance.ShowLeftWind();
        }
        if(other.gameObject.CompareTag("jumpFull")){
            jumpFull = true;
            GameUIManager.instance.ShowRightWind();

        }
        // if(other.gameObject.CompareTag("end")){
        //     GameManager.instance.AddLevel();
        //     StartCoroutine(NextSceneCounter());
        // }
       
        // if(other.gameObject.CompareTag("EnemyShooting")){
        //     GameManager.instance.isShootingEnemy = true;

        // }


    }
    private void OnTriggerStay2D(Collider2D other) {
         if(other.gameObject.CompareTag("Die")){
            //BgSound.Instance.PlayDie();
            Die();
        }
    }
    // public void NextLevel(){
    //      GameManager.instance.AddLevel();
    //     StartCoroutine(NextSceneCounter());
    // }
   
    public void DisableRigibody(){
        rb.bodyType = RigidbodyType2D.Static;
    }
    public void InvokeEnableRigigbody(){

        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public void EnableRigigbody(){
        Invoke("InvokeEnableRigigbody", 1f);
    }
    
    public void Die(){
        if(PlayerPrefs.GetInt("isSoundOn", 1) == 1){
            //BgSound.Instance.PlayDie();
        }
        foot.SetActive(false);
        rb.velocity = new Vector2(0, 8f);
        coll.enabled = false;
        spineAnimationState.SetAnimation(0, dieAnimationName, false);
        StartCoroutine(DieCouroutine());
        
       
    }
    IEnumerator NextSceneCounter(){
        yield return new WaitForSeconds(.2f);
        GameUIManager.instance.ShowVictoryPanel();
       
    }
    public void SetIdleAnimationState(){
        spineAnimationState.SetAnimation(0, idleAnimationName, true);
    }

}
