using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO; // File read

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private string[] enemyObjs;
    [SerializeField]
    private Transform[] spawnPoints;

    private float nextSpawnDelay;
    private float curSpawnDelay;

    public GameObject player;

    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;

    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;


    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "enemyA", "enemyB", "enemyC" };
        ReadSpawnFile();
    }


    void ReadSpawnFile()
    {
        //#1 initialize
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //#2 File open
        TextAsset textFile = Resources.Load("stage0") as TextAsset;

        //#3 String read
        StringReader stringReader = new StringReader(textFile.text);
        
        
        //#4 spawn Data
        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);

            if (line == null)
                break;

            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.name = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        // file close
        stringReader.Close();


        // apply delay
        nextSpawnDelay = spawnList[0].delay;

    }

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay >= nextSpawnDelay)
        {
            if(curSpawnDelay > nextSpawnDelay && !spawnEnd)
            {
                SpawnEnemy();
                curSpawnDelay = 0;

            }
        }

        //UI score
        PlayerController playerLogic = player.GetComponent<PlayerController>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);
    }

    public void UpdateLifeIcon(int life)
    {
        // UI Life Image disable
        for(int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }
        // UI Life Image enable
        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }
    public void UpdateBoomIcon(int boom)
    {
        // UI Boom Image disable
        for (int index = 0; index < 3; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }
        // UI Boom Image enable
        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].name)
        {
            case "Enemy A":
                enemyIndex = 0;
                break;
            case "Enemy B":
                enemyIndex = 1;
                break;
            case "Enemy C":
                enemyIndex = 2;
                break;
        }
        int enemyPoint = spawnList[spawnIndex].point;

        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();

        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.objectManager = objectManager;

        float speed = enemyLogic.Speed;


        if (enemyPoint == 5 || enemyPoint == 6)
        {
            rb.velocity = new Vector2(-1, -1) * speed;
            enemy.transform.Rotate(Vector3.back * 45);
            
        }
        else if(enemyPoint == 7 || enemyPoint == 8)
        {
            rb.velocity = new Vector2(1, -1) * speed;
            enemy.transform.Rotate(Vector3.forward * 45);

        }
        else
        {
            rb.velocity = new Vector2(0, -1) * speed;
        }
        // spawnIndex increase
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        // delay
        nextSpawnDelay = spawnList[spawnIndex].delay;

    }

    public void RespawnPlayer()
    {
        Invoke("RespawnPlayerExe", 2f);
        
        
    }
    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        PlayerController playerLogic = player.GetComponent<PlayerController>();
        playerLogic.isHit = false;
        
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameReplay()
    {
        SceneManager.LoadScene(0);
    }
}

