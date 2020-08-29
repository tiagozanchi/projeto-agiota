using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnSpot {
        public GameObject spotL;
        public GameObject spotR;

        [Range(0.1f, 2)]
        public float spawnRate = 0.5f;
        private float lastSpawnTime = 0f;

        public Boolean hasValidSpots() {
            return spotL != null && spotR != null;
        }

        public Boolean shouldInstantiate() {
            if (!hasValidSpots()) return false;
            return 1.0f / spawnRate <= Time.time - lastSpawnTime;
        }

        // When we call this, basically everything is fine, so we can update lastSpawnTime
        public Transform getRandomSpotTransform() {
            lastSpawnTime = Time.time;
            float randValue = UnityEngine.Random.Range(0, 2);
            return randValue == 0
                ? spotL.transform
                : spotR.transform;
        }
    }

    public SpawnSpot trafficProps;
    public SpawnSpot scenarioProps;

    // Start is called before the first frame update
    void Start()
    {
        if (!trafficProps.hasValidSpots()) {
            Debug.LogError("You haven't set spawn spots for traffic props");
        }

        if (!scenarioProps.hasValidSpots())
        {
            Debug.LogError("You haven't set spawn spots for scenario props");
        }

        GameObject obj;
        for (int i = 0; i < 15; i++)
        {
            obj = InstantiateScenarioProp();
            Vector3 newPos = Vector3.forward * UnityEngine.Random.Range(40, 100) * -1f;
            obj.transform.position -= newPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.RaceStarted) return;

        if (trafficProps.shouldInstantiate())
        {
            GameObject obj = PoolMaster.getInstance().getTrafficProp();
            Transform parent = trafficProps.getRandomSpotTransform();
            obj.transform.position = parent.position;
            obj.transform.rotation = parent.rotation;
            obj.AddComponent<MoveItselfAtRoadSpeed>();
        }

        if (scenarioProps.shouldInstantiate())
        {
            InstantiateScenarioProp();
        }
    }

    private GameObject InstantiateScenarioProp()
    {
        GameObject obj = PoolMaster.getInstance().getScenarioProp();
        Transform parent = scenarioProps.getRandomSpotTransform();
        Vector3 newPos = new Vector3((UnityEngine.Random.value - 0.5f) * (parent.localScale.x * 7), 0, 0);
        obj.transform.position = parent.position + newPos;

        Vector3 randomRotation = UnityEngine.Random.rotation.eulerAngles;
        randomRotation.x = 0;
        randomRotation.z = 0;
        Quaternion newRotation = Quaternion.Euler(randomRotation);
        obj.transform.rotation = newRotation;

        obj.AddComponent<MoveItselfAtRoadSpeed>();

        return obj;
    }



}
