/*
 * 
 * ------------------------------- menuScript.cs -------------------------------
 * 
 * Purpose:     To manage the Main menu and Exit menu functionality. 
 * 
 * Created By:  Chase Perdue
 * Date:        4/20/15
 * Class:       CS485 - Game Programming
 * 
 * Changelog:
 *      + 4/20/15 ------ Initial version. - Chase Perdue
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuScript : MonoBehaviour {

    public Canvas quitMenu;
    public Button startButton;
    public Button exitButton;

	// Use this for initialization
	void Start () {
        quitMenu = quitMenu.GetComponent<Canvas>();
        startButton = startButton.GetComponent<Button>();
        exitButton = exitButton.GetComponent<Button>();
        quitMenu.enabled = false;
	}

    /// <summary>
    /// When exit button is pressed, main menu buttons are disabled and quit menu 
    /// is enabled.
    /// </summary>
    public void ExitPress()
    {
        quitMenu.enabled = true;
        startButton.enabled = false;
        exitButton.enabled = false;
    }

    /// <summary>
    /// When the quit menu is canceled out of with the No button, the main menu is
    /// re-enabled.
    /// </summary>
    public void NoPress()
    {
        quitMenu.enabled = false;
        startButton.enabled = true;
        exitButton.enabled = true;
    }

    /// <summary>
    /// Function used by the Play button to start the game. The "1" in the LoadLevel 
    /// function refers to the scene number. Change this to whatever the first level 
    /// is.
    /// </summary>
    public void StartLevel()
    {
        Application.LoadLevel(1);
    }

    /// <summary>
    /// Function used by the Yes button within the quit menu to exit the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

}
