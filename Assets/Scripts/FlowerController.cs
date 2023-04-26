using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerController : MonoBehaviour
{
    private PlayerController player;
    private FlowerLocController locator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("ET").GetComponent<PlayerController>();
        locator = GameObject.Find("FlowerLoc").GetComponent<FlowerLocController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.getNearFlower() && locator.transform.position.x != 34f){
            gameObject.transform.position = new Vector3(-4f, -2.25f, 0f);
        }
    }
}
