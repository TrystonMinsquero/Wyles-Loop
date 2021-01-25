using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class finalTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Text>().text = "Your final Time was : \n" + formatTime(Data.totalTime);
    }

    public string formatTime(float time)
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

}
