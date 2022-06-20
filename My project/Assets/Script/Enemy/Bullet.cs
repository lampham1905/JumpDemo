using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyBullet());
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(-3, 0);
    }
     private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "DestroyBullet")
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
    }
    IEnumerator DestroyBullet(){
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    
}
