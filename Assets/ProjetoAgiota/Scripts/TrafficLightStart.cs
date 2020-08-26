using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightStart : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _lightsRenderer;
    [SerializeField]
    private Material _redLightMaterial;
    [SerializeField]
    private Material _yellowLightMaterial;
    [SerializeField]
    private Material _greenLightMaterial;
    [SerializeField]
    private Material _blackLightMaterial;
    
    void OnEnable()
    {
        StartCoroutine(TrafficLightAnimation());    
    }

    IEnumerator TrafficLightAnimation()
    {
        Material[] tempMats = new Material[3]{_blackLightMaterial,_blackLightMaterial,_blackLightMaterial}; 
        _lightsRenderer.materials = tempMats;

        yield return new WaitForSeconds(0.2f);

        tempMats = new Material[3]{_blackLightMaterial,_redLightMaterial,_blackLightMaterial}; 
        _lightsRenderer.materials = tempMats;

        yield return new WaitForSeconds(1f);

        tempMats = new Material[3]{_yellowLightMaterial,_redLightMaterial,_blackLightMaterial}; 
        _lightsRenderer.materials = tempMats;

        yield return new WaitForSeconds(1f);

        tempMats = new Material[3]{_yellowLightMaterial,_redLightMaterial,_greenLightMaterial}; 
        _lightsRenderer.materials = tempMats;

        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
