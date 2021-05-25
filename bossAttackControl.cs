using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAttackControl : MonoBehaviour
{
    public soundManager soundManager;
    public GameObject enemySpawn;
    public Transform[] spawnspots;

    public Animator firewallAnim;

    public Animator bossAnim;

    public float hp;

    public GameObject boss;

    public bool enabled;

    public GameObject elevator;
    public GameObject firewall;

    public bool isdead;

    public SpriteRenderer[] sprites;

    void Update() {

        if(enabled == true){
            engageFirewall();
            StartCoroutine("makeAMove");
            enabled = false;
        }

        if(hp < 0 && isdead == false){
            death();
            isdead = true;
            elevator.GetComponent<SwitchController>().disabled = false;
        }
    }

    IEnumerator makeAMove(){
        if(boss != null && hp > 0){
        float frng = Random.Range(2.8f,4.5f);
           yield return new WaitForSeconds(frng);


        int rng = Random.Range(0,2);
        if(rng == 0 && isdead == false){
            summonEnemies();
        } else if(isdead == false) {
            flamethrower();
        }
        StartCoroutine("makeAMove"); 
        }
    }

    public void summonEnemies(){

        PlayBossSound(4);
        bossAnim.Play("BossSummon");

        for (int i = 0; i < spawnspots.Length; i++){
            Instantiate(enemySpawn, spawnspots[i].position, Quaternion.identity);
        }
    }

    public void engageFirewall(){
        var objects = FindObjectsOfType<tag>();
        soundManager.stopSound("ingame_music1");
        soundManager.stopSound("ingame_music2");
        soundManager.stopSound("ingame_music3");
        soundManager.stopSound("ingame_music4");
        soundManager.playSound("boss_music1");

        foreach (tag thing in objects)
        {
            if (thing.tags == "removeNewStage")
            {
                GameObject.Destroy(thing.gameObject);
            }
        }

        firewallAnim.Play("FIrewallFireSize");
    }

    public void flamethrower(){
        bossAnim.Play("BossFlamethrower");
        PlayBossSound(3);
    }

    public void death(){
        PlayBossSound(0);
        bossAnim.Play("BossDeath");

        PlayBossSound(1);
        PlayBossSound(2);
        StartCoroutine("destroy");
        
    }

    IEnumerator destroy(){
        yield return new WaitForSeconds(3.5f);
        Destroy(boss);
        Destroy(firewall);
        Destroy(gameObject);
    }

    IEnumerator damage(){
        for (int i = 0; i < sprites.Length; i++){
            sprites[i].color = new Color(255, 0, 0, 255);
        }
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < sprites.Length; i++){
            sprites[i].color = new Color(255, 255, 255, 255);
        }
        yield break;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Projectile"){
            hp -= other.GetComponent<bulletController>().damage;
            StartCoroutine("damage");
            Destroy(other.gameObject);
        }
    }

    private void PlayBossSound(int i)
    {
        switch(i)
        {
            case 0:
                soundManager.playSound("boss_death");
                break;
            case 1:
                soundManager.stopSound("boss_music1");
                break;
            case 2:
                soundManager.playSound("ingame_music1");
                break;
            case 3:
                soundManager.playSound("boss_grunt5");
                break;
            case 4:
                soundManager.playSound("boss_grunt7");
                break;
        }
    }
}

