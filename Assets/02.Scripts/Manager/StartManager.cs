using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class StartManager : MonoBehaviour
{
    public static StartManager instance;
    public Cell[] cells;
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
            switch(item.name)
            {
                case "1":
                    {
                        AnimalBase animal = Resources.Load<AnimalBase>(SAnimalName.ELEPHANT_ONE);
                        AnimalBase animalinst = Instantiate(animal, item.transform.position, item.transform.rotation);
                        animalinst.transform.SetParent(item.transform);
                        animalinst.parentCell = animalinst.transform.parent.GetComponent<Cell>();
                        animalinst.transform.position = item.transform.position + new Vector3(0, 0, -0.1f);
                        break;
                    }
                case "4":
                    {
                        AnimalBase animal = Resources.Load<AnimalBase>(SAnimalName.GIRAFFE_TWO);
                        AnimalBase animalinst = Instantiate(animal, item.transform.position, item.transform.rotation);
                        animalinst.transform.SetParent(item.transform);
                        animalinst.parentCell = animalinst.transform.parent.GetComponent<Cell>();
                        animalinst.transform.position = item.transform.position + new Vector3(0, 0, -0.1f);
                        break;
                    }
                case "5":
                    {
                        AnimalBase animal = Resources.Load<AnimalBase>(SAnimalName.LION_ONE);
                        AnimalBase animalinst = Instantiate(animal, item.transform.position, item.transform.rotation);
                        animalinst.transform.SetParent(item.transform);
                        animalinst.parentCell = animalinst.transform.parent.GetComponent<Cell>();
                        animalinst.transform.position = item.transform.position + new Vector3(0, 0, -0.1f);
                        break;
                    }
                case "6":
                    {
                        AnimalBase animal = Resources.Load<AnimalBase>(SAnimalName.CHICK_ONE);
                        AnimalBase animalinst = Instantiate(animal, item.transform.position, item.transform.rotation);
                        animalinst.transform.SetParent(item.transform);
                        animalinst.parentCell = animalinst.transform.parent.GetComponent<Cell>();
                        animalinst.transform.position = item.transform.position + new Vector3(0, 0, -0.1f);
                        break;
                    }
                case "7":
                    {
                        AnimalBase animal = Resources.Load<AnimalBase>(SAnimalName.CHICK_TWO);
                        AnimalBase animalinst = Instantiate(animal, item.transform.position, item.transform.rotation);
                        animalinst.transform.SetParent(item.transform);
                        animalinst.parentCell = animalinst.transform.parent.GetComponent<Cell>();
                        animalinst.transform.position = item.transform.position + new Vector3(0, 0, -0.1f);
                        break;
                    }
                case "8":
                    {
                        AnimalBase animal = Resources.Load<AnimalBase>(SAnimalName.LION_TWO);
                        AnimalBase animalinst = Instantiate(animal, item.transform.position, item.transform.rotation);
                        animalinst.transform.SetParent(item.transform);
                        animalinst.parentCell = animalinst.transform.parent.GetComponent<Cell>();
                        animalinst.transform.position = item.transform.position + new Vector3(0, 0, -0.1f);
                        break;
                    }
                case "9":
                    {
                        AnimalBase animal = Resources.Load<AnimalBase>(SAnimalName.GIRAFFE_ONE);
                        AnimalBase animalinst = Instantiate(animal, item.transform.position, item.transform.rotation);
                        animalinst.transform.SetParent(item.transform);
                        animalinst.parentCell = animalinst.transform.parent.GetComponent<Cell>();
                        animalinst.transform.position = item.transform.position + new Vector3(0, 0, -0.1f);
                        break;
                    }
                case "12":
                    {
                        AnimalBase animal = Resources.Load<AnimalBase>(SAnimalName.ELEPHANT_TWO);
                        AnimalBase animalinst = Instantiate(animal, item.transform.position, item.transform.rotation);
                        animalinst.transform.SetParent(item.transform);
                        animalinst.parentCell = animalinst.transform.parent.GetComponent<Cell>();
                        animalinst.transform.position = item.transform.position + new Vector3(0, 0, -0.1f);
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
