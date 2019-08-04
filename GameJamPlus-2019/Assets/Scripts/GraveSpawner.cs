using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveSpawner : MonoBehaviour
{
    [SerializeField] GameObject gravePrefab;

    List<GraveStone> graves = new List<GraveStone>();



    public GraveStone Spawn()
    {
        var obj = Instantiate(gravePrefab, transform.position, transform. rotation);
        var graveStone = obj.GetComponent<GraveStone>();

        if (graveStone != null)
        {
            graves.Add(graveStone);
            return graveStone;
        }

        Destroy(obj);
        return null;
    }

    public void Clear()
    {
        while (graves.Count > 0)
        {
            GraveStone grave = graves[0];
            graves.RemoveAt(0);
            Destroy(grave.gameObject);
        }
    }
}
