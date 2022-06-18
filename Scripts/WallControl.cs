using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallControl : MonoBehaviour
{
    public Transform start;
    public Transform end;
    public Transform finish;
    public PlayerController player;
    public float distance = 5;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!player.runningOnWall) { 
            if (Vector3.Distance(player.transform.position, transform.position) < distance)
            {
                if (Input.GetKeyDown("f"))
                {
                    player.runOnWall(this);
                }
            }
        }
    }
}
