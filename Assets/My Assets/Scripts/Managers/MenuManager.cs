using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
	// -----> THIS IS ONLY A TEMP MENU SCRIPT <----- \\

	public string sceneToPlay;
	public GameObject optionsCanvas;
	public GameObject menuCanvas;

	// Set the correct Menu Canvas' active when the game runs, even if they are accidentally turned off in the editor //
	void Awake()
	{
		menuCanvas.SetActive (true);
		optionsCanvas.SetActive (false);
	}

	// Loads the sceneToPlay //
	public void Play()
	{
		SceneManager.LoadScene (sceneToPlay);
	}

	// Shows the Options Menu Canvas and hides the main menu // 
	public void Options()
	{
		optionsCanvas.SetActive (true);
		menuCanvas.SetActive (false);

	}

	public void Quit()
	{
		// If the game is running in a standalone build of the game //
		#if UNITY_STANDALONE
		// Quit the Application //
		Application.Quit();
		#endif

		// If the game is running in the editor //
		#if UNITY_EDITOR
		// Stop playing the scene //
		UnityEditor.EditorApplication.isPlaying = false;
		#endif
	}

	public void Back()
	{
		optionsCanvas.SetActive (false);
		menuCanvas.SetActive (true);
	}

}
