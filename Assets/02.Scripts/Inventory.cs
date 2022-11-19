using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
                animalBase.transform.SetParent(transforms[i].transform);
                animalBase.transform.localScale *= 0.5f;
                if(animalBase is Lion)
                {
                    string player = (animalBase.player).ToString();
                    PhotonManager.instance.LionDie(player);
                    WinManager.instance.LionDie(player);
                    Debug.Log("animalBase is Lion");
                }
                break;
            }
        }
    }
}
