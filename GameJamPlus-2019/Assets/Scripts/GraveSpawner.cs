using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveSpawner : MonoBehaviour
{
    [SerializeField] GameObject gravePrefab;

    public GraveStone Spawn()
    {
        var obj = Instantiate(gravePrefab, transform.position, transform. rotation);
        var graveStone = obj.GetComponent<GraveStone>();

        if (graveStone != null)
        {
            return graveStone;
        }

        Destroy(obj);
        return null;
    }
}
