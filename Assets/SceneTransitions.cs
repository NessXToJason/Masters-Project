using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    /*****************************************
    * Loads the main game's scene
    *****************************************/
    public void toMainScene() {
        SceneManager.LoadScene("SampleScene");
    }

    /******************************************
    * Loads the credits scene
    ******************************************/
    public void toCreditsScene() {
        SceneManager.LoadScene("CreditsScene");
    }

    /******************************************
    * Loads the title scene
    ******************************************/
    public void toTitleScene() {
        SceneManager.LoadScene("TitleScene");
    }

    /******************************************
    * Exits the game
    ******************************************/
    public void closeGame() {
        Application.Quit();
    }
}