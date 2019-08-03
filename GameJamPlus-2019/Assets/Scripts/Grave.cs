using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] GameObject Hole;
    [SerializeField] GameObject Ground;
    [SerializeField] GameObject Buried;
    [SerializeField] GameObject Stone;

    [Header("Settings")]
    public GraveState state = GraveState.Hidden;

    [Header("Enemy")]
    public EnemyScript enemy = null;
    public EnemyScript spawn = null;



    void Start()
    {
        SetState(state);
    }
    
    void Update()
    {
        // Se for um Buraco Enterrar o Inimigo
        if (state == GraveState.Hole)
        {
            BurryEnemy();
        }       
    }

    // Enterrar o Inimigo em cima do buraco
    void BurryEnemy()
    {
        // Se tiver um inimigo em cima do buraco e ele não tiver sido recente mente spawnado e ele não estiver sendo segurado pelo Jogador
        if (enemy != null && enemy != spawn && !enemy.isCarried)
        {
            Destroy(enemy.gameObject);
            SetBuried();
        }
    }

    // Muda o estado da Lápide para o parametro "s"
    void SetState(GraveState s)
    {
        if (s == GraveState.Hidden)
        {
            SetHidden();
        }
        else if (s == GraveState.Ground)
        {
            SetGround();
        }
        else if (s == GraveState.Hole)
        {
            SetHole();
        }
        else if (s == GraveState.Buried)
        {
            SetBuried();
        }
    }

    // Muda o estado da Lápide para um buraco que o Zombie cai
    public void SetHole()
    {
        Hole.SetActive(true);
        Ground.SetActive(false);
        Buried.SetActive(false);
        Stone.SetActive(true);
        state = GraveState.Hole;
    }

    // Muda o estado da Lápide para um pedaço de terra onde o Zombie foi enterrado
    public void SetBuried()
    {
        Hole.SetActive(false);
        Ground.SetActive(false);
        Buried.SetActive(true);
        Stone.SetActive(true);
        state = GraveState.Buried;
    }

    // Muda o estado da Lápide para um pedaço de terra onde o Zombie pode ser "Spawnado"
    public void SetGround()
    {
        Hole.SetActive(false);
        Ground.SetActive(true);
        Buried.SetActive(false);
        Stone.SetActive(true);
        state = GraveState.Ground;
    }

    // Muda o estado da Lápide para escondida (Ainda não aparece no jogo)
    public void SetHidden()
    {
        Hole.SetActive(false);
        Ground.SetActive(false);
        Buried.SetActive(false);
        Stone.SetActive(false);
        state = GraveState.Hidden;
    }

    // Verificar se o inimigo entrou do Collider
    private void OnTriggerEnter(Collider other)
    {
        var e = other.GetComponent<EnemyScript>();

        if (e != null)
        {
            enemy = e;
        }
    }

    // Verificar se o inimigo saiu do Collider
    private void OnTriggerExit(Collider other)
    {
        var e = other.GetComponent<EnemyScript>();

        if (e == enemy)
        {
            enemy = null;
        }

        if (e == spawn)
        {
            spawn = null;
        }
    }
}


public enum GraveState
{
    Hole,       // Buraco para enterrar o Zombie
    Ground,     // Terra onde o Zombie spawn
    Buried,     // Depois do Zombie ter sido enterrado
    Hidden      // A Lapide ainda não apareceu no campo
}