using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;




public class Server : MonoBehaviour
{

    [SerializeField]
    private GameObject textPrefab;


    private GameObject console;

    [SerializeField]
    private int maxConnections;

    [SerializeField]
    private string serverIPAddress;

    public string ServerIPAddress
    {
        get
        {
            return serverIPAddress;
        }
    }



    [SerializeField]
    private int port = 5701;

    private int hostID;

    //  private int webHost;
    private int reliableChannel;
    private int unreliablechannel;

    private bool isStarted = false;
    private byte error;

    private List<ServerClient> clients = new List<ServerClient>();


    // Use this for initialization
    void Start()
    {
        console = GameObject.Find("ScrollContent");

        NetworkTransport.Init();
        ConnectionConfig config = new ConnectionConfig();

        reliableChannel = config.AddChannel(QosType.ReliableSequenced);
        unreliablechannel = config.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(config, maxConnections);

        hostID = NetworkTransport.AddHost(topo, port, null);
        //  webHost = NetworkTransport.AddWebsocketHost(topo, port, null);

        serverIPAddress = LocalIPAddress();

        isStarted = true;

        PrintToConsole("Server Started");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStarted)
        {
            return;
        }

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        //List<object> conncetionIdArr = new List<object>();


        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        // conncetionIdArr.Add(connectionId);
        switch (recData)
        {
            case NetworkEventType.ConnectEvent:
                PrintToConsole("[SERVER] Client Connected with ID: " + connectionId);
                OnClientConnected(connectionId);
                break;
            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                PrintToConsole("[SERVER] Client With ID: " + connectionId + " Said: " + msg);
                cmdController(msg, connectionId);
                break;
            case NetworkEventType.DisconnectEvent:
                PrintToConsole("[SERVER] Client Disconnected with ID: " + connectionId);
                OnClientDisconnected(connectionId);
                break;
        }

    }

    private void OnClientConnected(int id)
    {

        clients.Add(new ServerClient(id, "Loading..."));

        string msg = "rpcASKNAME|" + id + "|";

        foreach (ServerClient sc in clients)
        {
            msg += sc.name + '%' + sc.connectionID + '|';
        }

        msg = msg.Trim('|');

        Send(msg, reliableChannel, id);

    }

    private void OnClientDisconnected(int cnnID)
    {
        //string msg = "cmdDC|" + cnnID;
        //Send(msg, reliableChannel, clients);
        //clients.Remove(clients.Find(x => x.connectionID == cnnID));
    }


    private void cmdController(string cmd, int cnnID)
    {
        string[] splitData;
        splitData = cmd.Split('|');

        switch (splitData[0])
        {

            case "cmdNAMEIS":
                rpcNameIs(cnnID, splitData[1]);
                break;
            case "cmdSENDLOBBYMSG":
                rpcSendMsgToOtherClient(int.Parse(splitData[1]), splitData[2]);
                break;
            case "cmdREADY":
                 rpcReady(int.Parse(splitData[1]));
                break;

        }

    }


    private void rpcSendMsgToOtherClient(int cnnID, string msg)
    {
        foreach(ServerClient c in clients)
        {
            if(c.connectionID != cnnID)
            {
                Send("rpcRECIVELOBBYMSG|" + cnnID.ToString() + "|" + msg, reliableChannel, c.connectionID);
            }
        }
    }

    private void rpcReady(int cnnID)
    {
        foreach (ServerClient c in clients)
        {
            if (c.connectionID != cnnID)
            {
                Send("rpcREADY|" + cnnID.ToString(), reliableChannel, c.connectionID);
            }
        }
    }

    public void rpcNameIs(int cnnID, string playerName)
    {

        clients.Find(x => x.connectionID == cnnID).name = playerName;

        Send("rpcCNN|" + playerName + '|' + cnnID, reliableChannel, clients);

    }


    public void Send(string message, int channelID, int cnnID)
    {
        List<ServerClient> c = new List<ServerClient>();
        c.Add(clients.Find(x => x.connectionID == cnnID));
        Send(message, channelID, c);
    }

    public void Send(string message, int channelID, List<ServerClient> c)
    {
        PrintToConsole("[SERVER] Sending All Clients: " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);

        foreach (ServerClient sc in c)
        {
            NetworkTransport.Send(hostID, sc.connectionID, channelID, msg, message.Length * sizeof(char), out error);
        }
        if (error != (byte)NetworkError.Ok)
        {
            Debug.Log((NetworkError)error);
        }
    }

    private string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }

    private void PrintToConsole(string a_msg)
    {
        Debug.Log(a_msg);
        GameObject serverText = Instantiate(textPrefab, console.transform);
        serverText.GetComponent<Text>().text = a_msg;
    }
}
