using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour, Triggerable
{
    [SerializeField]
    private int count = 1;
    public float createTime = 3f;
    private Animator animator;
    public GameObject meshObject;

    public bool collectable = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public bool IsTrigger(Player player)
    {
        if (!collectable)
            return false;

        collectable = false;
        player.AddGold(count);
        animator.SetBool("AddGold", true);
        return true;
    }

    public void GoldDestroy()
    {
        meshObject.SetActive(false);
        animator.SetBool("AddGold", false);

        Invoke("MeshStart", createTime);
    } 

    public void MeshStart()
    {
        collectable = true;
        meshObject.SetActive(true);
    }

}
