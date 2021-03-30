using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Invoke("Disable", 2f);
    }

     void Disable()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void StartExplosion(string target)
    {
        anim.SetTrigger("OnExplosion");

        switch (target)
        {
            case"Enemy A":
                transform.localScale = Vector3.one * 0.7f;
                break; 
            case "Enemy B":
            case "Player":
                transform.localScale = Vector3.one * 1f;
                break;
            case "Enemy C":
                transform.localScale = Vector3.one * 2f;
                break;
            case "Boss A":
                transform.localScale = Vector3.one * 3f;
                break;
        }

        
    }
}
