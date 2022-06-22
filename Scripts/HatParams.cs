using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatParams : MonoBehaviour
{
    public Vector3 offset;
    public float distance;
    public Transform target;
    public GameObject tip;
    public Transform targetPos;

    private bool picked = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (picked == false)
        {
            if (Vector3.Distance(target.position, transform.position) < distance)
            {
                if (Input.GetKeyDown(KeyCode.F)) {
                    tip.SetActive(false);
                    transform.parent = target.transform;
                    picked = true;
                }
            }
        } else
        {
            transform.position = targetPos.position;
            transform.rotation = target.transform.rotation;
        }
    }
}
