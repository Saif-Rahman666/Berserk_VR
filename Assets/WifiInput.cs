using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;

public class WifiInput : MonoBehaviour
{
    public int port = 8080;
    
    // Networking
    private TcpListener server;
    private Thread serverThread;
    private bool restartRequested = false; 
    private bool isRunning = true;
    
    // Reference to the script that works
    private FallReset gameManager;

    void Start()
    {
        // Find the script automatically
        gameManager = FindObjectOfType<FallReset>();
        
        serverThread = new Thread(ListenForMessages);
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    void Update()
    {
        if (restartRequested)
        {
            // If we lost the link, find it again
            if (gameManager == null) gameManager = FindObjectOfType<FallReset>();

            if (gameManager != null)
            {
                Debug.Log("[WifiInput] Triggering FallReset.RestartGame()...");
                // Use the EXACT same function the UI button uses
                gameManager.RestartGame();
            }
            
            restartRequested = false; 
        }
    }

    void ListenForMessages()
    {
        try
        {
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            while (isRunning)
            {
                if (!server.Pending()) 
                {
                    Thread.Sleep(100);
                    continue;
                }

                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();

                if (message == "RESTART")
                {
                    restartRequested = true;
                }
                client.Close();
            }
        }
        catch (System.Exception e) { }
    }

    void OnDestroy()
    {
        isRunning = false;
        if (server != null) server.Stop();
        if (serverThread != null) serverThread.Abort();
    }
}