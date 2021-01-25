using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public AudioSource enemyDeath;
    public int EnemySpeed;

    void Start()
    {
        rb.AddForce(new Vector2(EnemySpeed,0));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemyB" || other.gameObject.tag == "Enemy")
            rb.velocity = new Vector2(rb.velocity.x * -1, rb.velocity.y);

        if (other.gameObject.tag == "Weapon")
        {
            enemyDeath.Play();
            Destroy(gameObject);
        }
            

    }

    

}
