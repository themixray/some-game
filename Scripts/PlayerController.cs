using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 5f;
    public float rotateSpeed = 5f;
    public float jumpSpeed = 5f;
    public LayerMask platformMask;
    public Transform center;
    public float checkRadius = .1f;
    public Collider feet;
    public float walkTrigger;


    private Rigidbody rig;
    private Animator anim;
    private Vector3 startPos;
    private Quaternion startRotate;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        startPos = transform.position;
        startRotate = transform.rotation;
    }

    // Update is called once per fram

    public bool isOnGround()
    {
        return Physics.CheckSphere(new Vector3(feet.bounds.min.x, feet.bounds.min.y, feet.bounds.min.z), checkRadius, platformMask) ||
               Physics.CheckSphere(new Vector3(feet.bounds.max.x, feet.bounds.min.y, feet.bounds.max.z), checkRadius, platformMask) ||
               Physics.CheckSphere(new Vector3(feet.bounds.min.x, feet.bounds.min.y, feet.bounds.max.z), checkRadius, platformMask) ||
               Physics.CheckSphere(new Vector3(feet.bounds.max.x, feet.bounds.min.y, feet.bounds.min.z), checkRadius, platformMask);
    }

    void Update()
    {
        //float fps = 1.0f / Time.smoothDeltaTime;

        bool walking = false;
        if (isOnGround())
        {
            if (Input.GetKeyDown("space"))
            {

                rig.AddForce(new Vector3(0f, jumpSpeed, 0f));
            }
            if (Input.GetKey("a"))
            {
                transform.RotateAround(center.position, Vector3.up, -(rotateSpeed * Time.deltaTime));
                //rig.AddTorque(new Vector3(0, -rotateSpeed, 0), ForceMode.VelocityChange);
                walking = true;
            }
            if (Input.GetKey("d"))
            {
                transform.RotateAround(center.position, Vector3.up, rotateSpeed * Time.deltaTime);
                //rig.AddTorque(new Vector3(0, rotateSpeed, 0), ForceMode.VelocityChange);
                walking = true;
            }
            if (Input.GetKey("w"))
            {
                rig.velocity = transform.right * -speed;
                walking = true;
            }
            if (Input.GetKey("s"))
            {
                rig.velocity = transform.right * speed;
                walking = true;
            }
        }
        anim.SetBool("Walking", walking);


        if (transform.position.y < -5)
        {
            transform.position = startPos;
            transform.rotation = startRotate;
        }


    }
}