using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public int coins;

    public TextMeshProUGUI coinText;

    void Start()
    {
        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        coins += amount;

        UpdateUI();

        Debug.Log("Coin : " + coins);
    }

    void UpdateUI()
    {
        coinText.text = "Coin : " + coins;
    }
}
