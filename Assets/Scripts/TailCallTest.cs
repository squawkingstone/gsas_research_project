using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TailCallTest : MonoBehaviour {

	UnityAction printAction;

	// Use this for initialization
	void Start () 
	{
		printAction = () => { System.Threading.Thread.Sleep(3000); Debug.Log("Done"); };
		Print();
	}

	void Print()
	{
		printAction();
		Debug.Log("Printing...");
	}

}
