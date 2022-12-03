#define PC
#define MOBILE
#define COMMON
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MoveManager : MonoBehaviour
{
    public enum Player { player_one, player_two };
    public Player player;

    public Cell[] cells = new Cell[] { };
    public InventoryCell[] inventoryCells = new InventoryCell[] { };

    public GameObject selectAnimal;
    public AnimalBase selectAnimalBase;
    public AnimalBase toInvenAnimal;
    public AnimalBase fromInvenAnimal;
    public Inventory inventoryOne;
    public Inventory inventoryTwo;

    private int parentCellx;
    private int parentCelly;
    private int nextCellx;
    private int nextCelly;

    private bool alreadyMove;

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

            // �̵� �˰��� ���� �ڵ�
            if (hit.collider != null)
            {
                if (selectAnimal == null)
                {
                    if (hit.collider.gameObject.GetComponent<AnimalBase>())
                    {
                        SelectAnimal();
                        return;
                    }
                    else return;
                }
                //���õ� ���� �ִ� ���, �ڱ����� ������ ���
                else if (selectAnimal != null && IsMyTurn())
                {
                    //���õ� ���� �±װ� ANIMAL, hit collider �±�
                    if (TagCheck(hit, "ANIMAL", "GAMEBOARD"))
                    {
                        //���� ���� ���� ���� ���� ��
                        if ((ChildCountCalc(0, hit)))
                        {
                            selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                            //AnimalBase�� Move���� true���
                            if (selectAnimalBase.Move())
                            {
                                //RPC�� ������ int�� ���� ����, ���� ������, RPC ������ ȣ��, ������ ���� �ʱ�ȭ, �� �ѱ�
                                CalcCell();
                                LocalMovetoClickedGameBoard(hit);
                                CallMoveRPC();
                                SelectAnimalNull();
                                TurnOver();
                                return;
                            }
                        }
                        //���� ���� ���� ���� ���� ���� ��
                        else if (ChildCountCalc(1, hit) && ((ChildrenAnimalIsNotMine(hit))))
                        {
                            selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                            if (selectAnimalBase.Move())
                            {
                                CalcCell();
                                LocalMovetoClickedGameBoard(hit);
                                GetLocalAnimalAndCallRPC(hit, "GAMEBOARD");
                                CallMoveRPC();
                                SelectAnimalNull();
                                TurnOver();
                                return;
                            }
                        }
                    }
                    //���� ������ ��
                    else if (TagCheck(hit, "ANIMAL", "ANIMAL"))
                    {
                        //�� ���� ���� ���̶��
                        if (AnimalIsNotMine(hit))
                        {
                            selectAnimalBase.nextCell = hit.transform.parent.GetComponent<Cell>();
                            if (selectAnimalBase.Move())
                            {
                                CalcCell();
                                LocalMovetoClickedAnimal(hit);
                                GetLocalAnimalAndCallRPC(hit, "ANIMAL");
                                CallMoveRPC();
                                SelectAnimalNull();
                                TurnOver();
                                return;
                            }
                        }
                        else if (!AnimalIsNotMine(hit))
                        {
                            SelectAnimal();
                            return;
                        }
                    }

                    else if (TagCheck(hit, "BANNED", "GAMEBOARD"))
                    {
                        //���� ���� ���� ���� ���� ���� ������ �� �ִ���!
                        if ((ChildCountCalc(0, hit)))
                        {
                            selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                            if (CanMove.instance.ReturnCheck(selectAnimalBase.player.ToString(), selectAnimalBase.nextCell.coord))
                            {
                                BackToGameBoard(hit);
                                SelectAnimalNull();
                                TurnOver();
                                return;
                            }
                        }
                    }
                }
                SelectAnimalNull();
                return;

            }

            #region LocalMethod
            void LocalMovetoClickedGameBoard(RaycastHit hit)
            {
                selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                selectAnimal.transform.SetParent(hit.collider.transform);
                selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();
            }

            void LocalMovetoClickedAnimal(RaycastHit hit)
            {
                selectAnimal.transform.position = hit.collider.transform.parent.position + new Vector3(0, 0, -0.1f);
                selectAnimal.transform.SetParent(hit.collider.transform.parent);
                selectAnimalBase.parentCell = hit.collider.gameObject.GetComponentInParent<Cell>();
            }

            void BackToGameBoard(RaycastHit hit)
            {
                InventoryCell inventoryCell = selectAnimal.GetComponentInParent<InventoryCell>();

                selectAnimal.transform.tag = "ANIMAL";
                selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                selectAnimal.transform.parent = hit.collider.transform;
                selectAnimal.transform.localScale *= 2f;
                selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();

                PhotonManager.instance.AnimalComeBack(inventoryCell.x, inventoryCell.y, selectAnimalBase.parentCell.x, selectAnimalBase.parentCell.y);
            }

            void SelectAnimal()
            {
                selectAnimal = hit.collider.gameObject;
                selectAnimalBase = hit.collider.gameObject.GetComponent<AnimalBase>();
            }

            void SelectAnimalNull()
            {
                selectAnimal = null;
                selectAnimalBase = null;
            }

            void CallMoveRPC()
            {
                PhotonManager.instance.AnimalMove(parentCellx, parentCelly, nextCellx, nextCelly);
            }

            void GetLocalAnimalAndCallRPC(RaycastHit hit, string tag)
            {
                Cell deadCell = (tag == "GAMEBOARD") ? hit.collider.gameObject.GetComponent<Cell>() : hit.collider.gameObject.GetComponentInParent<Cell>();
                AnimalBase animalBase = (tag == "GAMEBOARD") ? hit.transform.GetComponentInChildren<AnimalBase>() : hit.transform.GetComponent<AnimalBase>();

                ToInven(animalBase);

                PhotonManager.instance.AnimalToInven(deadCell.x, deadCell.y);
            }

            void TurnOver()
            {
                TurnManager.instance.TurnOver();
            }

            bool TagCheck(RaycastHit hit, string selectAnimalTag, string hitColTag)
            {
                bool tagCheckResult;
                return tagCheckResult = (selectAnimalBase.transform.tag == selectAnimalTag && hit.transform.tag == hitColTag) ? true : false;
            }

            bool ChildCountCalc(int childCount, RaycastHit hit)
            {
                bool calcResult;
                return calcResult = (hit.transform.childCount == childCount && IsMyTurn()) ? true : false;
            }

            bool AnimalIsNotMine(RaycastHit hit)
            {
                bool isNotMine;
                return isNotMine = (hit.transform.GetComponent<AnimalBase>().player != selectAnimal.GetComponent<AnimalBase>().player) ? true : false;
            }

            bool ChildrenAnimalIsNotMine(RaycastHit hit)
            {
                bool isNotMine;
                return isNotMine = (hit.transform.GetComponentInChildren<AnimalBase>().player != selectAnimalBase.player) ? true : false;
            }
            #endregion
        }
    }
