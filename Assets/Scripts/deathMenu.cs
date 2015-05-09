/*
 * 
 * ------------------------------- deathMenu.cs -------------------------------
 * 
 * Purpose:     To manage the death menu. 
 * 
 * Created By:  Chase Perdue
 * Date:        5/8/15
 * Class:       CS485 - Game Programming
 * 
 * Changelog:
 *      + 5/8/15 ------ Initial version. - Chase Perdue
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class deathMenu : MonoBehaviour {

	//public Canvas deathtMenu;
	public Button restartButton;
	public Button quitButton;
	
	// Use this for initialization
	void Start () {
		//deathtMenu = deathtMenu.GetComponent<Canvas>();
		restartButton = restartButton.GetComponent<Button>();
		quitButton = quitButton.GetComponent<Button>();
		//quitMenu.enabled = false;
	}

	
	/// <summary>
	/// Function used by the Restart button to restart the game. The "2" in the LoadLevel 
	/// function refers to the scene number. Change this to whatever the first level 
	/// is.
	/// </summary>
	public void RestartGame()
	{
		Application.LoadLevel(1);
	}
	
	/// <summary>
	/// Function used by the Quit button within the quit menu to exit the game.
	/// </summary>
	public void ExitGame()
	{
		Application.Quit();
	}
}
