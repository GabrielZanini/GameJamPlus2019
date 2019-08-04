using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatesManager : MonoBehaviour
{
    [SerializeField] List<Gate> gates;
    [SerializeField] EnemySpawner enemySpawner;

    int id = 0;
    public Gate active;

    public float spawnMinTime = 15f;
    public float spawnMaxTime = 30f;
    private float spawnTimer;


    void Awake()
    {
        spawnTimer = Random.Range(spawnMinTime, spawnMaxTime);
        SelectGate();
    }

    private void Update()
    {
        Timer();
    }

    void SelectGate()
    {
        id = Random.Range(0, gates.Count);

        for (int i=0; i<gates.Count; i++)
        {
            if (i == id)
            {
                //gates[i].gameObject.SetActive(true);
                gates[i].Open();
                active = gates[i];
            }
            else
            {
                //gates[i].gameObject.SetActive(false);
                gates[i].Close();
            }
        }

        enemySpawner.ChangeTarget();
    }

    void Timer()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        else
        {
            spawnTimer = Random.Range(spawnMinTime, spawnMaxTime);
            SelectGate();
        }
    }
}