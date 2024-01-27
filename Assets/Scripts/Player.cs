
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Controller")]
    public CharacterController characterController;
    public ThirdPersonController thirdPersonController;
    public PlayerInput playerInput;
    public Respawn respawn;
    public bool attackActive = false;

    [Header("Camera")]
    public Transform cameraTarget;

    [Header("Statistic")]
    public bool death = false;
    public int health = 100;


    [SerializeField]
    [Header("Inventory")]
    private int goldCount;


    [Header("Events")]
    public UnityEvent<int> OnAddGold = new UnityEvent<int>();
    public UnityEvent<int> OnAddHealth = new UnityEvent<int>();
    public UnityEvent OnDeath = new UnityEvent();
    public UnityEvent OnRespawn = new UnityEvent();

    #region Inventory

    public void AddGold(int count = 1)
    {
        goldCount += count;
        OnAddGold.Invoke(goldCount);
    }

    #endregion

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

    public void Update()
    {
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
    }


    void Start()
    {
        goldCount = PlayerPrefs.GetInt("PlayerGold");
        OnAddGold.Invoke(goldCount);
        OnAddHealth.Invoke(health);
    }

    
    public void OnDeathMethod()
    {
        death = true;
        playerInput.DeactivateInput();
        thirdPersonController._animator.SetBool("Death", death);

        ObjectManager.sing.CreateObject<Gold>(transform.position).SetCount(goldCount / 2);
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
