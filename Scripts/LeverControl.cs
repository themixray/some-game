using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverControl : MonoBehaviour
{
    public float distance;
    public Transform target;

    private Animator anim;
    private bool pressed;

    void Start()
    {
        anim = GetComponent<Animator>();
        pressed = anim.GetBool("Up");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) < distance)
        {
            if (Input.GetKeyDown("f"))
            {
                pressed = !pressed;
                anim.SetBool("Up",pressed);
            }
        }
    }
}
