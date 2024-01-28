using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.UI;

public class Monster : MonoBehaviour , PlayerTriggerable
{
    protected Vector3 firsPosition;
    public bool firsPositionActive = false;
    public float firstPositionReturnTime = 5f;

    public Slider healtbar;

    public GameObject mesh;

    public Player target;
    public bool targeted = false;
    public bool attacked = false;

    public int addExpPoint = 75;

    public int health = 100;
    public float walkSpeed = 6;
    public float runSpeed = 10;

    public float healtRecoveryMin = 5;
    public float healtRecoveryMax = 5;

    public int minDamage = 4;
    public int maxDamage = 8;
    public float criticalChance = 15f; // Yüzde

    public float stopDistance = 6f;
    public bool isDeath = false;

    protected Animator animator;
    protected NavMeshAgent navMeshAgent;

    public UnityEvent<int> OnAddHealth = new UnityEvent<int>();

    [Header("Animation Names")]
    protected const string Idle = "Idle";
    protected const string Walk = "Walk";
    protected const string Run = "Run";
    protected const string Sit = "Sit";
    protected const string Death = "Death";
    protected string[] Attacks = { "Attack1", "Attack2", "Attack3", "Attack5", "Buff" };

    public void PlayerAttack(int damage)
    {
        health -= damage;

        OnAddHealth.Invoke(health);

        if (health <= 0)
        {
            health = 0;
            if(!isDeath)
                StartCoroutine(MonsterDeath());
        }

    }

    public IEnumerator MonsterDeath()
    {
        if (isDeath)
            yield return null;

        target.AddExp(addExpPoint);
        isDeath = true;
        target = null;
        targeted = false;
        attacked = false;
        firsPositionActive = false;
        animator.Play(Death);

        Chest chest = ObjectManager.sing.CreateObject<Chest>(transform.position + Vector3.right + Vector3.up);
        chest.goldCount = Random.Range(50,101);
        chest.resetTime = -1;

        yield return new WaitForSeconds(10);

        mesh.SetActive(false);

        yield return new WaitForSeconds(5);

        Restart();
    }

    public void Restart()
    {
        target = null;
        targeted = false;
        attacked = false;
        firsPositionActive = false;
        isDeath = false;
        transform.position = firsPosition;
        health = 100;
        OnAddHealth.Invoke(health);
        StartCoroutine(MonsterWait());
        mesh.SetActive(true);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = runSpeed;
        firsPosition = transform.position;
        OnAddHealth.AddListener((health) => healtbar.value = health);
        OnAddHealth.Invoke(health);
    }

    void Start()
    {
        StartCoroutine(MonsterWait());
    }

    public IEnumerator MonsterWait()
    {

        while(!attacked && !targeted)
        {
            animator.Play(Sit);

            yield return new WaitForSeconds(5);

            animator.Play(Idle);

        }
    }

    void Update()
    {
        if (isDeath)
            return;

        if (targeted)
            navMeshAgent.SetDestination(target.transform.position);

        else if (firsPositionActive)
        {
            navMeshAgent.SetDestination(firsPosition);
            if (Vector3.Distance(firsPosition, transform.position) < 7)
            {
                firsPositionActive = false;
                StartCoroutine(MonsterWait());
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

        if (isDeath)
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

        if (isDeath)
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

        if (isDeath)
            return true;

        StopAllCoroutines();
        if (collider.gameObject.name == "ViewTarget")
        {
            StartCoroutine(SetTarget(player));
        }
        else if (collider.gameObject.name == "ListenTarget")
        {
            if (player.thirdPersonController._input.sprint)
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

        if (targeted)
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
        while (attacked && !isDeath)
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

        target.AddHealth(Random.Range(minDamage, maxDamage + 1) * -1);
    }

}
