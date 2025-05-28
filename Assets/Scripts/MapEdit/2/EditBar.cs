using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EditBar : MonoBehaviour
{
    public float smoothSpeed = 10f;
    private float startY;
    private float distance;

    private void Update()
    {
        float progressTime = GameManager.Instance.time / 1000;
        float y = startY + (progressTime * distance);

        transform.position = new Vector3(transform.position.x, y);
    }

    public void Init(float y, float _distance)
    {
        startY = y;
        distance = _distance;
    }
}
