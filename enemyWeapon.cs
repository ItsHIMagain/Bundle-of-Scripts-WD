using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyWeapon : MonoBehaviour
{
    private GameObject player;

    private float remainingShotDelay;

    public Transform shotSpot;

    private Animator anim;

    public GameObject projectile;
    public Transform gunSpot;

    public int bullets;
    public float reloadTime;
    private int remainingBullets;

    public LayerMask layerMask;

    public bool hasSight;
    
    public GameObject muzzle;

    [Header("Projectile Stats")]

    public bool doesntUseStats = false;

    public float shotDelay;
    public float speed;
    public int damage;
    public float spread;
    public float lifetime;

    public bool onlyforSight = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        remainingShotDelay = Random.Range(0,shotDelay);
        anim = GetComponent<Animator>();
        if(bullets != 0)
        {
            remainingBullets = bullets-1;
        }
    }

    private void Update()
    {

        Vector3 difference = player.transform.position - transform.position;
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);

        //See if player is behind walls
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, player.transform.position-transform.position, Vector3.Distance(transform.position, player.transform.position), layerMask);
        if(hit2D.collider != null)
        {
            Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * Vector3.Distance(transform.position, player.transform.position), Color.red);
            hasSight = false;
        }
        if(hit2D.collider == null)
        {
            Debug.DrawRay(transform.position, (player.transform.position - transform.position).normalized * Vector3.Distance(transform.position, player.transform.position), Color.green);
            hasSight = true;
        }

        if(remainingBullets == -1)
        {
            StartCoroutine("reload");
            remainingBullets = -2;
        }



        if (remainingShotDelay > 0 && hasSight == true && onlyforSight == false)
        {
            remainingShotDelay -= Time.deltaTime;
        } else if (remainingShotDelay > 0 && hasSight == false && onlyforSight == false)
        {
            remainingShotDelay -= Time.deltaTime/2;
        }
        else if (hasSight == true && remainingBullets >= 0 && onlyforSight == false)
        {
            remainingShotDelay = shotDelay;
            if(muzzle != null){
                Instantiate(muzzle, shotSpot.position, transform.rotation);
            }
            var latestBullet = Instantiate(projectile, gunSpot.position, transform.rotation);
            if(bullets != 0)
            {
                remainingBullets--;
            }
            if(doesntUseStats == false){
                anim.Play("enemyWeaponShot");
                if (latestBullet.GetComponent<enemyProjectile>() == true)
                {
                    latestBullet.GetComponent<enemyProjectile>().speed = speed;
                    latestBullet.GetComponent<enemyProjectile>().damage = damage;
                    latestBullet.GetComponent<enemyProjectile>().spread = spread;
                    latestBullet.GetComponent<enemyProjectile>().lifetime = lifetime;
                } else if (latestBullet.GetComponent<enemyShotGunController>() == true)
                {
                    latestBullet.GetComponent<enemyShotGunController>().speed = speed;
                    latestBullet.GetComponent<enemyShotGunController>().damage = damage;
                    latestBullet.GetComponent<enemyShotGunController>().spread = spread;
                    latestBullet.GetComponent<enemyShotGunController>().lifetime = lifetime;
                }
            }
        }
    }

    IEnumerator reload()
    {
        yield return new WaitForSeconds(reloadTime);
        remainingBullets = bullets - 1;
    }
}
