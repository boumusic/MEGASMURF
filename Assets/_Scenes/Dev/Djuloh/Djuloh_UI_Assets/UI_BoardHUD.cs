using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BoardHUD : UIElement
{
    public TextMeshProUGUI unitName, enemyNumber;

    public TextMeshProUGUI unitHealthText;
    private int healthValue;
    private Animator anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void BoardRenameUnit(Unit unit)
    {
        unitName.text = unit.name;
    }

    public void BoardSetEnemyNumber(int newEnemyNumber)
    {
        enemyNumber.text = newEnemyNumber.ToString();
    }

    public void BoardUpdateHP(int newHealthValue)
    {
        if (newHealthValue < healthValue)
            BoardDamage();

        healthValue = newHealthValue;

        unitHealthText.text = healthValue.ToString();

    }

 public void BoardDamage()
    {
        anim.Play("BoardUnitInfo_Damage");
    }

    public void BoardDestroyed()
    {
        anim.Play("BoardUnitInfo_Death");
    }

   public void BoardAppear()
    {
        anim.Play("BoardUnitInfo_Appear");
    }

    public void BoardDisappear()
    {
        anim.Play("BoardUnitInfo_Disappear");
    }
 
}
