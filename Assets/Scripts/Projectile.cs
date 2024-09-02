using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed; // Assuming the projectile moves in the direction it's facing
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Damage handling and bullet destruction is managed in the Enemy script now.
            Destroy(gameObject); // Destroy the bullet on collision
        }
    }

    void Update()
    {
        if (transform.position.x > 20) // or any other boundary value
        {
            Destroy(gameObject);
        }
    }
}