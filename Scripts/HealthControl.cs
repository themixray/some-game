using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthControl : MonoBehaviour
{
    public Camera cam;
    public Transform now;
    public Transform max;
    public Transform min;
    public TextMeshPro[] texts;

    private float health = 100;
    private float nowStartScaleZ = 2;

    public void SetHealth(float hp)
    {
        health = hp;
        UpdateHealth();
    }
    public float GetHealth()
    {
        return health;
    }

    public void MinusHealth(float hp)
    {
        if (health - hp >= 0) {
            health -= hp;
        } else {
            health = 0;
        }
        UpdateHealth();
    }

    public void PlusHealth(float hp)
    {
        if (health + hp <= 100)
        {
            health += hp;
        } else
        {
            health = 100;
        }
        UpdateHealth();
    }

    public void UpdateHealth()
    {
        now.position = Vector3.Lerp(min.position, max.position, health / 100);
        now.localScale = new Vector3(now.localScale.x, now.localScale.y, nowStartScaleZ * (health / 100));
        foreach (TextMeshPro text in texts) {
            text.text = ((int)health) + "/100";
        }
    }

    void Start()
    {
        nowStartScaleZ = now.localScale.z;
    }

    void Update()
    {
        if (health <= 0)
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}
