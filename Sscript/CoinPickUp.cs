using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickUp : MonoBehaviour
{
    public int coinValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory player =
                other.GetComponent<PlayerInventory>();

            if (player != null)
            {
                player.AddCoin(coinValue);
            }

            Destroy(gameObject);
        }
    }
}
