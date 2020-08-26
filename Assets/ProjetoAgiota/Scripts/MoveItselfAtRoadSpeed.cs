using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveItselfAtRoadSpeed : MonoBehaviour
{
    private float speedMultiplier = 10f;

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.RaceStarted) return;
        transform.position += new Vector3(
            0, 0, RoadTexture.scrollSpeed * Time.deltaTime * speedMultiplier);
    }
}
