using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class StartManager : MonoBehaviour
{
    public static StartManager instance;
    public Cell[] cells;

    public List<Tuple<string, string>> createCellList = new List<Tuple<string, string>>
    {
        new Tuple<string,string>("1",SAnimalName.DOG_ONE),
        new Tuple<string,string>("4",SAnimalName.CAT_TWO),
        new Tuple<string,string>("5",SAnimalName.LION_ONE),
        new Tuple<string,string>("6",SAnimalName.CHICKEN_ONE),
        new Tuple<string,string>("7",SAnimalName.CHICKEN_TWO),
        new Tuple<string,string>("8",SAnimalName.LION_TWO),
        new Tuple<string,string>("9",SAnimalName.CAT_ONE),
        new Tuple<string,string>("12",SAnimalName.DOG_TWO)
    };

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        AnimalCreate();
    }

    private void AnimalCreate()
    {
        foreach (var item in cells)
        {
            for (int i = 0; i < createCellList.Count; i++)
            {
                if (item.name == createCellList[i].Item1)
                    AnimalLoadToBoard(item, createCellList[i].Item2);
            }
        }   
    }

    private void AnimalLoadToBoard(Cell cell, string animalName)
    {
        AnimalBase animal = Resources.Load<AnimalBase>(animalName);
        AnimalBase animalinst = Instantiate(animal, cell.transform.position, cell.transform.rotation);
        animalinst.transform.SetParent(cell.transform);
        animalinst.parentCell = animalinst.transform.parent.GetComponent<Cell>();
        animalinst.transform.position = cell.transform.position + new Vector3(0, 0, -0.1f);
    }
}
