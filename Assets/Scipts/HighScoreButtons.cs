using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreButtons : MonoBehaviour
{
    public HighScores manager;


    public void DisplayHighScores()
    {
        manager.displayHighScores();
    }
    public void NextPage()
    {
        if (manager.pageIndex < manager.maxPageIndex)
        {
            manager.pageIndex += 1;
            if (manager.pageIndex >= manager.maxPageIndex)
                manager.nextPage.interactable = false;

            DisplayHighScores();
            manager.prevPage.interactable = true; ;
        }
        else
            manager.nextPage.interactable = false;

    }

    public void PreviousPage()
    {
        if (manager.pageIndex > 0)
        {
            manager.pageIndex -= 1;
            if (manager.pageIndex <= 0)
                manager.prevPage.interactable = false;

            DisplayHighScores();
            manager.nextPage.interactable = true;
        }
        else
            manager.prevPage.interactable = false;

        
    }
}
