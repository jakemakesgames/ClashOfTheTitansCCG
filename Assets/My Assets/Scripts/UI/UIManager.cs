using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    private GameObject findGameCanvas;
    private GameObject menuCanvas;
    private GameObject hostGameCanvas;
    private GameObject lobbyCanvas;

    [SerializeField]
    private GameObject serverPrefab;

    [SerializeField]
    private GameObject clientPrefab;

    // Use this for initialization
    void Start()
    {
        findGameCanvas = GameObject.Find("FindGame");
        menuCanvas = GameObject.Find("Menu");
        hostGameCanvas = GameObject.Find("HostGame");
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
        GameObject client = Instantiate(clientPrefab);
        if (client.GetComponent<Client>().JoinServer())
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
