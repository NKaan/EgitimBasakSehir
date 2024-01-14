using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool validate = true;
    public int count;

    public Monster monster;

    public float speed = 1f;

    private void Awake()
    {
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
