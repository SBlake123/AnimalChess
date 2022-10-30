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
    // Update is called once per frame
    void Update()
    {
        MouseClickAndMove();
    }
    //Ŭ���ϰ� ���ϴ� �������� Ŭ���ϸ� �����δ�.
    //�ٸ� �⹰�� �����ϸ� ������ ��ҵȴ�.
    //������ ������ �������� ��, �� �� �ִ� ������ ���Ӻ���θ� �̵��� �� �ִ�.
    //�� ������ �������� �˰� �־�� �Ѵ�. ������ ������ �����;� �Ѵ�.
    private void MouseClickAndMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
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
                else if (hit.collider.tag == "GAMEBOARD" && selectAnimal != null)
                {
                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                    selectAnimal = null;
                    selectAnimalBase = null;
                }
                else if (hit.collider.tag == "BACKGROUND") { Debug.Log("null"); }
            }
        }
    }

#endif
}
