using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

public class TestSuite
{
    private GameObject et;
    private GameObject scientist;
    private GameObject agent;
    private GameObject elliot;
    private GameObject phone0;
    private GameObject phone1;
    private GameObject phone2;
    private GameObject phonePiece;
    private GameObject reesesPiece;

    [SetUp]
    public void SetUp() {
        SceneManager.LoadScene("MainScene");
    }

    [UnityTest]
    public IEnumerator ScientistMovesTowardsET() {
        et = GameObject.Find("ET");
        scientist = GameObject.Find("Scientist");
        var initScientistX = scientist.transform.position.x;
        yield return null;
        var newScientistX = scientist.transform.position.x;
        Assert.Less(initScientistX, newScientistX);
    }

    [UnityTest]
    public IEnumerator ScientistMovesTowardsHomeWhenScared() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        scientist = GameObject.Find("Scientist");
        yield return null;
        var initScientistX = scientist.transform.position.x;
        etScript.scare();
        yield return null;
        var newScientistX = scientist.transform.position.x;
        Assert.Greater(initScientistX, newScientistX);
    }

    [UnityTest]
    public IEnumerator AgentMovesTowardsET() {
        et = GameObject.Find("ET");
        agent = GameObject.Find("Agent");
        var initAgentX = agent.transform.position.x;
        yield return null;
        var newAgentX = agent.transform.position.x;
        Assert.Greater(initAgentX, newAgentX);
    }

    [UnityTest]
    public IEnumerator AgentMovesTowardsHomeWhenScared() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        agent = GameObject.Find("Agent");
        yield return null;
        var initAgentX = agent.transform.position.x;
        etScript.scare();
        yield return null;
        var newAgentX = agent.transform.position.x;
        Assert.Less(initAgentX, newAgentX);
    }

    [UnityTest]
    public IEnumerator ElliotMovesTowardsETWhenSummoned() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        elliot = GameObject.Find("Elliot");
        reesesPiece = GameObject.Find("Reeses' Piece");
        reesesPiece.transform.position = et.transform.position;
        var initElliotPos = elliot.transform.position.y;
        yield return null;
        etScript.summon();
        yield return null;
        var newElliotPos = elliot.transform.position.y;
        Assert.Less(initElliotPos, newElliotPos);
    }

    [UnityTest]
    public IEnumerator PhonePieces0And1SpawnInDifferentLocations() {
        phone0 = GameObject.Find("Phone0");
        phone1 = GameObject.Find("Phone1");
        yield return null;
        var phone0Pos = phone0.transform.position;
        var phone1Pos = phone1.transform.position;
        Assert.AreNotEqual(phone0Pos, phone1Pos);
    }

    [UnityTest]
    public IEnumerator PhonePieces0And2SpawnInDifferentLocations() {
        phone0 = GameObject.Find("Phone0");
        phone2 = GameObject.Find("Phone2");
        yield return null;
        var phone0Pos = phone0.transform.position;
        var phone2Pos = phone2.transform.position;
        Assert.AreNotEqual(phone0Pos, phone2Pos);
    }

    [UnityTest]
    public IEnumerator PhonePieces1And2SpawnInDifferentLocations() {
        phone1 = GameObject.Find("Phone1");
        phone2 = GameObject.Find("Phone2");
        yield return null;
        var phone1Pos = phone1.transform.position;
        var phone2Pos = phone2.transform.position;
        Assert.AreNotEqual(phone1Pos, phone2Pos);
    }

    [UnityTest]
    public IEnumerator PhonePieceCountIncreasesWhenCollected() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        var initPhonePieces = etScript.getPhonePieceCount();
        et.transform.position = new Vector3(-7.7f, 7.3f, 0f);
        yield return null;
        phonePiece = GameObject.Find("Phone Piece");
        phonePiece.transform.position = et.transform.position;
        yield return null;
        var newPhonePieces = etScript.getPhonePieceCount();
        Assert.AreEqual(initPhonePieces + 1, newPhonePieces);
    }

    [UnityTest]
    public IEnumerator ReesesPieceCountIncreasesWhenCollected() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        reesesPiece = GameObject.Find("Reeses' Piece");
        var initReesesPieces = etScript.getReesesPieceCount();
        reesesPiece.transform.position = et.transform.position;
        yield return null;
        var newReesesPieces = etScript.getReesesPieceCount();
        Assert.AreEqual(initReesesPieces + 1, newReesesPieces);
    }

    [UnityTest]
    public IEnumerator ReesesPieceRespawnsWhenCollected() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        reesesPiece = GameObject.Find("Reeses' Piece");
        var initReesesPiecePos = reesesPiece.transform.position;
        et.transform.position = reesesPiece.transform.position;
        yield return null;
        var newReesesPiecePos = reesesPiece.transform.position;
        Assert.AreNotEqual(initReesesPiecePos, newReesesPiecePos);
    }

    [UnityTest]
    public IEnumerator PhonePiece0IsRemovedWhenCollected() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        var phone0Script = GameObject.Find("Phone0").GetComponent<LocatorController>();
        et = GameObject.Find("ET");
        phone0 = GameObject.Find("Phone0");
        et.transform.position = phone0.transform.position;
        yield return null;
        et = GameObject.Find("ET");
        phonePiece = GameObject.Find("Phone Piece");
        et.transform.position = phonePiece.transform.position;
        yield return null;
        SceneManager.LoadScene("MainScene");
        yield return null;
        phone0Script = GameObject.Find("Phone0").GetComponent<LocatorController>();
        phone0 = GameObject.Find("Phone0");
        phone0Script.setPhoneCoords();
        Assert.AreEqual(phone0.transform.position, new Vector3(30f, 30f, 0f));
    }

    [UnityTest]
    public IEnumerator PhonePiece1IsRemovedWhenCollected() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        var phone1Script = GameObject.Find("Phone1").GetComponent<LocatorController>();
        et = GameObject.Find("ET");
        phone1 = GameObject.Find("Phone1");
        et.transform.position = phone1.transform.position;
        yield return null;
        et = GameObject.Find("ET");
        phonePiece = GameObject.Find("Phone Piece");
        et.transform.position = phonePiece.transform.position;
        yield return null;
        SceneManager.LoadScene("MainScene");
        yield return null;
        phone1Script = GameObject.Find("Phone1").GetComponent<LocatorController>();
        phone1 = GameObject.Find("Phone1");
        phone1Script.setPhoneCoords();
        Assert.AreEqual(phone1.transform.position, new Vector3(31f, 31f, 0f));
    }

    [UnityTest]
    public IEnumerator PhonePiece2IsRemovedWhenCollected() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        var phone2Script = GameObject.Find("Phone2").GetComponent<LocatorController>();
        et = GameObject.Find("ET");
        phone2 = GameObject.Find("Phone2");
        et.transform.position = phone2.transform.position;
        yield return null;
        et = GameObject.Find("ET");
        phonePiece = GameObject.Find("Phone Piece");
        et.transform.position = phonePiece.transform.position;
        yield return null;
        SceneManager.LoadScene("MainScene");
        yield return null;
        phone2Script = GameObject.Find("Phone2").GetComponent<LocatorController>();
        phone2 = GameObject.Find("Phone2");
        phone2Script.setPhoneCoords();
        Assert.AreEqual(phone2.transform.position, new Vector3(32f, 32f, 0f));
    }

    [UnityTest]
    public IEnumerator PhonePiece0RespawnsWhenAgentTouchesET() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        var phone0Script = GameObject.Find("Phone0").GetComponent<LocatorController>();
        et = GameObject.Find("ET");
        phone0 = GameObject.Find("Phone0");
        et.transform.position = phone0.transform.position;
        yield return null;
        et = GameObject.Find("ET");
        phonePiece = GameObject.Find("Phone Piece");
        et.transform.position = phonePiece.transform.position;
        yield return null;
        SceneManager.LoadScene("MainScene");
        yield return null;
        et = GameObject.Find("ET");
        agent = GameObject.Find("Agent");
        phone0Script = GameObject.Find("Phone0").GetComponent<LocatorController>();
        phone0 = GameObject.Find("Phone0");
        agent.transform.position = et.transform.position;
        yield return null;
        yield return null;
        Assert.AreNotEqual(phone0.transform.position, new Vector3(30f, 30f, 0f));
    }

    [UnityTest]
    public IEnumerator PhonePiece1RespawnsWhenAgentTouchesET() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        var phone1Script = GameObject.Find("Phone1").GetComponent<LocatorController>();
        et = GameObject.Find("ET");
        phone1 = GameObject.Find("Phone1");
        et.transform.position = phone1.transform.position;
        yield return null;
        et = GameObject.Find("ET");
        phonePiece = GameObject.Find("Phone Piece");
        et.transform.position = phonePiece.transform.position;
        yield return null;
        SceneManager.LoadScene("MainScene");
        yield return null;
        et = GameObject.Find("ET");
        agent = GameObject.Find("Agent");
        phone1Script = GameObject.Find("Phone1").GetComponent<LocatorController>();
        phone1 = GameObject.Find("Phone1");
        agent.transform.position = et.transform.position;
        yield return null;
        yield return null;
        Assert.AreNotEqual(phone1.transform.position, new Vector3(30f, 30f, 0f));
    }

    [UnityTest]
    public IEnumerator PhonePiece2RespawnsWhenAgentTouchesET() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        var phone2Script = GameObject.Find("Phone2").GetComponent<LocatorController>();
        et = GameObject.Find("ET");
        phone2 = GameObject.Find("Phone2");
        et.transform.position = phone2.transform.position;
        yield return null;
        et = GameObject.Find("ET");
        phonePiece = GameObject.Find("Phone Piece");
        et.transform.position = phonePiece.transform.position;
        yield return null;
        SceneManager.LoadScene("MainScene");
        yield return null;
        et = GameObject.Find("ET");
        agent = GameObject.Find("Agent");
        phone2Script = GameObject.Find("Phone2").GetComponent<LocatorController>();
        phone2 = GameObject.Find("Phone2");
        agent.transform.position = et.transform.position;
        yield return null;
        yield return null;
        Assert.AreNotEqual(phone2.transform.position, new Vector3(30f, 30f, 0f));
    }

    [UnityTest]
    public IEnumerator AgentStealsReesesPieces() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        agent = GameObject.Find("Agent");
        reesesPiece = GameObject.Find("Reeses' Piece");
        reesesPiece.transform.position = et.transform.position;
        yield return null;
        agent.transform.position = et.transform.position;
        yield return null;
        Assert.AreEqual(etScript.getReesesPieceCount(), 0);
    }

    [UnityTest]
    public IEnumerator ScareCooldownBegins() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        etScript.scare();
        Assert.AreEqual(etScript.getScareCD(), 10f);
        yield return null;
    }

    [UnityTest]
    public IEnumerator EatCooldownBegins() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        reesesPiece = GameObject.Find("Reeses' Piece");
        yield return null;
        reesesPiece.transform.position = et.transform.position;
        yield return null;
        etScript.eat();
        yield return null;
        Assert.AreEqual(etScript.getEatCD(), 10f);
    }

    [UnityTest]
    public IEnumerator SummonCooldownBegins() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        etScript.summon();
        Assert.AreEqual(etScript.getSummonCD(), 10f);
        yield return null;
    }

    [UnityTest]
    public IEnumerator FlyCooldownBegins() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        etScript.fly();
        Assert.AreEqual(etScript.getFlyCD(), 0.5f);
        yield return null;
    }

    // TESTED MANUALLY - Used the arrow keys to initiate a teleport
    // [UnityTest]
    // public IEnumerator TeleportCooldownBegins() {
    //     var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
    //     et = GameObject.Find("ET");
    //     etScript.teleport();
    //     Assert.AreEqual(etScript.getTeleportCD(), 15f);
    //     yield return null;
    // }

    [UnityTest]
    public IEnumerator SearchCooldownBegins() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        etScript.search();
        Assert.AreEqual(etScript.getSearchCD(), 20f);
        yield return null;
    }

    // TESTED MANUALLY - Collected all three pieces legitimately, then called home near the center
    // [UnityTest]
    // public IEnumerator CallCooldownBegins() {
    //     var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
    //     et = GameObject.Find("ET");
    //     yield return null;
    //     etScript.call();
    //     yield return null;
    //     Assert.AreEqual(etScript.getCallCD(), 30f);
    // }

    // TESTED MANUALLY - Paused and tried to move ET
    // [UnityTest]
    // public IEnumerator ETStaysStillWhenPaused() {
    //     yield return null;
    // }

    [UnityTest]
    public IEnumerator ScientistStaysStillWhenPaused() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        scientist = GameObject.Find("Scientist");
        etScript.togglePause();
        var initScientistPos = scientist.transform.position;
        yield return null;
        var newScientistPos = scientist.transform.position;
        Assert.AreEqual(initScientistPos, newScientistPos);
    }

    [UnityTest]
    public IEnumerator AgentStaysStillWhenPaused() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        agent = GameObject.Find("Agent");
        etScript.togglePause();
        var initAgentPos = agent.transform.position;
        yield return null;
        var newAgentPos = agent.transform.position;
        Assert.AreEqual(initAgentPos, newAgentPos);
    }

    [UnityTest]
    public IEnumerator ETCoordsLoadWhenExitingHole() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        var initETPos = new Vector3(-7.6f, 7.4f, 0f);
        et.transform.position = initETPos;
        yield return null;
        etScript.leaveHole();
        yield return null;
        var newETPos = GameObject.Find("ET").transform.position;
        Assert.AreEqual(initETPos, newETPos);
    }

    [UnityTest]
    public IEnumerator ScientistCoordsLoadWhenExitingHole() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        scientist = GameObject.Find("Scientist");
        et.transform.position = new Vector3(-7.7f, 7.3f, 0f);
        var initScientistPos = scientist.transform.position;
        yield return null;
        etScript.leaveHole();
        yield return null;
        var newScientistPos = GameObject.Find("Scientist").transform.position;
        Assert.AreEqual(initScientistPos, newScientistPos);
    }

    [UnityTest]
    public IEnumerator AgentCoordsLoadWhenExitingHole() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        agent = GameObject.Find("Agent");
        et.transform.position = new Vector3(-7.7f, 7.3f, 0f);
        var initAgentPos = agent.transform.position;
        yield return null;
        etScript.leaveHole();
        yield return null;
        var newAgentPos = GameObject.Find("Agent").transform.position;
        Assert.AreEqual(initAgentPos, newAgentPos);
    }

    [UnityTest]
    public IEnumerator ElliotCoordsLoadWhenExitingHole() {
        var etScript = GameObject.Find("ET").GetComponent<PlayerController>();
        et = GameObject.Find("ET");
        elliot = GameObject.Find("Elliot");
        et.transform.position = new Vector3(-7.7f, 7.3f, 0f);
        var initElliotPos = elliot.transform.position;
        yield return null;
        etScript.leaveHole();
        yield return null;
        var newElliotPos = GameObject.Find("Elliot").transform.position;
        Assert.AreEqual(initElliotPos, newElliotPos);
    }
}
