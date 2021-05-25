using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class playerController : MonoBehaviour
{
    Rigidbody2D body;
    Animator anim;

    public float horizontal;
    public float vertical;
    float moveLimiter = 0.7f;

    public float speed = 20.0f;
    public int rng;
    public soundManager soundManager;
    public Animator transition;

    Vector3 localscale;
    public bool facingLeft;

    // State machine's states
    private enum State {idle, running, death}
    private State state = State.idle;
    public bool alive = true;

    public float dashDistance;
    private float distToWall;
    public float dashDelay;
    private float remainingDashDelay;
    public LayerMask layerMask;

    public GameObject dashEffect;

    public bool dashDisabled;


    public SpriteRenderer dashIndicator;

    public GameObject destinationIndicator;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        localscale = transform.localScale;

        StartCoroutine(NumberGen());
    }

    void Update()
    {
        dashHandler();
        moveHandler();

        if(alive)
        {
            AnimationState();
        }

        anim.SetInteger("state", (int) state);

        //Dash
        /*
        if (Input.GetKey(KeyCode.Mouse1))
        {
            dashTime = startDashTime;
            Debug.Log(body.velocity);
        }

        if (dashTime > 0)
        {
            body.velocity = Vector2.left * dashSpeed;
            Debug.Log(body.velocity);
            dashTime -= Time.deltaTime;
            dashing = true;
        } else if (dashing == true)
        {
            dashing = false;
        }*/
    }

    private void moveHandler()
    {
        // Gives a value between -1 and 1
        transform.localScale = localscale;

        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).x > transform.position.x && facingLeft == false || Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x && facingLeft == true)
        {
            localscale.x *= -1;
            facingLeft = !facingLeft;
        }
    }

    void FixedUpdate()
    {
        if (horizontal != 0 && vertical != 0) // Check for diagonal movement
        {
            // limit movement speed diagonally, so you move at 70% speed
            horizontal *= moveLimiter;
            vertical *= moveLimiter;
        }
        if(alive == true){
            body.velocity = new Vector2(horizontal * speed, vertical * speed);
        } else {
            body.velocity = new Vector2(0, 0);
        }
    }
    IEnumerator NumberGen()
    {
        while (true)
        {
                rng = Random.Range(1, 9);
                yield return new WaitForSeconds(0.3f);
        }
    }

    private void AnimationState()
    {
        if(Mathf.Abs(body.velocity.x) > 2f || Mathf.Abs(body.velocity.y) > 2f)
        {
            state = State.running;

            switch (rng)
            {
                case 0:
                    break;
                case 1:
                    soundManager.playSound("footstep_gravel01");
                    //rng = 0;
                    break;
                case 2:
                    soundManager.playSound("footstep_gravel02");
                    //rng = 0;
                    break;
                case 3:
                    soundManager.playSound("footstep_gravel03");
                    //rng = 0;
                    break;
                case 4:
                    soundManager.playSound("footstep_gravel04");
                    //rng = 0;
                    break;
                case 5:
                    soundManager.playSound("footstep_gravel05");
                    //rng = 0;
                    break;
                case 6:
                    soundManager.playSound("footstep_stone01");
                    //rng = 0;
                    break;
                case 7:
                    soundManager.playSound("footstep_stone02");
                    //rng = 0;
                    break;
                case 8:
                    soundManager.playSound("footstep_stone03");
                    //rng = 0;
                    break;
                case 9:
                    soundManager.playSound("footstep_stone04");
                    //rng = 0;
                    break;
            }
        }

        else
        {
            state = State.idle;
        }
    }

    public void PlayerDeath()
    {
        newLevel.instance.resetLevels();
        alive = false;
        state = State.death;
        StartCoroutine(deathDelay());
    }

    IEnumerator deathDelay(){
        yield return new WaitForSeconds(3f);
        soundManager.playSound("transition01d");
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("shop");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "damageObject"){
            GameObject.FindGameObjectWithTag("hpManager").GetComponent<HpContainers>().takeDamage(1);
        }
    }

    private void dashHandler()
    {
    if (remainingDashDelay > 0)
    {
        remainingDashDelay -= Time.deltaTime;
    }
            

        if (remainingDashDelay > 0){
            dashIndicator.color = new Color32(220,0,0,83);
        } else{
            dashIndicator.color = new Color32(0,0,0,83);
        }

        if(dashDisabled == false){
            if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0), dashDistance * StatBoosts.dashDistBoostProcent, layerMask);
            Debug.DrawRay(transform.position, new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * dashDistance * StatBoosts.dashDistBoostProcent, Color.blue);
            
           if (hit2D.collider == null)
                {
                    Debug.DrawRay(transform.position, new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * dashDistance * StatBoosts.dashDistBoostProcent, Color.blue);
                    distToWall = dashDistance * StatBoosts.dashDistBoostProcent;
                }
                else
                {
                    Debug.DrawRay(transform.position, new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * dashDistance * StatBoosts.dashDistBoostProcent, Color.black);
                    distToWall = 0;
                    destinationIndicator.transform.position = hit2D.point;
                }

                if (Input.GetKeyDown(KeyCode.Mouse1) && remainingDashDelay <= 0 || Input.GetKeyDown(KeyCode.LeftShift) && remainingDashDelay <= 0 )
                {
                    Vector3 beforeDashPos = transform.position;
                    if(distToWall != 0){
                        transform.position += new Vector3(horizontal, vertical, 0) * distToWall;
                    } else {
                        transform.position = destinationIndicator.transform.position;
                    }

                    soundManager.playSound("dash_sound");
                    GameObject dashEffectTransform = Instantiate(dashEffect, beforeDashPos, Quaternion.identity);

                    Vector3 vectorToTarget = transform.position - dashEffectTransform.transform.position;
                    float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                    Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                    dashEffectTransform.transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 1000);

                    dashEffectTransform.transform.localScale = new Vector3(Vector3.Distance(beforeDashPos, transform.position) /2.5f, 0.5f, 1f);

                    remainingDashDelay = dashDelay * StatBoosts.dashDelayReductionProcent;
                }
            
            /*
            if (Input.GetKeyDown(KeyCode.Mouse1) && remainingDashDelay <= 0)
            {
                RaycastHit2D hit2D = Physics2D.Raycast(transform.position, new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0), dashDistance * StatBoosts.dashDistBoostProcent, layerMask);

                Vector3 beforeDashPos = transform.position;

                if (hit2D.collider == null)
                {
                    Debug.DrawRay(transform.position, new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * dashDistance * StatBoosts.dashDistBoostProcent, Color.blue);
                    distToWall = dashDistance * StatBoosts.dashDistBoostProcent;
                }
                else
                {
                    Debug.DrawRay(transform.position, new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0) * dashDistance * StatBoosts.dashDistBoostProcent, Color.black);
                    distToWall = Vector3.Distance(hit2D.collider.transform.position, transform.position);
                }

                transform.position += new Vector3(horizontal, vertical, 0) * (distToWall - (dashCorrection * StatBoosts.dashDistBoostProcent));

                GameObject dashEffectTransform = Instantiate(dashEffect, beforeDashPos, Quaternion.identity);

                Vector3 vectorToTarget = transform.position - dashEffectTransform.transform.position;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                dashEffectTransform.transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 1000);

                dashEffectTransform.transform.localScale = new Vector3(distToWall / 3, 0.5f, 1f);

                remainingDashDelay = dashDelay * StatBoosts.dashDelayReductionProcent;
            }
            else if (remainingDashDelay > 0)
            {
                remainingDashDelay -= Time.deltaTime;
            }
            */
        }
        }
    }
}
