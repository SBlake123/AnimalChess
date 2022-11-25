using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCanvas : MonoBehaviour
{
    public GameObject loginCanvas;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {          
            loginCanvas.SetActive(true);
            loginCanvas.GetComponent<Canvas>().enabled = true;
            this.gameObject.SetActive(false);
        }
    }
}
