using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bear : Monster, PlayerTriggerable
{
    private Vector3 firsPosition;
    public bool firsPositionActive = false;
    public float firstPositionReturnTime = 5f;

    public Player target;
    public bool targeted = false;
    public bool attacked = false;

    public int health = 100;
    public float walkSpeed = 6;
    public float runSpeed = 10;

    public float healtRecoveryMin = 5;
    public float healtRecoveryMax = 5;
    
    public int minDamage = 4;
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
        animator.Play(Sit);
    }

    void Update()
    {
        if (targeted)
            navMeshAgent.SetDestination(target.transform.position);

        else if (firsPositionActive)
        {
            navMeshAgent.SetDestination(firsPosition);
            if (Vector3.Distance(firsPosition, transform.position) < 7)
            {
                firsPositionActive = false;
                animator.Play(Sit);
            }

        }
            
    }

    public bool OnPlayerTriggerStay(Player player, Collider collider)
    {
        if (player.death)
            return true;

        if (!player.thirdPersonController._input.sprint)
            return true;

        if (target != null)
            return true;

        StopAllCoroutines();

        if (collider.gameObject.name == "ListenTarget")
        {
            StartCoroutine(SetTarget(player));
        }

        return true;
    }

    public bool OnPlayerTriggerExit(Player player, Collider collider)
    {
        if (player.death)
            return true;

        StopAllCoroutines();

        if (collider.gameObject.name == "ListenTarget")
        {
            StartCoroutine(SetTarget(null));
        }
        else if (collider.gameObject.name == "AttackCollider")
        {
            StartCoroutine(SetAttack(false));
        }
        

        return true;
    }

    public bool OnPlayerTriggerEnter(Player player, Collider collider)
    {
        if (player.death)
            return true;

        StopAllCoroutines();
        if(collider.gameObject.name == "ViewTarget")
        {
            StartCoroutine(SetTarget(player));
        }
        else if (collider.gameObject.name == "ListenTarget")
        {
            if(player.thirdPersonController._input.sprint)
                StartCoroutine(SetTarget(player));
        }
        else if (collider.gameObject.name == "AttackCollider")
        {
            StartCoroutine(SetAttack(true));
        }

        return true;
    }

    public IEnumerator SetTarget(Player _target)
    {
        target = _target;
        targeted = _target != null;
        navMeshAgent.isStopped = !targeted;

        if(targeted)
            animator.Play(Run);
        else
        {
            StopAllCoroutines();
            StartCoroutine(FirsPositionReturn());
        }
            

        yield return true;
    }

    public IEnumerator SetAttack(bool active)
    {
        yield return new WaitForSeconds(1f);

        navMeshAgent.isStopped = active;
        attacked = active;

        if (active)
            StartCoroutine(StartAttack());
        else
            animator.Play(Run);

    }

    IEnumerator FirsPositionReturn()
    {
        animator.Play(Idle);
        yield return new WaitForSeconds(firstPositionReturnTime);
        animator.Play(Run);
        navMeshAgent.isStopped = false;
        firsPositionActive = true;
    }


    IEnumerator StartAttack()
    {
        while (attacked)
        {
            animator.Play(Attacks[Random.Range(0, Attacks.Length)]);
            yield return new WaitForSeconds(1f);
            Damage();
        }
    }

    public void Damage()
    {
        if (target == null)
            return;

        if (target.death)
        {
            attacked = false;
            StartCoroutine(SetTarget(null));
            return;
        }

        target.AddHealth(Random.Range(minDamage,maxDamage +1) * -1);
    }

}
