using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Objects")]
    public Transform target;
    public GameObject fire;
    public EnemySpawner spawner;
    public Grave currentGrave;
    public Grave originalGrave;
    public PlayerMovement lastTouch;
    public Rigidbody rigidbody;

    [Header("Settings")]
    //velocidade do inimigo
    public float speed = 1;
    public EnemyState state = EnemyState.Normal;

    [Header("Counters")]
    public float stunCounter;
    public float holeCounter;
    public float stunTime = 10f;
    public float holeTime = 5f;
    

    // Properties
    public bool isNormal {
        get { return state == EnemyState.Normal; }
    }
    public bool isStunned {
        get { return state == EnemyState.Stunned; }
    }
    public bool isCarried {
        get { return state == EnemyState.Carried; }
    }
    public bool isInTheHole {
        get { return state == EnemyState.InTheHole; }
    }

    // Private
    Vector3 position;
    bool firstStun = true;
    bool firstHole = true;



    void FixedUpdate()
    {
        //chama os métodos de movimentação
        if (state == EnemyState.Normal)
        { 
            Move();
            Turning();
        }
        else if (state == EnemyState.Stunned) 
        {
            StunTimer();
        }
        else if (state == EnemyState.Carried)
        {
            
        }
        else if (state == EnemyState.InTheHole)
        {
            HoleTimer();
        }
    }

    private void OnDestroy()
    {
        if (currentGrave != null)
        {
            currentGrave.winner.score.AddScore(50);
        }

        spawner.alive.Remove(this);
    }

    void StunTimer()
    {
        if (stunCounter > 0)
        {
            stunCounter -= Time.fixedDeltaTime;
        }
        else
        {
            disengage();
        }
    }

    void HoleTimer()
    {
        if (holeCounter > 0)
        {
            holeCounter -= Time.fixedDeltaTime;
        }
        else
        {
            disengage();
        }
    }

    void Move()
    {
        //movimenta o inimigo
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
    }

    void Turning()
    {
        //zera a posição y do inimigo
        position = target.position;
        position.y = 0;
        //rotaciona o inimigo
        transform.rotation = Quaternion.LookRotation(position);
    }


    private void OnTriggerEnter(Collider col)
    {
        //se colidir com tag "Hit"
        if ( col.gameObject.tag == "Hit")
        {
            var player = col.GetComponentInParent<PlayerMovement>();

            if (player != null)
            {
                lastTouch = player;
            }

            Stun();
        }
    }

    void disengage()
    {
        Vector3 position = transform.position;
        position.y = 0;
        transform.position = position;

        if (currentGrave != null)
        {
            currentGrave.spawn = this;
            currentGrave.SetHole();
            currentGrave = null;
        }

        fire.SetActive(true);
        state = EnemyState.Normal;
    }

    void Stun()
    {
        if (state != EnemyState.Stunned)
        {
            if (firstStun)
            {
                lastTouch.score.AddScore(25);
                firstStun = false;
            }
            else
            {
                lastTouch.score.AddScore(10);
            }            
        }

        //está estunado
        state = EnemyState.Stunned;
        stunCounter = stunTime;
        fire.SetActive(false);
    }

    public void InTheHole()
    {
        state = EnemyState.InTheHole;
        holeCounter = holeTime;

        if (firstHole)
        {
            if (lastTouch != null)
            {
                lastTouch.score.AddScore(25);
                firstHole = false;
            }            
        }
        else
        {
            lastTouch.score.AddScore(10);
        }
    }

    public void Carried()
    {
        state = EnemyState.Carried;
    }

    public void Normal()
    {
        state = EnemyState.Normal;
    }

    public void BackStun()
    {
        state = EnemyState.Stunned;
    }
}


public enum EnemyState
{
    Normal,
    Stunned,
    Carried,
    InTheHole
}