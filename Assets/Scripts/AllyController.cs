using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : MonoBehaviour
{
    /* Keeps track of the player's location */
    private GameObject player;
    /* Keeps track of its home base's location */
    private GameObject home;

    private GameObject target;
    /* Stores the collision zone for the ally */
    private BoxCollider2D hitbox;
    public Renderer rend;

    private float moveSpeed;
    public bool active;
    public bool available;
    public float activationCooldown;

    private static float overworldX;
    private static float overworldY;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ET");
        home = GameObject.Find(gameObject.name + "Home");
        target = home;
        hitbox = GetComponent<BoxCollider2D>();
        moveSpeed = 3.5f;
        active = false;
        available = true;
        activationCooldown = 1000f;
        rend = GetComponent<Renderer>();
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameObject.Find("ET").GetComponent<PlayerController>().isPaused()) {
            if (Vector3.Distance(home.transform.position, gameObject.transform.position) == 0) {
                if (activationCooldown > 0) {
                    active = false;
                    available = true;
                }
            }

            if (active) {
                rend.enabled = true;
                hitbox.enabled = true;
                target = player;
            } else {
                hitbox.enabled = false;
                target = home;
            }

            if(gameObject.transform.position == home.transform.position) {
                rend.enabled = false;
            }

            // TODO: Replace with more robust pathfinding
            Vector3 dest = new Vector3(target.transform.position.x,
                target.transform.position.y, target.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position,
                dest, moveSpeed * Time.deltaTime);
            if(gameObject.transform.position.x < target.transform.position.x) {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            } else {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
            activationCooldown--;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        switch(collision.collider.gameObject.tag) {
                case "Player":
                    active = false;
                    available = false;
                    target = home;
                    break;
        }
    }

    public bool isActive() {
        return active;
    }

    public void saveAllyCoords(float x, float y) {
        overworldX = x;
        overworldY = y;
    }

    public void setAllyCoords() {
        transform.position = new Vector3((float)overworldX, (float)overworldY, (float)0);
    }
}
