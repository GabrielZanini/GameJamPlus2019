using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var enemy = other.GetComponent<EnemyScript>();

        if (enemy != null)
        {
            Destroy(enemy.gameObject);
        }
    }
}
