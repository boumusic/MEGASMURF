using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_BoardUnitInfo : MonoBehaviour
{
    public float healthValue;
    public TextMeshProUGUI healthText;
    private bool isSelected;

    public void UpdateDamageTextBoard(int newHealthValue)
    {
        healthValue = newHealthValue;

        healthText.text = healthValue.ToString();
    }


    public void BoardDamageAnim()
    {
        //dégâts

    }
   
    public void BoardDeathAnim()
    {
        //mort
    }
   
    public void BoardHealAnim()
    {
        //soin
    }

    public void BoardDisappearAnim()
    {
        //quand le joueur sort du hover, le truc disparaît, à part si le joueur a cliqué sur l'unité.
        //si l'unité est sélectionnée, il faut toujours qu'il y'ait son truc sur la gueule.

        if (!isSelected)
        {
            //jouer l'anim de disparition
        }
    }
   
    public void BoardLaunchAnim()
    {
        //quand le joueur rentre dans le hover de son unité

        //Anim : BoardUnitInfo_Launch
    }

    public void BoardAppearAnim()
    {
        //c'est une version plus rapide de l'apparition de l'UI on-board.
        //A jouer sur les ennemis, quand ils agissent, pour qu'on voit leurs HP et tout sans avoir la grosse anim de base

        //Anim : BoardUnitInfo_Appear

    }
}
