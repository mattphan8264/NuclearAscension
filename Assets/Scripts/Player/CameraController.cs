using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float speed;
    Transform player;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        speed = 1;
        //on player spawn
        transform.position = player.transform.position;
    }

    // Update is called once per frame
    Vector3 position;
    float roundX;
    float roundY;
    void LateUpdate()
    {
        if (Vector2.Distance((Vector2)transform.position, (Vector2)player.position) > 20)
        {
            speed = 20;
        }
        else if (Vector2.Distance((Vector2)transform.position, (Vector2)player.position) > 15)
        {
            speed = 15;
        }
        else if (Vector2.Distance((Vector2)transform.position, (Vector2)player.position) > 10)
        {
            speed = 10;
        }
        else
        {
            speed = 6;
        }

        float step = speed * Time.deltaTime;
        transform.position = (Vector3)Vector2.MoveTowards(transform.position, player.position, step) + new Vector3(0, 0, -20); 
    }

    float roundToNearestPixel(float input)
    {
        float value = input * 100f;
        value = Mathf.Round(value);
        float retVal = value * (1 / 100f);
        return retVal;
    }
}
