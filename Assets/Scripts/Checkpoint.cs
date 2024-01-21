using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, PlayerTriggerable
{
    public bool OnPlayerTriggerEnter(Player player, Collider collider)
    {
        Respawn respawn = player.GetComponent<Respawn>();
        respawn.respawnPosition = transform.position;
        return true;
    }

    public bool OnPlayerTriggerExit(Player player, Collider collider)
    {
        return true;
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
