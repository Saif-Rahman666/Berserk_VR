using UnityEngine;
using UnityEngine.SceneManagement;

public class FallReset : MonoBehaviour
{
    public GameObject gameOverCanvas; // Drag your canvas here
    public float fallThreshold = -5f; // How low can you fall?

    void Update()
    {
        // 1. Check if we fell below the threshold
        if (transform.position.y < fallThreshold)
        {
            ShowGameOver();
        }
    }

    void ShowGameOver()
    {
        // Turn on the UI
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            
            // Optional: Move the UI in front of the player's face so they can see it
            // (Simply parenting it to the camera or teleporting it works best here)
            gameOverCanvas.transform.position = transform.position + transform.forward * 2f + Vector3.up * 1f;
            gameOverCanvas.transform.LookAt(transform.position);
            gameOverCanvas.transform.Rotate(0, 180, 0); // Flip it to face the player
        }
    }

    // Call this function from the Button
    public void RestartGame()
    {
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}