using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bear : Monster, PlayerTriggerable
{
    private Vector3 firsPosition;
    public float firstPositionReturnTime = 5f;

    public Transform target;
    public bool targeted = false;
    public bool attacked = false;

    public int health = 100;
    public float walkSpeed = 6;
    public float runSpeed = 10;

    public float healtRecoveryMin = 5;
    public float healtRecoveryMax = 5;
    
    public int minDamage = 5;
    public int maxDamage = 8;
    public float criticalChance = 15f; // Yüzde

    public float stopDistance = 6f;

    private Animator animator;
    private NavMeshAgent navMeshAgent;

    [Header("Animation Names")]
    private const string Idle = "Idle";
    private const string Walk = "Walk";
    private const string Run = "Run";
    private const string Sit = "Sit";
    private const string Death = "Death";
    private string[] Attacks = { "Attack1", "Attack2" , "Attack3" , "Attack5", "Buff"};

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = runSpeed;
        firsPosition = transform.position;
    }


    void Start()
    {
        
    }

    void Update()
    {
        if (targeted)
        {
            navMeshAgent.SetDestination(target.position);
        }
    }

    public bool OnPlayerTriggerExit(Player player, Collider collider)
    {
        if(collider.gameObject.name == "TargetCollider")
        {
            SetTarget(null);
        }
        else if (collider.gameObject.name == "AttackCollider")
        {
            SetAttack(false);
        }

        return true;
    }

    public bool OnPlayerTriggerEnter(Player player, Collider collider)
    {
        if(collider.gameObject.name == "TargetCollider")
        {
            SetTarget(player.transform);
        }
        else if (collider.gameObject.name == "AttackCollider")
        {
            SetAttack(true);
        }

        return true;
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
        targeted = _target != null;
        navMeshAgent.isStopped = !targeted;
    }

    public void SetAttack(bool active)
    {
        navMeshAgent.isStopped = active;
        attacked = active;

        if (active)
            StartCoroutine(StartAttack());


    }


    IEnumerator StartAttack()
    {
        while (attacked)
        {
            animator.Play(Attacks[Random.Range(0, Attacks.Length)]);
            yield return new WaitForSeconds(1f);
        }
    }

}
