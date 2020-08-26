using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pista : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    private Material _mat;

    // Start is called before the first frame update
    void Awake()
    {
        _mat = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.RaceStarted) return;

        _mat.mainTextureOffset += Vector2.up * Time.deltaTime * _speed;

        if (_mat.mainTextureOffset.y > 10)
        {
            _mat.mainTextureOffset = Vector2.zero;
        }
    }
}
