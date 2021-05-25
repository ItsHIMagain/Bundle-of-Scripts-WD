using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class HpContainers : MonoBehaviour
{
    public int hp;
    public int maxHp;

    public float invTime;
    private float remainingInv;

    public Image[] heart;
    public Sprite fullHP;
    public Sprite emptyHP;

    public static HpContainers instance;

    public bool godMode = false;

    private void Start()
    {
        instance = this;
        maxHp += StatBoosts.hpBoost;
        hp += StatBoosts.hpBoost;
    }

    private void Update()
    {
        for(int i = 0; i < heart.Length; i++)
        {
            if(i < hp)
            {
                heart[i].sprite = fullHP;
            } else
            {
                heart[i].sprite = emptyHP;
            }

            if(i < maxHp)
            {
                heart[i].enabled = true;
            } else
            {
                heart[i].enabled = false;
            }
        }
        if(remainingInv >= 0)
        {
            remainingInv -= Time.deltaTime;
        }

        if(hp <= 0 && godMode == false)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Destroy(GameObject.FindGameObjectWithTag("Gun"));
            player.GetComponent<playerController>().PlayerDeath();
            cashManager.instance.exitScene(1);
        }
    }

    public void takeDamage(int damage)
    {
        if (remainingInv <= 0)
        {
            hp -= damage;
            soundManager.instance.playSound("hurt01");
            remainingInv = invTime + StatBoosts.invisBoost;
        }
    }
}