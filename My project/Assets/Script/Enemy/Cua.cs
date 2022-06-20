using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
public class Cua : MonoBehaviour
{
    public static Cua ins;
     private void Awake() {
         ins = this;
    }
    #region  Inspector;
    public Transform SpawnPoint;
    public SaoBien saoBien;
    public GameObject saoBienPrefab;
    [SpineAnimation] public string idleAnimationName;
    [SpineAnimation] public string attackAnimationName;

    #endregion
    
    SkeletonAnimation skeletonAnimation;
    public Spine.AnimationState spineAnimationState;
	public Spine.Skeleton skeleton;
    [SerializeField] private bool isDelay = true; // nem ngoi sao luon hoac delay 3s
    void Start()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
		spineAnimationState = skeletonAnimation.AnimationState;
		skeleton = skeletonAnimation.Skeleton;

        //saoBien.SetActive(false);
        if(isDelay){
            StartCoroutine(AttackAnimationCoroutine(3f));
        }
        else{
            StartCoroutine(AttackAnimationCoroutineNotDelay(3f));
        }
    }
    void Update()
    {
        
    }
    public void Attack(){
        spineAnimationState.SetAnimation(0, attackAnimationName, false);
    }
    IEnumerator AttackAnimationCoroutine(float delayTime){
        while(true){
            yield return new WaitForSeconds(delayTime);
            spineAnimationState.SetAnimation(0, attackAnimationName, false);
            spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
            yield return new WaitForSeconds(.2f);
            SpawnSaoBien();
        }
    }
     IEnumerator AttackAnimationCoroutineNotDelay(float delayTime){
        while(true){
            spineAnimationState.SetAnimation(0, attackAnimationName, false);
            spineAnimationState.AddAnimation(0, idleAnimationName, true, 0);
            yield return new WaitForSeconds(.2f);
            SpawnSaoBien();
            yield return new WaitForSeconds(delayTime);
        }
    }
   private  void SpawnSaoBien(){
       Instantiate(saoBienPrefab, SpawnPoint.position, SpawnPoint.rotation);
   }
}
