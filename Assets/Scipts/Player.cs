using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO.MemoryMappedFiles;
using System.Linq;
using UnityEngine.InputSystem;
using System.Xml.Schema;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //public Variables
    [Header("Movement")]
    public int jumpCount_MAX = 2;
    public float moveSpeed_MAX = 8f;
    public float jumpSpeed = 12f;
    public float dashStrength = 25f;
    [Header("Physics")]
    public float inAirMovementMultiplier = .6f;
    public float terminalVel = 30f;
    [Header("Cooldowns")]
    public float dashCooldown = .75f;
    public float wallJumpCooldown = .25f;
    public float hitDelay = 1f;
    public float flashDelay = .1f;

    [Header("Attachables")]
    public Rigidbody2D rb;
    public GameObject weapon;
    public Animator anim;
    public Text text;

    [Header("Canvases")]
    public List<GameObject> healthBar;
    public GameObject c;
    public GameObject IconCanvas;


    [Header("Sounds")]
    public AudioSource dash;
    public AudioSource swordOpen;
    public AudioSource jump;
    public AudioSource doubleJump;
    public AudioSource death;
    public AudioSource stickyHatStart;
    public AudioSource stickyHatStop;

    //private variables

    //Number trackers
    private int health_MAX;
    private int health;
    private Vector2 pos;
    private int jumpCount;
    private float moveSpeed;
    private float gravity_init;
    private float nextDashTime;
    private float nextWallJumpTime;
    private float nextHitOnTime;
    private float nextFlashTime;

    //Conditionals
    private bool isGrounded;
    private bool onRightWall;
    private bool onLeftWall;
    private bool isRight = true;
    private bool onRoof;
    private bool isSwordOut;
    private bool stickyHatActive;


    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Start()
    {
        health_MAX = Data.difficulty;
        health = health_MAX;
        for (int i = 0; i < healthBar.Count; i++)
            healthBar.ElementAt(i).SetActive(false);
        if (health_MAX > 1)
            for (int i = 0; i < health_MAX; i++)
                healthBar.ElementAt(i).SetActive(true);
        if (health_MAX == 2)
            healthBar.ElementAt(6).SetActive(true);
        if (health_MAX == 3)
            healthBar.ElementAt(7).SetActive(true);

        jumpCount_MAX = Data.doubleJump ? jumpCount_MAX : 1;
        jumpCount = jumpCount_MAX;
        moveSpeed = moveSpeed_MAX;
        gravity_init = rb.gravityScale;
    }

    IEnumerator waitThenClose(float num)
    {
        yield return new WaitForSeconds(num);
        UnityEngine.SceneManagement.SceneManager.LoadScene("The Loop");
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log(controls.Gameplay.Jump.triggered);
        //Check Direcection
        if (controls.Gameplay.MoveRight.ReadValue<float>() > 0 && controls.Gameplay.MoveLeft.ReadValue<float>() > 0)
            pos.x = 0;
        else if (controls.Gameplay.MoveRight.ReadValue<float>() > 0)
            pos.x = 1;
        else if (controls.Gameplay.MoveLeft.ReadValue<float>() > 0)
            pos.x = -1;
        else
            pos.x = 0;

        //move horizontal
        if (Time.time > nextWallJumpTime)
            rb.velocity = new Vector2((pos.x * moveSpeed), rb.velocity.y);

        //Set Direction
        anim.SetInteger("Walk", (int)pos.x);
        if (pos.x > 0)
            isRight = true;
        else if (pos.x < 0)
            isRight = false;

        //terminal velocity check
        if (Math.Abs(rb.velocity.y) >= terminalVel)
        {
            if (rb.velocity.y > 0)
                rb.velocity = new Vector2(rb.velocity.x, terminalVel);
            else
                rb.velocity = new Vector2(rb.velocity.x, -terminalVel);
        }
        if (Math.Abs(rb.velocity.x) >= terminalVel)
        {
            if (rb.velocity.x > 0)
                rb.velocity = new Vector2(terminalVel, rb.velocity.y);
            else
                rb.velocity = new Vector2(-terminalVel, rb.velocity.y);
        }



        //Check for in air movement reduction
        moveSpeed = !isGrounded ? moveSpeed_MAX * inAirMovementMultiplier : moveSpeed_MAX;

        //Prevent Wall Stuck
        if (!isGrounded && onRightWall && pos.x < 0)
            pos.x = 0;
        if (!isGrounded && onLeftWall && pos.x > 0)
            pos.x = 0;

        //Sticky Hat
        if (Data.stickyHat)
        {
            if (onRoof && controls.Gameplay.Jump.ReadValue<float>() > 0)
            {
                rb.gravityScale = 0;
                if (!stickyHatActive)
                    stickyHatStart.Play();
                stickyHatActive = true;
            }
            else
            {
                rb.gravityScale = gravity_init;
                if (stickyHatActive)
                    stickyHatStop.Play();
                stickyHatActive = false;
            }

        }

        //Wall Jump
        if (Data.wallJump && !isGrounded && Time.time > nextWallJumpTime)
        {
            if (onRightWall && controls.Gameplay.Jump.ReadValue<float>() > 0 && controls.Gameplay.WallJump.ReadValue<float>() > 0)
            {
                isRight = true;
                anim.SetBool("InAirRight", true);
                jump.Play();
                rb.velocity = new Vector2(jumpSpeed, jumpSpeed);
                nextWallJumpTime = Time.time + wallJumpCooldown;
            }
            if (onLeftWall && controls.Gameplay.Jump.ReadValue<float>() > 0 && controls.Gameplay.WallJump.ReadValue<float>() > 0)
            {
                isRight = false;
                anim.SetBool("InAirLeft", true);
                jump.Play();
                rb.velocity = new Vector2(-jumpSpeed, jumpSpeed);
                nextWallJumpTime = Time.time + wallJumpCooldown;
            }
        }



        //Dash
        if (Data.dash && Time.time > nextDashTime)
            if (controls.Gameplay.Dash.ReadValue<float>() > 0)
            {
                nextDashTime = Time.time + dashCooldown;
                dash.Play();
                Debug.Log("Dash " + dashStrength * pos.x);
                rb.gravityScale = 0;
                if (isRight)
                    for (int i = 0; i < 3; i++)
                        rb.AddForce(Vector2.right * dashStrength);
                else
                    for (int i = 0; i < 3; i++)
                        rb.AddForce(Vector2.left * dashStrength);
                rb.gravityScale = gravity_init;
            }

        //Jump/DoubleJump
        if (controls.Gameplay.Jump.triggered && jumpCount > 0)
        {
            if (!isGrounded && jumpCount == jumpCount_MAX)
                jumpCount -= 1;
            if (jumpCount_MAX > 1 && jumpCount > 0 && jumpCount < jumpCount_MAX && !onLeftWall && !onRightWall)
                doubleJump.Play();
            if (isGrounded && jumpCount == jumpCount_MAX)
                jump.Play();
            jumpCount -= 1;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        //Set Weapon Direction
        if (isRight)
        {
            weapon.transform.position = transform.position + new Vector3(.6f, -.06f, 0);
            weapon.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (!isRight)
        {
            weapon.transform.position = transform.position + new Vector3(-.6f, -.06f, 0);
            weapon.transform.localScale = new Vector3(1, 1, 1);
        }
        else
            weapon.transform.position = transform.position + new Vector3(0, -.06f, 0);

        //Weapon Active
        if (Data.weapon && controls.Gameplay.Weapon.ReadValue<float>() > 0)
        {
            weapon.SetActive(true);
            anim.SetBool("IsSword", true);
            if (!isSwordOut)
                swordOpen.Play();

            isSwordOut = true;
        }
        else
        {
            weapon.SetActive(false);
            anim.SetBool("IsSword", false);
            isSwordOut = false;
        }

        //Reset
        if (controls.Gameplay.Reset.triggered)
        if (controls.Gameplay.Reset.triggered)
            UnityEngine.SceneManagement.SceneManager.LoadScene("The Loop");


        //Anim Bool Check
        anim.SetBool("InAirRight", !isGrounded && isRight);
        anim.SetBool("InAirLeft", !isGrounded && !isRight);


        //Flash on hit
        if (Time.time < nextHitOnTime)
        {

            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            if (Time.time > nextFlashTime)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                nextFlashTime = Time.time + flashDelay;
            }


        }
        else
            gameObject.GetComponent<SpriteRenderer>().enabled = true;

        Data.totalTime += Time.deltaTime;

        text.text = (Data.totalTime).ToString("n2");

    }

    void UpdateHealthBar()
    {
        if (health_MAX > 1)
        {
            if (health_MAX == 2)
            {
                if (health == 1)
                {
                    healthBar.ElementAt(1).SetActive(false);
                    healthBar.ElementAt(4).SetActive(true);
                }
                if (health == 0)
                {
                    healthBar.ElementAt(0).SetActive(false);
                    healthBar.ElementAt(3).SetActive(true);

                }
            }
            if (health_MAX == 3)
            {
                if (health == 2)
                {
                    healthBar.ElementAt(2).SetActive(false);
                    healthBar.ElementAt(5).SetActive(true);
                }
                if (health == 1)
                {
                    healthBar.ElementAt(1).SetActive(false);
                    healthBar.ElementAt(4).SetActive(true);
                }
                if (health == 0)
                {
                    healthBar.ElementAt(0).SetActive(false);
                    healthBar.ElementAt(3).SetActive(true);

                }
            }
        }
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor")
        {
            jumpCount = jumpCount_MAX;
            isGrounded = true;
        }
        if (collision.collider.tag == "Right Wall")
        {
            Debug.Log("Right Wall");
            onRightWall = true;
        }
        if (collision.collider.tag == "Left Wall")
        {
            Debug.Log("Left Wall");
            onLeftWall = true;
        }
        if (collision.collider.tag == "Roof")
        {
            onRoof = true;
        }


    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor")
        {
            isGrounded = false;
        }
        if (collision.collider.tag == "Right Wall")
        {
            onRightWall = false;
        }
        if (collision.collider.tag == "Left Wall")
        {
            onLeftWall = false;
        }
        if (collision.collider.tag == "Roof")
        {
            onRoof = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Death Zone")
        {
            death.Play();
            weapon.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine(waitThenClose(.12f));
        }

        if (collision.tag == "Enemy" && Time.time > nextHitOnTime)
        {
            health--;
            UpdateHealthBar();
            if (health <= 0)
            {
                death.Play();
                weapon.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(waitThenClose(.12f));
            }
            else
            {
                death.Play();
                nextHitOnTime = Time.time + hitDelay;
            }
        }

        if (collision.tag == "P")
        {
            c.SetActive(true);
            IconCanvas.SetActive(false);

            isGrounded = true;
            anim.SetBool("InAirLeft", false);
            anim.SetBool("InAirLeftSword", false);
            anim.SetBool("InAirRight", false);
            anim.SetBool("InAirRightSword", false);
            anim.SetInteger("Walk", 0);
            gameObject.GetComponent<Player>().enabled = false;

        }


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" && Time.time > nextHitOnTime)
        {
            health--;
            UpdateHealthBar();
            if (health <= 0)
            {
                death.Play();
                weapon.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                StartCoroutine(waitThenClose(.12f));
            }
            else
            {
                death.Play();
                nextHitOnTime = Time.time + hitDelay;
            }

        }
    }
}

