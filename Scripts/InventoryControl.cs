using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryControl : MonoBehaviour
{
    public GameObject panel;
    public GameObject minipanel;
    public TextMeshProUGUI inventoryText;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            panel.SetActive(!panel.activeSelf);
            if (!panel.activeSelf)
                if (minipanel.activeSelf)
                    minipanel.SetActive(false);
        }
    }

    public bool HasLine(int line)
    {
        int count = 0;
        string text = inventoryText.text;
        foreach (string s in text.Split('\n'))
        {
            count++;
            if (line == count)
            {
                return s != "";
                break;
            }
        }
        return false;
    }

    public void ShowMiniPanel(int line)
    {
        if (HasLine(line))
            minipanel.SetActive(!minipanel.activeSelf);
    }
}
