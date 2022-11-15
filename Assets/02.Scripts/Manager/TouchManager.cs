#define PC
#define MOBILE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TouchManager : MonoBehaviour
{
    public GameObject selectAnimal;
    public AnimalBase selectAnimalBase;
    public Inventory inventoryOne;
    public Inventory inventoryTwo;

    void Update()
    {
        MouseClickAndMove();
        TouchAndMove();
    }

#if PC
    private void MouseClickAndMove()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
            {
                //���õ� ���� ���� �ڱ� ���� ������ ���
                if (selectAnimal == null)
                {
                    //� ������ �ľ��ؼ� ����ϴ� ��
                    if (hit.collider.gameObject.GetComponent<AnimalBase>())
                    {
                        selectAnimal = hit.collider.gameObject;
                        selectAnimalBase = hit.collider.gameObject.GetComponent<AnimalBase>();
                        Debug.Log($"{selectAnimalBase}");
                    }

                    else if (hit.collider.tag == "GAMEBOARD" || hit.collider.tag == "BACKGROUND")
                    {
                        selectAnimal = null;
                        selectAnimalBase = null;
                    }
                }
                //���õ� ���� �ִ� ��� �ڱ����� ������ ���
                else if (selectAnimal != null)
                {
                    if (selectAnimal.transform.tag == "ANIMAL")
                    {
                        if (hit.collider.tag == "GAMEBOARD")
                        {
                            //���� ���� ���� ���� ���� ��
                            if ((hit.transform.childCount == 0 && ((selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
                                {
                                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform;
                                    selectAnimalBase.parent = hit.collider.gameObject;

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
                            //���� ���� ���� ���� ���� ���� ��
                            else if (hit.transform.childCount == 1 && ((hit.transform.GetComponentInChildren<AnimalBase>().player != selectAnimalBase.player)))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
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
                            //�� ���� ��� �̵��� �Ͼ�� �ʴ´�
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //���� ������ ��
                        else if (hit.collider.tag == "ANIMAL")
                        {
                            //�� ���� ���� ���̶��
                            if (hit.transform.GetComponent<AnimalBase>().player != selectAnimal.GetComponent<AnimalBase>().player && (selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())
                            {
                                selectAnimalBase.nextCell = hit.transform.parent.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
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
                                    return;
                                }
                            }
                            //�� ���� �����
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //��� ������ ��
                        else if (hit.collider.tag == "BACKGROUND")
                        {
                            selectAnimal = null;
                            selectAnimalBase = null;
                            return;
                        }
                    }
                    else if(selectAnimal.transform.tag == "BANNED")
                    {
                        if (hit.collider.tag == "GAMEBOARD")
                        {
                            //���� ���� ���� ���� ���� ���� ������ �� �ִ���!
                            if ((hit.transform.childCount == 0 && ((selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Return())
                                {
                                    selectAnimal.transform.tag = "ANIMAL";
                                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform;
                                    selectAnimalBase.parent = hit.collider.gameObject;

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
                            //�� ���� ��� �̵��� �Ͼ�� �ʴ´�
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //��� ������ ��
                        else if (hit.collider.tag == "BACKGROUND")
                        {
                            selectAnimal = null;
                            selectAnimalBase = null;
                            return;
                        }
                    }
                }
                else
                    Debug.Log("null");
            }
        }
    }

    public void ToInven(AnimalBase animalBase)
    {
        if (animalBase.player == AnimalBase.Player.player_two)
        {
            animalBase.transform.tag = "BANNED";
            animalBase.player = AnimalBase.Player.player_one;
            inventoryOne.SetTransform(animalBase);
        }

        else
        {
            animalBase.transform.tag = "BANNED";
            animalBase.player = AnimalBase.Player.player_two;
            inventoryTwo.SetTransform(animalBase);
        }
        animalBase.ImageChange();
    }
#endif
#if MOBILE

    private void TouchAndMove()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
            {
                //���õ� ���� ���� �ڱ� ���� ������ ���
                if (selectAnimal == null)
                {
                    //� ������ �ľ��ؼ� ����ϴ� ��
                    if (hit.collider.gameObject.GetComponent<AnimalBase>())
                    {
                        selectAnimal = hit.collider.gameObject;
                        selectAnimalBase = hit.collider.gameObject.GetComponent<AnimalBase>();
                        Debug.Log($"{selectAnimalBase}");
                    }

                    else if (hit.collider.tag == "GAMEBOARD" || hit.collider.tag == "BACKGROUND")
                    {
                        selectAnimal = null;
                        selectAnimalBase = null;
                    }
                }
                //���õ� ���� �ִ� ��� �ڱ����� ������ ���
                else if (selectAnimal != null)
                {
                    if (selectAnimal.transform.tag == "ANIMAL")
                    {
                        if (hit.collider.tag == "GAMEBOARD")
                        {
                            //���� ���� ���� ���� ���� ��
                            if ((hit.transform.childCount == 0 && ((selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
                                {
                                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform;
                                    selectAnimalBase.parent = hit.collider.gameObject;

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
                            //���� ���� ���� ���� ���� ���� ��
                            else if (hit.transform.childCount == 1 && ((hit.transform.GetComponentInChildren<AnimalBase>().player != selectAnimalBase.player)))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
                                {
                                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform;
                                    selectAnimalBase.parent = hit.collider.gameObject;

                                    ToInvenMoblie(hit.transform.GetComponentInChildren<AnimalBase>());
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
                            //�� ���� ��� �̵��� �Ͼ�� �ʴ´�
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //���� ������ ��
                        else if (hit.collider.tag == "ANIMAL")
                        {
                            //�� ���� ���� ���̶��
                            if (hit.transform.GetComponent<AnimalBase>().player != selectAnimal.GetComponent<AnimalBase>().player && (selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())
                            {
                                selectAnimalBase.nextCell = hit.transform.parent.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
                                {
                                    selectAnimal.transform.position = hit.collider.transform.parent.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform.parent;
                                    selectAnimalBase.parent = hit.collider.transform.parent.gameObject;

                                    ToInvenMoblie(hit.transform.GetComponent<AnimalBase>());

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
                            //�� ���� �����
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //��� ������ ��
                        else if (hit.collider.tag == "BACKGROUND")
                        {
                            selectAnimal = null;
                            selectAnimalBase = null;
                            return;
                        }
                    }
                    else if (selectAnimal.transform.tag == "BANNED")
                    {
                        if (hit.collider.tag == "GAMEBOARD")
                        {
                            //���� ���� ���� ���� ���� ���� ������ �� �ִ���!
                            if ((hit.transform.childCount == 0 && ((selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Return())
                                {
                                    selectAnimal.transform.tag = "ANIMAL";
                                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform;
                                    selectAnimalBase.parent = hit.collider.gameObject;

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
                            //�� ���� ��� �̵��� �Ͼ�� �ʴ´�
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //��� ������ ��
                        else if (hit.collider.tag == "BACKGROUND")
                        {
                            selectAnimal = null;
                            selectAnimalBase = null;
                            return;
                        }
                    }
                }
                else
                    Debug.Log("null");
            }
        }
    }

    public void ToInvenMoblie(AnimalBase animalBase)
    {
        if (animalBase.player == AnimalBase.Player.player_two)
        {
            animalBase.transform.tag = "BANNED";
            animalBase.player = AnimalBase.Player.player_one;
            inventoryOne.SetTransform(animalBase);
        }

        else
        {
            animalBase.transform.tag = "BANNED";
            animalBase.player = AnimalBase.Player.player_two;
            inventoryTwo.SetTransform(animalBase);
        }
        animalBase.ImageChange();
    }
#endif
}
