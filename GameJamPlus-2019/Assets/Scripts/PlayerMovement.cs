﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Transform holder;

    [SerializeField]
    SightScript sight;

    PlayerState state = PlayerState.Normal;

    public float h;
    public float v;

    public bool action;

    //Handling
    public float speed = 6f;
    public float carrySpeed = 3f;
    public float rotationSpeed = 999f;

    private Vector3 movement;
    private Vector3 lookDirection;
    private Rigidbody playerRigidbody;

    [SerializeField]
    private BoxCollider hit;

    int floorMask;

    EnemyScript enemy;
    
    //float camRayLenght = 100f;

    void Awake()
    {
        //floorMask = LayerMask.GetMask("Floor");
        playerRigidbody = GetComponent<Rigidbody>();

        
    }

    private void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
       action = Input.GetButtonDown("Action");
       
        //chama função de bater
        Hit();
        Carry(); 
    }

    void FixedUpdate()
    {
       

        /*float h2 = Input.GetAxisRaw("Horizontal2");
        float v2 = Input.GetAxisRaw("Vertical2");
        */

        //Chama a função que faz o personagem se mover
        Move(h, v);
        //Chama a função que faz o personagem rotacionar
        Turning(h, v);
    }

    void Move( float h, float v)
    {
        movement.Set(h, 0f, v);

        movement = movement.normalized * Time.deltaTime;

        if (state == PlayerState.Normal)
        {
            movement *= speed;
        }
        else if (state == PlayerState.Carring)
        {
            movement *= carrySpeed;
        }
        
        playerRigidbody.MovePosition(transform.position + movement);

    }

    void Turning(float h, float v)
    {
        ///////////////////////////////script pra seguir o mouse //////////////////////////////
        /* Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

         RaycastHit floorHit;

         if(Physics.Raycast(camRay, out floorHit, camRayLenght, floorMask))
         {
             Vector3 playerToMouse = floorHit.point - transform.position;
             playerToMouse.y = 0f;

             Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
             playerRigidbody.MoveRotation(newRotation);

         }*/
        lookDirection.Set(h, 0F, v);

        if (lookDirection != Vector3.zero)
        {
            
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
            playerRigidbody.MoveRotation(lookRotation);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, lookRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }
        
    }

    void Hit()
    {
        //se aperta o botao de ação
        if (action)
        {
            //liga o collider da Pá
            hit.enabled = true;
        }
            
        if (!action)
        {
            //desliga o collider da pá
            hit.enabled = false;
        }
    }

    // Carregar ou soltar o inimigo
    void Carry()
    {
        //if (Input.GetButtonDown("Carry"))
        {
            if (Input.GetButtonDown("Carry") && sight.enemy != null && sight.enemy.isStunned == true)
            {
                // Pegar o inimigo caso ele esteja na visão do Jogador e esteja tonto
                PickUp();
            }
            else if (Input.GetButtonUp("Carry") && enemy != null && enemy.isCarried)
            {
                // Largar o inimigo no chão
                Throw();
            }
        }        
    }

    // Prender o inimigo tonto no jogador na posição do Holder
    void PickUp()
    {
        enemy = sight.enemy;
        enemy.transform.parent = holder.transform;
        enemy.transform.localPosition = Vector3.zero;
        enemy.Carried();
        state = PlayerState.Carring;
    }

    // Soltar o inimigo no chao
    void Throw()
    {
        enemy.transform.parent = null;
        enemy.BackStun();
        enemy = null;
        state = PlayerState.Normal;
    }
}


public enum PlayerState
{
    Normal,
    Carring,
    Stairs,
}
