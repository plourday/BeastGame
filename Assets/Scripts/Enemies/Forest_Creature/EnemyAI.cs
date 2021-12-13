using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
   [SerializeField] private BoxCollider2D boxCollider;
   [SerializeField] private LayerMask PlayerLayer;
    [SerializeField]private float range;
    private float damage = .1f;
    private Health playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        
    }

    //range needs to be negative as the sprite's scale is 1 while facing left this will cause the boxcast to face the wrong direction otherwise
    private bool PlayerInSight() 
    {
      //  Physics2D.BoxCast();
        return false;
    }

    private void OnDrawGizmos() 
    {
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerHealth = collision.transform.GetComponent<Health>();
            DamagePlayer();
        }
    }


    private void DamagePlayer() 
    {
        playerHealth.TakeDamage(damage);
    }
}
