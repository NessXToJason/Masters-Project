using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**********************************************************************
* A class that manages and controls the player in an ET-like game.
*
* Jason
**********************************************************************/
public class PlayerController : MonoBehaviour
{
    private BoxCollider2D hitbox;
    private int lives;
    private float moveSpeed;
    private float speedMod;
    private int phonePieces;
    private int reesesPieces;
    private AudioSource pickupSound;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();  
        lives = 3;  
        moveSpeed = 2f;
        speedMod = 1f;
        phonePieces = 0;
        reesesPieces = 0;
        //pickupSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (phonePieces == 3) {
            SceneManager.LoadScene("CreditsScene");
        }

        if (GameController.energy <= 0) {
            loseLife();
        }

        // Abilities
        speedMod = 1f;
        if (Input.GetKey(KeyCode.LeftShift)) {
            speedMod = 2f;
        }
        if (Input.GetKey(KeyCode.Space)) {
            if(reesesPieces >= 0) {
                GameController.energy += 341;
                reesesPieces--;
            }
        }

        // Movement
        if (Input.GetKey(KeyCode.W)) {
            transform.position += transform.up * moveSpeed * speedMod * Time.deltaTime;
            GameController.energy -= 1 * speedMod;
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.position += transform.right * -moveSpeed * speedMod  * Time.deltaTime;
            GameController.energy -= 1 * speedMod;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position += transform.up * -moveSpeed * speedMod  * Time.deltaTime;
            GameController.energy -= 1 * speedMod;
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.position += transform.right * moveSpeed * speedMod * Time.deltaTime;
            GameController.energy -= 1 * speedMod;
        }
    }

    /*******************************************************************
    * Manages collision detection for the player
    * @param collision The hitbox of the object being collided with
    *******************************************************************/
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.gameObject.tag == "Enemy") {
            loseLife();
        } else if(collision.collider.gameObject.tag == "Phone Piece") {
            Destroy(collision.collider.gameObject);
            //pickupSound.Play();
            phonePieces++;
        } else if(collision.collider.gameObject.tag == "Reeses' Piece") {
            Destroy(collision.gameObject);
            reesesPieces++;
        }
    }

    /*
    * Manages behavior for player deaths
    */
    private void loseLife() {
        lives--;
        Debug.Log("Lives: " + lives);
        if(lives == 0) {
            Destroy(gameObject);
            //SceneManager.LoadScene("CreditsScene");
        } else {
            transform.position = new Vector3((float)0, (float)0, (float)0);
            GameController.energy = 1500;
        }
    }
}
