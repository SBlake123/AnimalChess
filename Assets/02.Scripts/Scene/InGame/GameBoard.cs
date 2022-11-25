using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
	public static GameBoard instance;

    private void Awake()
    {
		instance = this;
    }

    [SerializeField]
	public SubArray[] m_mainArray;

	[Serializable]
	public struct SubArray
	{
		[SerializeField]
		public GameObject[] m_subArray;
	}
}
