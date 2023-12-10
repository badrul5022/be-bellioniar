using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public List<Transform> spawnPoints;
    void Awake()
    {
        instance = this;
        foreach(Transform obj in this.transform)
        {
            spawnPoints.Add(obj);
            obj.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
