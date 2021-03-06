﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneReloader: MonoBehaviour {

	public string mainMenu;

    public void ReloadScene()
    {
        // Command has some static members, so let`s make sure that there are no commands in the Queue
        Debug.Log("Scene reloaded");
        // reset all card and creature IDs
        IDFactory.ResetIDs();
        IDHolder.ClearIDHoldersList();
        Command.CommandQueue.Clear();
        Command.CommandExecutionComplete();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	public void ReturnToMenu()
	{
		Debug.Log ("Returning to Main Menu");
        // reset all card and creature IDs
        IDFactory.ResetIDs();
        IDHolder.ClearIDHoldersList();
        Command.CommandQueue.Clear();
        Command.CommandExecutionComplete();
        // probably find a better way to do this later
		SceneManager.LoadScene (mainMenu);
	}
}
