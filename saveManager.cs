using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class saveManager : MonoBehaviour
{
    public static saveManager instance;
    public statShop[] shops; //0:hp 1:Bullet Dmg 2:Invincibility 3:DashDelay 4:DashDistance 5:DashDamage
    public bool shop = false;

    private void Start()
    {
        instance = this;
        loadStats();
        saveStats();
        loadPurchases();
        if(shop == true){
            cashManager.totalCur += PlayerPrefs.GetInt("cash");
            saveCash();
        }
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Save");
            saveShop();
            savePurchases();
            saveStats();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Load");
            //loadShop();
            loadStats();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Reset");
            deleteSave();
        }
        */

        if (StatBoosts.gunDmgBoostPrecent == 0)
        {
            StatBoosts.gunDmgBoostPrecent = 1;
        }
        if(StatBoosts.dashDelayReductionProcent == 0)
        {
            StatBoosts.dashDelayReductionProcent = 1;
        }
        if (StatBoosts.dashDmgBoostProcent == 0)
        {
            StatBoosts.dashDmgBoostProcent = 1;
        }
        if (StatBoosts.dashDistBoostProcent == 0)
        {
            StatBoosts.dashDistBoostProcent = 1;
        }
    }

    public void saveShop()
    {
        if(shops[0] != null)
        {
            PlayerPrefs.SetInt("HpCost", shops[0].cost);
            PlayerPrefs.SetInt("DamageCost", shops[1].cost);
            PlayerPrefs.SetInt("InviCost", shops[2].cost);
            PlayerPrefs.SetInt("DashDelayCost", shops[3].cost);
            PlayerPrefs.SetInt("DashDistCost", shops[4].cost);
            PlayerPrefs.SetInt("DashDmgCost", shops[5].cost);
        }
    }

    public void savePurchases()
    {
        if(shops[0] != null)
        {
            PlayerPrefs.SetInt("HpPurchases", shops[0].currentPurchases);
            PlayerPrefs.SetInt("DamagePurchases", shops[1].currentPurchases);
            PlayerPrefs.SetInt("InvisPurchases", shops[2].currentPurchases);
            PlayerPrefs.SetInt("DashDelayPurchases", shops[3].currentPurchases);
            PlayerPrefs.SetInt("DashDistPurchases", shops[4].currentPurchases);
            PlayerPrefs.SetInt("DashDmgPurchases", shops[5].currentPurchases);
        }
    }

    public void loadPurchases(){
        if(shops[0] != null)
        {
            shops[0].currentPurchases = PlayerPrefs.GetInt("HpPurchases");
            shops[1].currentPurchases = PlayerPrefs.GetInt("DamagePurchases"); 
            shops[2].currentPurchases = PlayerPrefs.GetInt("InvisPurchases");   
            shops[3].currentPurchases = PlayerPrefs.GetInt("DashDelayPurchases");       
            shops[4].currentPurchases = PlayerPrefs.GetInt("DashDistPurchases");          
            shops[5].currentPurchases = PlayerPrefs.GetInt("DashDmgPurchases");
        }
    }

    public void saveStats()
    {
        PlayerPrefs.SetInt("HpBoost", StatBoosts.hpBoost);
        PlayerPrefs.SetFloat("DmgBoost", StatBoosts.gunDmgBoostPrecent);
        PlayerPrefs.SetFloat("dashDmg", StatBoosts.dashDmgBoostProcent);
        PlayerPrefs.SetFloat("dashDistance", StatBoosts.dashDistBoostProcent);
        PlayerPrefs.SetFloat("dashDelay", StatBoosts.dashDelayReductionProcent);
        PlayerPrefs.SetFloat("InvisBoost", StatBoosts.invisBoost);
    }
    public void saveCash()
    {
        PlayerPrefs.SetInt("cash", cashManager.totalCur);
    }

    public void loadCash()
    {
        cashManager.totalCur = PlayerPrefs.GetInt("cash");
    }

    public void loadStats()
    {
        StatBoosts.hpBoost = PlayerPrefs.GetInt("HpBoost");
        StatBoosts.gunDmgBoostPrecent = PlayerPrefs.GetFloat("DmgBoost");
        StatBoosts.dashDmgBoostProcent = PlayerPrefs.GetFloat("dashDmg");
        StatBoosts.dashDistBoostProcent = PlayerPrefs.GetFloat("dashDistance");
        StatBoosts.dashDelayReductionProcent = PlayerPrefs.GetFloat("dashDelay");
        StatBoosts.invisBoost = PlayerPrefs.GetFloat("InvisBoost");
        if(shop == true){
        shops[0].updateText();
        shops[1].updateText();
        shops[2].updateText();
        shops[3].updateText();
        shops[4].updateText();
        shops[5].updateText();
        }
    }

    public void loadShop()
    {
        if (shops[0] != null)
        {
            shops[0].cost = PlayerPrefs.GetInt("HpCost");
            shops[0].updateText();
            //Debug.Log(PlayerPrefs.GetInt("HpCost"));
            shops[1].cost = PlayerPrefs.GetInt("DamageCost");
            shops[1].updateText();
            //Debug.Log(PlayerPrefs.GetInt("DamageCost"));
            shops[2].cost = PlayerPrefs.GetInt("InviCost");
            shops[2].updateText();
            //Debug.Log(PlayerPrefs.GetInt("InviCost"));
            shops[3].cost = PlayerPrefs.GetInt("DashDelayCost");
            shops[3].updateText();
            //Debug.Log(PlayerPrefs.GetInt("DashDelayCost"));
            shops[4].cost = PlayerPrefs.GetInt("DashDistCost");
            shops[4].updateText();
            //Debug.Log(PlayerPrefs.GetInt("DashDistCost"));
            shops[5].cost = PlayerPrefs.GetInt("DashDmgCost");
            shops[5].updateText();
            //Debug.Log(PlayerPrefs.GetInt("DashDmgCost"));
        }
    }

    public void deleteSave()
    {
        PlayerPrefs.DeleteAll();
    }
}
