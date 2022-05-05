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
    bool isShooting = false;
     private void Start()
    {
        StartCoroutine(AnimationCoroutine());
       // StartCoroutine(ShotCoroutine());
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        spineAnimationState = skeletonAnimation.AnimationState;
        skeleton = skeletonAnimation.Skeleton;
    }

    private void Shot(){
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
    IEnumerator ShotCoroutine(){
        
        while(true){
            yield return new WaitForSeconds(1.6f);
            Shot();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // if(other.gameObject.tag == "foot"){
        //     Destroy(gameObject);
        //     FolowRoute.ins.JumpToEnd();
        //     PlayerController.instance.DisableRigibody();
        // }
    }
    IEnumerator AnimationCoroutine(){
        while(true){
          
             yield return new WaitForSeconds(1.5f);
            spineAnimationState.SetAnimation(0, AttackAnimationName, false);
              spineAnimationState.AddAnimation(0, idleNormalAnimationName, true, 0);
              yield return new WaitForSeconds(.2f);
              Shot();
        }
    }
}
