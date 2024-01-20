using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour , Triggerable
{

    public Transform startPos;
    public Portal targetPortal;

    public bool IsTrigger(Player player)
    {
        player.SetPos(targetPortal.startPos.position);

        return true;
    }

}
