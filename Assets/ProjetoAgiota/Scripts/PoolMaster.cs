using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolMaster : MonoBehaviour
{

    [System.Serializable]
    public class PropsPool
    {
        public GameObject[] objects;
        public int poolSize;
    }

    private static PoolMaster sInstance;

    public PropsPool trafficPropsPool;
    public PropsPool scenarioPropsPool;

    private Queue<GameObject> trafficPropsQueue = new Queue<GameObject>();
    private Queue<GameObject> scenarioPropsQueue = new Queue<GameObject>();

    private void Awake()
    {
        if (sInstance == null) sInstance = this;
        else Destroy(this);
    }

    public static PoolMaster getInstance()
    {
        return sInstance;
    }

    private void Start()
    {
        initializePool(trafficPropsPool, trafficPropsQueue);
        initializePool(scenarioPropsPool, scenarioPropsQueue);
    }

    private void initializePool(PropsPool pool, Queue<GameObject> poolQueue) {
        for (int i = 0; i < pool.poolSize; i++) {
            int rand = Random.Range(0, pool.objects.Length - 1);
            GameObject obj = Instantiate(pool.objects[rand]);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    public GameObject getTrafficProp() {
        GameObject obj = trafficPropsQueue.Dequeue();
        obj.SetActive(true);
        trafficPropsQueue.Enqueue(obj);

        return obj;
    }

    public GameObject getScenarioProp()
    {
        GameObject obj = scenarioPropsQueue.Dequeue();
        obj.SetActive(true);
        scenarioPropsQueue.Enqueue(obj);

        return obj;
    }
}
