using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipControl : MonoBehaviour
{
    public Transform target;
    public Camera cam;
    public float distance;
    public GameObject image;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) < distance)
        {
            image.SetActive(true);
            image.transform.LookAt(cam.transform.position);
        } else
        {
            image.SetActive(false);
        }
    }
}
