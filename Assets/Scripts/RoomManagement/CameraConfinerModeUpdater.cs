using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConfinerModeUpdater : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner cinemachineConfiner;
    [SerializeField] private bool activateEdgeConfiner;

    private float lowCameraDamping = 2;
    private float mediumCameraDamping = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Harmony.Tags.Player))
        {
            cinemachineConfiner.m_ConfineScreenEdges = activateEdgeConfiner;

            if (activateEdgeConfiner)
                cinemachineConfiner.m_Damping = mediumCameraDamping;
            else
                cinemachineConfiner.m_Damping = lowCameraDamping;
        }
    }
}
