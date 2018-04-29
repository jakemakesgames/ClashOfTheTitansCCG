using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private GameObject findGameCanvas;
    private GameObject menuCanvas;
  //  private GameObject hostGameCanvas;
    private GameObject lobbyCanvas;

    private Client client;


    [SerializeField]
    private GameObject serverPrefab;

    [SerializeField]
    private GameObject clientPrefab;

    [SerializeField]
    private GameObject textPrefab;

    private InputField chatInput;

    private GameObject chatBox;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (chatInput.text.Length > 0)
            {

                client.cmdSendLobbyChat(client.PlayerName + ": " + chatInput.text);
                AddTextToLobbyChatBox(client.PlayerName + ": " + chatInput.text);
                chatInput.text = "";
            }
        }

    }

    public void ReadyButton()
    {
        client.cmdImReady();
    }

    public void AddTextToLobbyChatBox(string a_msg)
    {
        GameObject tempText = Instantiate(textPrefab, chatBox.transform);
        tempText.GetComponent<Text>().text = a_msg;

    }


    // Use this for initialization
    void Awake()
    {
        chatBox = GameObject.Find("ScrollContent");
        chatInput = GameObject.Find("ChatInput").GetComponent<InputField>();
        findGameCanvas = GameObject.Find("FindGame");
        menuCanvas = GameObject.Find("Menu");
    //    hostGameCanvas = GameObject.Find("HostGame");
        lobbyCanvas = GameObject.Find("Lobby");
    }


    public void FindGameButton()
    {
        findGameCanvas.GetComponent<Canvas>().enabled = true;
        menuCanvas.GetComponent<Canvas>().enabled = false;
    }

    //public void HostGameButton()
    //{
    //    hostGameCanvas.GetComponent<Canvas>().enabled = true;
    //    menuCanvas.GetComponent<Canvas>().enabled = false;
    //}


    //public void StartHost()
    //{
    //    Instantiate(serverPrefab);
    //    StartCoroutine(WaitToSpawnClient());
    //    lobbyCanvas.GetComponent<Canvas>().enabled = true;
    //    hostGameCanvas.GetComponent<Canvas>().enabled = false;
    //}

    public void StartJoin()
    {
        GameObject clientGO = Instantiate(clientPrefab);
        client = clientGO.GetComponent<Client>();

        if (client.JoinServer())
        {
            lobbyCanvas.GetComponent<Canvas>().enabled = true;
            findGameCanvas.GetComponent<Canvas>().enabled = false;
        }
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                 Application.Quit();
#endif

    }

    private IEnumerator WaitToSpawnClient()
    {
        yield return new WaitForSeconds(5);
        GameObject client = Instantiate(clientPrefab);
        client.GetComponent<Client>().JoinServer();
    }


}
