using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerTriggerable
{
    bool OnPlayerTriggerStay(Player player, Collider collider);
    bool OnPlayerTriggerEnter(Player player, Collider collider);
    bool OnPlayerTriggerExit(Player player, Collider collider);
}
