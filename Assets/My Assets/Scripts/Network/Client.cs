using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ServerClient
{
    public ServerClient(int a_ID, string a_name)
    {
        connectionID = a_ID;
        name = a_name;
        ready = false;
    }

    public int connectionID;
    public string name;
    public bool ready;

}


public class Client : MonoBehaviour
{

    private static Client instance;

    private GameObject findGameCanvas;
    private int connectionID;
    private int hostID;
    //private int webHost;
    private int reliableChannel;
    private int unreliablechannel;
    private string ip;

    private int port = 5701;

    //private float connectionTime;
    private bool isConnected = false;

    //private bool isStarted = false;

    private byte error;

    private string playerName;

    public string PlayerName
    {
        get
        {
            return playerName;
        }
    }

    private float gameStartCountDown;


    private int ourClientID;

    ServerClient me;

    ServerClient other;

    private UIManager uim;

    private Text countDownText;



    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

        NetworkTransport.Init();
        findGameCanvas = GameObject.Find("FindGame");
        uim = FindObjectOfType<UIManager>();
        countDownText = GameObject.Find("CountDownText").GetComponent<Text>();
        gameStartCountDown = 4;
    }

    public bool JoinServer()
    {


        playerName = findGameCanvas.transform.GetChild(1).GetChild(2).GetComponent<Text>().text;


        ip = findGameCanvas.transform.GetChild(2).GetChild(2).GetComponent<Text>().text;



        if (ip == "")
        {
            Debug.Log("you must have a server ip");
            return false;
        }


        if (playerName == "")
        {
            Debug.Log("you must have a name");
            return false;
        }


        ////turns on the lobby canvas 
        //GameObject.Find("Lobby").GetComponent<Canvas>().enabled = true;

        //sets up a new conncetion config
        ConnectionConfig config = new ConnectionConfig();

        //reliable uses tcp to pass packets meaning it is reliable but slow
        reliableChannel = config.AddChannel(QosType.ReliableSequenced);

        //unreliable uses udp to pass packets meaning its unreliable but fast
        unreliablechannel = config.AddChannel(QosType.Unreliable);

        //sets up a topology
        HostTopology topo = new HostTopology(config, 3);

        //opens a socket on the trasport layer to send packets though
        hostID = NetworkTransport.AddHost(topo, 0);

        //sets the error to OK
        error = (byte)NetworkError.Ok;

        //sets up the connection id of the cl
        connectionID = NetworkTransport.Connect(hostID, ip, port, 0, out error);

        //if the is an error debug it
        if (error != (byte)NetworkError.Ok)
        {
            Debug.LogError("Network error is occurred: " + (NetworkError)error);
            SceneManager.LoadScene(0);
            return false;
        }

        //the client is connected
        isConnected = true;


        Debug.Log("[Client] Connected");

        return true;
    }

    void Update()
    {
        if (isConnected == true)
        {

            int recHostId;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            byte error;


            //recive a packet
            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);

            //check to see if there was an error
            if (error != (byte)NetworkError.Ok)
            {
                Debug.LogError("Network error is occurred: " + (NetworkError)error);
                SceneManager.LoadScene(0);
                // GameObject go = Instantiate(new GameObject(), new Vector3(500f, 500f, 0f), Quaternion.identity);
                //  go.AddComponent<TextMesh>().text = "NETWORK ERROR" + (NetworkError)error;

            }

            //check what type of packet you have recived
            switch (recData)
            {
                //recived data
                case NetworkEventType.DataEvent:
                    //turn the packet from unicode to string
                    string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    Debug.Log("[CLIENT" + connectionID + "] I Receiving: " + msg);
                    //call the command controller
                    cmdController(msg);
                    break;
            }

            //check to see if there was an error
            if (error != (byte)NetworkError.Ok)
            {
                Debug.LogError("Network error is occurred: " + (NetworkError)error);
                SceneManager.LoadScene(0);
            }

            if (countDownText != null)
            {
                if (me != null && other != null)
                {
                    if (me.ready && other.ready)
                    {
                        gameStartCountDown -= Time.deltaTime;
                        if (gameStartCountDown > 0)
                        {

                            if (countDownText.enabled != true)
                            {
                                countDownText.enabled = true;
                            }
                            countDownText.text = Mathf.Floor(gameStartCountDown).ToString();
                        }
                        else
                        {
                            //load the new scene
                            SceneManager.LoadScene(1);
                            gameStartCountDown = 4;
                        }
                    }
                    else
                    {
                        if (gameStartCountDown != 4)
                        {
                            countDownText.enabled = false;
                            gameStartCountDown = 4;
                        }
                    }
                }
            }
        }
    }

    private void cmdController(string cmd)
    {

        string[] splitData;
        splitData = cmd.Split('|');

        switch (splitData[0])
        {
            case "rpcASKNAME":
                cmdAskName(splitData);
                break;
            case "rpcCNN":
                SpawnServerPlayer(splitData[1], int.Parse(splitData[2]));
                break;
            case "rpcRECIVELOBBYMSG":
                uim.AddTextToLobbyChatBox(splitData[2]);
                break;
            case "rpcREADY":
                rpcReady();
                break;
            case "cmdDC":
                //  PlayerDisconnected(int.Parse(splitData[1]));
                break;
            //case "rpcSENDCOMMAND":
            //      (splitData);
            //  break;
            case "cmdCLIENTREADY":
                // SetClientReady(int.Parse(splitData[1]));
                break;
            case "cmdSTARTGAME":
                // cmdStartGame(splitData);
                break;

        }
    }

    public void cmdSendCommand(Command command)
    {
        string msg = "cmdSENDCOMMAND|" + command.ToString();

        Send(msg, reliableChannel);

    }

    public void cmdImReady()
    {
        if (me != null)
        {
            me.ready = !me.ready;
            if (other != null)
            {
                if (me.ready == true)
                {
                    GameObject.Find("MyUsername").GetComponent<Text>().color = Color.green;
                }
                else
                {
                    GameObject.Find("MyUsername").GetComponent<Text>().color = Color.red;
                }
            }
            else
            {
                return;
            }


            Send("cmdREADY|" + ourClientID.ToString(), reliableChannel);
        }
    }

    private void rpcReady()
    {
        if (other != null)
        {
            other.ready = !other.ready;
            if (other.ready == true)
            {
                GameObject.Find("OtherUsername").GetComponent<Text>().color = Color.green;
            }
            else
            {
                GameObject.Find("OtherUsername").GetComponent<Text>().color = Color.red;
            }
        }
    }

    public void cmdSendLobbyChat(string a_msg)
    {
        string msg = "cmdSENDLOBBYMSG|" + ourClientID.ToString() + "|" + a_msg;

        Send(msg, reliableChannel);
    }

    private void cmdAskName(string[] cmd)
    {
        //this for the lobby
        GameObject.Find("MyUsername").GetComponent<Text>().text = playerName;
        // set the clients ID
        ourClientID = int.Parse(cmd[1]);

        //send the our name back to the server
        Send("cmdNAMEIS|" + playerName, reliableChannel);

        me = new ServerClient(ourClientID, playerName);

        // Create All Other Players
        for (int i = 2; i < cmd.Length - 1; i++)
        {
            string[] d = cmd[i].Split('%');
            SpawnServerPlayer(d[0], int.Parse(d[1]));
        }
    }




    private void SpawnServerPlayer(string a_playerName, int cnnID)
    {
        if (cnnID != ourClientID)
        {
            other = new ServerClient(cnnID, a_playerName);
            GameObject.Find("OtherUsername").GetComponent<Text>().text = other.name;
        }


    }

    private void Send(string message, int channelID)
    {
        Debug.Log("[CLIENT" + connectionID.ToString() + "] Sending This To Server: " + message);
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostID, connectionID, channelID, msg, message.Length * sizeof(char), out error);
        if (error != (byte)NetworkError.Ok)
        {
            Debug.LogError("Network error is occurred: " + (NetworkError)error);
            SceneManager.LoadScene(0);
        }
    }

}
