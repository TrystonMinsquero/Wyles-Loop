using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemLoss : MonoBehaviour
{
    public GameObject c;
    public Button doubleJump;
    public Button dash;
    public Button sword;
    public Button wallJump;
    public Button stickyHat;
    public Button random;
    public Button keep;
    public GameObject outOfItems;

    public Image djIcon;
    public Image dIcon;
    public Image sIcon;
    public Image wjIcon;
    public Image shIcon;

    public GameObject tutorialBlock;

    public GameObject Player;


    public Sprite djOff;
    public Sprite dOff;
    public Sprite sOff;
    public Sprite wjOff;  
    public Sprite shOff;
    public Sprite rOff;
    public Sprite kiOff;

    public Sprite djIconOff;
    public Sprite dIconOff;
    public Sprite sIconOff;
    public Sprite wjIconOff;
    public Sprite shIconOff;



    private int itemCount=5;
    private bool noItems = false;

    private void Start()
    {
        c.SetActive(false);

        if (!Data.doubleJump)
        {
            doubleJump.GetComponent<Image>().sprite = djOff;
            djIcon.GetComponent<Image>().sprite = djIconOff;
            doubleJump.GetComponent<Button>().enabled = false;
            itemCount--;
        }
        if (!Data.dash)
        {
            dash.GetComponent<Image>().sprite = dOff;
            dIcon.GetComponent<Image>().sprite = dIconOff;
            dash.GetComponent<Button>().enabled = false;
            itemCount--;
        }
        if (!Data.weapon)
        {
            sword.GetComponent<Image>().sprite = sOff;
            sIcon.GetComponent<Image>().sprite = sIconOff;
            sword.GetComponent<Button>().enabled = false;
            itemCount--;
        }
        if (!Data.wallJump)
        {
            wallJump.GetComponent<Image>().sprite = wjOff;
            wjIcon.GetComponent<Image>().sprite = wjIconOff;
            wallJump.GetComponent<Button>().enabled = false;
            itemCount--;
        }
        if (!Data.stickyHat)
        {
            stickyHat.GetComponent<Image>().sprite = shOff;
            shIcon.GetComponent<Image>().sprite = shIconOff;
            stickyHat.GetComponent<Button>().enabled = false;
            itemCount--;
        }

        if(Data.tutorialFinish)
        {
            tutorialBlock.SetActive(true);

            Player.transform.position = new Vector3(Data.xPos, Data.yPos, 0);
        }
        else
        {
            Player.transform.position = new Vector3(GameObject.Find("Tutorial Start").transform.position.x, GameObject.Find("Tutorial Start").transform.position.y, 0);
        }

        if (itemCount <= 0)
        {
            random.GetComponent<Image>().sprite = rOff;
            random.enabled = false;
            noItems = true;
        }

        if(noItems)
        {
            keep.GetComponent<Image>().sprite = kiOff;
            keep.GetComponent<Button>().enabled = false;

            outOfItems.SetActive(true);
        }
            

    }

    public void turnOffDoubleJump()
    {
        Data.doubleJump = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("The Loop");
        
    }
    public void turnOffDash()
    {
        Data.dash = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("The Loop");
    }
    public void turnOffSword()
    {
        Data.weapon = false; 
        UnityEngine.SceneManagement.SceneManager.LoadScene("The Loop");
    }
    public void turnOffWallJump()
    {
        Data.wallJump = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("The Loop");
    }
    public void turnOffStickyHat()
    {
        Data.stickyHat = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene("The Loop");
    }
    public void turnOffRandom()
    {
        int count = -1;
    
        
        while(count<0)
        {
            int r = (int)((Random.value * 5) + 1);

            Debug.Log(r);

            if (r == 5 && Data.doubleJump)
            {
                turnOffDoubleJump();
                count++;
            }  
            else if (r == 4 && Data.dash)
            {
                turnOffDash();
                count++;
            }
                
            else if (r == 3 && Data.weapon)
            {
                turnOffSword();
                count++;
            }  
            else if (r == 2 && Data.wallJump)
            {
                turnOffWallJump();
                count++;
            }   
            else if (r == 1 && Data.stickyHat)
            {
                turnOffStickyHat();
                count++;
            }
                
        }

        
        

        UnityEngine.SceneManagement.SceneManager.LoadScene("The Loop");
    }
    public void turnOffNone()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("The Loop");
    }
    public void toVictory()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Victory");
    }
}
