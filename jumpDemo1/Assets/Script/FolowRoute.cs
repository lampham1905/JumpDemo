using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolowRoute : MonoBehaviour
{
    [SerializeField]
    private Transform[] routes;

    private int routeToGo;

    private float tParam;

    private Vector2 objectPosition;

    private float speedModifier;

    private bool coroutineAllowed;
    public  Transform Player;
    public Transform point1;
    public Transform point2;
    //public Transform endPoint;

    public static FolowRoute ins;
     private void Awake() {
         ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.5f;
        coroutineAllowed = true;   
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    private IEnumerator GoByTheRoute(int routeNum)
    {
        coroutineAllowed = false;

        Vector2 p0 = Player.position;
        Vector2 p1 = point1.position;
        Vector2 p2 = point2.position;
        Vector2 p3 = GameManager.instance.endPointCurrent.transform.position;

        while(tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier * 2f;
            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;
            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;

        routeToGo += 1;

        if(routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineAllowed = true;
    }
    public void JumpToEnd(){
         if (coroutineAllowed)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
    }
}
