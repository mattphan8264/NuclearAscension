using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InstantiateCave : MonoBehaviour
{
    public int height;
    public int width;

    public GameObject box;
    public GameObject tl;
    public GameObject tr;
    public GameObject bl;
    public GameObject br;

    void Start()
    {
        readString();
    }

    void readString()
    {
        string path = "Assets/Scripts/Stage2.txt";
        StreamReader sr = new StreamReader(path);


        string line;
        line = sr.ReadLine();
        string[] tokens = line.Split(' ');
        int[] conversion = Array.ConvertAll<string, int>(tokens, int.Parse);

        height = conversion[0];
        width = conversion[1];

        for (int i = height - 1; i > -1; i--)
        {
            line = sr.ReadLine();
            for (int a = 0; a < width; a++)
            {
                char temp = line[a];
                if (temp == '0' || temp == '1')
                {
                    GameObject o = Instantiate(box, new Vector3 ((float) a,(float) i, 0f), Quaternion.identity);
                    o.transform.SetParent(transform, false);
                }
                else if (temp == '2')
                {
                    GameObject o = Instantiate(tl, new Vector3 ((float) a,(float) i, 0f), Quaternion.identity);
                    o.transform.SetParent(transform, false);
                }
                else if (temp == '4')
                {
                    GameObject o = Instantiate(bl, new Vector3 ((float) a,(float) i, 0f), Quaternion.identity);
                    o.transform.SetParent(transform, false);
                }
                else if (temp == '3')
                {
                    GameObject o = Instantiate(tr, new Vector3 ((float) a,(float) i, 0f), Quaternion.identity);
                    o.transform.SetParent(transform, false);
                }
                else if (temp == '5')
                {
                    GameObject o = Instantiate(br, new Vector3 ((float) a,(float) i, 0f), Quaternion.identity);
                    o.transform.SetParent(transform, false);
                }

            }
        }

        sr.Close();
    }
}