using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public static int energy;
    private TMP_Text energyText;

    // Start is called before the first frame update
    void Start()
    {
        energy = 9999;
        energyText = GameObject.Find("ET/Canvas/Energy").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        energyText.text = "Energy: " + energy;
    }
}
