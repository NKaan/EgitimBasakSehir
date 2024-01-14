using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour , Triggerable
{

    public Transform startPos;
    public Portal targetPortal;

    public bool IsTrigger(Player player)
    {

        player.characterController.enabled = false;
        player.transform.position = targetPortal.startPos.position;
        player.characterController.enabled = true;

        return true;
    }

}
