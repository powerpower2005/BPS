using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //This script is for player movemnet
    private GameObject player;

    [SerializeField]
    private int speed;

    private bool isTouchedTop;
    private bool isTouchedBottom;
    private bool isTouchedLeft;
    private bool isTouchedRight;

    [SerializeField]
    private int bulletSpeed;
    [SerializeField]
    private int power;
    private int maxPower = 6;
    [SerializeField]
    private int boom;
    private int maxBoom = 3;
    public bool isBoomed;

    [SerializeField]
    private GameObject bulletA;
    [SerializeField]
    private GameObject bulletB;
    [SerializeField]
    public GameObject boomEffect;

    Animator anim;

    private float curShotDelay = 0f;
    private float maxShotDelay = 0.3f;

    public SpawnManager spawnManager;
    public ObjectManager objectManager;

    public GameObject[] followers;


    public int life;
    public int score;

    public bool isHit;

    public bool isRespawnTime;
    SpriteRenderer spriteRenderer;

    public bool[] joyControl;
    public bool isControl;

    public bool isButtonA;
    public bool isButtonB;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        Unbeatable();
        Invoke("Unbeatable", 3);
    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;

        if (isRespawnTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            for(int index = 0; index<followers.Length; index++)
            {
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            for (int index = 0; index < followers.Length; index++)
            {
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();
    }

    public void JoyPanel(int type)
    {
        for(int index = 0; index < 9; index++)
        {
            joyControl[index] = index == type;
        }
    }
    public void JoyDown()
    {
        isControl = true;
    }
    public void JoyUp()
    {
        isControl = false;
    }
    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (joyControl[0]) { h = -1; v = 1; }
        if (joyControl[1]) { h = 0; v = 1; }
        if (joyControl[2]) { h = 1; v = 1; }
        if (joyControl[3]) { h = -1; v = 0; }
        if (joyControl[4]) { h = 0; v = 0; }
        if (joyControl[5]) { h = 1; v = 0; }
        if (joyControl[6]) { h = -1; v = -1; }
        if (joyControl[7]) { h = 0; v = -1; }
        if (joyControl[8]) { h = 1; v = -1; }


        if (isTouchedRight && h == 1 || isTouchedLeft && h == -1)
        {
            h = 0;
        }
        if (isTouchedTop && v == 1 || isTouchedBottom && v == -1)
        {
            v = 0;
        }
        /*button input
        if (isTouchedRight && h == 1 || isTouchedLeft && h == -1 || !isControl)
        {
            h = 0;
        }
        if (isTouchedTop && v == 1 || isTouchedBottom && v == -1 || !isControl)
        {
            v = 0;
        }
        */

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Horizontal", (int)h);
        }
    }

    public void ButtonADown()
    {
        isButtonA = true;
    }
    public void ButtonAUp()
    {
        isButtonA = false;
    }
    public void ButtonBDown()
    {
        isButtonB = true;
    }
    void Fire()
    {
        //Mouse Input
        if (!Input.GetButton("Fire1"))
            return;

        //Button Input
        //if (!isButtonA)
        //    return;

        if (curShotDelay < maxShotDelay)
            return;

        
        switch (power)
        {
            case 1:
                GameObject bullet = objectManager.MakeObj("bulletPlayerA");
                bullet.transform.position = transform.position;
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(Vector2.up * bulletSpeed);
                break;
                    
            case 2:
                GameObject bulletR = objectManager.MakeObj("bulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;
                GameObject bulletL = objectManager.MakeObj("bulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rbR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rbL = bulletL.GetComponent<Rigidbody2D>();

                rbR.AddForce(Vector2.up * bulletSpeed);
                rbL.AddForce(Vector2.up * bulletSpeed);
                break;

            case 3:
            case 4:
            case 5:
            case 6:
                GameObject bulletRR = objectManager.MakeObj("bulletPlayerA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.2f;
                GameObject bulletC = objectManager.MakeObj("bulletPlayerB");
                bulletC.transform.position = transform.position;
                GameObject bulletLL = objectManager.MakeObj("bulletPlayerA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.2f;


                Rigidbody2D rbRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rbC = bulletC.GetComponent<Rigidbody2D>();
                Rigidbody2D rbLL = bulletLL.GetComponent<Rigidbody2D>();

                rbRR.AddForce(Vector2.up * bulletSpeed);
                rbC.AddForce(Vector2.up * bulletSpeed);
                rbLL.AddForce(Vector2.up * bulletSpeed);

                break;

            }
            curShotDelay = 0;
        

    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void Boom()
    {
        //Mouse Input
        if (!Input.GetButton("Fire2"))
            return;
        //if (!isButtonB)
        //    return;
        if (isBoomed)
            return;
        if (boom == 0)
            return;
        boom--;
        spawnManager.UpdateBoomIcon(boom);
        isBoomed = true;
        isButtonB = false;
        //1. effect enable, boomEffect disable in 3 secs
        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 3f);

        //2. enemy hit
        GameObject[] enemyA = objectManager.GetPool("enemyA");
        GameObject[] enemyB = objectManager.GetPool("enemyB");
        GameObject[] enemyC = objectManager.GetPool("enemyC");

        for (int index = 0; index < enemyA.Length; index++)
        {
            if (enemyA[index].activeSelf)
            {
                Enemy enemyLogic = enemyA[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }
            
        }
        for (int index = 0; index < enemyB.Length; index++)
        {
            if (enemyB[index].activeSelf)
            {
                Enemy enemyLogic = enemyB[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }

                
        }
        for (int index = 0; index < enemyC.Length; index++)
        {
            if (enemyC[index].activeSelf)
            {
                Enemy enemyLogic = enemyC[index].GetComponent<Enemy>();
                enemyLogic.OnHit(1000);
            }

                
        }

        //3. enemy bullet disable
        GameObject[] bulletA = objectManager.GetPool("bulletEnemyA");
        GameObject[] bulletB = objectManager.GetPool("bulletEnemyB");
        GameObject[] bulletC = objectManager.GetPool("bulletEnemyC");
        GameObject[] bulletD = objectManager.GetPool("bulletEnemyD");


        for (int index = 0; index < bulletA.Length; index++)
        {
            if (bulletA[index].activeSelf)
            {
                bulletA[index].SetActive(false);
            }
        }
        for (int index = 0; index < bulletB.Length; index++)
        {
            if (bulletB[index].activeSelf)
            {
                bulletB[index].SetActive(false);
            }
        }
        for (int index = 0; index < bulletC.Length; index++)
        {
            if (bulletC[index].activeSelf)
            {
                bulletC[index].SetActive(false);
            }
        }
        for (int index = 0; index < bulletD.Length; index++)
        {
            if (bulletD[index].activeSelf)
            {
                bulletD[index].SetActive(false);
            }
        }


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchedTop = true;
                    break;
                case "Bottom":
                    isTouchedBottom = true;

                    break;
                case "Left":
                    isTouchedLeft = true;

                    break;
                case "Right":
                    isTouchedRight = true;

                    break;

            }
        }
        else if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            // More than 2 Bullets make a collision with  player at the same time. it cause a bug that decreases player's life more than 1.
            // For debug this, Make a boolean variable to know whether being hit or not.
            if (isRespawnTime)
                return;
            if (isHit)
                return;

            isHit = true;
            life--;
            spawnManager.UpdateLifeIcon(life);
            spawnManager.CallExplosion(transform.position, "Player");

            if(life == 0)
            {
                spawnManager.GameOver();
            }
            else
            {
                spawnManager.RespawnPlayer();
            }
            gameObject.SetActive(false);

        }
        else if(collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Coin":
                    score += 100;
                    break;
                case "Power":
                    if(power == maxPower)
                    {
                        score += 50;
                    }
                    else
                    {
                        power++;
                        AddFollower();
                    }
                    break;
                case "Boom":
                   if(boom == maxBoom)
                    {
                        score += 50;
                    }
                    else
                    {
                        boom++;
                        spawnManager.UpdateBoomIcon(boom);
                    }
                    break;
            }
            collision.gameObject.SetActive(false);
        }
    }

    private void AddFollower()
    {
        if (power == 4)
        {
            followers[0].SetActive(true);
        }
        else if (power == 5)
        {
            followers[1].SetActive(true);
        }
        else if (power == 6)
        {
            followers[2].SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchedTop = false;
                    break;
                case "Bottom":
                    isTouchedBottom = false;

                    break;
                case "Left":
                    isTouchedLeft = false;

                    break;
                case "Right":
                    isTouchedRight = false;

                    break;

            }
        }
    }

    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomed = false;
    }
}