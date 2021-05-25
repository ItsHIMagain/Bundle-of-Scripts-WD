using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public float hp;
    public float speed;
    public bool facingRight;

    public bool shocked;
    public bool onFire;
    public float fireDelay;
    public bool stillOnFire;
    public bool frozen;
    public float shockDur;
    public bool stillShocked;
    public GameObject shockParticle;
    public float fireDur;
    public float iceDur;

    public GameObject fireParticle;
    public GameObject iceParticle;

    public AIPath pf;
    public AIDestinationSetter ad;

    public bool contactDamage;
    public float stopDistance;
    private float distanceFromPlayer;
    public bool checkSight;
    private bool sight;
    private GameObject player;

    public bool ghost;

    public bool charging;
    public float chargeInitRange;
    public float chargeSpeed;
    private Vector3 ChargeDirection;

    public GameObject stunEffect;
    private bool dontchangeDir;

    private void Start()
    {
        if(ghost == false)
        {
            ad.target = null;
        }
        ad.target = GameObject.FindGameObjectWithTag("Player").transform;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if(ghost == true && stillShocked == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
        }
        pf.maxSpeed = speed;

        //Flip
        if(transform.position.x > player.transform.position.x && facingRight == false && dontchangeDir == false)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            facingRight = true;

            foreach (Transform child in transform)
            {
                if (child.tag == "enemyWeapon")
                    child.localScale = new Vector3(child.transform.localScale.x * -1, child.transform.localScale.y * -1, child.transform.localScale.z);
            }
        }

        if (transform.position.x < player.transform.position.x && facingRight == true && dontchangeDir == false)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            facingRight = false;

            foreach (Transform child in transform)
            {
                if (child.tag == "enemyWeapon")
                    child.localScale = new Vector3(child.transform.localScale.x * -1, child.transform.localScale.y * -1, child.transform.localScale.z);
            }
        }

        //Kill
        if (hp <= 0)
        {
            kill();
        }

        //Stop if distance = x
        if (stopDistance != 0)
        {
            distanceFromPlayer = Vector2.Distance(player.transform.position, transform.position);
        }



        if (checkSight == false)
        {
            if (stopDistance != 0 && stopDistance >= distanceFromPlayer)
            {
                pf.canMove = false;
            }
            else if (stopDistance != 0)
            {
                pf.canMove = true;
            }
        } else
        {
            sight = gameObject.GetComponentInChildren<enemyWeapon>().hasSight;
            if (stopDistance != 0 && stopDistance >= distanceFromPlayer && sight == true)
            {
                pf.canMove = false;
            }
            else if (stopDistance != 0)
            {
                pf.canMove = true;
            }

            //Charging
            if (chargeInitRange != 0 && chargeInitRange >= Vector2.Distance(player.transform.position, transform.position) && charging == false && pf.canMove == true &&
                sight == true)
            {
                Debug.Log(Vector2.Distance(player.transform.position, transform.position));
                charging = true;
                dontchangeDir = true;
                ChargeDirection = (player.transform.position - transform.position).normalized;
            }

            if (charging == true)
            {
                pf.canMove = false;
                dontchangeDir = true;
                transform.position += ChargeDirection * chargeSpeed * Time.deltaTime;
            }
        }

        //Effects
        if (shocked == true || onFire == true || frozen == true)
        {
            int rng = Random.Range(1, 100);

            if (rng <= 10 && shocked == true)
            {
                rng = 100;
                pf.canMove = false;
                stillShocked = true;
                Instantiate(shockParticle, transform.position, Quaternion.identity);
                StartCoroutine(shock());
            }
            if (rng <= 15 && onFire == true)
            {
                rng = 100;
                StartCoroutine(fire());
                fireDelay++;
                stillOnFire = true;
            }
            if (rng <= 20 && frozen == true)
            {
                rng = 100;
                speed /= 2;
                StartCoroutine(freeze());
            }
            shocked = false;
            onFire = false;
            frozen = false;
        }

        if(fireDelay > 0)
        {
            fireDelay -= Time.deltaTime;
        } else if (stillOnFire == true)
        {
            hp -= 5;
            gameObject.GetComponent<EnemyAnimator>().damageAnimation();
            fireDelay++;
        }
    }

    IEnumerator shock()
    {
        yield return new WaitForSeconds(shockDur);
        pf.canMove = true;
        stillShocked = false;
    }

    IEnumerator stun(float stunDur)
    {
        dontchangeDir = true;
        Instantiate(stunEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(stunDur);
        pf.canMove = true;
        dontchangeDir = false;
    }

    IEnumerator fire()
    {
        GameObject fire = Instantiate(fireParticle, transform.position, transform.rotation) as GameObject;
        fire.transform.parent = gameObject.transform;
        ParticleSystem parts = fire.GetComponent<ParticleSystem>();
        yield return new WaitForSeconds(fireDur);
        stillOnFire = false;
        Destroy(fire);
        yield return 0;
    }


    IEnumerator freeze()
    {
        GameObject ice = Instantiate(iceParticle, transform.position, transform.rotation) as GameObject;
        ice.transform.parent = gameObject.transform;
        ParticleSystem parts = ice.GetComponent<ParticleSystem>();
        Destroy(ice, iceDur);
        yield return new WaitForSeconds(iceDur);
        speed *= 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall" && charging == true || collision.tag == "lowerWall" && charging == true)
        {
            charging = false;
            StartCoroutine("stun", 3);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "damageObject")
        {
            hp -= 3f;
        }

        if(collision.tag == "Projectile")
        {
            Destroy(collision.gameObject);
            hp -= collision.GetComponent<bulletController>().damage;
            switch (collision.GetComponent<bulletController>().effect)
            {
                case 0:
                    break;
                case 1:
                    shocked = true;
                    break;
                case 2:
                    onFire = true;
                    break;
                case 3:
                    frozen = true;
                    break;
            }
        }

        if (collision.gameObject.tag == "Player" && contactDamage == true)
        {
            GameObject.FindGameObjectWithTag("hpManager").GetComponent<HpContainers>().takeDamage(1);
        }
    }

    public void kill()
    {
        if ((gameObject.GetComponent("soulDrop") as soulDrop) != null)
        {
            gameObject.GetComponent<soulDrop>().spawnSouls();
        }
        Destroy(gameObject);
    }
}
