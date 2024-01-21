using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerTriggerable
{
    bool OnPlayerTriggerEnter(Player player, Collider collider);
    bool OnPlayerTriggerExit(Player player, Collider collider);
}
