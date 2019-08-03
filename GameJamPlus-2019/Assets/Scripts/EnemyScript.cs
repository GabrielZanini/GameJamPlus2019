using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [Header("Objects")]
    public Transform target;

    [Header("Settings")]
    //velocidade do inimigo
    public float speed = 1;

    public EnemyState state = EnemyState.Normal;

    public bool isNormal
    {
        get { return state == EnemyState.Normal; }
    }
    public bool isStunned
    {
        get { return state == EnemyState.Stunned; }
    }
    public bool isCarried {
        get { return state == EnemyState.Carried; }
    }
    public bool isInTheHole {
        get { return state == EnemyState.InTheHole; }
    }

    Vector3 position;

    float stunCounter;
    float holeCounter;
    public float stunTime = 10f;
    public float holeTime = 5f;


    public EnemySpawner spawner;



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
            Stun();
        }
    }

    void disengage()
    {
        Vector3 position = transform.position;
        position.y = 0;
        transform.position = position;

        state = EnemyState.Normal;
    }

    void Stun()
    {
        //está estunado
        state = EnemyState.Stunned;
        stunCounter = stunTime;
    }

    public void InTheHole()
    {
        state = EnemyState.InTheHole;
        holeCounter = holeTime;
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