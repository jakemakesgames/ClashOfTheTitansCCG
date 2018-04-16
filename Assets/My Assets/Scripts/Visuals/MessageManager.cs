using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour 
{
    public Text MessageText;
    public GameObject MessagePanel;

    public static MessageManager Instance;

    void Awake()
    {
        Instance = this;
        MessagePanel.SetActive(false);
    }

    public void ShowMessage(string Message, float Duration, Command com)
    {
        StartCoroutine(ShowMessageCoroutine(Message, Duration, com));
    }

    IEnumerator ShowMessageCoroutine(string Message, float Duration, Command com)
    {
        //Debug.Log("Showing some message. Duration: " + Duration);
        MessageText.text = Message;
        MessagePanel.SetActive(true);

        yield return new WaitForSeconds(Duration);

        // FADE THE PANEL OUT //

        MessagePanel.SetActive(false);
        Command.CommandExecutionComplete();
    }

    // DEBUGGING ONLY - DELETE THE UPDATE FUNCTION LATER //
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ShowMessage("Your Turn", 3f);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ShowMessage("Enemy Turn", 3f);
        }
    }
    */
}
