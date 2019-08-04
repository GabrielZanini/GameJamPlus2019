using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveStone : MonoBehaviour
{
    [Header("Objects")]
    public BoxCollider collider;
    public GraveSpawner spawner;

    [Header("State")]
    public StoneState state = StoneState.OnPlayer;

    [Header("Lifetime")]
    public float lifetime = 3f;
    private float lifeTimer;



    private void Update()
    {
        if (state == StoneState.OnPlayer)
        {
            lifeTimer = lifetime;
        }
        else if (state == StoneState.OnTheGround)
        {
            Timer();
        }
        else if (state == StoneState.OnTheGrave)
        {

        }
    }

    private void OnDestroy()
    {
        spawner.graves.Remove(this);
    }

    void Timer()
    {
        if (lifeTimer > 0f)
        {
            lifeTimer -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

public enum StoneState
{
    OnPlayer,
    OnTheGround,
    OnTheGrave
}