using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{
    private Socket clientSocket;
    [SerializeField]
    private string serverIP = "172.26.90.110";  // 目标服务器的IP地址
    [SerializeField]
    private int serverPort = 8888;         // 目标服务器的端口

    void Start()
    {
        // 启动连接到服务器
        ConnectToServer();
    }

    // 连接到服务器
    void ConnectToServer()
    {
        try
        {
            // 创建一个Socket，指定地址族、套接字类型和协议类型
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 服务器的IP地址和端口
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

            // 尝试连接到服务器
            clientSocket.Connect(serverEndPoint);
            Debug.Log("Connected to server!");

            // 发送消息
            SendMessageToServer("Explain_goal");

            // 开始接收消息
            ReceiveMessageFromServer();
        }
        catch (Exception ex)
        {
            Debug.LogError("Error connecting to server: " + ex.Message);
        }
    }

    // 发送消息到服务器
    void SendMessageToServer(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(data);
            Debug.Log("Message sent to server: " + message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Error sending message: " + ex.Message);
        }
    }

    // 接收从服务器返回的消息
    void ReceiveMessageFromServer()
    {
        try
        {
            byte[] buffer = new byte[1024]; // 创建接收缓存区
            int receivedBytes = clientSocket.Receive(buffer); // 接收数据

            if (receivedBytes > 0)
            {
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, receivedBytes);
                Debug.Log("Message received from server: " + receivedMessage);
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error receiving message: " + ex.Message);
        }
    }

    // 关闭Socket连接
    void OnApplicationQuit()
    {
        if (clientSocket != null)
        {
            clientSocket.Close();
            Debug.Log("Connection closed.");
        }
    }
}