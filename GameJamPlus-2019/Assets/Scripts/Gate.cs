using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Animator anim;
    public GameObject target;

    public LevelManager level;



    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyScript>();

        Debug.Log("Trigger Gate");

        if (enemy != null)
        {
            Destroy(enemy.gameObject);
            level.LoseLife();
        }
    }



    public void Open()
    {
        anim.SetBool("Open", true);
        //target.SetActive(false);
        //target.SetActive(true);
    }

    public void Close()
    {
        anim.SetBool("Open", false);
        //target.SetActive(false);
    }
}
