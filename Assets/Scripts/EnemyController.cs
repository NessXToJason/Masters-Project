using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**********************************************************************
* A class that manages and controls enemy behavior in an ET-like game.
*
* Jason
**********************************************************************/
public class EnemyController : MonoBehaviour
{
    /* Keeps track of the player's location */
    private GameObject player;
    /* Keeps track of its home base's location */
    private GameObject home;

    private GameObject target;
    /* Stores the collision zone for the enemy */
    private BoxCollider2D hitbox;

    public bool scared;
    //private float moveSpeed = 1.5f;

    private static float scienOverworldX;
    private static float scienOverworldY;
    private static float agentOverworldX;
    private static float agentOverworldY;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("ET");
        home = GameObject.Find(gameObject.name + "Home");
        target = player;
        hitbox = GetComponent<BoxCollider2D>();
        scared = false;
    }

    // Update is called once per frame
    void Update() {
        if(!GameObject.Find("ET").GetComponent<PlayerController>().isPaused()) {
            if (Vector3.Distance(home.transform.position, gameObject.transform.position) == 0) {
                scared = false;
            }

            if (!scared) {
                target = player;
                hitbox.enabled = true;
            } else {
                target = home;
                hitbox.enabled = false;
            }

            GetComponent<NavMeshAgent2D>().destination = target.transform.position;
            // OLD IMPLEMENTATION
            // Vector3 dest = new Vector3(target.transform.position.x,
            //     target.transform.position.y, target.transform.position.z);
            // transform.position = Vector3.MoveTowards(transform.position,
            //     dest, moveSpeed * Time.deltaTime);
            if(gameObject.transform.position.x < target.transform.position.x) {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            } else {
                gameObject.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    public void saveEnemyCoords(float scienX, float scienY, float agentX, float agentY) {
        scienOverworldX = scienX;
        scienOverworldY = scienY;
        agentOverworldX = agentX;
        agentOverworldY = agentY;
    }

    public void setEnemyCoords() {
        if(gameObject.name == "Scientist") {
            transform.position = new Vector3((float)scienOverworldX, (float)scienOverworldY, (float)0);
        } else if(gameObject.name == "Agent") {
            transform.position = new Vector3((float)agentOverworldX, (float)agentOverworldY, (float)0);
        }
    }
}