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
    /* Stores the collision zone for the enemy */
    private BoxCollider2D hitbox;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ET");
        hitbox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Find the player's position and move towards it
        Vector3 dest = new Vector3(player.transform.position.x,
            player.transform.position.y, player.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position,
            dest, 1.5f * Time.deltaTime);
    }
}
