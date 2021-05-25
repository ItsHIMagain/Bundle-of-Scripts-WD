using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class sign : MonoBehaviour
{
    public Text text;
    public float activationDistance;
    private float distance;

    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        distance = Vector2.Distance(player.transform.position, transform.position);

        if(text.enabled == true && distance > activationDistance){
            text.enabled = false;
        } else if(text.enabled == false && distance < activationDistance){
            text.enabled = true;
        }
    }
}
