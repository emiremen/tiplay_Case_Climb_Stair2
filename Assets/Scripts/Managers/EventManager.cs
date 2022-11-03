using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    #region Player
    public static Func<GameObject> getPlayer;
    public static Func<Animator> getPlayerAnimator;
    public static Action characterJump;
    public static Action<bool> setPlayerVisibility;
    #endregion

    #region Environments
    public static Func<string, GameObject> callObjectFromPool;
    public static Func<GameObject> spawnStair;
    public static Action<float> spawnWood;
    public static Action spawnWall;
    public static Func<Vector3> getLastSpawnedWallPos;
    public static Func<Transform> getLastSpawnedStairPos;
    public static Action<Transform> setLastSpawnedStairPos;
    public static Func<GameObject> getStairSpawner;
    public static Func<GameObject> getWoodSpawner;
    public static Action setScoreBoardPosition;
    #endregion

    #region Score
    public static Action saveMoneyData;
    public static Action saveBestScore;
    public static Action decreaseScore;
    public static Action gainMoney;
    public static Action<Vector3> spawnMoney;
    #endregion

    #region UI
    public static Action showGameOverPanel;
    public static Func<bool> getIsTouchedScreen;
    public static Action<bool> setIsTouchedScreen;
    #endregion

    #region GameManagement
    public static Action<GameState> setGameState;
    public static Func<GameState> getGameState;
    public static Func<GameData> getGameData;
    #endregion
}
