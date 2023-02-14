using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/**********************************************************************
* A class that manages and controls the player in an ET-like game.
*
* Jason
**********************************************************************/
public class PlayerController : MonoBehaviour
{
    private BoxCollider2D hitbox;

    /* LIFE SYSTEM */
    private static float energy;
    private int lives;

    /* STATS */
    private float moveSpeed;
    private float speedMod;
    private bool captured;
    private int struggleAmt;

    /* INVENTORY */
    private int phonePieces;
    private int reesesPieces;

    /* ABILITY COOLDOWNS */
    private float eatCD;
    private float scareCD;
    private float struggleCD;

    /* UI TEXT */
    private TMP_Text livesText;
    private TMP_Text energyText;
    private TMP_Text phoneText;
    private TMP_Text reesesText;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
        energy = 9999f;
        lives = 3;  
        moveSpeed = 2f;
        speedMod = 1f;
        captured = false;
        phonePieces = 0;
        reesesPieces = 0;

        eatCD = 0f;
        scareCD = 0f;
        struggleCD = 100f;

        livesText = GameObject.Find("ET/User Interface/Lives").GetComponent<TMP_Text>();
        energyText = GameObject.Find("ET/User Interface/Energy").GetComponent<TMP_Text>();
        phoneText = GameObject.Find("ET/User Interface/Phone Pieces").GetComponent<TMP_Text>();
        reesesText = GameObject.Find("ET/User Interface/Reeses' Pieces").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (energy <= 0) {
            loseLife();
        }

        /* ABILITIES */
        speedMod = 1f;
        // sprint
        if (Input.GetKey(KeyCode.LeftShift)) {
            speedMod = 2f;
        }
        // eat
        if (Input.GetKey(KeyCode.Space)) {
            if(reesesPieces > 0) {
                if (eatCD <= 0) {
                    energy += 341;
                    reesesPieces -= 1;
                    eatCD = 60f;
                }
            }
        }
        // scare
        if (Input.GetKey(KeyCode.P)) {
            if (scareCD == 0f) {
                scare();
                // FIXME: adjust cooldown
                scareCD = 60f;
            }
        }
        // TODO: fly
        if (Input.GetKey(KeyCode.O)) {
            if (SceneManager.GetActiveScene().name == "HoleScene") {

            }
        }
        // TODO: interact
        if (Input.GetKey(KeyCode.I)) {

        }
        // TODO: summon Elliot
        if (Input.GetKey(KeyCode.E)) {
            if(reesesPieces > 0) {
                GameObject.Find("Elliot").GetComponent<AllyController>().active = true;
            }
        }
        // TODO: call mothership
        if (Input.GetKey(KeyCode.C)) {
            if (phonePieces == 3) {
                SceneManager.LoadScene("CreditsScene");
            }
        }

        /* MOVEMENT */
        if (captured) {
            struggle();
        } else {
            if (Input.GetKey(KeyCode.W)) {
                transform.position += transform.up * moveSpeed * speedMod * Time.deltaTime;
                energy -= .1f * speedMod;
            }
            if (Input.GetKey(KeyCode.A)) {
                transform.position += transform.right * -moveSpeed * speedMod  * Time.deltaTime;
                energy -= .1f * speedMod;
            }
            if (Input.GetKey(KeyCode.S)) {
                transform.position += transform.up * -moveSpeed * speedMod  * Time.deltaTime;
                energy -= .1f * speedMod;
            }
            if (Input.GetKey(KeyCode.D)) {
                transform.position += transform.right * moveSpeed * speedMod * Time.deltaTime;
                energy -= .1f * speedMod;
            }
        }

        Debug.Log(transform.position.x + ", " + transform.position.y);

        /* WORLD WRAP */
        // FIXME: Adjust for map size
        if(transform.position.y > 10.32 || transform.position.y < -10.32) {
            gameObject.transform.position = new Vector3(transform.position.x, -transform.position.y, 0);
        }
        if(transform.position.x > 18.11 || transform.position.x < -18.11) {
            gameObject.transform.position = new Vector3(-transform.position.x, transform.position.y, 0);
        }

        /* UI */
        livesText.text = "Lives: " + lives;
        energyText.text = "Energy: " + (int)energy;
        phoneText.text = "Phone Pieces: " + phonePieces + "/3";
        reesesText.text = "Space: Eat Candy (" + reesesPieces + ")";
    }

    /*******************************************************************
    * Manages collision detection for the player
    * @param collision The hitbox of the object being collided with
    *******************************************************************/
    void OnCollisionEnter2D(Collision2D collision) {
        if(!captured) {
            switch(collision.collider.gameObject.tag) {
                // FIXME
                case "Scientist":
                    captured = true;
                    struggleAmt = 20;
                    GameObject.Find("Scientist").GetComponent<EnemyController>().scared = true;
                    break;
                case "Agent":
                    if (phonePieces > 0){
                        phonePieces--;
                    } else if (reesesPieces > 0) {
                        reesesPieces = 0;
                    }
                    // TODO: respawn phone piece somewhere
                    break;
                case "Ally":
                    // FIXME: adjust energy gain
                    energy += (reesesPieces * 100);
                    break;
                case "Hole":
                    SceneManager.LoadScene("HoleScene");
                    break;
                case "Phone Piece":
                    Destroy(collision.collider.gameObject);
                    phonePieces++;
                    break;
                case "Reeses' Piece":
                    Destroy(collision.gameObject);
                    reesesPieces++;
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName("HoleScene"));
        SceneManager.LoadScene("HoleScene");
    }

    public void scare() {
        GameObject.Find("Scientist").GetComponent<EnemyController>().scared = true;
        GameObject.Find("Agent").GetComponent<EnemyController>().scared = true;
    }

    private void struggle() {
        GameObject scientist = GameObject.Find("Scientist");

        Vector3 dest = new Vector3(scientist.transform.position.x,
            scientist.transform.position.y, scientist.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position,
            dest, moveSpeed * Time.deltaTime);

        struggleCD--;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
            struggleAmt--;
            struggleCD = 10000f;
        }

        if (struggleAmt == 0) {
            captured = false;
        }
    }

    /*
    * Manages behavior for player deaths
    */
    private void loseLife() {
        lives--;
        if(lives == 0) {
            SceneManager.LoadScene("CreditsScene");
        } else {
            transform.position = new Vector3((float)0, (float)0, (float)0);
            energy = 1500;
        }
    }
}