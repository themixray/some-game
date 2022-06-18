using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public float normalSpeed = 5f;
    public float shiftSpeed = 2f;
    public float ctrlSpeed = 7f;
    public float rotateSpeed = 5f;
    public float jumpSpeed = 5f;
    public float wallspeed = 0.125f;
    public float speed = 5f;


    public LayerMask platformMask;
    public Transform center;
    public float checkRadius = .1f;
    public Collider feet;
    public float walkTrigger;
    public Vector3 wallOffset;
    


    private Rigidbody rig;
    private Animator anim;
    private Vector3 startPos;
    private Quaternion startRotate;
    public bool runningOnWall = false;
    private WallControl wall = null;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        startPos = transform.position;
        startRotate = transform.rotation;
        speed = normalSpeed;
    }

    // Update is called once per fram

    public bool isOnGround()
    {
        return Physics.CheckSphere(new Vector3(feet.bounds.min.x, feet.bounds.min.y, feet.bounds.min.z), checkRadius, platformMask) ||
               Physics.CheckSphere(new Vector3(feet.bounds.max.x, feet.bounds.min.y, feet.bounds.max.z), checkRadius, platformMask) ||
               Physics.CheckSphere(new Vector3(feet.bounds.min.x, feet.bounds.min.y, feet.bounds.max.z), checkRadius, platformMask) ||
               Physics.CheckSphere(new Vector3(feet.bounds.max.x, feet.bounds.min.y, feet.bounds.min.z), checkRadius, platformMask);
    }

    public void runOnWall(WallControl w)
    {
        wall = w;
        runningOnWall = true;
        anim.SetBool("RunningOnWall", true);
        anim.SetBool("Walking", false);
        transform.position = wall.start.position + wallOffset;
        rig.useGravity = false;
        transform.rotation = new Quaternion(0, 0, 0, 0);

    }

    void Update()
    {
        if (!runningOnWall)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                speed = ctrlSpeed;
                anim.SetBool("Ctrl", true);
            }
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                speed = shiftSpeed;
                anim.SetBool("Shift", true);
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                speed = normalSpeed;
                anim.SetBool("Ctrl", false);
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = normalSpeed;
                anim.SetBool("Shift", false);
            }
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
        } else
        {
            transform.position = Vector3.Lerp(transform.position, wall.end.position, wallspeed);
            if (((int)transform.position.x) == ((int)wall.end.position.x) &&
                ((int)transform.position.y) == ((int)wall.end.position.y) &&
                ((int)transform.position.z) == ((int)wall.end.position.z))
            {
                rig.useGravity = true;
                runningOnWall = false;
                anim.SetBool("RunningOnWall",false);
                transform.position = wall.finish.position;
            }
        }
    }
}