using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour , PlayerTriggerable
{

    public Transform startPos;
    public Portal targetPortal;

    public bool OnPlayerTriggerEnter(Player player, Collider collider)
    {
        player.SetPos(targetPortal.startPos.position);

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

}
