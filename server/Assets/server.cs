using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System;
using System.Net;
using System.IO;

public class server : MonoBehaviour
{
    public Button start;
    public int port = 4321;

    private List<ServerClient> client;
    private List<ServerClient> dis;
    private TcpListener ser;
    private bool serverStarted;

    // Use this for initialization
    void Start()
    {
        client = new List<ServerClient>();
        dis = new List<ServerClient>();

        //startserver();
        //开启服务器
        start.onClick.AddListener(Start1);
        clients();
        //显示已连接的clients
        //刷新聊天记录
    }

    // Update is called once per frame
    void Update()
    {
        if (!serverStarted)
            return;

        foreach (ServerClient c in client)
        {
            //check if the client connected
            if (!ClientCo(c.tcp))
            {
                c.tcp.Close();
                dis.Add(c);
                continue;
            }
            //check message
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                    {
                        OnIncomingData(c, data);
                    }
                }
            }

        }
    }

    private bool ClientCo(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        catch
        {
            return false;
        }
    }

    private void startserver()
    {
        try
        {
            ser = new TcpListener(IPAddress.Any, port);
            ser.Start();

            StartLis();
            serverStarted = true;
        }
        catch (Exception e)
        {
            Debug.Log("Socket error " + e.Message);
        }

        Debug.Log("server is ready ");
    }

    private void StartLis()
    {
        ser.BeginAcceptTcpClient(ATC, ser);
    }

    private void ATC(IAsyncResult ar)
    {
        TcpListener listener = (TcpListener)ar.AsyncState;
        client.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
        StartLis();

        //send message to clients
        BroadCast(client[client.Count - 1].clientName + " has connected !", client);
    }

    private void BroadCast(string data, List<ServerClient> cl) {
        foreach (ServerClient c in cl) {
            try
            {
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch(Exception e) {
                Debug.Log("Error is :" + e.Message + "send it to client" + c.clientName);
            }
        }
    }

    private void OnIncomingData(ServerClient c, string data)
    {
        Debug.Log(c.clientName + ": \r\n" + data);

        BroadCast(c.clientName + ": \r\n" + data, client);
    }


    public void Start1()
    {
        //启动服务器
        Text server = GameObject.Find("serverstart").GetComponent<Text>();
        Text breaks = GameObject.Find("break").GetComponent<Text>();
        if (breaks.text.Equals("break server"))
        {

            breaks.text = "start server";
            server.text = "";
        }
        else
        {
            startserver();
            server.text = "server started.";
            breaks.text = "break server";
        }


        //breaked server


    }
    public void clients()
    {   /*
        if( ){
            Text clients = GameObject.Find("clients").GetComponent<Text>();
            clients.text = "clienr address";
        }*/
    }
    public void massage()
    {
        /*
         if(){
              Text massages = GameObject.Find("massages").GetComponent<Text>();
            clients.text = "chat massages"; 
         }
         */
    }
}

public class ServerClient
{
    public TcpClient tcp;
    public string clientName;
    public ServerClient(TcpClient clientSocket)
    {
        clientName = "Chen";
        tcp = clientSocket;
    }

}
