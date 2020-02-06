using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsScene : MonoBehaviour
{
    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            source.loop = false;
            source.Play();
            float delayTime = 1.0f;
            float timer = 0.0f;
            while (timer < delayTime)
            {
                timer += Time.deltaTime;
            }
            SceneManager.LoadScene("TheGame");

        }
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
        {
            SceneManager.LoadScene("MainMenu");
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
