using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAgain : MonoBehaviour
{
    public void playAgain()
    {
        Data.doubleJump = true;
        Data.dash = true;
        Data.weapon = true;
        Data.wallJump = true;
        Data.stickyHat = true;

        Data.tutorialFinish = false;

        UnityEngine.SceneManagement.SceneManager.LoadScene("Start Menu");
    }
}
