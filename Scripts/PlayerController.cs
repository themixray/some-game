using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

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


    public CameraController cameraControl;
    public LayerMask platformMask;
    public Transform center;
    public float checkRadius = .1f;
    public Collider feet;
    public float walkTrigger;
    public GameObject[] items;
    public float itemDistance = 3f;
    public bool runningOnWall = false;
    public TextMeshProUGUI inventoryText;
    public List<ItemParams> inventory = new List<ItemParams>();



    private Rigidbody rig;
    private Animator anim;
    private Vector3 startPos;
    private Quaternion startRotate;
    private WallControl wall = null;
    private float wallProgress = 0;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        startPos = transform.position;
        startRotate = transform.rotation;
        speed = normalSpeed;
        items = GameObject.FindGameObjectsWithTag("item");
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
        wallProgress = 0;

        anim.SetBool("RunningOnWall", true);
        anim.SetBool("Walking", false);

        transform.position = wall.start.position;
        transform.rotation = new Quaternion(0, 0, 0, 0);

        rig.useGravity = false;
        rig.velocity = new Vector3(0, 0, 0);
    }

    public void reloadInventory()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        foreach (ItemParams i in inventory)
        {
            if (dict.ContainsKey(i.Name)) dict[i.Name]++;
            else dict.Add(i.Name, 1);
        }
        string text = "";
        foreach (KeyValuePair<string, int> entry in dict)
        {
            if (entry.Value > 1)
                text += entry.Key+" "+entry.Value+"x"+"\n";
            else 
                text += entry.Key+"\n";
        }
        inventoryText.text = text;
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
            bool jumped = false;
            if (isOnGround())
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rig.AddForce(new Vector3(0f, jumpSpeed, 0f));
                    jumped = true;
                }
                if (Input.GetKey(KeyCode.W))
                {
                    rig.velocity = transform.right * -speed;
                    walking = true;
                }
                if (Input.GetKey(KeyCode.S))
                {
                    rig.velocity = transform.right * speed;
                    walking = true;
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.RotateAround(center.position, Vector3.up, -(rotateSpeed * Time.deltaTime));
                walking = true;
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.RotateAround(center.position, Vector3.up, rotateSpeed * Time.deltaTime);
                walking = true;
            }

            if (walking || jumped)  
            {
                cameraControl.back = true;
                cameraControl.angle.x = 0;
                cameraControl.angle.y = 0;
            }

            anim.SetBool("Walking", walking);

            if (transform.position.y < -5)
            {
                transform.position = startPos;
                transform.rotation = startRotate;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                float ld = -1;
                GameObject li = null;
                foreach (GameObject i in items)
                {
                    if (i.activeSelf)
                    {
                        float d = Vector3.Distance(center.transform.position, i.transform.position);
                        if (d < itemDistance && (ld > d || ld == -1))
                        {
                            ld = d;
                            li = i;
                        } 
                    }
                }

                if (li != null)
                {
                    li.SetActive(false);
                    inventory.Add(li.GetComponent<ItemParams>());
                    reloadInventory();
                }
            }
        } else
        {
            transform.position = Vector3.Lerp(wall.start.position, wall.end.position, wallProgress);
            wallProgress += wallspeed;
            if (wallProgress >= 1)
            {
                rig.useGravity = true;
                runningOnWall = false;
                anim.SetBool("RunningOnWall",false);
                transform.position = wall.finish.position;
            }
        }
    }
}