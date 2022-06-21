using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public Vector2 moveSpeed = new Vector2(1,1);
    public Vector2 moveStrong = new Vector2(5,5);
    public Vector2 angle = new Vector2(0,0);
    public bool back = false;

    void Start()
    {
        angle.x = 0;
        angle.y = 0;
    }

    Vector3 RYtoXZ(float r)
    { 
        float x = 2 * Mathf.PI / 360 * r;
        return new Vector3(Mathf.Cos(x), 0, Mathf.Sin(x));
    }
    Vector3 RZtoXY(float r)
    {
        float x = 2 * Mathf.PI / 360 * r;
        return new Vector3(Mathf.Cos(x), Mathf.Sin(x), 0);
    }

    float normalizeRotate(float r)
    {
        if (r < 0) return 360 + r;
        else if (r > 360) return r - (Mathf.Round(r / 360) * 360);
        return r;
    }

    void FixedUpdate()
    {
        if (back)
        {
            transform.position = Vector3.Lerp(transform.position, 
                target.position + new Vector3(target.right.x * moveStrong.x, offset.y, target.right.z * moveStrong.x), smoothSpeed);
        }
        else
        {
            Vector3 v = RYtoXZ(angle.x) + RZtoXY(angle.y);
            transform.position = Vector3.Lerp(transform.position, 
                target.position + offset + ((new Vector3(v.x/2*moveStrong.x,v.y*moveStrong.y,moveStrong.x*v.z) * moveStrong.y)), smoothSpeed);
        }
        transform.LookAt(target);

        if (Input.GetMouseButton(1))
        {
            back = false;
            angle.x -= Input.GetAxis("Mouse X") * moveSpeed.x;
            angle.x = normalizeRotate(angle.x);
            angle.y -= Input.GetAxis("Mouse Y") * moveSpeed.y;
            angle.y = normalizeRotate(angle.y);
        }



    }
}
