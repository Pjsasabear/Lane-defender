using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Move up and down (both arrow keys and WS keys)
    // Player fires a bullet
    // When player fires a bullet, generate explosion on the barrel
    // Gun firing delay
    public float moveSpeed = 5f; // Speed the player moves
    public float boundaryY = 4.5f; // Vertical boundary offset from the center
    public float boundaryYCenter = 0f; // Center of the vertical boundary
    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject explosionPrefab;
    public float bulletForce = 10f;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;
    public float explosionLifetime = 1f; // Time in seconds before explosion is destroyed

    private Vector2 moveInput;
    private bool isMoving = false;
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Move"].started += OnMoveStarted;
        playerInput.actions["Move"].canceled += OnMoveCanceled;
        playerInput.actions["Shoot"].performed += OnFire;
        playerInput.actions["Restart"].performed += OnRestart;
    }

    private void OnMoveStarted(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        isMoving = true;
        Debug.Log($"Move Input Started: {moveInput}");
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
        isMoving = false;
        Debug.Log($"Move Input Canceled: {moveInput}");
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            // Player movement
            Vector2 playerMovement = new Vector2(0f, moveInput.y * moveSpeed * Time.fixedDeltaTime);
            Vector2 newPosition = rb.position + playerMovement;

            // Ensure player stays within vertical boundary
            float clampedY = Mathf.Clamp(newPosition.y, boundaryYCenter - boundaryY, boundaryYCenter + boundaryY);
            newPosition.y = clampedY;

            rb.MovePosition(newPosition);
        }
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RestartGame();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        rbBullet.AddForce(Vector2.right * bulletForce, ForceMode2D.Impulse);

        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, firePoint.position, Quaternion.identity);
            Destroy(explosion, explosionLifetime); // Destroy the explosion
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}