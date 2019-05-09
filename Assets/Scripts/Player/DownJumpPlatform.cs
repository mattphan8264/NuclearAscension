using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownJumpPlatform : MonoBehaviour
{

    Collider2D myCollider;
    void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void turnOffCollider()
    {
        if (myCollider.enabled == true)
        {
            myCollider.enabled = false;
            Invoke("turnOnCollider", .5f);
        }

    }

    void turnOnCollider()
    {
        myCollider.enabled = true;
    }
}
