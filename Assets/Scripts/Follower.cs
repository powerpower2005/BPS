using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{

    private float curShotDelay = 0f;
    private float maxShotDelay = 0.6f;

    [SerializeField]
    private int bulletSpeed;

    public Vector3 followPos;
    public int followeDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;



    public ObjectManager objectManager;


    void Awake()
    {
        parentPos = new Queue<Vector3>();
    }


    void Update()
    {
        Watch();
        Follow();
        Fire();
        Reload();
    }

    private void Watch()
    {
        //Input
        if (!parentPos.Contains(parent.position))
        {
            parentPos.Enqueue(parent.position);
        }
        

        //Return value in few frames.
        //Count = frame.
        if (parentPos.Count > followeDelay)
        {
            followPos = parentPos.Dequeue();
        }
        else if (parentPos.Count > followeDelay)
        {
            followPos = parent.position;
        }
        
    }

    private void Reload()
    {
        curShotDelay += Time.deltaTime;
    }

    void Fire()
    {
        if (Input.GetButton("Fire1") && curShotDelay > maxShotDelay)
        {
            GameObject bullet = objectManager.MakeObj("bulletFollower");
            bullet.transform.position = transform.position;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.up * bulletSpeed);

            curShotDelay = 0;
        }

    }

     void Follow()
    {
        transform.position = followPos;
    }
}
