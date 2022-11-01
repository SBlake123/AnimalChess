#define PC

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
#if PC
    public GameObject selectAnimal;
    public AnimalBase selectAnimalBase;
    public Inventory inventoryOne;
    public Inventory inventoryTwo;
    // Update is called once per frame
    void Update()
    {
        MouseClickAndMove();
    }
    
    private void MouseClickAndMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
            {
                //선택된 말이 없이 자기 턴인 상태인 경우
                if (selectAnimal == null)
                {
                    if (hit.collider.gameObject.GetComponent<Giraffe>())
                    {
                        selectAnimal = hit.collider.gameObject;
                        selectAnimalBase = hit.collider.gameObject.GetComponent<Giraffe>();
                        Debug.Log("Giraffe");
                    }
                    else if (hit.collider.gameObject.GetComponent<Lion>())
                    {
                        selectAnimal = hit.collider.gameObject;
                        selectAnimalBase = hit.collider.gameObject.GetComponent<Lion>();
                        Debug.Log("Lion");
                    }
                    else if (hit.collider.gameObject.GetComponent<Elephant>())
                    {
                        selectAnimal = hit.collider.gameObject;
                        selectAnimalBase = hit.collider.gameObject.GetComponent<Elephant>();
                        Debug.Log("Elephant");
                    }
                    else if (hit.collider.gameObject.GetComponent<Chick>())
                    {
                        selectAnimal = hit.collider.gameObject;
                        selectAnimalBase = hit.collider.gameObject.GetComponent<Chick>();
                        Debug.Log("Chick");
                    }
                    else if (hit.collider.tag == "GAMEBOARD" || hit.collider.tag == "BACKGROUND")
                    {
                        selectAnimal = null;
                        selectAnimalBase = null;
                    }
                }
                //선택된 말이 있는 경우 자기턴인 상태인 경우
                else if (selectAnimal != null)
                {
                    if (hit.collider.tag == "GAMEBOARD")
                    {
                        if ((hit.transform.childCount == 0 && ((selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())))      
                        {
                            selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                            selectAnimal.transform.parent = hit.collider.transform;
                            selectAnimalBase.parent = hit.collider.gameObject;

                            selectAnimal = null;
                            selectAnimalBase = null;
                            TurnManager.instance.TurnOver();
                        }
                        else if(hit.transform.childCount == 1 && ((hit.transform.GetComponentInChildren<AnimalBase>().player != selectAnimalBase.player)))
                        {
                            selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                            selectAnimal.transform.parent = hit.collider.transform;
                            selectAnimalBase.parent = hit.collider.gameObject;

                            ToInven(hit.transform.GetComponentInChildren<AnimalBase>());
                            selectAnimal = null;
                            selectAnimalBase = null;
                            TurnManager.instance.TurnOver();
                        }
                        else
                        {
                            selectAnimal = null;
                            selectAnimalBase = null;
                            return;
                        }
                        
                    }
                    else if (hit.collider.tag == "ANIMAL")
                    {
                        if (hit.transform.GetComponent<AnimalBase>().player != selectAnimal.GetComponent<AnimalBase>().player && (selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())
                        {
                            selectAnimal.transform.position = hit.collider.transform.parent.position + new Vector3(0, 0, -0.1f);
                            selectAnimal.transform.parent = hit.collider.transform.parent;
                            selectAnimalBase.parent = hit.collider.transform.parent.gameObject;

                            ToInven(hit.transform.GetComponent<AnimalBase>());
                            selectAnimal = null;
                            selectAnimalBase = null;
                            TurnManager.instance.TurnOver();
                        }
                        else
                        {
                            selectAnimal = null;
                            selectAnimalBase = null;
                        }
                        
                    }
                    else if (hit.collider.tag == "BACKGROUND") 
                    {
                        selectAnimal = null;
                        selectAnimalBase = null;
                    }
                }
                else
                    Debug.Log("null");
            }
        }
    }

    public void ToInven(AnimalBase animalBase)
    {
        if (animalBase.player == AnimalBase.Player.player_one)
            inventoryOne.SetTransform(animalBase);
        else
            inventoryTwo.SetTransform(animalBase);
    }
    

#endif
}
