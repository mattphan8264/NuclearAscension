using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeCheckBottom : MonoBehaviour
{
    public bool slopeBottomCheck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            slopeBottomCheck = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            slopeBottomCheck = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            slopeBottomCheck = false;
        }
    }
}
