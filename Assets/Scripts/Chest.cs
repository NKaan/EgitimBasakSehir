using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, Triggerable
{

    private Animator animator;
    private Player firstPlayer;
    public float resetTime = 20f;

    [SerializeField]
    private int goldCount;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    public bool IsTrigger(Player player)
    {

        if (firstPlayer != null)
            return false;

        firstPlayer = player;

        animator.Play("Open");
        return true;
    }

    public void AddGift()
    {
        firstPlayer.AddGold(goldCount);

        Invoke("ResetObject", resetTime);
    }

    public void ResetObject()
    {
        animator.Play("Close");
        firstPlayer = null;
    }

}
