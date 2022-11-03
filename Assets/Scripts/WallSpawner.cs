using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Playables;

public class WallSpawner : MonoBehaviour
{
    private GameData gameData;
    [SerializeField] private float spawnSpace = 2.7f;
    private Vector3 lastSpawnedWallPos;

    private void OnEnable()
    {
        EventManager.spawnWall += SpawnWall;
        EventManager.getLastSpawnedWallPos += GetLastSpawnedWallPos;
    }

    private void OnDisable()
    {
        EventManager.spawnWall -= SpawnWall;
        EventManager.getLastSpawnedWallPos -= GetLastSpawnedWallPos;
    }

    private void Start()
    {
        gameData = EventManager.getGameData?.Invoke();
        SpawnWall();
    }

    private void SpawnWall()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject spawnedWall = EventManager.callObjectFromPool?.Invoke("Wall");
            spawnedWall.GetComponent<Renderer>().materials[0].SetColor("_Color", gameData.bacgroundColor);
            spawnedWall.GetComponent<Renderer>().materials[1].SetColor("_Color", gameData.bacgroundColor);
            spawnedWall.transform.position = new Vector3(0, lastSpawnedWallPos.y, 0);
            lastSpawnedWallPos = spawnedWall.transform.position + new Vector3(0, spawnSpace, 0);
        }
    }

    private Vector3 GetLastSpawnedWallPos()
    {
        return lastSpawnedWallPos;
    }
}
