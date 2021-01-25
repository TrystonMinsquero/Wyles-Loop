using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchText : MonoBehaviour
{
    public GameObject WelcomeToLoop;
    public GameObject WelcomeBack;

    // Start is called before the first frame update
    void Start()
    {
        if (!Data.tutorialFinish)
        {
            WelcomeToLoop.SetActive(true);
            WelcomeBack.SetActive(false);
        }
        else
        {
            WelcomeToLoop.SetActive(false);
            WelcomeBack.SetActive(true);
        }
            
            
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Data.tutorialFinish = true;
       
    }
}
