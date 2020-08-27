using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _offset;
    private Transform _carToFollow;
    public Transform CarToFollow
    {
        set => _carToFollow = value;
    }
    
    void Update()
    {
        if (!GameManager.Instance.RaceStarted) return;
        
        //Vector3 newPos = new Vector3(transform.position.x, transform.position.y, _carToFollow.transform.position.z + _offset);
        //transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime);
    }
}
