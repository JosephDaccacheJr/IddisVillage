using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Base : MonoBehaviour
{
    [Header("Enemy Stats")]
    public int health;
    public int goldReward;


    [Header("References")]
    public GameObject enemyBody;
    public GameObject enemyAttackZone;
    public ParticleSystem bloodSquirt;

    GameObject _player;
    NavMeshAgent _nav;
    Animator _anim;

    // State Controllers
    IEnemyState currentState;

    [Header("State Information")]
    public string currentStateString;
    public EnemyState_Attack attackState = new EnemyState_Attack();
    public EnemyState_Dead deadState = new EnemyState_Dead();

    private void Awake()
    {
        _anim = enemyBody.GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        GameManager.instance.addEnemy(gameObject);
    }

    private void Start()
    {
        currentState = attackState;
    }

    private void FixedUpdate()
    {
        currentState = currentState.DoState(this);
        currentStateString = currentState.ToString();
    }

    public void DoDamage(int dmg)
    {
        if (health <= 0)
            return;

        bloodSquirt.Play();
        health -= dmg;
        if (health <= 0)
        {
            Kill();
        }
        else
        {
            _anim.SetTrigger("Hit");
        }
    }

    void Kill()
    {
        foreach(BoxCollider bc in GetComponents<BoxCollider>()) 
        {
            bc.enabled = false;
        }
        GameManager.instance.changeGold(goldReward);
        GameManager.instance.removeEnemy(gameObject);

        _anim.SetBool("IsAttacking", false);
        _nav.isStopped = true;
        _nav.enabled = false;
        currentState = deadState;
        _anim.Play("Dead");
    }

    public void Movement()
    {
        _nav.destination = _player.transform.position;
    }

    public void AttackController()
    {
        if (_nav.remainingDistance <= _nav.stoppingDistance && health > 0)
        {
            _anim.SetBool("IsAttacking", true);
            Vector3 rotationTarget = new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
            transform.LookAt(rotationTarget);
        }
        else
        {
            _anim.SetBool("IsAttacking", false);
        }
    }

    public void MakeAttack()
    {
        Vector3 attackPosition = transform.position + transform.forward;
        Instantiate(enemyAttackZone, attackPosition, Quaternion.identity);
    }
}
