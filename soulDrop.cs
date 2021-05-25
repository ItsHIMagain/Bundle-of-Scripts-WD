using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soulDrop : MonoBehaviour
{
    public GameObject souls;

    public float smallSoulChance;
    public int minSmallSoul;
    public int maxSmallSoul;

    public float mediumSoulChance;
    public int minMediumSoul;
    public int maxMediumSoul;

    public float largeSoulChance;
    public int minLargeSoul;
    public int maxLargeSoul;

    public int maxLargeSoulValue;

    private int rng;

    public void spawnSouls()
    {
        rng = Random.Range(0, 100);

        if(rng <= smallSoulChance)
        {
            rng = Random.Range(minSmallSoul, maxSmallSoul);

            for (int i = 0; i < rng;)
            {
                GameObject newSoul = Instantiate(souls);
                newSoul.transform.position = transform.position;
                newSoul.GetComponent<collectSoul>().value = Random.Range(1, 14);
                newSoul.transform.SetParent(null);
                i++;
            }
        }
        
        rng = Random.Range(0, 100);
        if (rng <= mediumSoulChance)
        {
            rng = Random.Range(minMediumSoul, maxMediumSoul);

            for (int i = 0; i < rng; i++)
            {
                GameObject newSoul = Instantiate(souls);
                newSoul.transform.position = transform.position;
                newSoul.GetComponent<collectSoul>().value = Random.Range(15, 29);
                newSoul.transform.SetParent(null);
            }
        }

        rng = Random.Range(0, 100);
        if (rng <= smallSoulChance)
        {
            rng = Random.Range(minLargeSoul, maxLargeSoul);

            for (int i = 0; i < rng; i++)
            {
                GameObject newSoul = (souls);
                newSoul.transform.position = transform.position;
                newSoul.GetComponent<collectSoul>().value = Random.Range(30, maxLargeSoulValue);
                newSoul.transform.SetParent(null);
            }
        }
    }
}
