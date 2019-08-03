﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightScript : MonoBehaviour
{
    public EnemyScript enemy;
    public GraveStone graveStone;
    public Grave grave;


    // Seleciona o inimigo quando Colidir
    public void OnTriggerEnter(Collider col)
    {
        var e = col.GetComponent<EnemyScript>();

        if (e != null)
        {
            enemy = e;
        }

        var g = col.GetComponent<Grave>();

        if (g != null)
        {
            grave = g;
        }

        var gs = col.GetComponent<GraveStone>();

        if (gs != null)
        {
            graveStone = gs;
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

        var g = col.GetComponent<Grave>();

        if (g == grave)
        {
            grave = null;
        }

        var gs = col.GetComponent<GraveStone>();

        if (gs == graveStone)
        {
            graveStone = null;
        }
    }
}
