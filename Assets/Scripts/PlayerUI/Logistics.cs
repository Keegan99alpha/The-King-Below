using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logistics : MonoBehaviour
{
    public Canvas playerHud;
    public Text hudGold;
    public int playerGold;
    // Start is called before the first frame update
    void Start()
    {
        playerGold = 1000;
    }

    void UpdateGoldDisplay()
    {
        hudGold.text = "Gold: " + playerGold;
    }

    // Update is called once per frame
    void Update()
    {
        hudGold.text = "Gold: " + playerGold;
    }
}
