using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureRoll : MonoBehaviour
{

    Renderer roadRenderer;
    [Range(1.2f, 10f)]
    public float scrollSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        roadRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.RaceStarted) return;
        
        if (roadRenderer != null) {
            float offset = Time.deltaTime * scrollSpeed;
            roadRenderer.material.mainTextureOffset += new Vector2(0, offset);
        }
    }
}
