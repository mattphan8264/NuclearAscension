using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnTrigger : MonoBehaviour
{

    void Awake()
    {
        Camera camera = GameObject.FindObjectOfType<Camera>();
        BoxCollider2D myCollider = GetComponent<BoxCollider2D>();
        float halfHeight = camera.orthographicSize;
        float halfWidth = camera.aspect * halfHeight;
        myCollider.size = new Vector2(halfWidth * 2, halfHeight * 2);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
