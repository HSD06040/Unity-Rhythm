using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 5f;

    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if(Mathf.Abs(scrollInput) > 0f)
        {
            Vector3 newPosition = transform.position;

            newPosition.y += scrollInput * scrollSpeed;

            transform.position = newPosition;
        }
    }
}
