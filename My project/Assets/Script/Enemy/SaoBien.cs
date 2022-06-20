using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class SaoBien : MonoBehaviour
{
    //public static SaoBien ins;
   

    bool _isMoving;
    bool facingLeft = true;
    [SerializeField] private Rigidbody2D rb;
    #region Inspector
    [SerializeField] float speed = 1;
    [SerializeField] float speedRotate;
    [SerializeField] float top;
    [SerializeField] float bottom;

    [Header("Spine Animation")]
    [SpineAnimation] public string upAnimationName;
    [SpineAnimation] public string downAnimationName;
    public GameObject player;
    #endregion

    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
	public Spine.Skeleton skeleton;
    
     // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        skeletonAnimation = GetComponent<SkeletonAnimation>();
		spineAnimationState = skeletonAnimation.AnimationState;
		skeleton = skeletonAnimation.Skeleton;

       // gameObject.SetActive(true);
       StartCoroutine(DestroySelf());
    }

    // Update is called once per frame
    void Update()
    {
        Move();
      

    }
    private void RotateAround(){
        Vector3 RotateAxis = Vector3.Cross(Vector3.up, Vector3.left);
        transform.RotateAround(transform.position, RotateAxis, speedRotate);
    }
   
      private void Move()
    {
        if (facingLeft)
        {

            if (transform.position.y > bottom)
            {
                // make sure sprite is facing location, and if it is not, then face the right direction
               
                // test to see if we are on the ground, if so jump
               
                 rb.velocity = new Vector2(rb.velocity.x, -speed);
                 //rb.AddTorque(-speed/4, 0);   
                 //RotateAround();
               
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.y < top)
            {
                // make sure sprite is facing location, and if it is 
                // test to see if we are on the ground, if so jump
                rb.velocity = new Vector2(rb.velocity.x, speed);
                //rb.AddTorque(speed/4, 0);
                //RotateAround();
               
            }
            else
            {
                facingLeft = true;
            }
        }
        
    }
   
   private void OnTriggerEnter2D(Collider2D other) {
       if(other.gameObject.CompareTag("bot")){
           spineAnimationState.SetAnimation(0, upAnimationName, true);
           //Cua.ins.Attack();
           Debug.Log("bot");
           //StartCoroutine(SetActive());
           }
       if(other.gameObject.CompareTag("top")){
           spineAnimationState.SetAnimation(0, downAnimationName, true);
           Debug.Log("top");
       }
   }
   public void SetActive(bool SetActive){
        this.gameObject.SetActive(SetActive);
   }
    IEnumerator DestroySelf(){
        yield return new WaitForSeconds(1.35f);
        Destroy(gameObject);
    }
}
