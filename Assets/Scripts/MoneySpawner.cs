using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class MoneySpawner : MonoBehaviour
{
    private GameData gameData;
    private float incomeAmount;

    private void OnEnable()
    {
        EventManager.spawnMoney += SpawnMoney;
    }
    private void OnDisable()
    {
        EventManager.spawnMoney -= SpawnMoney;
    }

    private void Start()
    {
        gameData = EventManager.getGameData?.Invoke();
        incomeAmount = 0.5f + ((float)gameData.incomeLevel/10) - .1f;
    }

    private void SpawnMoney(Vector3 position)
    {
        TextMeshPro spawnedMoney = EventManager.callObjectFromPool?.Invoke("Money").transform.GetComponentInChildren<TextMeshPro>();
        spawnedMoney.alpha = 1;
        spawnedMoney.text = "$" + incomeAmount.ToString("0.0");
        spawnedMoney.transform.position = position + new Vector3(0,0,.3f);
        StartCoroutine(MakeInvisibleMoney(spawnedMoney));
    }

    private IEnumerator MakeInvisibleMoney(TextMeshPro moneyObj)
    {
        yield return new WaitForSeconds(.3f);

        moneyObj.DOFade(0,1);
    }
}
