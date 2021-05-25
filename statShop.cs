using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class statShop : MonoBehaviour
{
    public Text text;
    public int upgradeType; //1:hp 2:Bullet Dmg 3:Invincibility 4:DashDelay 5:DashDistance 6:DashDamage
    public string upgradeName;
    public float currentValue;
    public bool precentile;
    public int startCost;
    public int cost;
    public float costMultiplierMultiplier;
    public float costMultiplier = 1;

    public int maxPurchase;
    public int currentPurchases;

    public SpriteRenderer sprite;
    public Sprite[] sprites;


    private void Start()
    {
        if(cost == 0)
        {
            cost = startCost;
        }
        text.enabled = false;
        sprite.sprite = sprites[upgradeType-1];
        updateText();
    }

    public void updateText()
    {
        switch (upgradeType)
        {
            case 1:
                upgradeName = "HP up";
                currentValue = StatBoosts.hpBoost + 3;
                break;
            case 2:
                upgradeName = "Gun Damage";
                currentValue = StatBoosts.gunDmgBoostPrecent;
                precentile = true;
                break;
            case 3:
                upgradeName = "Invincibility Time";
                currentValue = StatBoosts.invisBoost + 0.5f;
                break;
            case 4:
                upgradeName = "Dash delay";
                currentValue = StatBoosts.dashDelayReductionProcent;
                precentile = true;
                break;
            case 5:
                upgradeName = "Dash Distance";
                currentValue = StatBoosts.dashDistBoostProcent;
                precentile = true;
                break;
            case 6:
                upgradeName = "Dash Damage";
                currentValue = StatBoosts.dashDmgBoostProcent;
                precentile = true;
                break;
        }



        if (precentile == true)
        {
            text.text = upgradeName + "\n" + "Cost: " + cost + "\n" + Mathf.RoundToInt(currentValue * 100) + "%";
        } else
        {
            text.text = upgradeName + "\n" + "Cost: " + cost + "\n" + currentValue;
        }
    }

    void purchase()
    {
        if(maxPurchase == 0 || maxPurchase > currentPurchases)
        {
            currentPurchases++;
            if(cost + (Mathf.CeilToInt(Mathf.RoundToInt((cost * costMultiplier) * (costMultiplier / 1.9f)) / 10) * 5) < 2500)
            {
                //costMultiplier += costMultiplierMultiplier - (((float)currentPurchases + 1) / (20 + currentPurchases));
                //Debug.Log(costMultiplierMultiplier - (((float)currentPurchases+1) / (20+currentPurchases)));
                int tempCost = Mathf.RoundToInt((cost * costMultiplier) * (costMultiplier / 1.9f));
                cost = cost + (Mathf.CeilToInt(tempCost/10)*5);
            } else
            {
                cost = 2500;
            }
            cashManager.instance.UpdateTotal();
            saveManager.instance.saveShop();
            saveManager.instance.saveCash();

            switch (upgradeType)
            {
                case 1:
                    StatBoosts.hpBoost++;
                    HpContainers.instance.maxHp++;
                    HpContainers.instance.hp++;
                    break;
                case 2:
                    StatBoosts.gunDmgBoostPrecent += 0.1f;
                    break;
                case 3:
                    StatBoosts.invisBoost += 0.1f;
                    break;
                case 4:
                    StatBoosts.dashDelayReductionProcent -= 0.05f;
                    break;
                case 5:
                    StatBoosts.dashDistBoostProcent += 0.05f;
                    break;
                case 6:
                    StatBoosts.dashDmgBoostProcent += 0.05f;
                    break;
            }
            saveManager.instance.saveStats();
            saveManager.instance.savePurchases();
            updateText();
    }
    }

    private void Update()
    {
        if(text.enabled == true && Input.GetKeyDown(KeyCode.E) && cost <= cashManager.totalCur)
        {
            cashManager.totalCur -= cost;
            purchase();
        }

        if(cost > cashManager.totalCur || maxPurchase == currentPurchases && maxPurchase != 0)
        {
            text.color = new Color32(147, 17, 17, 255);
        } else
        {
            text.color = new Color32(214, 214, 214, 255);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            text.enabled = true;
            updateText();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            text.enabled = false;
        }
    }
}
