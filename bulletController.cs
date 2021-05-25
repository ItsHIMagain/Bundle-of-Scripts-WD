using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public float speed;
    public float damage;
    public float spread;
    private float finalSpread;
    public float recoil;
    public int effect; //0 = none, 1 = Shock, 2 = Fire, 3 = Freeze

    public float lifetime;

    private Rigidbody2D rb;

    cameraController Cam;
    Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Cam = FindObjectOfType<cameraController>();
        shake();
        rb = GetComponent<Rigidbody2D>();
        finalSpread = Random.Range(-spread, spread);
        lifetime = lifetime + (Random.Range(-0.03f, 0.12f));
        Invoke("DestroyProjectile", lifetime);
        soundManager.instance.playSound("shoot01");
    }

    void shake()
    {
        Cam.Shake((player.position - transform.position).normalized, recoil, 0.025f); //call camera shake for recoil
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        transform.Translate(Vector2.up * finalSpread * Time.deltaTime);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
