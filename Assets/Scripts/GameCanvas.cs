using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameCanvas : MonoBehaviour
{

    public static GameCanvas sing;

    public TextMeshProUGUI goldTxt;


    private void Awake()
    {
        sing = this;
    }

    public void SetGold(int setCount) => goldTxt.text = setCount.ToString();

    //public void SetGold(int setCount)
    //{
    //    goldTxt.text = "Altýn : " + setCount;
    //}
    

}
