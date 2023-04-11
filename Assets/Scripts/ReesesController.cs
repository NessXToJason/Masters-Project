using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReesesController : MonoBehaviour
{
    private static float overworldX;
    private static float overworldY;

    // Start is called before the first frame update
    void Start()
    {
        place();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.gameObject.tag == "Player") {
            place();
        }
    }

    private void place() {
        gameObject.transform.position = new Vector3((float)Random.Range(-17, 17), (float)Random.Range(-9, 9), 0f);
    }
}
