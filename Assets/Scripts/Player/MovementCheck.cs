using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCheck : MonoBehaviour
{
    public int collide;
    List<Transform> collidedBlocks;

    // Start is called before the first frame update
    void Start()
    {
        collide = 0;
        collidedBlocks = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void destroyBlock(Transform input)
    {
        if (collidedBlocks.Contains(input))
        {
            collidedBlocks.Remove(input);
            collide--;
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (!collidedBlocks.Contains(other.transform))
            {
                collide++;
                collidedBlocks.Add(other.transform);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            if (collidedBlocks.Contains(other.transform))
            {
                collide--;
                if (collide < 0)
                {
                    collide = 0;
                }
                collidedBlocks.Remove(other.transform);
            }

        }
    }
}
