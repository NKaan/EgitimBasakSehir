using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Item
{
    
    public int minDamage;
    public int maxDamage;


    public int GetDamage()
    {
        return Random.Range(minDamage, maxDamage + 1);
    }

    public void Attack(Monster monster)
    {
        if (myPlayer == null)
            return;

        myPlayer.Attack(monster);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (myPlayer == null)
            return;

        if (myPlayer.animAttackActive)
            return;

        if (!myPlayer.thirdPersonController._input.attack)
            return;

        myPlayer.animAttackActive = true;

        Monster monster = other.GetComponentInParent<Monster>();

        if (monster != null)
            Attack(monster);

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
