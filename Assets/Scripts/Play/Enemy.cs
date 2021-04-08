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
    public ObjectManager objectManager;
    public SpawnManager spawnManager;

    Animator anim;

    private float curShotDelay = 0f;
    private float maxShotDelay = 1.5f;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    public AudioClip explosionSound;



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

        if (enemyName == "Boss A")
            anim = GetComponent<Animator>();
    }

    //When Component is activated this method will be called.
    void OnEnable()
    {
        switch (enemyName)
        {
            case "Boss A":
                health = 3000;
                Invoke("Stop", 2);
                break;
            case "Enemy A":
                health = 3;
                break;
            case "Enemy B":
                health = 15;
                break;
            case "Enemy C":
                health = 40;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyName == "Boss A")
        {
            return;
        }
            
        Fire();
        Reload();
    }

    void Stop()
    {
        if (gameObject.activeSelf)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
        }
        Invoke("Think", 2);
        
    }
    
    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        curPatternCount = 0;

        if (health <= 0) return;

        switch (patternIndex)
        {
            case 0:
                //shoot forward
                FireForward();
                break;
            case 1:
                FireShot();
                //shoot like shot gun
                break;
            case 2:
                //shoot like Arc
                FireArc();
                break;
            case 3:
                //shoot to all direction
                FireAround();
                break;
        }
    }

    void FireForward()
    {

        //Fire 4 bullets
        GameObject bulletR = objectManager.MakeObj("bulletEnemyB");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletL = objectManager.MakeObj("bulletEnemyB");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletRR = objectManager.MakeObj("bulletEnemyC");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        GameObject bulletLL = objectManager.MakeObj("bulletEnemyC");
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;

        Rigidbody2D rbR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rbL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rbRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rbLL = bulletLL.GetComponent<Rigidbody2D>();

        Vector3 dirVec = (player.transform.position - transform.position).normalized;

        rbR.AddForce(dirVec * 8, ForceMode2D.Impulse);
        rbL.AddForce(dirVec * 8, ForceMode2D.Impulse);
        rbRR.AddForce(dirVec * 6, ForceMode2D.Impulse);
        rbLL.AddForce(dirVec * 6, ForceMode2D.Impulse);

        curPatternCount++;

        if(curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireFoward", 2);
        }
        else
        {
            Invoke("Think", 3);
        }
    }
    void FireShot()
    {
        //Fire bullets in Shotgun style
        for(int index = 0; index < 5; index++)
        {
            GameObject bullet = objectManager.MakeObj("bulletEnemyD");
            bullet.transform.position = transform.position;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = (player.transform.position - transform.position).normalized;
            Vector2 ranVec = new Vector2(Random.Range(-0.4f, 0.4f), Random.Range(0, 0.4f));

            dirVec += ranVec;

            rb.AddForce(dirVec * 5, ForceMode2D.Impulse);
        }
        

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireShot", 1.5f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }
    void FireArc()
    {
        //Fire Arc style
       
        GameObject bullet = objectManager.MakeObj("bulletEnemyD");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPatternCount / maxPatternCount[patternIndex]), -1);

        rb.AddForce(dirVec * 5, ForceMode2D.Impulse);
        

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireArc", 0.1f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }
    void FireAround()
    {
        int roundNumA = 40;
        int roundNumB = 30;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;
        for( int index = 0; index < roundNum; index++)
        {

            GameObject bullet = objectManager.MakeObj("bulletEnemyD");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum), Mathf.Sin(Mathf.PI * 2 * index / roundNum));
            

            rb.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireAround", 3);
        }
        else
        {
            Invoke("Think", 3);
        }
    }




    void Fire()
    {
        if (curShotDelay < maxShotDelay)
        {
            return;
        }

        if(enemyName == "Enemy A")
        {
            GameObject bullet = objectManager.MakeObj("bulletEnemyA");
            bullet.transform.position = transform.position;
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = (player.transform.position - transform.position).normalized;
            
            rb.AddForce(dirVec * bulletSpeed, ForceMode2D.Impulse);
        }
        else if(enemyName == "Enemy C")
        {
            GameObject bulletR = objectManager.MakeObj("bulletEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            GameObject bulletL = objectManager.MakeObj("bulletEnemyB");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;

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
        if(enemyName == "Boss A")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }
        

        if (health <= 0)
        {
            PlayerController playerLogic = player.GetComponent<PlayerController>();
            playerLogic.score += enemyScore;

            //Item drop
            int ran = enemyName == "Boss A" ? 9 : Random.Range(0, 10);
            if(ran < 2)
            {
                //Boom
                GameObject itemBoom = objectManager.MakeObj("itemBoom");
                itemBoom.transform.position = transform.position;

            }
            else if( ran <= 5)
            {
                //Power
                GameObject itemPower = objectManager.MakeObj("itemPower");
                itemPower.transform.position = transform.position;

            }
            else if( ran < 8)
            {
                //Coin
                GameObject itemCoin = objectManager.MakeObj("itemCoin");
                itemCoin.transform.position = transform.position;
            }
            else
            {

            }
            //Destroy(gameObject); -> setactive(false)
            AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            gameObject.SetActive(false);
            CancelInvoke();
            transform.rotation = Quaternion.identity;
            spawnManager.CallExplosion(transform.position, enemyName);

            //Boss kill
            if(enemyName =="Boss A")
            {
                spawnManager.StageEnd();
            }
        }
    }
    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "BorderBullet" && enemyName != "Boss A")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;

        }
        else if(collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);
            collision.gameObject.SetActive(false);

        }


    }
}

