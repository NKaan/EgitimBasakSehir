using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, PlayerTriggerable
{

    private Animator animator;
    private Player firstPlayer;
    public float resetTime = 20f;

    [SerializeField]
    public int goldCount;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public bool OnPlayerTriggerEnter(Player player, Collider collider)
    {

        if (firstPlayer != null)
            return false;

        firstPlayer = player;

        animator.Play("Open");
        return true;
    }

    public bool OnPlayerTriggerExit(Player player, Collider collider)
    {
        return true;
    }

    public bool OnPlayerTriggerStay(Player player, Collider collider)
    {
        return true;
    }

    public void AddGift()
    {
        firstPlayer?.AddGold(goldCount);

        if (resetTime == -1)
        {
            Destroy(gameObject);
            return;
        }
            
        Invoke("ResetObject", resetTime);
    }

    public void ResetObject()
    {
        animator.Play("Close");
        firstPlayer = null;
    }

}
