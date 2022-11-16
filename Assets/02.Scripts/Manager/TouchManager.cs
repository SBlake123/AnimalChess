#define PC
//#define MOBILE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TouchManager : MonoBehaviour
{
    public enum Player { player_one, player_two };
    public Player player;

    public Cell[] cells = new Cell[] { };

    public GameObject selectAnimal;
    public AnimalBase selectAnimalBase;
    public AnimalBase toInvenAnimal;
    public Inventory inventoryOne;
    public Inventory inventoryTwo;

    private int parentCellx;
    private int parentCelly;
    private int nextCellx;
    private int nextCelly;

    [SerializeField]
    private GameObject parentCell;
    [SerializeField]
    private GameObject nextCell;
    private GameObject ToInvenParentCell;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            player = Player.player_one;
        else
            player = Player.player_two;
    }

    void Update()
    {
#if PC
        MouseClickAndMove();
#endif
#if MOBILE
        TouchAndMove();
#endif
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
                //선택된 말이 없이 자기 턴인 상태인 경우
                if (selectAnimal == null)
                {
                    //어떤 말인지 파악해서 기억하는 중
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
                //선택된 말이 있는 경우 자기턴인 상태인 경우
                else if (selectAnimal != null)
                {
                    if (selectAnimal.transform.tag == "ANIMAL")
                    {
                        if (hit.collider.tag == "GAMEBOARD")
                        {
                            //게임 보드 위에 말이 없을 때
                            if ((hit.transform.childCount == 0 && ((selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
                                {
                                    CalcCell();

                                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform;
                                    selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();
                           
                                    PhotonManager.instance.AnimalMove(parentCellx, parentCelly, nextCellx, nextCelly);

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
                            //게임 보드 위에 상대방 말이 있을 때
                            else if (hit.transform.childCount == 1 && ((hit.transform.GetComponentInChildren<AnimalBase>().player != selectAnimalBase.player)))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
                                {
                                    CalcCell();

                                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform;
                                    selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();

                                    ToInven(hit.transform.GetComponentInChildren<AnimalBase>());
                                    PhotonManager.instance.AnimalMove(parentCellx, parentCelly, nextCellx, nextCelly);

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
                            //그 외의 경우 이동이 일어나지 않는다
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //말을 눌렀을 때
                        else if (hit.collider.tag == "ANIMAL")
                        {
                            //그 말이 상대방 것이라면
                            if (hit.transform.GetComponent<AnimalBase>().player != selectAnimal.GetComponent<AnimalBase>().player && (selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())
                            {
                                selectAnimalBase.nextCell = hit.transform.parent.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
                                {
                                    CalcCell();

                                    selectAnimal.transform.position = hit.collider.transform.parent.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform.parent;
                                    selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();

                                    ToInven(hit.transform.GetComponent<AnimalBase>());

                                    PhotonManager.instance.AnimalMove(parentCellx, parentCelly, nextCellx, nextCelly);

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
                            else if(hit.transform.GetComponent<AnimalBase>().player == selectAnimal.GetComponent<AnimalBase>().player && (selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())
                            {
                                selectAnimal = hit.collider.gameObject;
                                selectAnimalBase = hit.collider.gameObject.GetComponent<AnimalBase>();
                                return;
                            }
                            //그 외의 경우라면
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //배경 눌렀을 때
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
                            //게임 보드 위에 말이 없을 때만 움직일 수 있다잉!
                            if ((hit.transform.childCount == 0 && ((selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Return())
                                {

                                    selectAnimal.transform.tag = "ANIMAL";
                                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform;
                                    selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();

                                    PhotonManager.instance.AnimalComeBack(selectAnimal.transform.position, "ANIMAL");

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
                            //그 외의 경우 이동이 일어나지 않는다
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //배경 눌렀을 때
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

    private void CalcCell()
    {
        parentCellx = selectAnimalBase.parentCell.x;
        parentCelly = selectAnimalBase.parentCell.y;
        nextCellx = selectAnimalBase.nextCell.x;
        nextCelly = selectAnimalBase.nextCell.y;
    }

    private bool MyTurn()
    {
        return TurnManager.instance.player.ToString() == player.ToString();
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
        toInvenAnimal = animalBase;
        PhotonManager.instance.AnimalToInven(toInvenAnimal.x, toInvenAnimal.y, "BANNED");
    }

    public void AnimalMove(int parentCellx, int parentCelly, int nextCellx, int nextCelly)
    {
        GameObject animal;
        foreach (Cell item in cells)
        {
            if (item.x == parentCellx && item.y == parentCelly)
            {
                parentCell = item.gameObject;
            }
            else if (item.x == nextCellx && item.y == nextCelly)
            {
                nextCell = item.gameObject;
            }
        }

        if (parentCell.transform.childCount > 0 ) 
        animal = parentCell.transform.GetChild(0).gameObject;
        else
            return;

        animal.transform.position = nextCell.transform.position + new Vector3(0, 0, -0.1f);
        animal.transform.SetParent(nextCell.transform);
    }

    public void AnimalComeBack(Vector3 transform, string tag)
    {
        selectAnimalBase.transform.position = transform;
        selectAnimalBase.transform.SetParent(selectAnimalBase.nextCell.transform);
    }

    public void AnimalToInven(int parentCellx, int parentCelly, string tag)
    {
        GameObject animal;
        foreach (Cell item in cells)
        {
            if (item.x == parentCellx && item.y == parentCelly)
            {
                ToInvenParentCell = item.gameObject;
            }
        }
        if (ToInvenParentCell.transform.childCount > 0)
            animal = ToInvenParentCell.transform.GetChild(0).gameObject;
        else
            return;

        ToInvenLocal(animal.GetComponent<AnimalBase>());
    }

    public void ToInvenLocal(AnimalBase animalBase)
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
                //선택된 말이 없이 자기 턴인 상태인 경우
                if (selectAnimal == null)
                {
                    //어떤 말인지 파악해서 기억하는 중
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
                //선택된 말이 있는 경우 자기턴인 상태인 경우
                else if (selectAnimal != null)
                {
                    if (selectAnimal.transform.tag == "ANIMAL")
                    {
                        if (hit.collider.tag == "GAMEBOARD")
                        {
                            //게임 보드 위에 말이 없을 때
                            if ((hit.transform.childCount == 0 && ((selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
                                {
                                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform;
                                    selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();

                                    PhotonManager.instance.AnimalMove(selectAnimal.transform.position);

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
                            //게임 보드 위에 상대방 말이 있을 때
                            else if (hit.transform.childCount == 1 && ((hit.transform.GetComponentInChildren<AnimalBase>().player != selectAnimalBase.player)))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
                                {
                                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform;
                                    selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();

                                    ToInvenMoblie(hit.transform.GetComponentInChildren<AnimalBase>());
                                    PhotonManager.instance.AnimalMove(selectAnimal.transform.position);

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
                            //그 외의 경우 이동이 일어나지 않는다
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //말을 눌렀을 때
                        else if (hit.collider.tag == "ANIMAL")
                        {
                            //그 말이 상대방 것이라면
                            if (hit.transform.GetComponent<AnimalBase>().player != selectAnimal.GetComponent<AnimalBase>().player && (selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())
                            {
                                selectAnimalBase.nextCell = hit.transform.parent.GetComponent<Cell>();
                                if (selectAnimalBase.Move())
                                {
                                    selectAnimal.transform.position = hit.collider.transform.parent.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform.parent;
                                    selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();

                                    ToInvenMoblie(hit.transform.GetComponent<AnimalBase>());
                                    PhotonManager.instance.AnimalMove(selectAnimal.transform.position);

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
                            //그 외의 경우라면
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //배경 눌렀을 때
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
                            //게임 보드 위에 말이 없을 때만 움직일 수 있다잉!
                            if ((hit.transform.childCount == 0 && ((selectAnimalBase.player).ToString() == (TurnManager.instance.player).ToString())))
                            {
                                selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                                if (selectAnimalBase.Return())
                                {
                                    selectAnimal.transform.tag = "ANIMAL";
                                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                                    selectAnimal.transform.parent = hit.collider.transform;
                                    selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();

                                    PhotonManager.instance.AnimalComeBack(selectAnimal.transform.position, "ANIMAL");

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
                            //그 외의 경우 이동이 일어나지 않는다
                            else
                            {
                                selectAnimal = null;
                                selectAnimalBase = null;
                                return;
                            }

                        }
                        //배경 눌렀을 때
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
