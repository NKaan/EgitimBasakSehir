using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour, Triggerable
{
    public bool IsTrigger(Player player)
    {
        Destroy(gameObject);
        return true;
    }
}
