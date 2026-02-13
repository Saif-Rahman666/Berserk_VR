using UnityEngine;
using UnityEngine.SceneManagement;

public class FallReset : MonoBehaviour
{
    public GameObject gameOverCanvas; 
    public float fallThreshold = -5f; 
    
    // Add this flag to stop the infinite loop
    private bool isGameOver = false; 

    void Update()
    {
        // Only run if the game is NOT over yet
        if (!isGameOver && transform.position.y < fallThreshold)
        {
            ShowGameOver();
        }
    }

    void ShowGameOver()
    {
        isGameOver = true; // Lock it so it doesn't happen again

        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            
            // Move it once, then let it stay still so we can click it!
            gameOverCanvas.transform.position = transform.position + transform.forward * 2f + Vector3.up * 1f;
            gameOverCanvas.transform.LookAt(transform.position);
            gameOverCanvas.transform.Rotate(0, 180, 0); 
        }
    }

    public void RestartGame()
    {
        Debug.Log("ATTEMPTING TO RELOAD SCENE...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}