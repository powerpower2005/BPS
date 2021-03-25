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
    private int maxPower = 3;
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


    public int life;
    public int score;

    public bool isHit;


    void Awake()
    {
        anim = GetComponent<Animator>();
        
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Fire();
        Boom();
        Reload();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (isTouchedRight && h == 1 || isTouchedLeft && h == -1)
        {
            h = 0;
        }
        float v = Input.GetAxisRaw("Vertical");
        if (isTouchedTop && v == 1 || isTouchedBottom && v == -1)
        {
            v = 0;
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Horizontal", (int)h);
        }
    }

    void Fire()
    {
        if (Input.GetButton("Fire1") && curShotDelay > maxShotDelay)
        {
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

    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void Boom()
    {
        if (!Input.GetButton("Fire2"))
            return;
        if (isBoomed)
            return;
        if (boom == 0)
            return;
        boom--;
        spawnManager.UpdateBoomIcon(boom);
        isBoomed = true;
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

            if (isHit)
                return;

            isHit = true;
            life--;
            spawnManager.UpdateLifeIcon(life);

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