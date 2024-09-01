using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
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
            Destroy(gameObject); // Destroy the projectile on collision
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