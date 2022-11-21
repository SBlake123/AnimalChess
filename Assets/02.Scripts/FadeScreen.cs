using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeScreen : MonoBehaviour
{
    public Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mat.DOFade(1, 0);
        mat.DOFade(0, 5);
    }

    
}
