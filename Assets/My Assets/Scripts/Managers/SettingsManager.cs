using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour 
{
	public AudioMixer audioMixer;

	[Header("Resolution Variables")]
	public Dropdown resolutionDropdown;
	Resolution[] resolutions;

	void Start()
	{
		resolutions = Screen.resolutions;

		resolutionDropdown.ClearOptions ();

		List<string> options = new List<string>();

		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++) 
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add (option);

			if (resolutions[i].width == Screen.currentResolution.width &&
				resolutions[i].height == Screen.currentResolution.height) 
			{
				currentResolutionIndex = i;
			}
		}

		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();
	}


	// Function to Set the Master Volume // 
	public void SetVolume (float volume)
	{
		Debug.Log("Volume: " + volume);

		audioMixer.SetFloat ("volume", volume);
	}

	// Function to Set the Quality Settings //
	public void SetQuality (int qualityIndex)
	{
		QualitySettings.SetQualityLevel (qualityIndex);
	}

	// Function to Toggle Fullscreen //
	public void SetFullscreen (bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}

	// Function to Set the Resolution //
	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions [resolutionIndex];
		Screen.SetResolution (resolution.width, resolution.height, Screen.fullScreen);
	}

}
