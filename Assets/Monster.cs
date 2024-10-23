using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] randomPosition;
    [SerializeField] private GameObject _light, preventGoingToMiddleObstacle, ladderArea, teleportOutLadder, sequence9;
    [SerializeField] private AudioSource monsterSounds, bossMusic;
    [SerializeField] private AudioClip[] chaseSounds, hurtSounds;
    [SerializeField] private AudioClip idleSound;
    private NavMeshAgent agent;

    private bool isAttacking = false, isSpawned = false, bossMusicActive = false, isEndingFight = false, despawnChecking = false;

    private int health = 8;
    private bool hasBeenEnabled = false;

    private void OnEnable() {
        sequence9.SetActive(false);
        teleportOutLadder.SetActive(false);
        
        health = 8;
        agent = GetComponent<NavMeshAgent>();

        isAttacking = false; isSpawned = false; bossMusicActive = false; isEndingFight = false; despawnChecking = false;

        transform.position = ladderArea.transform.position;
        agent.ResetPath();


        bossMusic.volume = 0;
        CancelInvoke();
        monsterSounds.clip = idleSound;
        monsterSounds.Play();

        if(hasBeenEnabled){
            Spawn();
            ActivateBossMusic();
        }else{
            Invoke("Spawn", 7f);
            Invoke("ActivateBossMusic", 7f);
        }
        
        hasBeenEnabled = true;
    }

    private void ActivateBossMusic () {
        bossMusicActive = true;
    }

    private void Spawn () {
        isSpawned = true;

        agent.speed = 20;

        monsterSounds.clip = idleSound;
        monsterSounds.Play();

        isAttacking = false;
        CancelInvoke();
        _light.SetActive(false);
        preventGoingToMiddleObstacle.SetActive(true);
        agent.destination = randomPosition[Random.Range(0, randomPosition.Length)].transform.position;
        Invoke("Attack", Random.Range(health + 2f, health + 3f));

        monsterSounds.clip = chaseSounds[Random.Range(0, chaseSounds.Length)];
        monsterSounds.Play();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.transform.gameObject.tag != "Player" && isSpawned && !isEndingFight) return;
        ResetSequence();
        NewFPSController.OnTakeDamage(50);
    }

    private void Update() {
        if(bossMusicActive && bossMusic.volume < 1){
            bossMusic.volume += Time.deltaTime / 3f;
        }

        if(isEndingFight && agent.velocity.magnitude <= .6f && despawnChecking){
            Destroy(this.gameObject);
            sequence9.SetActive(true);
            teleportOutLadder.SetActive(true);
        }
        anim.SetFloat("Speed", agent.velocity.magnitude);
        if(isSpawned && !isEndingFight){
            if(isAttacking){
                agent.destination = player.transform.position;
            }else{
                if(Vector3.Distance(transform.position, player.transform.position) < 8){
                    Attack();
                }else{
                    if(agent.velocity.magnitude <= 2){
                        agent.destination = randomPosition[Random.Range(0, randomPosition.Length)].transform.position;
                    }
                }
            }
        }
    }

    public void Hit(){
        if(isSpawned && !isEndingFight){
            health -= 1;
            
            monsterSounds.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Length)]);

            if(health <= 0){
                isEndingFight = true;
                Invoke("BeginDespawnChecks", 1.5f);
                agent.speed = 20;
                agent.destination = ladderArea.transform.position;
                return;
            }

            ResetSequence();
        }
    }

    private void BeginDespawnChecks () {
        despawnChecking = true;
    }
    private void ResetSequence () {
        agent.speed = 20;

        monsterSounds.clip = idleSound;
        monsterSounds.Play();

        isAttacking = false;
        CancelInvoke();
        _light.SetActive(false);
        preventGoingToMiddleObstacle.SetActive(true);
        transform.position = randomPosition[Random.Range(0, randomPosition.Length)].transform.position;
        agent.destination = randomPosition[Random.Range(0, randomPosition.Length)].transform.position;
        Invoke("Attack", Random.Range((health / 1.5f) + 2f, (health / 1.5f) + 3f));
    }

    private void Attack () {
        agent.speed = 13;
        monsterSounds.clip = chaseSounds[Random.Range(0, chaseSounds.Length)];
        monsterSounds.Play();

        isAttacking = true;
        _light.SetActive(true);
        preventGoingToMiddleObstacle.SetActive(false);
        agent.destination = player.transform.position;
    }
}
