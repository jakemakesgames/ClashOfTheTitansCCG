using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour 
{

	public string menuName;

	public GameObject pauseCanvas;
	public bool isPaused;

	void Awake()
	{
		pauseCanvas.SetActive (false);
	}

	public void Start()
	{
		isPaused = false;
	}

	public void Update()
	{
		if (Input.GetButtonDown ("Cancel")) 
		{
			DoPause ();
		}
		else if (Input.GetButtonDown ("Cancel") && isPaused == true) 
		{
			UnPause ();
		}
	}

	public void DoPause()
	{
		isPaused = true;
		Time.timeScale = 0;
		pauseCanvas.SetActive (true);
	}

	public void UnPause()
	{
		isPaused = false;
		Time.timeScale = 1;
		pauseCanvas.SetActive (false);
	}

	public void ReturnToMenu()
	{
		SceneManager.LoadScene (menuName);
	}
}
