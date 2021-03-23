using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyObjs;
    [SerializeField]
    private Transform[] spawnPoints;

    private float maxSpawnDelay = 1;
    private float curSpawnDelay;

    public GameObject player;

    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;




    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay >= maxSpawnDelay)
        {
            SpawnEnemy();
            maxSpawnDelay = Random.Range(0.5f, 2f);
            curSpawnDelay = 0;
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
        int ranEnemy = Random.Range(0, 3);
        int ranPoint = Random.Range(0, 9);

        GameObject enemy = Instantiate(enemyObjs[ranEnemy], spawnPoints[ranPoint].position, spawnPoints[ranPoint].rotation);

        Rigidbody2D rb = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        float speed = enemyLogic.Speed;
        Debug.Log(speed);

        if (ranPoint == 5 || ranPoint == 6)
        {
            rb.velocity = new Vector2(-1, -1) * speed;
            enemy.transform.Rotate(Vector3.back * 45);
            
        }
        else if(ranPoint == 7 || ranPoint == 8)
        {
            rb.velocity = new Vector2(1, -1) * speed;
            enemy.transform.Rotate(Vector3.forward * 45);

        }
        else
        {
            rb.velocity = new Vector2(0, -1) * speed;
        }
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

