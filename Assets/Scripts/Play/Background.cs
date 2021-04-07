using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float speed;

    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    float cameraHeight;

    void Awake()
    {
        cameraHeight = Camera.main.orthographicSize * 2; // make it double
    }

    void Update()
    {
        BackgroundMove();
        BackgroundScroll();
    }
    void BackgroundMove()
    {
        //Background movement
        Vector3 curpos = transform.position;
        Vector3 nextpos = Vector3.down * speed * Time.deltaTime;
        transform.position = curpos + nextpos;
    }

    void BackgroundScroll()
    {
        //Backgrouond reuse
        if (sprites[endIndex].position.y < -cameraHeight)
        {
            //sprites reuse
            Vector3 backSpritesPos = sprites[startIndex].localPosition;
            Vector3 frontSpritesPos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritesPos + Vector3.up * 10;

            //index change
            int startIndexSave = startIndex;
            startIndex = endIndex;
            endIndex = startIndexSave - 1 == -1 ? sprites.Length - 1 : --startIndexSave;
        }
    }
}
