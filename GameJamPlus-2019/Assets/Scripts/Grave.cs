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
    [SerializeField] BoxCollider collider;

    [Header("Settings")]
    public GraveState state = GraveState.Hidden;

    [Header("Enemy")]
    public EnemyScript enemy = null;
    public EnemyScript spawn = null;


    public Transform StoneTranform {
        get { return Stone.transform; }
    }

    void Start()
    {
        SetState(state);
    }
    
    void Update()
    {
        // Se for um Buraco Enterrar o Inimigo
        if (state == GraveState.Hole)
        {
            FallEnemy();
        }       
    }

    // O Inimigo cai dentro do buraco
    void FallEnemy()
    {
        // Se tiver um inimigo em cima do buraco e ele não tiver sido recentemente spawnado e ele não estiver sendo segurado pelo Jogador
        if (enemy != null && enemy != spawn && !enemy.isCarried)
        {            
            SetWithEnemy();
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
        else if (s == GraveState.WithEnemy)
        {
            SetWithEnemy();
        }
    }

    // Muda o estado da Lápide para um buraco que o Zombie cai
    public void SetHole()
    {
        DisableObjects();
        Hole.SetActive(true);
        state = GraveState.Hole;
    }

    // Muda o estado da Lápide para um pedaço de terra onde o Zombie foi enterrado
    public void SetBuried()
    {
        DisableObjects();
        Buried.SetActive(true);
        state = GraveState.Buried;
        Destroy(enemy.gameObject);
    }

    // Muda o estado da Lápide para um buraco com um Zombie dentro
    public void SetWithEnemy()
    {
        DisableObjects();
        Hole.SetActive(true);
        state = GraveState.WithEnemy;
        enemy.transform.position = transform.position + (Vector3.down * 0.5f);
        enemy.currentGrave = this;
        enemy.InTheHole();
    }

    // Muda o estado da Lápide para um pedaço de terra onde o Zombie pode ser "Spawnado"
    public void SetGround()
    {
        DisableObjects();
        Ground.SetActive(true);
        state = GraveState.Ground;
    }

    // Muda o estado da Lápide para escondida (Ainda não aparece no jogo)
    public void SetHidden()
    {
        DisableObjects();
        collider.enabled = false;
        state = GraveState.Hidden;
    }


    void DisableObjects()
    {
        Hole.SetActive(false);
        Ground.SetActive(false);
        Buried.SetActive(false);
        Stone.SetActive(false);
        collider.enabled = true;
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
    WithEnemy,  // Buraco para enterrar o Zombie
    Ground,     // Terra onde o Zombie spawn
    Buried,     // Depois do Zombie ter sido enterrado
    Hidden      // A Lapide ainda não apareceu no campo
}