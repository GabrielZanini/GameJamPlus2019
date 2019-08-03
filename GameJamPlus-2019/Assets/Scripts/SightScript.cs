using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightScript : MonoBehaviour
{
    public EnemyScript enemy;


    // Seleciona o inimigo quando Colidir
    public void OnTriggerEnter(Collider col)
    {
        var e = col.GetComponent<EnemyScript>();

        if (e != null)
        {
            enemy = e;
        }
    }

    // Remove o inimigo da Seleção quando ele deixar a area de colisão
    public void OnTriggerExit(Collider col)
    {
        var e = col.GetComponent<EnemyScript>();

        if (e == enemy)
        {
            enemy = null;
        }
    }
}
