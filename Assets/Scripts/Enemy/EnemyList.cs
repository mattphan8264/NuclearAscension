using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    static List<GameObject> list;
    GameObject Enemy;

    void Awake()
    {
        list = new List<GameObject>();
        list.Add((GameObject)Resources.Load("Prefabs/Enemy/BasicEnemyDF1", typeof(GameObject)));
        list.Add((GameObject)Resources.Load("Prefabs/Enemy/BasicEnemyDF2", typeof(GameObject)));
        list.Add((GameObject)Resources.Load("Prefabs/Enemy/HoppingEnemyDF1", typeof(GameObject)));
        list.Add((GameObject)Resources.Load("Prefabs/Enemy/HoppingEnemyDF2", typeof(GameObject)));
    }

    public static GameObject getEnemy()
    {
        int temp = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (temp + 1 > GlobalStats.spawnCap)
        {
            return null;
        }

        temp = Random.Range(0, list.Count);
        return list[temp];
    }


}