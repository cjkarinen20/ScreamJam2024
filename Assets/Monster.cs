using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] randomPosition;
    [SerializeField] private GameObject _light, preventGoingToMiddleObstacle;
    [SerializeField] private AudioSource monsterSounds;
    [SerializeField] private AudioClip[] chaseSounds, hurtSounds;
    [SerializeField] private AudioClip idleSound;
    private NavMeshAgent agent;

    private bool isAttacking = false;

    private int health = 8;

    private void OnEnable() {
        agent = GetComponent<NavMeshAgent>();
        ResetSequence();
    }

    private void OnCollisionEnter(Collision other) {
        if(other.transform.gameObject.tag != "Player") return;
        ResetSequence();
        NewFPSController.OnTakeDamage(33);
    }

    private void Update() {
        anim.SetFloat("Speed", agent.velocity.magnitude);
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

    public void Hit(){
        health -= 1;

        ResetSequence();

        monsterSounds.PlayOneShot(hurtSounds[Random.Range(0, hurtSounds.Length)]);
    }
    private void ResetSequence () {
        agent.speed = 25;

        monsterSounds.clip = idleSound;
        monsterSounds.Play();

        isAttacking = false;
        CancelInvoke();
        _light.SetActive(false);
        preventGoingToMiddleObstacle.SetActive(true);
        transform.position = randomPosition[Random.Range(0, randomPosition.Length)].transform.position;
        agent.destination = randomPosition[Random.Range(0, randomPosition.Length)].transform.position;
        Invoke("Attack", Random.Range(health + 2f, health + 3f));
    }

    private void Attack () {
        agent.speed = 16;
        monsterSounds.clip = chaseSounds[Random.Range(0, chaseSounds.Length)];
        monsterSounds.Play();

        isAttacking = true;
        _light.SetActive(true);
        preventGoingToMiddleObstacle.SetActive(false);
        agent.destination = player.transform.position;
    }
}
