using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform enemyHolder;
    [SerializeField] Transform graveHolder;
    [SerializeField] BoxCollider hit;
    public ScoreScript score;
    [SerializeField] GameObject runParticles;
    public Animator anim;
    public AudioSource audio;

    [SerializeField]
    SightScript sight;

    PlayerState state = PlayerState.Normal;

    [Header("Inputs")]
    public int inputId = 0;
    public float h;
    public float v;
    public bool action;

    //Handling
    [Header("Handling")]
    public float speed = 6f;
    public float carrySpeed = 3f;
    public float rotationSpeed = 999f;

    [Header("Stun")]
    public float stunCounter;
    public float stunTime = 1.5f;

    //
    private Vector3 movement;
    private Vector3 lookDirection;
    private Rigidbody playerRigidbody;
    private bool carryDown = false;
    private bool carryUp = true;


    int floorMask;

    EnemyScript enemy;
    GraveStone graveStone;
    
    //float camRayLenght = 100f;

    void Awake()
    {
        //floorMask = LayerMask.GetMask("Floor");
        playerRigidbody = GetComponent<Rigidbody>();
        stunCounter = stunTime;


    }

    private void Update()
    {
        ReadInput();
        ShowParticles();
        Animate();
        //chama função de bater
        Hit();
        Carry(); 
    }

    void FixedUpdate()
    {
        if (state == PlayerState.Normal || state == PlayerState.Carring)
        {
            //Chama a função que faz o personagem se mover
            Move(h, v);
            //Chama a função que faz o personagem rotacionar
            Turning(h, v);
        }
        else if (state == PlayerState.Stun)
        {
            StunTimer();
        }        
    }

    void ReadInput()
    {
        if (inputId > 0)
        {
            h = Input.GetAxisRaw("Horizontal" + inputId);
            v = Input.GetAxisRaw("Vertical" + inputId);
            action = Input.GetButtonDown("Action" + inputId);

            carryDown = Input.GetButtonDown("Carry" + inputId);
            carryUp = Input.GetButtonUp("Carry" + inputId);
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");
            action = Input.GetButtonDown("Action");

            carryDown = Input.GetButtonDown("Carry");
            carryUp = Input.GetButtonUp("Carry");
        }
    }

    void ShowParticles()
    {
        runParticles.SetActive(h != 0f || v != 0f);
    }

    void Animate()
    {
        anim.SetBool("IsWalking", false);

        if (state == PlayerState.Normal)
        {
            anim.SetBool("IsWalking", h != 0f || v != 0f);
        }
        else if (state == PlayerState.Carring)
        {
            
        }
        else if (state == PlayerState.Stun)
        {
            
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
            stunCounter = stunTime;
            state = PlayerState.Normal;
        }
    }


    private void OnTriggerEnter(Collider col)
    {
        //se colidir com tag "Hit"
        if (col.gameObject.tag == "Hit")
        {
            Debug.Log("Hit");

            var player = col.GetComponentInParent<PlayerMovement>();

            if (player != this)
            {
                Debug.Log("player");
                Stun();
            }
        }
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
            audio.Play();
        }
            
        if (!action)
        {
            //desliga o collider da pá
            hit.enabled = false;
        }
    }

    void Stun()
    {
        //está estunado
        
        // Soltar algo que esteja segurando
        Throw();

        state = PlayerState.Stun;
        stunCounter = stunTime;
    }

    // Carregar ou soltar o inimigo
    void Carry()
    {
        //if (Input.GetButtonDown("Carry"))
        {
            if (carryDown)
            {
                PickUp();
            }
            else if (carryUp)
            {
                Throw();
            }
        }        
    }

    // Prender o inimigo tonto no jogador na posição do Holder
    void PickUpEnemy()
    {
        enemy = sight.enemy;
        enemy.transform.parent = enemyHolder.transform;
        enemy.transform.localPosition = Vector3.zero;
        enemy.currentGrave = null;
        enemy.Carried();
        state = PlayerState.Carring;
        enemy.lastTouch = this;
    }

    // Soltar o inimigo no chao
    void ThrowEnemy()
    {
        enemy.transform.parent = null;
        enemy.BackStun();
        enemy.lastTouch = this;
        //enemy.rigidbody.AddForce(transform.forward * 2);
        enemy = null;
        state = PlayerState.Normal;
    }
    
    // Prender o inimigo tonto no jogador na posição do Holder
    void PickUpGraveStone()
    {
        graveStone = sight.graveStone;
        graveStone.transform.parent = graveHolder.transform;
        graveStone.transform.localPosition = Vector3.zero;
        graveStone.transform.localRotation = Quaternion.identity;
        graveStone.state = StoneState.OnPlayer;

        state = PlayerState.Carring;
    }

    void PickUpNewGrave()
    {
        var newGrave = sight.graveSpawner.Spawn();

        if (newGrave != null)
        {
            graveStone = newGrave;
            graveStone.transform.parent = graveHolder.transform;
            graveStone.transform.localPosition = Vector3.zero;
            graveStone.transform.localRotation = Quaternion.identity;
            graveStone.state = StoneState.OnPlayer;
        }
    }

    // Soltar o inimigo no chao
    void ThrowGraveStone()
    {
        graveStone.transform.parent = null;

        if (sight.grave != null && sight.grave.state == GraveState.WithEnemy)
        {
            graveStone.state = StoneState.OnTheGrave;
            graveStone.transform.position = sight.grave.StoneTranform.position;
            graveStone.collider.enabled = false;
            sight.grave.winner = this;
            sight.grave.graveStone = graveStone;
            sight.grave.SetBuried();
        }
        else
        {
            graveStone.state = StoneState.OnTheGround;
            graveStone.transform.position = transform.position + transform.forward;
        }

        graveStone.transform.rotation = Quaternion.identity * Quaternion.Euler(0f,-45f,0f);
        graveStone = null;
        
        state = PlayerState.Normal;
    }


    void PickUp()
    {
        if (sight.enemy != null && sight.enemy.isStunned == true)
        {
            // Pegar o inimigo caso ele esteja na visão do Jogador e esteja tonto
            PickUpEnemy();
        }
        //else if (sight.graveStone != null )
        //{
        //    PickUpGraveStone();
        //}
        else if (sight.graveSpawner != null)
        {
            PickUpNewGrave();
        }
    }

    void Throw()
    {
        if (enemy != null && enemy.isCarried)
        {
            // Largar o inimigo no chão
            ThrowEnemy();
        }
        else if (graveStone != null)
        {
            ThrowGraveStone();
        }
    }
}


public enum PlayerState
{
    Normal,
    Stun,
    Carring,
}
