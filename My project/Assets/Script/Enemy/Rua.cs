using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Rua : MonoBehaviour
{
    #region Inspector
    [Header("Animation")]
    [SpineAnimation] public string idleNormalAnimationName;
    [SpineAnimation] public string moveAnimationName;
    [SpineAnimation] public string idleShellAnimationName;

    [Header("Transitions")]
    [SpineAnimation] public string normalToShellAnimationName;

    [Header("PointMove")] 
    [SerializeField] private float left;
    [SerializeField] private float right;
    [SerializeField] private float speed = 5f;
    #endregion

    private Rigidbody2D rb;
    private bool facingLeft = true;
    private bool canMove = true;
    public Transform endPoint;

    // Spine Animation
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
    public Spine.Skeleton skeleton;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //SpineAnimation
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove){
            Move();
        }
        //spineAnimationState.SetAnimation(0, moveAnimationName, true);

    }
    private void Move(){
        if (facingLeft)
        {

            if (transform.position.x > left)
            {
                // make sure sprite is facing location, and if it is not, then face the right direction
                if (transform.localScale.x != 1)
                {
                    //transform.localScale = new Vector3(1, 1);
                    skeleton.ScaleX = 1;
                }
                // test to see if we are on the ground, if so jump
              
                    rb.velocity = new Vector2(-speed, 0);
                    //spineAnimationState.SetAnimation(0, moveAnimationName, true);
                
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < right)
            {
                // make sure sprite is facing location, and if it is not, then face the right direction
                if (transform.localScale.x != -1)
                {
                    //transform.localScale = new Vector3(-1, 1);
                    skeleton.ScaleX = -1;
                }
                // test to see if we are on the ground, if so jump
               
                    rb.velocity = new Vector2(speed, 0);
                    //spineAnimationState.SetAnimation(0, moveAnimationName, true);
                
                   
            }
            else
            {
                facingLeft = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("foot")){
            if(PlayerPrefs.GetInt("isSoundOn", 1) == 1){
                BgSound.Instance.PlayHit();
            }
            PlayerController.instance.DisableRigibody();
            PlayerController.instance.EnableRigigbody();
            FolowRoute.ins.JumpToEnd(endPoint);
            NormalToShell();
            // GameManager.instance.isJumpingEnemy = true;
            canMove = false;
        }
        
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            PlayerController.instance.Die();
        }
    }
    public void  NormalToShell(){
        spineAnimationState.SetAnimation(0, normalToShellAnimationName, false);
        spineAnimationState.AddAnimation(0, idleShellAnimationName, true, 0);
    }

}
