using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StairSpawner : MonoBehaviour
{
    private Transform lastSpawnedStair;
    [SerializeField] private float distanceFromMiddleWood;
    private GameObject frontStair;

    int baseRotateAngle = 10;
    [SerializeField] int rotateAngle = 10;


    private void OnEnable()
    {
        EventManager.getLastSpawnedStairPos += GetLastSpawnedStairPos;
        EventManager.setLastSpawnedStairPos += SetLastSpawnedStairPos;
        EventManager.spawnStair += SpawnFrontStair;
        EventManager.getStairSpawner += GetStairSpawner;
        EventManager.setGameState += StartToSpawn;
    }

    private void OnDisable()
    {
        EventManager.getLastSpawnedStairPos -= GetLastSpawnedStairPos;
        EventManager.setLastSpawnedStairPos -= SetLastSpawnedStairPos;
        EventManager.spawnStair -= SpawnFrontStair;
        EventManager.getStairSpawner -= GetStairSpawner;
        EventManager.setGameState -= StartToSpawn;
    }

    void Start()
    {
        baseRotateAngle = rotateAngle;
        lastSpawnedStair = new GameObject().transform;
        lastSpawnedStair.position = new Vector3(0, 0, distanceFromMiddleWood);
    }
    private void StartToSpawn(GameState st)
    {
        frontStair = SpawnStair();
    }
    private GameObject SpawnFrontStair()
    {
        GameObject stairObj = frontStair;
        frontStair = SpawnStair();
        return stairObj;
    }

    private GameObject SpawnStair()
    {
        GameObject spawnedStair = EventManager.callObjectFromPool?.Invoke("Stair");
        spawnedStair.transform.parent = transform;
        spawnedStair.transform.localPosition = new Vector3(lastSpawnedStair.position.x, lastSpawnedStair.position.y + .08f, 0);
        spawnedStair.transform.GetChild(0).localPosition = new Vector3(0, 0, distanceFromMiddleWood);
        spawnedStair.transform.rotation = Quaternion.Euler(0, rotateAngle, 0);
        rotateAngle += baseRotateAngle;
        lastSpawnedStair.position = spawnedStair.transform.localPosition;
        spawnedStair.transform.GetChild(0).localScale = Vector3.zero;
        spawnedStair.transform.GetChild(0).DOScale(1, .3f);

        EventManager.spawnMoney?.Invoke(spawnedStair.transform.GetChild(0).TransformPoint(spawnedStair.transform.position));
        EventManager.spawnWood?.Invoke(spawnedStair.transform.position.y);
        if (EventManager.getLastSpawnedWallPos?.Invoke().y - lastSpawnedStair.position.y < 10)
        {
            EventManager.spawnWall?.Invoke();
        }

        return spawnedStair;
    }

    private Transform GetLastSpawnedStairPos()
    {
        return lastSpawnedStair;
    }

    private void SetLastSpawnedStairPos(Transform position)
    {
        lastSpawnedStair = position;
    }

    private GameObject GetStairSpawner()
    {
        return gameObject;
    }
}
