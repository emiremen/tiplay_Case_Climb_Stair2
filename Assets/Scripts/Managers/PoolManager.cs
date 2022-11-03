using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PoolManager : MonoBehaviour
{
    #region Pool Variables
    [SerializeField] private List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    #endregion

    

    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int poolSize;
    }

    private void OnEnable()
    {
        EventManager.callObjectFromPool += CallObjectFromPool;
    }

    private void OnDisable()
    {
        EventManager.callObjectFromPool -= CallObjectFromPool;
    }

    void Awake()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject spawnedObject = Instantiate(pool.prefab, Vector3.zero, Quaternion.identity);
                spawnedObject.SetActive(false);
                objectPool.Enqueue(spawnedObject);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    void Update()
    {

    }

    private GameObject CallObjectFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with {tag} tag dosn't exist!");
            return null;
        }
        GameObject calledObject = poolDictionary[tag].Dequeue();
        calledObject.SetActive(true);

        poolDictionary[tag].Enqueue(calledObject);
        return calledObject;
    }

}
