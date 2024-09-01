using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // For scene management
using TMPro; // For TextMesh Pro UI elements
using System.IO; // For file operations

public class GameManager : MonoBehaviour
{
    // Life system
    // Enemy spawning
    // Audio
    // High score
    // Saving system
    // Public variables for easy modification in the Unity Inspector
    public int totalLives = 3; // Starting number of lives
    public GameObject[] enemyPrefabs; // Array of enemies for spawning
    public Transform[] spawnPoints; // Array of spawn points for enemies
    public float enemySpawnInterval = 2.0f; // Time between the enemy spawning
    public TMP_Text scoreText; // UI to display the score
    public TMP_Text livesText; // UI text element to display remaining lives
    public AudioSource audioSource; // AudioSource for playing sounds
    public AudioClip gameOverSound; // Sound to play when game is over
    public AudioClip spawnSound; // Sound to play when an enemy spawns
    public TMP_Text highScoreText; // UI text element to display high score

    private int score = 0; // Player's current score
    private int lives; // Player's remaining lives
    private int highScore; // High score
    private string saveFilePath; // File path for saving high score data
    private bool isGameActive = true;


    private void Start()
    {
        // Initialize lives and load high score
        lives = totalLives;
        saveFilePath = Path.Combine(Application.persistentDataPath, "highscore.txt");
        LoadHighScore();

        // Update UI
        UpdateScoreText();
        UpdateLivesText();
        UpdateHighScoreText();

        // Start spawning enemies
        StartCoroutine(SpawnEnemies());
    }

    private void Update()
    {

    }

    // enemy spawning
    private IEnumerator SpawnEnemies()
    {
        while (isGameActive)
        {
            GameObject enemyPrefab = GetRandomEnemyPrefab();
            Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
            audioSource.PlayOneShot(spawnSound);
            yield return new WaitForSeconds(enemySpawnInterval);
        }
    }

    // Return a random spawn position from the array of spawn points
    private Vector3 GetRandomSpawnPosition()
    {
        if (spawnPoints.Length == 0) return Vector3.zero; // No spawn points available

        int randomIndex = Random.Range(0, spawnPoints.Length);
        return spawnPoints[randomIndex].position;
    }

    // Select a random enemy prefab from the array
    private GameObject GetRandomEnemyPrefab()
    {
        if (enemyPrefabs.Length == 0) return null; // No prefabs available
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[randomIndex];
    }

    // Restart the game
    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Handle player taking damage
    public void TakeDamage(int damage)
    {
        lives -= damage;
        UpdateLivesText();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    // End the game and save the high score
    private void GameOver()
    {
        audioSource.PlayOneShot(gameOverSound);
        SaveHighScore();
        // Show game over screen or restart the game
        RestartGame();
    }

    // Update the score and UI
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    // Update the score text UI
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // Update the lives text UI
    private void UpdateLivesText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + lives;
        }
    }

    // Update the high score text UI
    private void UpdateHighScoreText()
    {
        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    // Save the current high score to a file
    private void SaveHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            File.WriteAllText(saveFilePath, highScore.ToString());
        }
    }

    // Load the high score from a file
    private void LoadHighScore()
    {
        if (File.Exists(saveFilePath))
        {
            int.TryParse(File.ReadAllText(saveFilePath), out highScore);
        }
    }
}