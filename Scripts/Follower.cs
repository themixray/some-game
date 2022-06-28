using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform target;
    public bool followX = true;
    public bool followY = true;
    public bool followZ = true;
    public Vector3 offset;

    void Update()
    {
        Vector3 pos = target.position + offset;
        if (!followX) pos.x = transform.position.x;
        if (!followY) pos.y = transform.position.y;
        if (!followZ) pos.z = transform.position.z;
        transform.position = pos;
    }
}
