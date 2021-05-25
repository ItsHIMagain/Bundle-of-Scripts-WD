using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPopup : MonoBehaviour
{
    public gunController gunController;

    public GameObject IcePopUp;
    public GameObject FirePopUp;
    public GameObject ShockPopUp;

    private void Update()
    {
        switch(gunController.effect)
        {
            case 0:
                IcePopUp.SetActive(false);
                FirePopUp.SetActive(false);
                ShockPopUp.SetActive(false);
                break;
            case 1:
                ShockPopUp.SetActive(true);
                IcePopUp.SetActive(false);
                FirePopUp.SetActive(false);
                break;
            case 2:
                FirePopUp.SetActive(true);
                ShockPopUp.SetActive(false);
                IcePopUp.SetActive(false);
                break;
            case 3:
                IcePopUp.SetActive(true);
                FirePopUp.SetActive(false);
                ShockPopUp.SetActive(false);
                break;
        }
    }

}
