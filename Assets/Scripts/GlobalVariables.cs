using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {
	public static int soundOn = 1;
	// Use this for initialization
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
