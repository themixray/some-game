using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public GameObject panel;
    public GameObject minipanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) ShowPanel();
    }

    public void ShowPanel()
    {
        panel.SetActive(!panel.activeSelf);
        if (!panel.activeSelf)
            if (minipanel.activeSelf)
                minipanel.SetActive(false);
    }

    public void ShowMiniPanel()
    {
        minipanel.SetActive(!minipanel.activeSelf);
    }

    public void SetStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

}
