using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhonePieceController : MonoBehaviour
{
    private PlayerController player;
    private string currentPiece;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ET").GetComponent<PlayerController>();
        currentPiece = "Phone" + player.getNearbyPhone();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.getNearbyPhone() != -1){
            gameObject.transform.position = new Vector3(-4f, -2.25f, 0f);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.gameObject.tag == "Player") {
            player.collectPiece(int.Parse(currentPiece));
        }
    }
}
