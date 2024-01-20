using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [Header("Controller")]
    public CharacterController characterController;

    [SerializeField]
    [Header("Inventory")]
    private int gold;


    [Header("Events")]
    public UnityEvent<int> OnAddGold = new UnityEvent<int>();
    
    [Header("Example")]
    public bool validate = true;
    public int count;
    public Monster monster;
    public float speed = 1f;


    #region Inventory

    public void AddGold(int count = 1)
    {
        gold += count;
        OnAddGold.Invoke(gold);
        Debug.Log("Karaktere eklenen gold miktarý : " + count);
    }

    #endregion




    private void OnTriggerEnter(Collider other)
    {
        Triggerable triggerable = other.GetComponentInParent<Triggerable>();
        if (triggerable == null)
            return;

        if (!triggerable.IsTrigger(this))
        {
            //Debug.Log("Bir Hata Oldu");
        }
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        Debug.Log("Moonster Null mu " + (monster == null));
        OnAddGold.Invoke(gold);
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {



        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveTransform = new Vector3(h, 0, v);
        transform.Translate(moveTransform * speed * Time.deltaTime);
    }

}
