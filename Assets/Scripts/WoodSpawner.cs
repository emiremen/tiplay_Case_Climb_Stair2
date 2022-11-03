using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WoodSpawner : MonoBehaviour
{

    private void OnEnable()
    {
        EventManager.spawnWood += SpawnWood;
        EventManager.getWoodSpawner += GetWoodSpawner;
    }

    private void OnDisable()
    {
        EventManager.spawnWood -= SpawnWood;
        EventManager.getWoodSpawner -= GetWoodSpawner;
    }

    void Update()
    {
        
    }

    private void SpawnWood(float height)
    {
        GameObject spawnedWood = EventManager.callObjectFromPool?.Invoke("Wood");
        spawnedWood.transform.parent = transform;
        spawnedWood.transform.position = new Vector3(0, height,0);
        spawnedWood.transform.localScale = Vector3.zero;
        spawnedWood.transform.DOScale(1, .4f);
        EventManager.setScoreBoardPosition?.Invoke();
    }


    private GameObject GetWoodSpawner()
    {
        return gameObject;
    }

}
