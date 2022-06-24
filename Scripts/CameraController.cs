using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public Vector2 moveSpeed = new Vector2(1, 1);
    public Vector2 moveStrong = new Vector2(5, 5);
    public Vector2 angle = new Vector2(0, 0);
    public bool back = true;
    public float Period;
    bool gameIsStart;
    bool pressed;

    void Start()
    {
        angle.x = 0;
        angle.y = 0;
    }

    float normalizeRotate(float r)
    {
        if (r < 0) return 360 + r;
        else if (r > 360) return r - (Mathf.Round(r / 360) * 360);
        return r;
    }


    void LateUpdate()
    {
        transform.LookAt(target);

        if (!back && pressed && gameIsStart)
        {
            transform.RotateAround(target.position,
                                       Vector3.up,
                                       normalizeRotate(Input.GetAxis("Mouse X") * 5));

            transform.RotateAround(target.position,
                                            new Vector3(1, 0, 1),
                                            -Input.GetAxis("Mouse Y") * 5);
        }


        if (Input.GetMouseButton(1))
        {
            back = false;
            pressed = true;
            angle.x -= Input.GetAxis("Mouse X") * moveSpeed.x;
            angle.x = normalizeRotate(angle.x);
            angle.y -= Input.GetAxis("Mouse Y") * moveSpeed.y;
            angle.y = normalizeRotate(angle.y);
        }
        else pressed = false;

    }



    private void FixedUpdate()
    {
        

        if (!gameIsStart)
        {
            float CornerSpeed = (Mathf.PI * 2 / Period) * Time.time;
            transform.Translate(new Vector3(Mathf.Cos(CornerSpeed) * 10, 0, Mathf.Sin(CornerSpeed) * 10) * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < 5) gameIsStart = true;
        }
        else
        {

            if (back)
            {
                transform.position = Vector3.Lerp(transform.position,
                    target.position + new Vector3(target.right.x * moveStrong.x, offset.y, target.right.z * moveStrong.x), smoothSpeed);
            }
        }
    }
}
