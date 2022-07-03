using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int speed;
    Vector3 lastPos;

    void Start()
    {

        lastPos = transform.position;

    }


    void Update()
    {

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        RaycastHit hit;
        if (Physics.Linecast(lastPos, transform.position, out hit))
        {

            print(hit.transform.name);
            //Destroy(gameObject);
            gameObject.SetActive(false);

        }

        lastPos = transform.position;

    }
}
