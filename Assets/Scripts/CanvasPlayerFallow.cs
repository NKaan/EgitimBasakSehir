using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasPlayerFallow : MonoBehaviour
{
    public Canvas myCanvas;


    private void Awake()
    {
        myCanvas = GetComponent<Canvas>();
        if(myCanvas.worldCamera == null)
            myCanvas.worldCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(myCanvas.worldCamera.transform);
    }
}
