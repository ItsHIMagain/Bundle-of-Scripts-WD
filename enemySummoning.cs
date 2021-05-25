using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemySummoning : MonoBehaviour
{
    public GameObject[] enemyList;
    public float[] chanceList;
    public int chanceIncrease;

    private GameObject chosenEnemy;

    private void Start()
    {
        for (int i = 0; i < chanceList.Length - 1; i++)
        {
            chanceList[i] += newLevel.level * chanceIncrease;
        }
        StartCoroutine(spawnEnemy());
    }

    IEnumerator spawnEnemy()
    {
        float rng = Random.Range(0, 100);

        for (int i = 0; i < enemyList.Length; i++)
        {
            if (chanceList[i] >= rng)
            {
                chosenEnemy = enemyList[i];
                break;
            }
        }

        yield return new WaitForSeconds(1.1f);

        Instantiate(chosenEnemy, transform.position, Quaternion.identity);
    }
}
