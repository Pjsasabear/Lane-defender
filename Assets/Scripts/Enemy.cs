using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //move left only, no up or down
    //adjustable health
    //animations
    //when hit, generate explosion
    //reach the end, player loses a life
    public float moveSpeed = 2f; // Speed at which the enemy moves left
    public int health = 3; // Adjustable health for the enemy
    public GameObject explosionEffect; // Explosion effect to instantiate when hit
    public Transform endPoint; // The point where the enemy will cause the player to lose a life
    public Transform[] spawnPoints; // Array of spawn points for the enemies

    private void Start()
    {
        // Check if there are any spawn points assigned
        if (spawnPoints.Length > 0)
        {
            // Select a random spawn point from the array
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            // Set the position of the enemy to the selected spawn point
            transform.position = randomSpawnPoint.position;
        }
    }

    private void Update()
    {
        // Move the enemy left
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        // Check if the enemy has reached end point
        if (endPoint != null && transform.position.x <= endPoint.position.x)
        {
            Destroy(gameObject);
        }

        if (transform.position.x < -20)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the enemy was hit by a projectile or other object
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        health--;

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Instantiate explosion effect at the enemy's position
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // Destroy the enemy
        Destroy(gameObject);
    }
}