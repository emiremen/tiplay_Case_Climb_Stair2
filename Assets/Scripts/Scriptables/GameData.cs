using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    #region Earnings
    public float bestScore;
    public float bestScoreYPos;
    public float money;
    #endregion

    #region Level
    public int level = 1;
    public Color bacgroundColor = Color.white;
    #endregion

    #region Market

    public float maxLevel = 20;
    public int staminaLevel = 1;
    public int incomeLevel = 1;
    public int speedLevel = 1;
    
    public int staminaCost = 40;
    public int incomeCost = 40;
    public int speedCost = 40;
    #endregion

}