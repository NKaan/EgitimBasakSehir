using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, Triggerable
{
    public bool IsTrigger(Player player)
    {
        Respawn respawn = player.GetComponent<Respawn>();
        respawn.respawnPosition = transform.position;
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
