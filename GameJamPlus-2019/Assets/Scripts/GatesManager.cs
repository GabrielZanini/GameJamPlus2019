using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatesManager : MonoBehaviour
{
    [SerializeField] List<Gate> gates;

    int id = 0;
    public Gate active;
    

    void Awake()
    {
        SelectGate();
    }
        
    void SelectGate()
    {
        id = Random.Range(0, gates.Count);

        for (int i=0; i<gates.Count; i++)
        {
            if (i == id)
            {
                gates[i].gameObject.SetActive(true);
                active = gates[i];
            }
            else
            {
                gates[i].gameObject.SetActive(false);
            }
        }
    }
}