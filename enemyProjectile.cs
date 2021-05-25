using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemyProjectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public float spread;
    private float finalSpread;

    public float lifetime;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        finalSpread = Random.Range(-spread, spread);
        lifetime = lifetime + (Random.Range(-0.03f, 0.12f));
        Invoke("DestroyProjectile", lifetime);
        soundManager.instance.playSound("shoot01");
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
        if (collision.tag == "Player")
        {
            HpContainers.instance.takeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
