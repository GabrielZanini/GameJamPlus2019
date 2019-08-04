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
    List<int> hiddenIds = new List<int>();

    [Header("Spawning Settings")]
    [SerializeField] float spawnTime = 5f;
    [SerializeField] float spawnRandom = 1f;
    [SerializeField] float spawnTimer;
        
    
    [Header("Enemys")]
    public List<EnemyScript> alive = new List<EnemyScript>();







    void Awake()
    {
        ResetTimer();
    }
        
    void Update()
    {
        Timer();
    }

    // Cronometro para Spawnar um Inimigo
    void Timer()
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

    // Reiniciar o Cronometro com um modificador randomico
    void ResetTimer()
    {
        spawnTimer = spawnTime + Random.Range(-spawnRandom, spawnRandom);
    }

    // Tentar Spawnar inimigos
    void TrySpawn()
    {
        List<Grave> notBuried = new List<Grave>();

        // Listar as Lapides que podem Spawnar inimigos
        foreach (var g in graves)
        {
            if (g.state != GraveState.Hidden && g.state != GraveState.Buried && !g.hasSpawnedEnemy)
            {
                notBuried.Add(g);
            }
        }

        // Verificar se existem lapides que podem Spawnar inimigos
        if (notBuried.Count > 0)
        {
            // Escolhe uma lapide aleatória
            int index = Random.Range(0, notBuried.Count);
            SpawnEnemy(notBuried[index]);
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
        ground.spawn.originalGrave = ground;
        ground.spawn.target = gates.active.transform;
        ground.spawn.spawner = this;
        alive.Add(ground.spawn);
        ground.SetHole();
    }


    public void ChangeTarget()
    {
        foreach (var enemy in alive)
        {
            enemy.target = gates.active.transform;
        }
    }
}
