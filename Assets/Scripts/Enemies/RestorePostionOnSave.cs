using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorePostionOnSave : MonoBehaviour
{
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }
    private void OnDisable()
    {
        transform.position = initialPosition;
    }

    private void OnEnable()
    {
        initialPosition = transform.position;
    }
}
