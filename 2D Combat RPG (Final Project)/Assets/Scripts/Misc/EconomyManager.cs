using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EconomyManager : Singleton<EconomyManager>
{
    TMP_Text goldText;
    int currentGold = 0;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";

    public void UpdateCurrentGold()
    {
        currentGold += 1;

        // Null reference check
        if (goldText == null)
        {
            goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
        }

        // Set our text object to be the string of our current gold
        // "D3" formats the text to always have 3 digits. Ex: 046
        goldText.text = currentGold.ToString("D3");
    }
}
