using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class StartButtons : MonoBehaviour
{

    public Button playButton;
    public Button exitButton;

    void Start()
    {
        playButton.onClick.AddListener(Play);
        exitButton.onClick.AddListener(Exit);
    }

    // Update is called once per frame
    void Update()
    { 

    }
        // Start is called before the first frame update
    void Exit()
    {
        Application.Quit();
    }

    void Play()
    {
        SceneManager.LoadScene("SampleScene",LoadSceneMode.Single);
    }
}
