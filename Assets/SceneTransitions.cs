using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private bool pause = false;

    /*****************************************
    * Loads the main game's scene
    *****************************************/
    public void toMainScene() {
        SceneManager.LoadScene("MainScene");
    }

    /*****************************************
    * Loads the hole scene
    *****************************************/
    public void toHoleScene() {
        SceneManager.LoadScene("HoleScene");
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

    public void togglePause() {
        pause = !pause;
    }

    public bool isPaused() {
        return pause;
    }

    /******************************************
    * Exits the game
    ******************************************/
    public void closeGame() {
        Application.Quit();
    }
}