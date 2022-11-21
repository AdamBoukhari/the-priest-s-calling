using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointCollisions : MonoBehaviour
{
    private EndlessPit pitScript;

    void Start()
    {
        pitScript = transform.parent.GetComponent<EndlessPit>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Harmony.Tags.Player))
        {
            pitScript.SetSpawnPoint(transform.position);
        }
    }
}
