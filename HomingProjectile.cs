using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    private GameObject player;
    public ParticleSystem fire;
    public bool isAlive = true;
    public float lifetime;
    public bool destroyWithProjectile;
    private float invis = 0.5f;
    public int damage = 1;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
     player = GameObject.FindGameObjectWithTag("Player");   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isAlive == true){
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
        speed += 0.5f * Time.deltaTime;
        lifetime -= Time.deltaTime;
        }
        if(lifetime <= 0)
        {
            isAlive = false;
            StartCoroutine(destroy());
        }
        if(invis > 0)
        {
            invis -= Time.deltaTime;
        }
       
    }

    private IEnumerator destroy(){
        fire.Stop();
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Wall" && isAlive == true && invis <= 0){
            isAlive = false;
            StartCoroutine(destroy());
        }
        if(other.tag == "Player" && isAlive == true){
            HpContainers.instance.takeDamage(damage);
            isAlive = false;
            StartCoroutine(destroy());
        }
        if (other.tag == "Projectile" && isAlive == true && destroyWithProjectile == true)
        {
            Destroy(other.gameObject);
            isAlive = false;
            StartCoroutine(destroy());
        }
    }
}
