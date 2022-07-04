using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform center;
    public GameObject[] items;
    public bool runningOnWall = false;
    public TextMeshProUGUI inventoryText;
    public List<ItemParams> inventory = new List<ItemParams>();
    public float wallspeed;
    public HealthControl health;
    public WeaponControl weapon;

    [HideInInspector] public Animator anim;
    private Rigidbody rig;
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
        items = GameObject.FindGameObjectsWithTag("item");
    }

    public void Heal(float hp)
    {
        health.PlusHealth(hp);
    }

    public void UnDrink()
    {
        Drink(false);
    }

    public void Drink(bool v)
    {
        anim.SetBool("Drinking", v);
    }

    public void Shot()
    {
        weapon.Shot();
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
            if (Input.GetKeyDown(KeyCode.F))
            {
                float ld = -1;
                GameObject li = null;
                foreach (GameObject i in items)
                {
                    if (i.activeSelf)
                    {
                        ItemParams ip = i.GetComponent<ItemParams>();
                        float d = Vector3.Distance(center.transform.position, i.transform.position);
                        if (d < ip.distance && (ld > d || ld == -1))
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

            anim.SetBool("Walking",Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ||
                                   Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ||
                                   Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) ||
                                   Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow));
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

        if (center.transform.position.y < -5f)
        {
            transform.position = startPos;
            transform.rotation = startRotate;
        }
    }
}