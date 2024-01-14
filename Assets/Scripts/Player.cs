using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public CharacterController characterController;

    public bool validate = true;
    public int count;

    public Monster monster;

    public float speed = 1f;

    private void OnTriggerEnter(Collider other)
    {
        Triggerable triggerable = other.GetComponent<Triggerable>();
        if (triggerable == null)
            return;

        if (!triggerable.IsTrigger(this))
        {
            Debug.Log("Bir Hata Oldu");
        }
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        Debug.Log("Moonster Null mu " + (monster == null));
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
