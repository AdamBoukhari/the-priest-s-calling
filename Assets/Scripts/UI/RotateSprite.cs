using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSprite : MonoBehaviour
{
    private float rotateSpeed = 3f;

    void Update()
    {
        gameObject.transform.Rotate(0, rotateSpeed, 0, Space.Self);
    }
}
