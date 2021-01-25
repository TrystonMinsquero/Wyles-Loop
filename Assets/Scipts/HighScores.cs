using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HighScores : MonoBehaviour
{
    const string privateCode = "fJqeeaqwSk2Aud-5irhTxQ8aEuhQb_7kimgKMm75paCQ";
    const string publicCode = "5f814712eb371809c4748bfa";
    const string webURL = "http://dreamlo.com/lb/";

    private List<Score> highScores;

    [Header("Canvases")]
    public GameObject victoryCanvas;
    public GameObject highScoreCanvas;
    [Header("Text Boxes")]
    public Text[] userTextBoxes;
    public Text[] timeTextBoxes;
    [Header("Buttons")]
    public UnityEngine.UI.Button nextPage;
    public UnityEngine.UI.Button prevPage;

    public int pageIndex = 0;
    public int maxPageIndex;


    public void Awake()
    {


        //Set Up Canvases
        victoryCanvas.SetActive(true);
        highScoreCanvas.SetActive(false);

        //Instaitate and add score
        highScores = new List<Score>();

        Debug.Log(Data.username + ": " + formatFloatToInt(Data.totalTime));
        AddNewHighScore(Data.username, formatFloatToInt(Data.totalTime));


        prevPage.interactable = false;
        

    }

    public void displayHighScores()
    {
        //Switch Canvases
        victoryCanvas.SetActive(false);
        highScoreCanvas.SetActive(true);


        //Clear text boxes
        for(int j = 0; j < 10; j++)
        {
            userTextBoxes[j].text = "";
            timeTextBoxes[j].text = "";
        }


        //Print to Screen
        for (int i = 0; i < 10; ++i)
        {
            if(i + (10 * pageIndex) < highScores.Count)
            {
                userTextBoxes[i].text = (i + (10 * pageIndex) + 1) + ". " + highScores[i + (10 * pageIndex)].username;
                timeTextBoxes[i].text = formatFloatToTime(formatIntToFloat(highScores.ElementAt<Score>(i + (10 * pageIndex)).time));
            }
        }

    }

    public void AddNewHighScore(string username, int score)
    {
        StartCoroutine(UploadNewHighscore(username, score));
    }
    public void DownloadHighScores()
    {
        Debug.Log("Intiate Download");
        StartCoroutine(DownloadHighScoresCoroutine());
    }

    IEnumerator UploadNewHighscore(string username, int score)
    {
        UnityWebRequest www = UnityWebRequest.Get(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(username) + "/" + score);
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            print("Upload Succesful");
            DownloadHighScores();
        }
        else
            print("Error Uploading: " + www.error);
        
    }

    IEnumerator DownloadHighScoresCoroutine()
    {
        Debug.Log("Downloading");
        UnityWebRequest www = UnityWebRequest.Get(webURL + publicCode + "/pipe");
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            Debug.Log("Downloaded");
            Debug.Log(www.downloadHandler.text);
            FormatHighScores(www.downloadHandler.text);
            maxPageIndex = (highScores.Count - 1) / 10;
        }
        else
            print("Error Downloading: " + www.error);
        
    }

    void FormatHighScores(string text)
    {
        highScores.Clear();
        string[] entries = text.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < entries.Length; ++i)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            highScores.Add(new Score(entryInfo[0], int.Parse(entryInfo[1])));
            Debug.Log(highScores.ElementAt<Score>(i).username + ": " + highScores.ElementAt<Score>(i).time);
        }
        highScores.Reverse();
    }

    public struct Score
    {
        public string username;
        public int time;

        public Score(string user, int tim)
        {
            username = user;
            time = tim;
        }
    }

    public string formatFloatToTime(float time)
    {
        string deci = ".";
        string sec = "";
        string min = "";
        if ((int)((time - (int)time) * 100) < 10)
            deci += "0";
        deci += +(int)((time - (int)time) * 100);
        if (time < 60)
        {
            if (time < 10)
                sec += "0";
            sec += (int)(time);
            return "00:" + sec + deci;
        }
        else
        {
            if (time % 60 < 10)
                sec += "0";
            if ((int)(time % 60) == 0)
                sec += "0";
            sec += (int)(time % 60);

            min = "" + (int)(time) / 60;
            return min + ":" + sec + deci;
        }

    }

    public int formatFloatToInt(float seconds)
    {
        return (int)(seconds * 100);
    }

    public float formatIntToFloat(int seconds)
    {
        return seconds / 100 + (seconds % 100)/100.0f;
    }
}
