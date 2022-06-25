using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform head;
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public Vector2 moveSpeed = new Vector2(1,1);
    public Vector2 moveStrong = new Vector2(5,5);
    public Vector2 angle = new Vector2(0,0);
    public bool back = true;
    bool pressed;
    void CameraRotate()
    {
        angle.x += Input.GetAxis("Mouse X") * 5;
        angle.y -= Input.GetAxis("Mouse Y") * 5;
        //angle.y = Mathf.Clamp();


        target.rotation = Quaternion.Euler(0, angle.x, angle.y);
    }

    void Start()
    {
        angle.x = 0;
        angle.y = 0;
    }


    void FixedUpdate()
    {
        if (back)
        {
            target.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = Vector3.Lerp(transform.position, 
                head.position + new Vector3(head.right.x * moveStrong.x, offset.y, head.right.z * moveStrong.x), smoothSpeed);
            
        }
        if(!back && pressed)
        {
            
            CameraRotate();
        }
        transform.LookAt(target);

        if (Input.GetMouseButton(1))
        {
            
            pressed = true;
            back = false;
        }
        else pressed = false;
    }
}
