using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
	public string sceneToPlay;
	public GameObject optionsCanvas;

	void Awake()
	{
		optionsCanvas.SetActive (false);
	}

	public void Play()
	{
		SceneManager.LoadScene (sceneToPlay);
	}

	public void Options()
	{
		optionsCanvas.SetActive (true);
	}

	public void Quit()
	{
		Application.Quit ();
	}

	public void Back()
	{
		optionsCanvas.SetActive (false);
	}

}
