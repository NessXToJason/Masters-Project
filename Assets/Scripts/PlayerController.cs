using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/**********************************************************************
* A class that manages and controls the player in an ET-like game.
*
* Jason
**********************************************************************/
public class PlayerController : MonoBehaviour
{
    private BoxCollider2D hitbox;
    private Rigidbody2D rb;
    private SceneTransitions st;

    /* LIFE SYSTEM */
    private static float energy = 9999f;
    private static int lives = 3;

    /* STATS */
    private static float moveSpeed = 2f;
    private static float speedMod = 1f;
    private static bool captured = false;
    private static int struggleAmt = 0;

    /* INVENTORY */
    private static int phonePieces = 0;
    private static int reesesPieces = 0;
    private static int nearbyPhone = -1;
    private static int justCollected;
    private static bool collectedFlower;
    private static bool nearFlower = false;

    /* ABILITIES */
    private static float eatCD = 0f;
    private static float scareCD = 0f;
    private static float callCD = 0f;
    private static float summonCD = 0f;
    private static bool flying = false;
    private static float flyCD = 0f;
    private static float teleportCD = 0f;
    private static float searchCD = 0f;
    private static float pauseCD = 0f;

    /* WIN CONDITION */
    private static bool shipComing = false;
    private static float shipETA = 30f;
    private static bool gameJustEnded = false;

    /* UI TEXT */
    private static TMP_Text scareText;
    private static TMP_Text summonText;
    private static TMP_Text callText;
    private static TMP_Text livesText;
    private static TMP_Text energyText;
    private static TMP_Text phoneText;
    private static TMP_Text reesesText;
    private static TMP_Text flyText;
    private static TMP_Text searchText;
    private static TMP_Text teleportText;
    private static TMP_Text objectiveText;
    private static TMP_Text eatText;
    private static TMP_Text warningText;
    private static float warningVisible = 0f;
    private static TMP_Text pausedText;

    /* SOUND EFFECTS */
    private AudioSource[] sfx;
    private AudioSource enemySound;
    private AudioSource allySound;
    private AudioSource itemSound;
    private AudioSource teleportSound;
    private AudioSource eatSound;
    private AudioSource scareSound;
    private AudioSource fallSound;
    private AudioSource dieSound;
    private AudioSource callSound;

    /* POSITION */
    private static float overworldX;
    private static float overworldY;
    private static bool justEscaped = false;

    /* OTHER ENTITIES */
    private static EnemyController scientist;
    private static EnemyController agent;
    private static AllyController elliot;
    private static LocatorController phone0;
    private static LocatorController phone1;
    private static LocatorController phone2;
    private static FlowerLocController flower;
    private static GameObject mothership;

    private static bool pause = false;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sfx = GetComponents<AudioSource>();

        enemySound = sfx[0];
        allySound = sfx[1];
        itemSound = sfx[2];
        teleportSound = sfx[3];
        eatSound = sfx[4];
        scareSound = sfx[5];
        fallSound = sfx[6];
        dieSound = sfx[7];
        callSound = sfx[8];

        if(SceneManager.GetActiveScene().name == "MainScene" || SceneManager.GetActiveScene().name == "HoleScene") {
            scareText = GameObject.Find("User Interface/Controls/ScareCD").GetComponent<TMP_Text>();
            summonText = GameObject.Find("User Interface/Controls/SummonCD").GetComponent<TMP_Text>();
            callText = GameObject.Find("User Interface/Controls/CallCD").GetComponent<TMP_Text>();
            livesText = GameObject.Find("User Interface/Stats/Lives").GetComponent<TMP_Text>();
            energyText = GameObject.Find("User Interface/Stats/Energy").GetComponent<TMP_Text>();
            phoneText = GameObject.Find("User Interface/Stats/Phone Pieces").GetComponent<TMP_Text>();
            reesesText = GameObject.Find("User Interface/Stats/Reeses' Pieces").GetComponent<TMP_Text>();
            flyText = GameObject.Find("User Interface/Controls/Fly").GetComponent<TMP_Text>();
            searchText = GameObject.Find("User Interface/Controls/SearchCD").GetComponent<TMP_Text>();
            teleportText = GameObject.Find("User Interface/Controls/TeleportCD").GetComponent<TMP_Text>();
            objectiveText = GameObject.Find("User Interface/Stats/Objective").GetComponent<TMP_Text>();
            eatText = GameObject.Find("User Interface/Stats/EatCD").GetComponent<TMP_Text>();
            warningText = GameObject.Find("User Interface/Warning").GetComponent<TMP_Text>();
            pausedText = GameObject.Find("User Interface/Paused").GetComponent<TMP_Text>();

            phone0 = GameObject.Find("Phone0").GetComponent<LocatorController>();
            phone1 = GameObject.Find("Phone1").GetComponent<LocatorController>();
            phone2 = GameObject.Find("Phone2").GetComponent<LocatorController>();
            flower = GameObject.Find("FlowerLoc").GetComponent<FlowerLocController>();
            DontDestroyOnLoad(phone0);
            DontDestroyOnLoad(phone1);
            DontDestroyOnLoad(phone2);
            DontDestroyOnLoad(flower);
        }

