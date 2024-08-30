using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //move up and down to grid points (both arrow keys and WS keys)
    //Player fires a bullet
    //when player fires a bullet, generate explosion on the barrel
    //gun firing delay
    public float moveSpeed = 5f;
    public float boundaryY = 3.5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject explosionPrefab; // Add an explosion prefab
    public float bulletForce = 10f;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;
    public Transform[] lanePoints; // Array to store the lane positions

    private int currentLane = 2; // Start at the middle lane (index 2)

    private Vector2 moveInput;
    private Rigidbody2D rb;
    private PlayerInput playerInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Shoot"].performed += OnFire;
        playerInput.actions["Restart"].performed += OnRestart;

        //make it so the player starts at the correct lane
        transform.position = new Vector2(transform.position.x, lanePoints[currentLane].position.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();

        if (moveInput.y > 0)
        {
            MoveUp();
        }
        else if (moveInput.y < 0)
        {
            MoveDown();
        }
    }

    private void MoveUp()
    {
        if (currentLane > 0) // Move up only if not at the top lane
        {
            currentLane--;
            MoveToCurrentLane();
        }
    }

    private void MoveDown()
    {
        if (currentLane < lanePoints.Length - 1) // Move down only if not at the bottom lane
        {
            currentLane++;
            MoveToCurrentLane();
        }
    }

    private void MoveToCurrentLane()
    {
        // Smoothly move to the target lane
        Vector2 targetPosition = new Vector2(transform.position.x, lanePoints[currentLane].position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
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

    private void FixedUpdate()
    {
        Vector2 movement = moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Restrict player to boundaries and snap to grid points
        float clampedY = Mathf.Clamp(rb.position.y, -boundaryY, boundaryY);
        clampedY = Mathf.Round(clampedY); // Snap to grid points
        rb.position = new Vector2(rb.position.x, clampedY);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
        rbBullet.AddForce(Vector2.right * bulletForce, ForceMode2D.Impulse);

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, firePoint.position, Quaternion.identity);
        }
    }

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}