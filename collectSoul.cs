using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectSoul : MonoBehaviour
{
    private bool pull = false;
    private Rigidbody2D rb;
    private GameObject player;
    private float power;

    public int value;
    private int type = 0; //0 Minor, 1 Medium, 2 Large
    public Sprite[] typeImg;

    void Start()
    {
        if(value >= 30)
        {
            type = 2;
            gameObject.GetComponent<SpriteRenderer>().sprite = typeImg[2];
        } else if (value >= 15)
        {
            type = 1;
            gameObject.GetComponent<SpriteRenderer>().sprite = typeImg[1];
        } else
        {
            type = 0;
            gameObject.GetComponent<SpriteRenderer>().sprite = typeImg[0];
        }


        player = GameObject.FindGameObjectWithTag("Player");
        rb = gameObject.GetComponent<Rigidbody2D>();

        Vector2 direction = new Vector2((float)Random.Range(-1000, 1000), (float)Random.Range(-1000, 1000));

        float force = (float)Random.Range(-5, 5);
        rb.AddForce(direction * force);
        StartCoroutine(collectSouls());
    }

    IEnumerator collectSouls()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.8f));
        pull = true;
        power = 2;
    }

    private void Update()
    {
        if(pull == true)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            rb.AddForce((player.transform.position - transform.position) * power);
            power += 0.2f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("hpManager").GetComponent<cashManager>().addFunds(type, value);
            Destroy(gameObject);
        }
    }
}
