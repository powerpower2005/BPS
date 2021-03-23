using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float speed;

    public int enemyScore;
    [SerializeField]
    private int health;
    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private int bulletSpeed;

    [SerializeField]
    private string enemyName;


    [SerializeField]
    private GameObject bulletA;
    [SerializeField]
    private GameObject bulletB;

    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;



    public GameObject player;

    private float curShotDelay = 0f;
    private float maxShotDelay = 0.5f;



    public float Speed {
        get{
            return speed;
        }
        set{
            speed = value;
        }
}
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Fire();
        Reload();
    }

    void Fire()
    {
        if (curShotDelay < maxShotDelay)
        {
            return;
        }

        if(enemyName == "Enemy A")
        {
            GameObject bullet = Instantiate(bulletA, transform.position, transform.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = (player.transform.position - transform.position).normalized;
            
            rb.AddForce(dirVec * bulletSpeed, ForceMode2D.Impulse);
        }
        else if(enemyName == "Enemy C")
        {
            GameObject bulletR = Instantiate(bulletB, transform.position + Vector3.right * 0.3f, transform.rotation);
            GameObject bulletL = Instantiate(bulletB, transform.position + Vector3.left * 0.3f, transform.rotation);

            Rigidbody2D rbR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rbL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 dirVec = (player.transform.position - transform.position).normalized;
            rbR.AddForce(dirVec * bulletSpeed, ForceMode2D.Impulse);
            rbL.AddForce(dirVec * bulletSpeed, ForceMode2D.Impulse);


        }
        curShotDelay = 0;
        
    }

    void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    public void OnHit(int dmg)
    {
        if (health <= 0)
            return;

        health -= dmg;
        spriteRenderer.sprite = sprites[1];
        Invoke("ReturnSprite", 0.1f);

        if (health <= 0)
        {
            PlayerController playerLogic = player.GetComponent<PlayerController>();
            playerLogic.score += enemyScore;

            //Item drop
            int ran = Random.Range(0, 10);
            if(ran == 0)
            {
                //Boom
                Instantiate(itemBoom, transform.position, itemBoom.transform.rotation);
            }
            else if( 1<= ran || ran <= 3)
            {
                //Power
                Instantiate(itemPower, transform.position, itemPower.transform.rotation);

            }
            else
            {
                //Coin
                Instantiate(itemCoin, transform.position, itemCoin.transform.rotation);

            }
            Destroy(gameObject);
        }
    }
    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BorderBullet")
        {
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            Destroy(collision.gameObject);
        }

        
    }
}

