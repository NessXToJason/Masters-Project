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

    private float moveSpeed;

    public bool scared;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ET");
        home = GameObject.Find(gameObject.name + "Home");
        target = player;
        hitbox = GetComponent<BoxCollider2D>();
        moveSpeed = 1.5f;
        scared = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(home.transform.position, gameObject.transform.position) == 0) {
            scared = false;
        }

        if (!scared) {
            target = player;
        } else {
            target = home;
        }

        Vector3 dest = new Vector3(target.transform.position.x,
            target.transform.position.y, target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position,
            dest, moveSpeed * Time.deltaTime);
    }
}
