using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunController : MonoBehaviour
{
    public float offset;
    private GameObject player;

    private float remainingShotDelay;

    public Transform gunSpot;

    public Animator anim;

    public static gunController instance;

    public SpriteRenderer spriteControl;
    public Sprite gunSprite;

    public GameObject muzzle;

    [Header("Gun Stats")]
    public GameObject projectile;
    public float shotDelay;
    public int ammoType; // 1 = small, 2 = medium, 3 = large
    public int ammoTypeCount;

    [Header("Projectile Stats")]

    public float speed;
    public float damage;
    public float spread;
    public float recoil;
    public float lifetime;
    public int effect;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        instance = this;
    }

    void Update()
    {
        //Change Sprite
        if(spriteControl != null && spriteControl.sprite != gunSprite)
        {
            spriteControl.sprite = gunSprite;
        }


        //Rotate toward mouse
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotz = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, rotz - offset);

        //Stay attached to player
        transform.position = player.transform.position;

        //Rotate
        if(rotz > 90 || rotz < -90)
        {
            transform.Rotate(180, 0, 0);
        } else
        {
            transform.Rotate(0, 0, 0);
        }

        //Shoot
        if (Input.GetKey(KeyCode.Mouse0) && remainingShotDelay <= 0)
        {
            if(ammoType == 1 && cashManager.instance.minorCur >= ammoTypeCount)
            {
                cashManager.instance.reduceFunds(0, ammoTypeCount);
                shoot();
            }
            if (ammoType == 2 && cashManager.instance.mediumCur >= ammoTypeCount)
            {
                cashManager.instance.reduceFunds(1, ammoTypeCount);
                shoot();
            }
            if (ammoType == 3 && cashManager.instance.majorCur >= ammoTypeCount)
            {
                cashManager.instance.reduceFunds(2, ammoTypeCount);
                shoot();
            }
        }

        if(remainingShotDelay > 0)
        {
            remainingShotDelay -= Time.deltaTime;
        }
    }

    private void shoot()
    {
        anim.Play("GunShot");
        Instantiate(muzzle, gunSpot.position, transform.rotation);
        remainingShotDelay = shotDelay;
        var latestBullet = Instantiate(projectile, gunSpot.position, transform.rotation);
        if (latestBullet.GetComponent<bulletController>() == true)
        {
            bulletController lastBulletStats = latestBullet.GetComponent<bulletController>();
            lastBulletStats.speed = speed;
            lastBulletStats.damage = damage * StatBoosts.gunDmgBoostPrecent;
            lastBulletStats.spread = spread;
            lastBulletStats.lifetime = lifetime;
            lastBulletStats.recoil = recoil;
            lastBulletStats.effect = effect;
        }
        if (latestBullet.GetComponent<shotGunController>() == true)
        {
            shotGunController lastBulletStats = latestBullet.GetComponent<shotGunController>();
            lastBulletStats.speed = speed;
            lastBulletStats.damage = damage * StatBoosts.gunDmgBoostPrecent;
            lastBulletStats.spread = spread;
            lastBulletStats.lifetime = lifetime;
            lastBulletStats.recoil = recoil;
            lastBulletStats.effect = effect;
        }

    }
}
