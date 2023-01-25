using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**********************************************************************
* A class that manages and controls the player in an ET-like game.
*
* Jason Truskowski
**********************************************************************/
public class PlayerController : MonoBehaviour
{
    private BoxCollider2D hitbox;
    private float movementSpeed;
    private int phonePieces;
    private AudioSource pickupSound;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();    
        movementSpeed = 2f;
        phonePieces = 0;
        pickupSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (phonePieces == 3) {
            SceneManager.LoadScene("CreditsScene");
        }

        if (Input.GetKey(KeyCode.W)) {
            transform.position += transform.up * movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.position += transform.right * -movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position += transform.up * -movementSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.position += transform.right * movementSpeed * Time.deltaTime;
        }
    }

    /*******************************************************************
    * Manages collision detection for the player
    * @param collision The hitbox of the object being collided with
    *******************************************************************/
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.gameObject.tag == "Enemy") {
            SceneManager.LoadScene("CreditsScene");
        } else if(collision.collider.gameObject.tag == "Phone Piece") {
            Destroy(collision.collider.gameObject);
            pickupSound.Play();
            phonePieces++;
        }
    }
}