        if(SceneManager.GetActiveScene().name == "MainScene") {
            scientist = GameObject.Find("Scientist").GetComponent<EnemyController>();
            agent = GameObject.Find("Agent").GetComponent<EnemyController>();
            elliot = GameObject.Find("Elliot").GetComponent<AllyController>();
        }

        if(SceneManager.GetActiveScene().name == "CutScene") {
            mothership = GameObject.Find("Mothership");
        }
    }

    // Update is called once per frame
    void Update() {
        if(SceneManager.GetActiveScene().name == "CutScene") {
            if(!shipComing) {
                if(gameObject.transform.position.y <= 0f) {
                    mothership.transform.position += transform.up * moveSpeed * speedMod * Time.deltaTime;
                    if(mothership.transform.position.y > 6f) {
                        SceneManager.LoadScene("MainScene");
                    }
                } else {
                    transform.position += transform.up * -moveSpeed * speedMod * Time.deltaTime;
                    mothership.transform.position += transform.up * -moveSpeed * speedMod * Time.deltaTime;
                }
            } else {
                if(gameJustEnded) {
                    gameObject.transform.position = new Vector3(0f, 0f, 0f);
                    gameJustEnded = false;
                }
                if(mothership.transform.position.y <= gameObject.transform.position.y) {
                    transform.position += transform.up * moveSpeed * speedMod * Time.deltaTime;
                    mothership.transform.position += transform.up * moveSpeed * speedMod * Time.deltaTime;
                    if(gameObject.transform.position.y > 6f) {
                        SceneManager.LoadScene("CreditsScene");
                    }
                } else {
                    mothership.transform.position += transform.up * -moveSpeed * speedMod * Time.deltaTime;
                }
            }
        } else {
            // pause / unpause
            pauseCD = updateCD(pauseCD);
            if (Input.GetKey(KeyCode.Escape) && pauseCD == 0) {
                togglePause();
                pauseCD = 0.25f;
            }

            if(!isPaused()) {
                if (energy <= 0) {
                    loseLife();
                }

                /* COOLDOWN */
                scareCD = updateCD(scareCD);
                eatCD = updateCD(eatCD);
                summonCD = updateCD(summonCD);
                flyCD = updateCD(flyCD);
                teleportCD = updateCD(teleportCD);
                searchCD = updateCD(searchCD);
                if(searchCD < 18f && SceneManager.GetActiveScene().name == "MainScene") {
                    phone0.hideLocation();
                    phone1.hideLocation();
                    phone2.hideLocation();
                }
                warningVisible = updateCD(warningVisible);
                if(warningVisible == 0f) {
                    warningText.text = "";
                }

                if(shipComing) {
                    shipETA -= Time.deltaTime;
                    callCD = shipETA;
                } else {
                    callCD = updateCD(callCD);
                }

                /* ABILITIES */
                speedMod = 1f;
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { speedMod = 2f; }
                if (Input.GetKey(KeyCode.Space)) { eat(); }
                if (Input.GetKey(KeyCode.O) && SceneManager.GetActiveScene().name == "MainScene") { search(); }
                if (Input.GetKey(KeyCode.P)) { scare(); }
                if (Input.GetKey(KeyCode.Q)) { fly(); }
                if (Input.GetKey(KeyCode.E)) { summon(); }
                if (Input.GetKey(KeyCode.C)) { call(); }

                /* MOVEMENT */
                if (captured) {
                    rb.isKinematic = false;
                    struggle();
                    if(scientist.transform.position == GameObject.Find("ScientistHome").transform.position) {
                        scientist.transform.position = new Vector3(-12.6f, 20.4f, 0f);
                        captured = false;
                    }
                } else {
                    move();
                    tryTeleport();
                }
                if(flying) {
                    energy -= .1f * speedMod;

                    if(SceneManager.GetActiveScene().name == "MainScene") {
                        rb.isKinematic = true;
                    }
                } else {
                    rb.isKinematic = false;
                }

                /* WORLD WRAP */
                if(SceneManager.GetActiveScene().name == "MainScene") {
                    if(justEscaped && (transform.position.x != overworldX || transform.position.y != overworldY)) {
                        setPlayerCoords();
                        scientist.setEnemyCoords();
                        agent.setEnemyCoords();
                        elliot.setAllyCoords();
                        phone0.setPhoneCoords();
                        phone1.setPhoneCoords();
                        phone2.setPhoneCoords();
                        flower.setFlowerCoords();
                        justEscaped = false;
                    }
                    
                    if(transform.position.y > 10.32) {
                        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 20.64f, 0);
                    }
                    if(transform.position.y < -10.32) {
                        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + 20.64f, 0);
                    }
                    if(transform.position.x > 17.96) {
                        gameObject.transform.position = new Vector3(transform.position.x - 35.92f, transform.position.y, 0);
                    }
                    if(transform.position.x < -17.96) {
                        gameObject.transform.position = new Vector3(transform.position.x + 35.92f, transform.position.y, 0);
                    }
                } else if (SceneManager.GetActiveScene().name == "HoleScene") {
                    if(transform.position.y > 5) {
                        leaveHole();
                        string trueJustCollected = "" + (justCollected - 1);
                        if(trueJustCollected == "0") {
                            phone0.removeFromPlay();
                        } else if(trueJustCollected == "1") {
                            phone1.removeFromPlay();
                        } else if(trueJustCollected == "2") {
                            phone2.removeFromPlay();
                        }
                    }
                }

                /* UI */
                updateUI();

                /* WIN CONDITION */
                if(shipETA <= 0) {
                    if(nearSpawn()) {
                        gameJustEnded = true;
                        SceneManager.LoadScene("CutScene");
                    } else {
                        shipComing = false;
                        shipETA = 30f;
                        setWarningText("You missed the ship, call again!");
                    }
                }
            }
        }
    }

    /*******************************************************************
    * Manages collision detection for the player
    * @param collision The hitbox of the object being collided with
    *******************************************************************/
    void OnCollisionEnter2D(Collision2D collision) {
        if(!captured) {
            switch(collision.collider.gameObject.tag) {
                case "Scientist":
                    enemySound.Play();
                    captured = true;
                    struggleAmt = 2000;
                    GameObject.Find("Scientist").GetComponent<EnemyController>().scared = true;
                    break;
                case "Agent":
                    enemySound.Play();
                    if (phonePieces > 0){
                        phonePieces--;
                        for (int i = 0; i < 3; i++) {
                            if(GameObject.Find("Phone" + i).transform.position == new Vector3((float)(30 + i), (float)(30 + i), 0f)) {
                                GameObject.Find("Phone" + i).GetComponent<LocatorController>().newPosition();
                            }
                        }
                    } else if (reesesPieces > 0) {
                        reesesPieces = 0;
                    }
                    GameObject.Find("Agent").GetComponent<EnemyController>().scared = true;
                    break;
                case "Ally":
                    allySound.Play();
                    energy += (reesesPieces * 773);
                    reesesPieces = 0;
                    break;
                case "Hole":
                    if(!flying) {
                        //fallSound.Play();
                        savePlayerCoords(gameObject.transform.position.x, gameObject.transform.position.y);
                        scientist.saveEnemyCoords(scientist.transform.position.x, scientist.transform.position.y,
                                                    agent.transform.position.x, agent.transform.position.y);
                        elliot.saveAllyCoords(elliot.transform.position.x, elliot.transform.position.y);
                        phone0.savePhoneCoords(phone0.transform.position.x, phone0.transform.position.y,
                                                phone1.transform.position.x, phone1.transform.position.y,
                                                phone2.transform.position.x, phone2.transform.position.y);
                        flower.saveFlowerCoords(flower.transform.position.x, flower.transform.position.y);
                        energy -= 269f;
                        if(Vector3.Distance(transform.transform.position, phone0.transform.position) < 3f) {
                            nearbyPhone = 0;
                        } else if (Vector3.Distance(transform.transform.position, phone1.transform.position) < 3f) {
                            nearbyPhone = 1;
                        } else if (Vector3.Distance(transform.transform.position, phone2.transform.position) < 3f) {
                            nearbyPhone = 2;
                        }
                        if(Vector3.Distance(transform.transform.position, flower.transform.position) < 3f) {
                            nearFlower = true;
                        }
                        SceneManager.LoadScene("HoleScene");
                        gameObject.transform.position = new Vector3(0f, 4.5f, 0f);
                        rb.gravityScale = 1;
                    }
                    break;
                case "Phone Piece":
                    itemSound.Play();
                    Destroy(collision.collider.gameObject);
                    phonePieces++;
                    collectPiece(getNearbyPhone());
                    break;
                case "Reeses' Piece":
                    itemSound.Play();
                    reesesPieces++;
                    break;
                case "Flower":
                    itemSound.Play();
                    Destroy(collision.gameObject);
                    lives++;
                    collectedFlower = true;
                    break;
            }
        }
    }

    public void move() {
        if (Input.GetKey(KeyCode.W)) {
            transform.position += transform.up * moveSpeed * speedMod * Time.deltaTime;
            energy -= .01f * speedMod;
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.position += transform.right * -moveSpeed * speedMod  * Time.deltaTime;
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            energy -= .01f * speedMod;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.position += transform.up * -moveSpeed * speedMod  * Time.deltaTime;
            energy -= .01f * speedMod;
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.position += transform.right * moveSpeed * speedMod * Time.deltaTime;
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            energy -= .01f * speedMod;
        }
    }

    public void tryTeleport() {
        Vector3 teleportDirection = new Vector3(0,0,0);

        if(teleportCD <= 0) {
            if(Input.GetKey(KeyCode.UpArrow)) 
                teleportDirection = transform.up * 5;
            else if(Input.GetKey(KeyCode.LeftArrow)) 
                teleportDirection = -transform.right * 5;
            else if(Input.GetKey(KeyCode.DownArrow)) 
                teleportDirection = -transform.up * 5;
            else if(Input.GetKey(KeyCode.RightArrow)) 
                teleportDirection = transform.right * 5;

            if(teleportDirection != new Vector3(0,0,0)) {
                teleportSound.Play();
                transform.position += teleportDirection;
                energy -= 100;
                teleportCD = 15f;
                flying = true;
            }
        }
    }

    public float updateCD(float cooldown) {
        if(cooldown > 0) {
            cooldown -= Time.deltaTime;
        }
        if(cooldown < 0) {
            cooldown = 0;
        }

        return cooldown;
    }

    public void eat() {
        if(reesesPieces > 0) {
            if (eatCD <= 0) {
                eatSound.Play();
                energy += 341;
                reesesPieces -= 1;
                eatCD = 10f;
            }
        } else {
            setWarningText("No Reeses' Pieces!");
        }
    }

    public void scare() {
        if(SceneManager.GetActiveScene().name == "MainScene") {
            if (scareCD == 0f) {
                scareSound.Play();
                GameObject.Find("Scientist").GetComponent<EnemyController>().scared = true;
                GameObject.Find("Agent").GetComponent<EnemyController>().scared = true;
                scareCD = 10f;
            }
        } else {
            setWarningText("You can't do that here!");
        }
    }

    public void summon() {
        if(SceneManager.GetActiveScene().name == "MainScene") {
            if(reesesPieces > 0) {
                GameObject.Find("Elliot").GetComponent<AllyController>().active = true;
                summonCD = 10f;
            } else {
                setWarningText("No Reeses' Pieces!");
            }
        } else {
            setWarningText("Elliot can't reach you down here!");
        }
    }

    public void fly() {
        if(flyCD == 0) {
            flyCD = 0.5f;
            if (flying) {
                flying = false;
                if (SceneManager.GetActiveScene().name == "HoleScene") {
                    rb.gravityScale = 1;
                }
            } else {
                flying = true;
                rb.gravityScale = 0;
            }
        }
    }

    public void search() {
        if(SceneManager.GetActiveScene().name == "MainScene") {
            phone0.showLocation();
            phone1.showLocation();
            phone2.showLocation();
            searchCD = 20f;
        } else {
            setWarningText("You can't do that here!");
        }
    }

    public void call() {
        callSound.Play();
        if(SceneManager.GetActiveScene().name == "MainScene") {
            if (phonePieces >= 3) {
                if(nearSpawn()) { 
                    shipComing = true;
                    callCD = 30f;
                } else {
                    setWarningText("Not close enough to the center!");
                }
            } else {
                setWarningText("Not enough phone pieces!");
            }
        } else {
            setWarningText("The reception's not good enough down here!");
        }
    }

    private void struggle() {
        GameObject scientist = GameObject.Find("Scientist");

        Vector3 dest = new Vector3(scientist.transform.position.x,
            scientist.transform.position.y, scientist.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position,
           dest, moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {
            struggleAmt--;
            energy -= .1f;
        }

        if (struggleAmt == 0) {
            captured = false;
            rb.isKinematic = true;
        }
    }

    /*
    * Manages behavior for player deaths
    */
    private void loseLife() {
        dieSound.Play();
        lives--;
        if(lives == 0) {
            SceneManager.LoadScene("CreditsScene");
        } else {
            transform.position = new Vector3((float)0, (float)0, (float)0);
            energy = 5000;
        }
    }

    private bool nearSpawn() {
        return (transform.position.x > -6 && transform.position.x < 6 && transform.position.y > -3.5 && transform.position.y < 3.5);
    }

    private void updateUI() {
        scareText.text = updateCDUI(scareCD);
        summonText.text = updateCDUI(summonCD);
        callText.text = updateCDUI(callCD);
        livesText.text = "Lives: " + lives;
        energyText.text = "Energy: " + (int)energy;
        phoneText.text = "Phone Pieces: " + phonePieces + "/3";
        reesesText.text = "Reeses' Pieces: " + reesesPieces;
        flyText.text = flying ? "Drop" : "Fly";
        searchText.text = updateCDUI(searchCD);
        teleportText.text = updateCDUI(teleportCD);
        eatText.text = updateCDUI(eatCD);

        if(SceneManager.GetActiveScene().name == "HoleScene") {
            objectiveText.text = "Fly out of the hole!";
        } else if(captured) {
            objectiveText.text = "Move around to break free!";
        } else if(phonePieces < 3) {
            objectiveText.text = "Collect phone pieces!";
        } else {
            if(nearSpawn()) {
                if(shipComing) {
                    objectiveText.text = "Wait here for " + ((int)shipETA + 1) + " second" + (((int)shipETA + 1 == 1) ? "" : "s") + "!";
                } else {
                    objectiveText.text = "E.T. phone home!";
                }
            } else {
                if(shipComing) {
                    objectiveText.text = "Get to the center within " + (int)shipETA + " seconds!";
                } else {
                    objectiveText.text = "Get to the center and phone home!";
                }
            }
        }
    }

    private string updateCDUI(float cd) {
        return "" + (cd == 0 ? "" : "" + ((int)cd + 1));
    }

    public void savePlayerCoords(float x, float y) {
        overworldX = x;
        overworldY = y;
    }

    public void setPlayerCoords() {
        gameObject.transform.position = new Vector3((float)overworldX, (float)overworldY, (float)0);
    }

    public void togglePause() {
        pause = !pause;
        if(pause) {
            pausedText.text = "PAUSED";
        } else {
            pausedText.text = "";
        }
    }

    public bool isPaused() {
        return pause;
    }

    private void setWarningText(string message) {
        warningText.text = message;
        warningVisible = 5f;
    } 

    public int getNearbyPhone() {
        return nearbyPhone;
    }

    public void collectPiece(int piece) {
        justCollected = piece;
    }

    public int getJustCollected() {
        return justCollected;
    }

    public bool getNearFlower() {
        return nearFlower;
    }

    public bool getCollectedFlower() {
        return collectedFlower;
    }

    public void leaveHole() {
        SceneManager.LoadScene("MainScene");
        rb.gravityScale = 0;
        justEscaped = true;
        nearbyPhone = -1;
    }

    public int getPhonePieceCount() {
        return phonePieces;
    }

    public int getReesesPieceCount() {
        return reesesPieces;
    }

    public float getEatCD() {
        return eatCD;
    }

    public float getScareCD() {
        return scareCD;
    }

    public float getSummonCD() {
        return summonCD;
    }

    public float getFlyCD() {
        return flyCD;
    }

    public float getTeleportCD() {
        return teleportCD;
    }

    public float getSearchCD() {
        return searchCD;
    }

    public float getCallCD() {
        return searchCD;
    }
}