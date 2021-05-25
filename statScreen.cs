using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class statScreen : MonoBehaviour
{
    public float shotspeed;
    public float speed;
    public float damage;
    public float spread;
    public float recoil;
    public float lifetime;
    public int effect;
    public int ammoType;
    public int ammoCount;

    public Text spdT;
    public Text dmgT;
    public Text recT;
    public Text durT;
    public Text sprT;
    public Text shsT;
    public Text ammoTypeCount;

    public SpriteRenderer effS; //0 = none, 1 = Shock, 2 = Fire, 3 = Freeze
    public Sprite[] effectVisuals;

    // Update is called once per frame
    void Update()
    {

        spdT.text = ((Mathf.Round(speed * 10.0f) * 0.1f) * StatBoosts.gunDmgBoostPrecent).ToString();
        dmgT.text = (Mathf.Round(damage * 10.0f) * 0.1f).ToString();
        recT.text = (Mathf.Round(recoil * 10.0f) * 0.1f).ToString();
        durT.text = (Mathf.Round(lifetime * 10.0f) * 0.1f).ToString();
        sprT.text = (Mathf.Round(spread * 10.0f) * 0.1f).ToString();
        shsT.text = (Mathf.Round(shotspeed * 10.0f) * 0.1f).ToString();

        switch (ammoType){
            case 0:
                ammoTypeCount.text = "-";
                break;
            case 1:
                ammoTypeCount.text = "S" + ammoCount.ToString();
                break;
            case 2:
                ammoTypeCount.text = "M" + ammoCount.ToString();
                break;
            case 3:
                ammoTypeCount.text = "L" + ammoCount.ToString();
                break;
        }


        switch (effect)
        {
            case 0:
                effS.sprite = null;
                break;
            case 1:
                effS.sprite = effectVisuals[0];
                break;
            case 2:
                effS.sprite = effectVisuals[1];
                break;
            case 3:
                effS.sprite = effectVisuals[2];
                break;
        }

    }
}
