using UnityEngine;
using System.Collections;

public class ShowPanels : MonoBehaviour {

	public GameObject settingsPanel;						//Store a reference to the Game Object SettingsPanel 
	public GameObject optionsTint;							//Store a reference to the Game Object OptionsTint 
    public GameObject controlsPanel;                        //Store a reference to the Game Object ControlsPanel
    public GameObject menuPanel;							//Store a reference to the Game Object MenuPanel 
	public GameObject pausePanel;							//Store a reference to the Game Object PausePanel 

    public GameObject creditsPanel;                         //Store a reference to the Game Object CrediitsPanel 

    private bool isActive = true;

    float fadeTime = 3f;
    Color colorToFadeTo;

    #region MenuPanel
    //Call this function to activate and display the main menu panel during the main menu
    public void ShowMenu()
    {
        menuPanel.SetActive(true);
        isActive = true;
    }

    //Call this function to deactivate and hide the main menu panel during the main menu
    public void HideMenu()
    {
        menuPanel.SetActive(false);
        isActive = false;
    }
    #endregion

    #region ControlsPanel
    //Call this function to activate and display the Options panel during the main menu
    public void ShowControlsPanel()
    {
        controlsPanel.SetActive(true);
        isActive = false;
    }

    //Call this function to deactivate and hide the Options panel during the main menu
    public void HideControlsPanel()
    {
        controlsPanel.SetActive(false);
        isActive = true;
    }
    #endregion

    #region SettingsPanel
    //Call this function to activate and display the Options panel during the main menu
    public void ShowSettingsPanel()
	{
        settingsPanel.SetActive(true);
		optionsTint.SetActive(true);
	}

	//Call this function to deactivate and hide the Options panel during the main menu
	public void HideSettingsPanel()
	{
        settingsPanel.SetActive(false);
		optionsTint.SetActive(false);
	}
    #endregion

    #region PausePanel
    //Call this function to activate and display the Pause panel during game play
    public void ShowPausePanel()
	{
		pausePanel.SetActive (true);
		optionsTint.SetActive(true);
	}

	//Call this function to deactivate and hide the Pause panel during game play
	public void HidePausePanel()
	{
		pausePanel.SetActive (false);
		optionsTint.SetActive(false);

	}
    #endregion

    #region CreditsPanel
    //Call this function to activate and display the Credits panel during the main menu
    public void ShowCreditsPanel()
    {
        creditsPanel.SetActive(true);
    }

    //Call this function to deactivate and hide the Credits panel during the main menu
    public void HideCreditsPanel()
    {
        creditsPanel.SetActive(false);
    }
    #endregion
}
