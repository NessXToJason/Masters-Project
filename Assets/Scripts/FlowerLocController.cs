using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerLocController : MonoBehaviour
{
    private static float overworldX;
    private static float overworldY;
    private PlayerController player;
    public Renderer rend;
    private static float[] possibleX = {-7.7f, 0.0f, 7.7f, -15.3f, -9.9f, -15.3f, -9.9f, 10.2f, 15.0f, -13.6f, -6.2f,  7.0f, 12.8f};
    private static float[] possibleY = { 7.3f, 7.3f, 7.3f,   2.5f,  2.5f,  -2.5f, -2.5f,  0.0f,  0.0f,  -7.2f, -7.2f, -7.4f, -7.4f};

    // Start is called before the first frame update
    void Start() {
        newPosition();
        player = GameObject.Find("ET").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void newPosition() {
        int posIndex = Random.Range(0, 12);
        gameObject.transform.position = new Vector3(possibleX[posIndex], possibleY[posIndex], 0f);
    }

    public void removeFromPlay() {
        gameObject.transform.position = new Vector3(34f, 34f, 0f);
    }

    private bool validLocation() {
        if(gameObject.transform.position.x < -16 || gameObject.transform.position.x > 16 || gameObject.transform.position.y < -9 || gameObject.transform.position.y > 9
            || (gameObject.transform.position.x > -8 && gameObject.transform.position.x < 8 && gameObject.transform.position.y > -5 && gameObject.transform.position.y < 5)) {
            return false;
        }
        return true;
    }

    public void saveFlowerCoords(float x, float y) {
        overworldX = x;
        overworldY = y;
    }

    public void setFlowerCoords() {
        if(player.getCollectedFlower()) {
            transform.position = new Vector3(34f, 34f, 0f);
        } else {
            transform.position = new Vector3((float)overworldX, (float)overworldY, (float)0);
        }
    }
}