
using StarterAssets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Cinemachine.DocumentationSortingAttribute;

public class Player : MonoBehaviour
{
    [Header("Drops")]
    public float expDrop = 20f;


    [Header("Controller")]
    public CharacterController characterController;
    public ThirdPersonController thirdPersonController;
    public PlayerInput playerInput;
    public Respawn respawn;
    public bool attackActive = false;
    public bool animAttackActive = false;

    [Header("Camera")]
    public Transform cameraTarget;

    [Header("Statistic")]
    public float baseExp = 100;
    public float expPoint = 0;
    public int playerLevel = 1;

    public bool death = false;
    public int health = 100;
    public int _damage = 0;
    public int damage 
    {
        get 
        {
            if (_damage == -1)
                DamageCalculate();

            return _damage + WeaponsDamageCalculate();
        }
        set 
        { 
            _damage = value; 
        }
    }

    [SerializeField]
    [Header("Inventory")]
    private int goldCount;
    public List<Item> myItemList;

    [Header("Stats")]
    public int strStat = 1;
    public int hpStat = 1;

    [Header("Events")]
    public UnityEvent<int> OnAddGold = new UnityEvent<int>();
    public UnityEvent<int> OnAddHealth = new UnityEvent<int>();
    public UnityEvent OnDeath = new UnityEvent();
    public UnityEvent OnRespawn = new UnityEvent();
    public UnityEvent<Item> OnItemAdd = new UnityEvent<Item>();
    public UnityEvent<int,float,float> OnAddExp = new UnityEvent<int,float,float>();

    public void AddExp(float addExp)
    {
        expPoint += addExp *= expDrop / 100;

        float istenilenExp = (float)(baseExp * Math.Pow(1.05, playerLevel - 1));

        if(expPoint >= istenilenExp)
        {
            playerLevel++;
            expPoint -= istenilenExp;
            AddExp(0);
            return;
        }
        OnAddExp.Invoke(playerLevel,expPoint, istenilenExp);
    }

    
    #region Inventory

    public void AddGold(int count = 1)
    {
        goldCount += count;
        OnAddGold.Invoke(goldCount);
    }

    #endregion

    public void Attack(Monster monster)
    {
        monster.PlayerAttack(damage);
    }

    public int DamageCalculate()
    {
        int calculateDamage = strStat * 3;
        damage = calculateDamage;

        return calculateDamage;
    }

    public int WeaponsDamageCalculate()
    {
        int calculateDamage = 0;
        foreach (var item in myItemList.Where(x => x.GetType().IsSubclassOf(typeof(Weapons))).OfType<Weapons>())
            calculateDamage += item.GetDamage();

        return calculateDamage;
    }

    public void AddHealth(int value)
    {
        health += value;

        if (health < 0)
            health = 0;
        else if(health > 100)
            health = 100;

        OnAddHealth.Invoke(health);

        if (health == 0)
            OnDeath.Invoke();
    }

    public void AttackAnimActive()
    {
        animAttackActive = false;
    }

    public void Update()
    {

        if (Input.GetMouseButtonDown(1))
        {
            AddExp(2000);
        }

        if (thirdPersonController._input.attack && !attackActive)
        {
            attackActive = true;
            thirdPersonController._animator.SetBool("Attack", thirdPersonController._input.attack);
        }
        else if(!thirdPersonController._input.attack && attackActive)
        {
            attackActive = false;
            thirdPersonController._animator.SetBool("Attack", thirdPersonController._input.attack);
        }
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        thirdPersonController = GetComponentInParent<ThirdPersonController>();
        playerInput = GetComponent<PlayerInput>();
        respawn = GetComponent<Respawn>();
        OnAddGold.AddListener((_gold) => PlayerPrefs.SetInt("PlayerGold", _gold));

        OnDeath.AddListener(() => OnDeathMethod());

        OnAddExp.AddListener((level, exp,current) => 
        {
            PlayerPrefs.SetFloat("PlayerExpPoint", exp);
            PlayerPrefs.SetInt("PlayerLevel", level);
        });

        DamageCalculate();

        OnItemAdd.AddListener((item) => 
        {
            item.myPlayer = this;
            damage = -1; 
        });

        myItemList.ForEach(x => x.myPlayer = this);

        

    }


    void Start()
    {
        expPoint = PlayerPrefs.GetFloat("PlayerExpPoint");
        playerLevel = PlayerPrefs.GetInt("PlayerLevel");
        AddExp(0);

        goldCount = PlayerPrefs.GetInt("PlayerGold");
        OnAddGold.Invoke(goldCount);
        OnAddHealth.Invoke(health);
    }

    
    public void OnDeathMethod()
    {
        death = true;
        playerInput.DeactivateInput();
        thirdPersonController._animator.SetBool("Death", death);

        ObjectManager.sing.CreateObject<Gold>(transform.position + Vector3.right + Vector3.up).SetCount(goldCount / 2);
        AddGold(goldCount * -1);


        StopAllCoroutines();
        StartCoroutine(StartRespawn());


    }

    IEnumerator StartRespawn()
    {
        yield return new WaitForSeconds(3f);
        respawn.RespawnMethod();
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerTriggerable triggerable = other.GetComponentInParent<PlayerTriggerable>();
        if (triggerable == null)
            return;

        if (!triggerable.OnPlayerTriggerStay(this, other))
        {
            //Debug.Log("Bir Hata Oldu");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerTriggerable triggerable = other.GetComponentInParent<PlayerTriggerable>();
        if (triggerable == null)
            return;

        if (!triggerable.OnPlayerTriggerEnter(this, other))
        {
            //Debug.Log("Bir Hata Oldu");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerTriggerable triggerable = other.GetComponentInParent<PlayerTriggerable>();
        if (triggerable == null)
            return;

        if (!triggerable.OnPlayerTriggerExit(this, other))
        {
            //Debug.Log("Bir Hata Oldu");
        }
    }

    public void Restart()
    {
        playerInput.ActivateInput();
        death = false;
        thirdPersonController._animator.SetBool("Death", death);
        AddHealth(100);
    }
    

    public void SetPos(Vector3 newPos)
    {
        characterController.enabled = false;
        transform.position = newPos;
        characterController.enabled = true;
    }


}
