using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{


    //Load Scene by title
    public void LoadPlayScene()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadOptionScene()
    {
        SceneManager.LoadScene(2);
    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }


    
}
