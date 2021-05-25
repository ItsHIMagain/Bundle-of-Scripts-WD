using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class cashManager : MonoBehaviour
{
    public static cashManager instance;

    public int minorCur;
    public int mediumCur;
    public int majorCur;

    public static int totalCur;
    public int unsafeCur;

    public Text minorCurT;
    public Text mediumCurT;
    public Text majorCurT;

    public Text totalCurText;
    public Text unsafeCurText;

    public bool isShop;

    private void Start()
    {
        //totalCur += 100000000; //Just for debugging

        if(isShop == false)
        {
            addFunds(3, 0);
            totalCurText.text = PlayerPrefs.GetInt("cash").ToString();
        }
        else
        {
            majorCurT.text = ":" + totalCur.ToString();
            saveManager.instance.loadShop();
        }
        instance = this;
    }

    void Update() {
       if(unsafeCurText != null){
           unsafeCurText.text = "+ " + unsafeCur.ToString();
       } 
    }

    public void UpdateTotal()
    {
        majorCurT.text = ":" + totalCur.ToString();
    }

    public void addFunds(int type, int value)
    {
        unsafeCur += value;
        switch (type)
        {
            case 0:
                minorCur++;
                break;
            case 1:
                mediumCur++;
                break;
            case 2:
                majorCur++;
                break;
        }

        minorCurT.text = ":" + minorCur.ToString();
        mediumCurT.text = ":" + mediumCur.ToString();
        majorCurT.text = ":" + majorCur.ToString();
    }

    public void reduceFunds(int type, int value)
    {
        switch (type)
        {
            case 0:
                minorCur -= value;
                break;
            case 1:
                mediumCur -= value;
                break;
            case 2:
                majorCur -= value;
                break;
        }

        minorCurT.text = ":" + minorCur.ToString();
        mediumCurT.text = ":" + mediumCur.ToString();
        majorCurT.text = ":" + majorCur.ToString();
    }

    public void exitScene(int end){
        //END: 0 = leaves, 1 = Dies;

        if(end == 0){
            totalCur += unsafeCur;
            unsafeCur = 0;
        } else {
            totalCur += unsafeCur/4;
            unsafeCur = 0;
        }
    }
}
