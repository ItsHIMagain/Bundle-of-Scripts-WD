using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newLevel : MonoBehaviour
{
    public static newLevel instance;

    public static int level = 0;

    public Transition transitionAnim;

    public Camera cam;

    private void Start()
    {
        transitionAnim = GameObject.Find("Transition").GetComponent<Transition>();
        instance = this;
        cam.backgroundColor = new Color32(57, 57, 57, 255);
    }

    //StartCoroutine("stageGen");

    IEnumerator stageGen()
    {
        transitionAnim.StartAnimation();
        yield return new WaitForSeconds(0.9f);
        var objects = FindObjectsOfType<tag>();
        foreach (tag thing in objects)
        {
            if (thing.tags == "removeNewStage")
            {
                GameObject.Destroy(thing.gameObject);
            }
        }

        gameObject.GetComponent<LevelGenerator>().newStage();
        level++;

        if(level == 29){
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(113.5f, -15.75f, 0);
            GameObject.Find("BossController").GetComponent<bossAttackControl>().enabled = true;

            level = 15;
        }

        Debug.Log(level);

        if(level == 14)
        {
            cam.backgroundColor = new Color32(60,15,11,255);
        } else if (level == 31)
        {
            cam.backgroundColor = new Color32(59, 3, 3, 255);
        }
    }

    public void resetLevels(){
        level = 0;
    }
}
