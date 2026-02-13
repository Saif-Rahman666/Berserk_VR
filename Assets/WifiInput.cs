using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;

public class WifiInput : MonoBehaviour
{
    // 1. Settings
    public int port = 8080; // The "Door" we are opening
    public FallReset gameManager; // Reference to your existing Restart script

    // 2. Network variables
    private TcpListener server;
    private Thread serverThread;
    private bool restartRequested = false; // The flag
    private bool isRunning = true;

    void Start()
    {
        // Start the background listener
        serverThread = new Thread(ListenForMessages);
        serverThread.IsBackground = true;
        serverThread.Start();
        Debug.Log($"[WifiInput] Server started on Port {port}. Waiting for ESP32...");
    }

    void Update()
    {
        // 3. Check the flag on the Main Thread
        if (restartRequested)
        {
            Debug.Log("RESTART SIGNAL RECEIVED!");
            if (gameManager != null)
            {
                gameManager.RestartGame();
            }
            restartRequested = false; // Reset the flag
        }
    }

    // 4. The Background Worker
    void ListenForMessages()
    {
        try
        {
            // Listen on "Any" IP address of this computer
            server = new TcpListener(IPAddress.Any, port);
            server.Start();

            while (isRunning)
            {
                // Wait for a connection (Blocking)
                if (!server.Pending()) 
                {
                    Thread.Sleep(100); // Sleep if no one is knocking
                    continue;
                }

                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead).Trim();

                Debug.Log($"[WifiInput] Message received: {message}");

                if (message == "RESTART")
                {
                    restartRequested = true;
                }

                client.Close();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log($"[WifiInput] Error: {e.Message}");
        }
    }

    void OnApplicationQuit()
    {
        isRunning = false;
        if (server != null) server.Stop();
        if (serverThread != null) serverThread.Abort();
    }
}