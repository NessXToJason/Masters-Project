using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocatorController : MonoBehaviour
{
    private static float overworldX0;
    private static float overworldY0;
    private static float overworldX1;
    private static float overworldY1;
    private static float overworldX2;
    private static float overworldY2;
    private bool collected;
    private int index;
    private PlayerController player;
    public Renderer rend;

    private string[] allPieces = {"Phone1", "Phone2", "Phone3"};
    private static float[] possibleX = {-7.7f, 0.0f, 7.7f, -15.3f, -9.9f, -15.3f, -9.9f, 10.2f, 15.0f, -13.6f, -6.2f,  7.0f, 12.8f};
    private static float[] possibleY = { 7.3f, 7.3f, 7.3f,   2.5f,  2.5f,  -2.5f, -2.5f,  0.0f,  0.0f,  -7.2f, -7.2f, -7.4f, -7.4f};

    // Start is called before the first frame update
    void Start() {
        index = int.Parse(gameObject.name.Substring(5));
        collected = false;
        newPosition();
        player = GameObject.Find("ET").GetComponent<PlayerController>();
        rend = GetComponent<Renderer>();
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void newPosition() {
        // FIXME: Move if in the same spot as another marker
        do {
            int posIndex = Random.Range(0, 12);
            gameObject.transform.position = new Vector3(possibleX[posIndex], possibleY[posIndex], 0f);
        } while((gameObject.transform.position == GameObject.Find("Phone0").transform.position && gameObject.name != "Phone0") ||
                (gameObject.transform.position == GameObject.Find("Phone1").transform.position && gameObject.name != "Phone1") ||
                (gameObject.transform.position == GameObject.Find("Phone2").transform.position && gameObject.name != "Phone2"));
    }

    public void removeFromPlay() {
        gameObject.transform.position = new Vector3((30 + index), 30f, 0f);
    }

    private bool validLocation() {
        if(gameObject.transform.position.x < -16 || gameObject.transform.position.x > 16 || gameObject.transform.position.y < -9 || gameObject.transform.position.y > 9
            || (gameObject.transform.position.x > -8 && gameObject.transform.position.x < 8 && gameObject.transform.position.y > -5 && gameObject.transform.position.y < 5)) {
            return false;
        }
        return true;
    }

    public void savePhoneCoords(float x0, float y0, float x1, float y1, float x2, float y2) {
        overworldX0 = x0;
        overworldY0 = y0;
        overworldX1 = x1;
        overworldY1 = y1;
        overworldX2 = x2;
        overworldY2 = y2;
    }

    public void setPhoneCoords() {
        if(player.getJustCollected() == index) {
            transform.position = new Vector3((float)(30 + index), (float)(30 + index), 0f);
        } else {
            switch(index) {
                case 0:
                    transform.position = new Vector3((float)overworldX0, (float)overworldY0, (float)0);
                    break;
                case 1:
                    transform.position = new Vector3((float)overworldX1, (float)overworldY1, (float)0);
                    break;
                case 2:
                    transform.position = new Vector3((float)overworldX2, (float)overworldY2, (float)0);
                    break;
            }
        }
    }

    public void hideLocation() {
        rend.enabled = false;
    }

    public void showLocation() {
        rend.enabled = true;
    }

    public void collect() {
        collected = true;
    }
}
