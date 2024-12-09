using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] GameObject goldCoin, healthGlobe, staminaGlobe;

    // This public method is called in other classes when certain
    // objects or enemies are destroyed. It spawns our pickup items.
    public void DropItems()
    {
        int randomNum = Random.Range(1, 5);

        if (randomNum == 1)
        {
            Instantiate(healthGlobe, transform.position, Quaternion.identity);
        }

        if (randomNum == 2)
        {
            Instantiate(staminaGlobe, transform.position, Quaternion.identity);
        }

        if (randomNum == 3)
        {
            int randomAmountOfGold = Random.Range(1, 4);

            for (int i = 0; i < randomAmountOfGold; i++)
            {
                Instantiate(goldCoin, transform.position, Quaternion.identity);
            }
        }
    }
}