#endif

#if MOBILE
    private void TouchAndMove()
    {
        if (Input.touchCount > 0)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);

            if (hit.collider != null)
            {
                //���õ� ���� ���� ���
                if (selectAnimal == null)
                {
                    if (hit.collider.gameObject.GetComponent<AnimalBase>())
                    {
                        SelectAnimal();
                        return;
                    }
                    else return;
                }
                //���õ� ���� �ִ� ��� �ڱ����� ������ ���
                else if (selectAnimal != null)
                {
                    if (TagCheck(hit, "ANIMAL", "GAMEBOARD"))
                    {
                        //���� ���� ���� ���� ���� ��
                        if ((ChildCountCalc(0, hit)))
                        {
                            selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                            //AnimalBase�� Move���� true���
                            if (selectAnimalBase.Move())
                            {
                                //RPC�� ������ int�� ���� ����, ���� ������, RPC ������ ȣ��, ������ ���� �ʱ�ȭ, �� �ѱ�
                                CalcCell();
                                LocalMovetoClickedGameBoard(hit);
                                CallMoveRPC();
                                SelectAnimalNull();
                                TurnOver();
                                return;
                            }
                        }
                        //���� ���� ���� ���� ���� ���� ��
                        else if (ChildCountCalc(1, hit) && ((ChildrenAnimalIsNotMine(hit))))
                        {
                            selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                            if (selectAnimalBase.Move())
                            {
                                CalcCell();
                                LocalMovetoClickedGameBoard(hit);
                                GetLocalAnimalAndCallRPC(hit, "GAMEBOARD");
                                CallMoveRPC();
                                SelectAnimalNull();
                                TurnOver();
                                return;
                            }
                        }
                    }
                    //���� ������ ��
                    else if (TagCheck(hit, "ANIMAL", "ANIMAL"))
                    {
                        //�� ���� ���� ���̶��
                        if (AnimalIsNotMine(hit) && IsMyTurn())
                        {
                            selectAnimalBase.nextCell = hit.transform.parent.GetComponent<Cell>();
                            if (selectAnimalBase.Move())
                            {
                                CalcCell();
                                LocalMovetoClickedAnimal(hit);
                                GetLocalAnimalAndCallRPC(hit, "ANIMAL");
                                CallMoveRPC();
                                SelectAnimalNull();
                                TurnOver();
                                return;
                            }
                        }
                        else if (!AnimalIsNotMine(hit) && IsMyTurn())
                        {
                            SelectAnimal();
                            return;
                        }
                    }

                    else if (TagCheck(hit, "BANNED", "GAMEBOARD"))
                    {
                        //���� ���� ���� ���� ���� ���� ������ �� �ִ���!
                        if ((ChildCountCalc(0, hit)))
                        {
                            selectAnimalBase.nextCell = hit.transform.GetComponent<Cell>();
                            if (CanMove.instance.ReturnCheck(selectAnimalBase.player.ToString(), selectAnimalBase.nextCell.coord))
                            {
                                BackToGameBoard(hit);
                                SelectAnimalNull();
                                TurnOver();
                                return;
                            }
                        }
                    }
                }
                SelectAnimalNull();
                return;

                #region LocalMethod
                void SelectAnimal()
                {
                    selectAnimal = hit.collider.gameObject;
                    selectAnimalBase = hit.collider.gameObject.GetComponent<AnimalBase>();
                }

                void LocalMovetoClickedGameBoard(RaycastHit hit)
                {
                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                    selectAnimal.transform.SetParent(hit.collider.transform);
                    selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();
                }

                void LocalMovetoClickedAnimal(RaycastHit hit)
                {
                    selectAnimal.transform.position = hit.collider.transform.parent.position + new Vector3(0, 0, -0.1f);
                    selectAnimal.transform.SetParent(hit.collider.transform.parent);
                    selectAnimalBase.parentCell = hit.collider.gameObject.GetComponentInParent<Cell>();
                }

                void CallMoveRPC()
                {
                    PhotonManager.instance.AnimalMove(parentCellx, parentCelly, nextCellx, nextCelly);
                }

                void GetLocalAnimalAndCallRPC(RaycastHit hit, string tag)
                {
                    Cell deadCell = (tag == "GAMEBOARD") ? hit.collider.gameObject.GetComponent<Cell>() : hit.collider.gameObject.GetComponentInParent<Cell>();
                    AnimalBase animalBase = (tag == "GAMEBOARD") ? hit.transform.GetComponentInChildren<AnimalBase>() : hit.transform.GetComponent<AnimalBase>();

                    ToInven(animalBase);

                    PhotonManager.instance.AnimalToInven(deadCell.x, deadCell.y);
                }

                void BackToGameBoard(RaycastHit hit)
                {
                    InventoryCell inventoryCell = selectAnimal.GetComponentInParent<InventoryCell>();

                    selectAnimal.transform.tag = "ANIMAL";
                    selectAnimal.transform.position = hit.collider.transform.position + new Vector3(0, 0, -0.1f);
                    selectAnimal.transform.parent = hit.collider.transform;
                    selectAnimal.transform.localScale *= 2f;
                    selectAnimalBase.parentCell = hit.collider.gameObject.GetComponent<Cell>();

                    PhotonManager.instance.AnimalComeBack(inventoryCell.x, inventoryCell.y, selectAnimalBase.parentCell.x, selectAnimalBase.parentCell.y);

                    SelectAnimalNull();
                    TurnOver();
                }

                void SelectAnimalNull()
                {
                    selectAnimal = null;
                    selectAnimalBase = null;
                }

                void TurnOver()
                {
                    TurnManager.instance.TurnOver();
                }

                bool TagCheck(RaycastHit hit, string selectAnimalTag, string hitColTag)
                {
                    bool tagCheckResult = (selectAnimalBase.transform.tag == selectAnimalTag && hit.transform.tag == hitColTag) ? true : false;
                    return tagCheckResult;
                }

                bool ChildCountCalc(int childCount, RaycastHit hit)
                {
                    bool calcResult;
                    return calcResult = (hit.transform.childCount == childCount && IsMyTurn()) ? true : false;
                }

                bool AnimalIsNotMine(RaycastHit hit)
                {
                    bool isNotMine;
                    return isNotMine = (hit.transform.GetComponent<AnimalBase>().player != selectAnimal.GetComponent<AnimalBase>().player) ? true : false;
                }

                bool ChildrenAnimalIsNotMine(RaycastHit hit)
                {
                    bool isNotMine;
                    return isNotMine = (hit.transform.GetComponentInChildren<AnimalBase>().player != selectAnimalBase.player) ? true : false;
                }
                #endregion
            }
        }
    }
