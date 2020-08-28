using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightPole : MonoBehaviour
{
    
    void OnEnable()
    {
        StartCoroutine(TrafficLightAnimation());    
    }

    IEnumerator TrafficLightAnimation()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
