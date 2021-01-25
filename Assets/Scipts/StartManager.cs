using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public Dropdown d;

    public GameObject controlCanvas;
    public GameObject canvas;
    public GameObject startUp;
    public Text usernameTextBox;

    public void Submit()
    {
        Data.username = usernameTextBox.text;
        canvas.SetActive(true);
        startUp.SetActive(false);
    }

    public void startGame()
    {
        int i = d.value;

        if (i == 0)
        {
            Data.difficulty = 3;
        }
        else if (i == 1)
        {
            Data.difficulty = 2;
        }
        else if (i == 2)
        {
            Data.difficulty = 1;
        }
        else
            Data.difficulty = 3;

        Data.totalTime = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("The Loop");
    }
    public void back()
    {
        controlCanvas.SetActive(false);
        canvas.SetActive(true);
    }
    public void controls()
    {
        canvas.SetActive(false);
        controlCanvas.SetActive(true);
        controlCanvas.SetActive(true);
    }
}
