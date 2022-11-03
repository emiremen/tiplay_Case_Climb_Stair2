using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 distance;
    [SerializeField] float smoothing = .1f;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(0, target.transform.position.y + distance.y, distance.z), smoothing);
    }
}