#endif

#if COMMON
    private void CalcCell()
    {
        parentCellx = selectAnimalBase.parentCell.x;
        parentCelly = selectAnimalBase.parentCell.y;
        nextCellx = selectAnimalBase.nextCell.x;
        nextCelly = selectAnimalBase.nextCell.y;
    }

    private bool IsMyTurn()
    {
        if ((selectAnimalBase.player.ToString() == TurnManager.instance.me.ToString()) &&
            (TurnManager.instance.me.ToString() == TurnManager.instance.player.ToString()))
            return true;
        else
            return false;
    }

    private void AnimalReset()
    {
        selectAnimal = null;
        selectAnimalBase = null;
        toInvenAnimal = null;
        parentCell = null;
        nextCell = null;
        fromInvenAnimal = null;
        ToInvenParentCell = null;
    }

    public void ToInven(AnimalBase animalBase)
    {
        toInvenAnimal = animalBase;

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
            else { }
        }

        if (parentCell.transform.childCount > 0)
            animal = parentCell.transform.GetChild(0).gameObject;
        else
        {
            AnimalReset();
            return;
        }

        animal.transform.position = nextCell.transform.position + new Vector3(0, 0, -0.1f);
        animal.transform.SetParent(nextCell.transform);
        AnimalReset();
    }

    public void AnimalComeBack(int invenCellx, int invenCelly, int parentCellx, int parentCelly)
    {
        foreach (InventoryCell item in inventoryCells)
        {
            if (item.x == invenCellx && item.y == invenCelly)
            {
                fromInvenAnimal = item.transform.GetChild(0).GetComponent<AnimalBase>();
                break;
            }
        }

        foreach (Cell item in cells)
        {
            if (item.x == parentCellx && item.y == parentCelly)
            {
                fromInvenAnimal.transform.tag = "ANIMAL";
                fromInvenAnimal.transform.position = item.transform.position + new Vector3(0, 0, -0.1f);
                fromInvenAnimal.transform.parent = item.transform;
                fromInvenAnimal.transform.localScale *= 2f;
                fromInvenAnimal.parentCell = item.GetComponent<Cell>();
            }
        }
        AnimalReset();
    }

    public void AnimalToInven(int deadCellx, int deadCelly)
    {
        AnimalBase animal;
        foreach (Cell item in cells)
        {
            if (item.x == deadCellx && item.y == deadCelly)
            {
                ToInvenParentCell = item.gameObject;
                break;
            }
        }

        animal = ToInvenParentCell.GetComponentInChildren<AnimalBase>();
        ToInven(animal);
    }
#endif
}
