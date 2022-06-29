using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleControl : MonoBehaviour
{
    public float distance;
    public PlayerController player;
    public Transform me;

    private bool picked = false;

    void Start()
    {

    }

    void Update()
    {
        if (!picked && !player.anim.GetBool("Drinking"))
        {
            if (Vector3.Distance(player.center.position, me.position) < distance)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    picked = true;
                    gameObject.SetActive(false);
                    player.Drink(true);
                }
            }
        }
    }
}
