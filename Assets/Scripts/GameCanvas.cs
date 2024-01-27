using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameCanvas : MonoBehaviour
{

    public static GameCanvas sing;

    public TextMeshProUGUI goldTxt;
    public TextMeshProUGUI playerHealthTxt;
    public Slider playerHealth;


    private void Awake()
    {
        sing = this;
    }

    public void SetHealth(int value) 
    {
        playerHealth.value = value;
        playerHealthTxt.text = value.ToString();
    } 

    public void SetGold(int setCount) => goldTxt.text = "Altýn : " + setCount.ToString();

    //public void SetGold(int setCount)
    //{
    //    goldTxt.text = "Altýn : " + setCount;
    //}
    

}
