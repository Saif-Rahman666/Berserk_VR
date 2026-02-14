using UnityEngine;
using UnityEngine.SceneManagement;

public class FallReset : MonoBehaviour
{
    public GameObject gameOverCanvas; 
    public float fallThreshold = -5f; 
    
    // Add this flag to stop the infinite loop
    private bool isGameOver = false; 

    void Start()
    {
        // 1. Force the player back to the center of the world
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        // 2. Stop any falling physics immediately
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = false; // Make sure physics is on
        }
        
        Debug.Log("Player Position Reset to Zero.");
    }
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
        isGameOver = true; 

        if (gameOverCanvas != null)
        {
            // 1. Activate the Canvas
            gameOverCanvas.SetActive(true);

            // 2. PARENT it to the Camera so it falls WITH you
            Transform head = Camera.main.transform;
            gameOverCanvas.transform.SetParent(head);

            // 3. Position it 2 meters in front of your face
            gameOverCanvas.transform.localPosition = new Vector3(0, 0, 2f);
            
            // 4. Reset rotation so it faces you directly
            gameOverCanvas.transform.localRotation = Quaternion.identity; 
        }
    }

    public void RestartGame()
    {
        Debug.Log("ATTEMPTING TO RELOAD SCENE...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}