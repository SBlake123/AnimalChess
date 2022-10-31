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
    public Inventory inventory;
    // Update is called once per frame
    void Update()
    {
        MouseClickAndMove();
    }
    //클릭하고 원하는 방향으로 클릭하면 움직인다.
    //다른 기물을 선택하면 선택이 취소된다.
    //각각의 동물을 선택했을 때, 갈 수 있는 방향의 게임보드로만 이동할 수 있다.
    //그 방향은 동물들이 알고 있어야 한다. 동물의 정보를 빌려와야 한다.
    private void MouseClickAndMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
            {
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
                }
                else if (selectAnimal != null)
                {
                    if (hit.collider.tag == "GAMEBOARD")
                    {
                        if (hit.transform.childCount == 0 || hit.transform.GetComponentInChildren<AnimalBase>().player != selectAnimal.GetComponent<AnimalBase>().player)
                        {
                            selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                            selectAnimal.transform.parent = hit.collider.transform;
                            selectAnimal = null;
                            selectAnimalBase = null;
                        }
                        else Debug.Log("Can't move");
                    }
                    else if (hit.collider.tag == "ANIMAL")
                    {
                        if(hit.transform.GetComponentInChildren<AnimalBase>().player != selectAnimal.GetComponent<AnimalBase>().player)
                        {
                            selectAnimal.transform.position = hit.collider.transform.parent.position + new Vector3(0, 0, -0.1f);
                            selectAnimal.transform.parent = hit.collider.transform.parent;
                            ToInven();
                            selectAnimal = null;
                            selectAnimalBase = null;
                        }
                    }
                    else if (hit.collider.tag == "BACKGROUND") { Debug.Log("null"); }
                }
            }
        }
    }

    public void ToInven()
    {
        //플레이어 원이 딴 애는 플레이어 원 인벤토리에, 투가 따면 투 인벤토리에, 인벤토리에는 enum이 있으며, 
    }

#endif
}
