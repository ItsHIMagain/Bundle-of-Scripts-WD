using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public GameObject lever;
    public Animator anim;
    public Animator transition;
    public float transitionTime = 1f;
    public soundManager soundManager;

    private enum State {idle, turnLeft, turnRight}
    private State state = State.idle;

    public bool leverAccessible = false;

    public bool isShop;
    public bool isTutorial;

    public bool isBoss;

    public playerController playerScript;

    public bool disabled = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<playerController>();
    }

    private void Update()
    {
        if(disabled == true){
            leverAccessible = false;
        }
        StartCoroutine(AnimationState());
        anim.SetInteger("state", (int)state);
    }

    IEnumerator AnimationState()
    {

        if(isTutorial == true){
            if (Input.GetKeyDown(KeyCode.E) && state == State.idle && leverAccessible == true)
            {
                cashManager.totalCur = 0;
                //playerScript.alive = false;
                cashManager.totalCur = 0;
                state = State.turnLeft;
                saveManager.instance.deleteSave();
                soundManager.playSound("transition01d");
                transition.SetTrigger("Start");

                yield return new WaitForSeconds(transitionTime);
                yield return new WaitForSeconds(0.6f);
                SceneManager.LoadScene("Shop");
                SceneManager.LoadScene(2);
                state = State.idle;
                yield break;
            }
        }
        else if (isShop == true)
        {
            if (Input.GetKeyDown(KeyCode.E) && state == State.idle && leverAccessible == true)
            {
                playerScript.alive = false;
                cashManager.totalCur = 0;
                state = State.turnLeft;
                soundManager.playSound("transition01d");
                transition.SetTrigger("Start");

                //yield return new WaitForSeconds(transitionTime);
                yield return new WaitForSeconds(0.6f);
                SceneManager.LoadScene("Game");
                state = State.idle;
            }
            yield break;
        } else if(isBoss == true){
            if (Input.GetKeyDown(KeyCode.E) && state == State.idle && leverAccessible == true)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
                {
                    playerScript.alive = false;
                    playerScript.dashDisabled = true;
                }
                state = State.turnLeft;
                yield return new WaitForSeconds(0.6f);
                if (GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
                {
                    newLevel.level = 30;
                    newLevel.instance.StartCoroutine("stageGen");
                }
                yield return new WaitForSeconds(0.6f);
                if (GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
                {
                    playerScript.alive = true;
                    playerScript.dashDisabled = false;
                }
                player.transform.position = new Vector3(0.5f, 0.5f, 0);
                state = State.idle;
            } else if (Input.GetKeyDown(KeyCode.Q) && state == State.idle && leverAccessible == true)
            {
                state = State.turnRight;
                if (GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
                {
                    playerScript.alive = false;
                    playerScript.dashDisabled = true;
                }
                yield return new WaitForSeconds(0.6f);
                if (GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
                {
                    soundManager.playSound("transition01d");
                    transition.SetTrigger("Start");

                    yield return new WaitForSeconds(transitionTime);
                    newLevel.instance.resetLevels();
                    cashManager.instance.exitScene(0);
                    SceneManager.LoadScene("Shop");
                }

                state = State.idle;
            }
            yield break;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.E) && state == State.idle && leverAccessible == true)
            {
                if (GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
                {
                    playerScript.alive = false;
                    playerScript.dashDisabled = true;
                }
                state = State.turnLeft;
                yield return new WaitForSeconds(0.6f);
                if (GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
                {
                    newLevel.instance.StartCoroutine("stageGen");
                }
                yield return new WaitForSeconds(0.6f);
                if (GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
                {
                    playerScript.alive = true;
                    playerScript.dashDisabled = false;
                }
                state = State.idle;
            }

            else if (Input.GetKeyDown(KeyCode.Q) && state == State.idle && leverAccessible == true)
            {
                state = State.turnRight;
                if (GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
                {
                    playerScript.alive = false;
                    playerScript.dashDisabled = true;
                }
                yield return new WaitForSeconds(0.6f);
                if (GameObject.FindGameObjectsWithTag("enemy").Length <= 0)
                {
                    soundManager.playSound("transition01d");
                    transition.SetTrigger("Start");

                    yield return new WaitForSeconds(transitionTime);
                    newLevel.instance.resetLevels();
                    cashManager.instance.exitScene(0);
                    SceneManager.LoadScene("Shop");
                }

                state = State.idle;
            }
            yield break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && disabled == false)
        {
            leverAccessible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            leverAccessible = false;
        }
    }
}
