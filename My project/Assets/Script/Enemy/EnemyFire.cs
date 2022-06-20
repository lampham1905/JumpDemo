using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class EnemyFire : MonoBehaviour
{
   
    #region Spector Variables
    public Transform firePoint;
    public GameObject bulletPrefab;
    [Header("Animation")]
    [SpineAnimation] public string idleNormalAnimationName;
    [SpineAnimation] public string AttackAnimationName;
    [SpineAnimation] public string DieAnimationName;
    
    #endregion

    private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState spineAnimationState;
    private Spine.Skeleton skeleton;
    public bool isShooting = false;
    public Transform endPoint;
    
     private void Start()
    {
      
       //StartCoroutine(AnimationCoroutine());
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
    }
    private void Update() {
        //  if(GameManager.instance.isShootingEnemy){
        //     StartCoroutine(AnimationCoroutine());
        //      GameManager.instance.isShootingEnemy = false;
        //  }
    }

    private void Shot(){
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
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
            GameManager.instance.isJumpingEnemy = true;
            isShooting = false;
            spineAnimationState.SetAnimation(0, DieAnimationName, false);
            spineAnimationState.AddAnimation(0,idleNormalAnimationName, true, 0);
           
        }
        if(other.gameObject.CompareTag("MainCamera")){
            isShooting = true;
            ShootingEnemy();
        }
    }
    public void ShootingEnemy(){
        StartCoroutine(AnimationCoroutine());
    }
    IEnumerator AnimationCoroutine(){
     
            while(isShooting) {
            spineAnimationState.SetAnimation(0, AttackAnimationName, false);
            spineAnimationState.AddAnimation(0, idleNormalAnimationName, true, 0);
            yield return new WaitForSeconds(1f);
            Shot();
            yield return new WaitForSeconds(1.5f);
        }
      
    }
    

    
}
