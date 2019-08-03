using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GatesManager gates;
    [Space]
    [SerializeField] List<Grave> graves;

    [Header("Spawning Settings")]
    [SerializeField] float spawnTime = 5f;
    [SerializeField] float spawnRandom = 1f;
    [SerializeField] float spawnTimer;

    [Header("Enemys Settings")]
    [SerializeField] int startEnemys = 2;
    [SerializeField] int moreEnemys = 2;
    [SerializeField] int totalEnemys = 0;
    [SerializeField] int enemyCount = 1;

    [Header("Graves Settings")]
    [SerializeField] int startGraves= 3;
    [SerializeField] int moreGraves = 1;
    [SerializeField] int totalGraves = 0;
    [SerializeField] int gravesCount = 0;


    [Header("Enemys")]
    [SerializeField] List<EnemyScript> alive = new List<EnemyScript>();



    void Awake()
    {
        StartGraves();
        ResetTimer();
    }
        
    void Update()
    {
        Timer();
    }

    // Cronometro para Spawnar um Inimigo
    void Timer()
    {
        if (enemyCount > 0)
        {
            if (spawnTimer > 0f)
            {
                spawnTimer -= Time.deltaTime;
            }
            else
            {
                TrySpawn();
                ResetTimer();
            }
        }        
    }

    // Reiniciar o Cronometro com um modificador randomico
    void ResetTimer()
    {
        spawnTimer = spawnTime + Random.Range(-spawnRandom, spawnRandom);
    }

    // Tentar Spawnar inimigos
    void TrySpawn()
    {
        List<Grave> grounds = new List<Grave>();

        // Listar as Lapides que podem Spawnar inimigos
        foreach (var g in graves)
        {
            if (g.state == GraveState.Ground)
            {
                grounds.Add(g);
            }
        }

        // Verificar se existem lapides que podem Spawnar inimigos
        if (grounds.Count > 0)
        {
            // Escolhe uma lapide aleatória
            int index = Random.Range(0, grounds.Count);
            SpawnEnemy(grounds[index]);
        }
        else
        {
            // Nenhuma Lapide pode Spawnar (Fim de Fase(?))
            //Debug.Log("No Grave Left.");
        }
    }

    // Spawn o Inimigo na Lapide que foi passada por parametro
    void SpawnEnemy(Grave ground)
    {
        var enemy = Instantiate(enemyPrefab, ground.transform.position, Quaternion.identity);
        ground.spawn = enemy.GetComponent<EnemyScript>();
        ground.spawn.target = gates.active.transform;
        ground.spawn.spawner = this;
        alive.Add(ground.spawn);
        ground.SetHole();
    }

    
    void StartGraves()
    {
        List<int> ids = new List<int>();

        for (int i=0; i< graves.Count; i++)
        {
            if (graves[i].state == GraveState.Hidden)
            {
                ids.Add(i);
            }            
        }

        // Spawnar as covas do que Zombies nacem
        for (int i=0; i<startGraves; i++)
        {
            int id = ids[Random.Range(0, ids.Count)];
            ids.Remove(id);
            graves[id].SetGround();
        }
    }
}
