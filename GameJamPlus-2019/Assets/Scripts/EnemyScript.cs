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

    public bool isCarried
    {
        get { return state == EnemyState.Carried; }
    }

    public bool xxxx
    {
        get; set;
    }

    Vector3 position;

    float counter;
    public float stunTime = 10f;

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
            if(counter > 0)
            {
                counter -= Time.fixedDeltaTime;
            }
            else
            {
                disengage();
            }
        }
        else if (state == EnemyState.Carried)
        {
            
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
        state = EnemyState.Normal;
    }

    void Stun()
    {
        //está estunado
        state = EnemyState.Stunned;
        counter = stunTime;
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
    Carried
}