using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform[] transforms;

    public void SetTransform(AnimalBase animalBase)
    {
        for(int i = 0; i < transforms.Length; i++)
        {
            if(transforms[i].childCount == 0)
            {
                animalBase.transform.position = transforms[i].transform.position + new Vector3(0, 0, -0.1f);
                animalBase.transform.parent = transforms[i].transform;
                break;
            }
        }
    }
}
