using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    void Awake(){
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
            return;
        }
    }
    public Vector2 jumpForce;
    public Vector2 jumpForceUp;
     public float minForceX;
    public float maxForceX;
    public float minForceY;
    public float maxForceY;
    public AnimationCurve curve;
    
    
    
    // Set power
    bool m_didJump;
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
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        lr = GetComponent<LineRenderer>();
        // rend = GetComponent<Renderer>();
        // rend.material.mainTextureScale =
        //   new Vector2(Vector2.Distance(lr.GetPosition(0), lr.GetPosition(lr.positionCount - 1)) / lr.widthMultiplier,1);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
    SetPower();
    if(Input.GetMouseButtonDown(0)){
            SetPower(true);
        }
    if(Input.GetMouseButtonUp(0)){
            SetPower(false);
            lr.positionCount = 0;
        }
    
        // rend.material.mainTextureScale =
        //   new Vector2(Vector2.Distance(lr.GetPosition(0), lr.GetPosition(lr.positionCount - 1)) / lr.widthMultiplier,1);
    lr.material.mainTextureScale = new Vector2(1f / lr.startWidth, 1.0f);
#endif
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
            minForceX = .2f;
            maxForceX = 2.2f;
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
                jumpForce.x += jumpForceUp.x * Time.deltaTime;
                jumpForce.y += jumpForceUp.y * Time.deltaTime;
                jumpForce.x = Mathf.Clamp(jumpForce.x, minForceX, maxForceX);
                jumpForce.y = Mathf.Clamp(jumpForce.y, minForceY, maxForceY);
            }

            if(maxPower){
                // Effect by wind
                //jumpForce.x -= jumpForceUp.x * Time.deltaTime + speedWind/300;

                // Not effect by wind
                jumpForce.x -= jumpForceUp.x * Time.deltaTime;
                jumpForce.y -= jumpForceUp.y * Time.deltaTime;
                jumpForce.x = Mathf.Clamp(jumpForce.x, minForceX, maxForceX);
                jumpForce.y = Mathf.Clamp(jumpForce.y, minForceY, maxForceY);
            }
                           
           



            // tranjectory line
            Vector2[] tranjectory = Plot(rb, (Vector2)transform.position, jumpForce, 500);
            lr.positionCount = tranjectory.Length;
            Vector3[] positions = new Vector3[tranjectory.Length];
            for(int i = 0; i < tranjectory.Length; i++)
            {
                positions[i] = tranjectory[i];
            }
            lr.SetPositions(positions);
           
            float width = lr.startWidth;
            //lr.material.mainTextureScale = new Vector2(1f / lr.startWidth, 1.0f);

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

 
    public void JumpEnemy(){
        
    }
   
    
 
   
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag  == "Ground"){
            if(m_didJump){
                m_didJump = false;
                if(rb){
                rb.velocity = Vector2.zero;
                }
                jumpForce = Vector2.zero;
                currPowerBarVal = 0;
            }
        }    
        if(other.gameObject.tag == "Die"){
            Die();
            StartCoroutine(DieCouroutine());
        }
        if(other.gameObject.CompareTag("endPoint")){
            if(GameManager.instance.endPoint.Length > GameManager.instance.currEndPoint + 1){
                GameManager.instance.currEndPoint++;
            }
            else{
                return;
            }
        }
       
    }
    IEnumerator DieCouroutine(){
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
     private void OnTriggerEnter2D(Collider2D other)
    {
         if(other.gameObject.tag == "nextPlatform"){
            CamController.instance.LerpTrigeer(transform.position.x + 1.8f);
        }    
        if(other.gameObject.tag == "jumpHalf"){
            jumpFull = false;
        }
        if(other.gameObject.tag == "jumpFull"){
            jumpFull = true;
        }
        if(other.gameObject.tag == "end"){
            NextScene(nextScene);
        }
        if(other.gameObject.CompareTag("Die")){
            Die();
            StartCoroutine(DieCouroutine());
        }
    }
    
    public void DisableRigibody(){
        rb.bodyType = RigidbodyType2D.Static;
    }
    public void InvokeEnableRigigbody(){
    
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    public void EnableRigigbody(){
        Invoke("InvokeEnableRigigbody", 1f);
    }
    private void NextScene(string nextScene){
        SceneManager.LoadScene(nextScene);
    }
    public void Die(){
        rb.velocity = new Vector2(0, 10f);
        coll.enabled = false;
    }

}
