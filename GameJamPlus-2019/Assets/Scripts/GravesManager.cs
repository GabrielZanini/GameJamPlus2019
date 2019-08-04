using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravesManager : MonoBehaviour
{
    [Header("Objects")]
    [Space]
    public List<Grave> graves;
    List<int> hiddenIds = new List<int>();
    
    [Header("Spawn Settings")]
    public int startGraves = 3;
    [Space]
    public float delay = 8;
    public float random = 2;
    public float spawnTimer = 0;




    void Awake()
    {
        StartGraves();
        spawnTimer = delay + Random.Range(-random, random);
    }
    
    void Update()
    {
        SpawnTimer();
    }

    void SpawnTimer()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        else
        {
            spawnTimer = delay + Random.Range(-random, random);
            GetHiddenGraves();
            SpawnRandomGrave();
        }
    }

    void StartGraves()
    {
        GetHiddenGraves();

        // Spawnar as covas do que Zombies nacem
        for (int i = 0; i < startGraves; i++)
        {
            SpawnRandomGrave();
        }
    }

    void SpawnRandomGrave()
    {
        int id = hiddenIds[Random.Range(0, hiddenIds.Count)];
        hiddenIds.Remove(id);
        graves[id].SetGround();
    }

    void GetHiddenGraves()
    {
        for (int i = 0; i < graves.Count; i++)
        {
            if (graves[i].state == GraveState.Hidden)
            {
                hiddenIds.Add(i);
            }
        }
    }
}
