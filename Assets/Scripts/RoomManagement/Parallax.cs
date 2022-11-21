using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float dist; //if it's 0, it'll move with the level. If it's 100, it'll follow the camera. 
    [SerializeField] private GameObject cam;
    private Vector2 initialLocation;

    // Start is called before the first frame update
    void Start()
    {
        initialLocation = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(initialLocation.x + cam.transform.position.x * (dist / 100), initialLocation.y + cam.transform.position.y * (dist / 100));
    }
}
