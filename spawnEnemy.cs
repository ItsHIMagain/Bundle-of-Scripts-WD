using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemy : MonoBehaviour
{
    public GameObject enemy;
    public float chance;
    private float trueChance;
    public int minEnemies = 1;

    private void Start()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 0.1f);
        if(hit.collider.tag == "lowerWall" || hit.collider.tag == "Wall")
        {
            Destroy(gameObject);
        }
        StartCoroutine("spawnEnemyDelay");
    }

    void tryToSpawn()
    {
        trueChance = chance + ((float)newLevel.level/10);
        int rng = Random.Range(0, 100);

        if (rng <= chance)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
        }
        rng = 100;

        if (GameObject.FindGameObjectsWithTag("enemy").Length < minEnemies)
        {
            tryToSpawn();
        }
    }

    public IEnumerator spawnEnemyDelay(){
        yield return new WaitForSeconds(0.4f);
        tryToSpawn();
    }
}
