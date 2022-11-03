using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    private GameState gameState;

    private void Awake()
    {
        gameState = GameState.stop;
    }

    private void OnEnable()
    {
        EventManager.getGameState += GetGameState;
        EventManager.setGameState += SetGameState;
        EventManager.getGameData += GetGameData;
    }

    private void OnDisable()
    {
        EventManager.getGameState -= GetGameState;
        EventManager.setGameState -= SetGameState;
        EventManager.getGameData -= GetGameData;
    }

    private GameState GetGameState()
    {
        return gameState;
    }

    private void SetGameState(GameState _gameState)
    {
        gameState = _gameState;
    }
    private GameData GetGameData()
    {
        return gameData;
    }
}
