using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Vector3 respawnPosition;
    private Player myPlayer;

    private void Awake()
    {
        respawnPosition = transform.position;
        myPlayer = GetComponent<Player>();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y < -2)
            myPlayer.SetPos(respawnPosition);

    }
}
