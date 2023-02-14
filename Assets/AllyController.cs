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
    /* Stores the collision zone for the enemy */
    private BoxCollider2D hitbox;

    private float moveSpeed;

    public bool active;

    public bool available;

    public float activationCooldown;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(home.transform.position, gameObject.transform.position) == 0) {
            if (activationCooldown > 0) {
                active = false;
                available = true;
            }
        }

        if (active) {
            target = player;
        } else {
            target = home;
        }

        // TODO: Replace with more robust pathfinding
        Vector3 dest = new Vector3(target.transform.position.x,
            target.transform.position.y, target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position,
            dest, moveSpeed * Time.deltaTime);

        activationCooldown--;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        switch(collision.collider.gameObject.tag) {
                case "Player":
                    active = false;
                    available = false;
                    break;
        }
    }

    public bool isActive() {
        return active;
    }
}
