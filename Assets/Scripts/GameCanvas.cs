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

    public Slider expBar;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI expTxt;
    public TextMeshProUGUI expPTxt;

    private void Awake()
    {
        sing = this;
    }

    public void SetExp(int level,float expPoint,float currentExp)
    {
        expBar.maxValue = currentExp;
        expBar.value = expPoint;
        levelTxt.text = "Level : " + level;
        expTxt.text = expPoint.ToString("N0") + "/" + currentExp.ToString("N0");
        expPTxt.text = "%" + (((double)expPoint / currentExp) * 100).ToString("N0");
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
