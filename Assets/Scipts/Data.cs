using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data Instance;

    public static bool stickyHat;
    public bool STICKYHAT;

    public static bool doubleJump;
    public bool DOUBLEJUMP;

    public static bool weapon;
    public bool WEAPON;

    public static bool wallJump;
    public bool WALLJUMP;

    public static bool dash;
    public bool DASH;

    public static float xPos;
    public static float yPos;

    public static bool tutorialFinish;
    public bool TUTORIALFINISH;

    public static int difficulty;
    public int DIFFICULTY;

    public static string username;
    public static float totalTime;

    

    private void Start()
    {
        stickyHat = STICKYHAT;

        doubleJump = DOUBLEJUMP;

        weapon = WEAPON;

        wallJump = WALLJUMP;

        dash = DASH;

        tutorialFinish = TUTORIALFINISH;

        difficulty = DIFFICULTY;

        username = "";
        totalTime = 0;

        
    }


    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);

            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);

        }


        if (GameObject.Find("Wyle"))
        {
            xPos = GameObject.Find("Wyle").transform.position.x;
            yPos = GameObject.Find("Wyle").transform.position.y;
        }
    }
}
