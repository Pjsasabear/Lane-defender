using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Move left only, no up or down
    // Adjustable health
    // Animations
    // When hit, generate explosion
    // Reach the end, player loses a life
    public float moveSpeed = 2f;
    public int health = 3;
    public int scoreValue = 10;
    public GameObject explosionEffect;
    public Transform endPoint;
    public float explosionLifetime = 1f;
    public float hitPauseDuration = 0.5f;

    private GameManager gameManager;
    private Animator animator;
    private bool isPaused = false;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isPaused)
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        }

        if (endPoint != null && transform.position.x <= endPoint.position.x)
        {
            if (gameManager != null)
            {
                gameManager.TakeDamage(1);
            }
            Destroy(gameObject);
        }

        if (transform.position.x < -9.25)
        {
            if (gameManager != null)
            {
                gameManager.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(collision.contacts[0].point);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerCollision();
        }
    }

    private void HandlePlayerCollision()
    {
        // Handle the collision with the player here
        if (gameManager != null)
        {
            gameManager.TakeDamage(1); // Player loses a life or similar
        }

        Die(); // Trigger the explosion and destroy the enemy
    }

    private void TakeDamage(Vector2 contactPoint)
    {
        if (gameManager != null)
        {
            gameManager.audioSource.PlayOneShot(gameManager.enemyHitSound);
        }

        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }

        health--;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(PauseMovement());
        }
    }

    private IEnumerator PauseMovement()
    {
        isPaused = true;
        yield return new WaitForSeconds(hitPauseDuration);
        isPaused = false;
    }

    private void Die()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, explosionLifetime);
        }

        if (gameManager != null)
        {
            gameManager.audioSource.PlayOneShot(gameManager.enemyDeathSound);
            gameManager.AddScore(scoreValue);
        }

        Destroy(gameObject);
    }
}