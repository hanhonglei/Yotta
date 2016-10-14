using UnityEngine;
using System.Collections;

public class StartMenu0 : MonoBehaviour {
	public GameObject teamMem = null;
	// Use this for initialization
	void Start () {
		this.transform.FindChild("mouseOver").gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	}
	/*void OnGUI()
	{
		if(GUI.Button(new Rect(Screen.width/2-64,Screen.height*0.75f-16,128,32),"Start"))
		{
			Application.LoadLevel("Yotta_World");
		}
		
	}*/
	void OnMouseEnter()
	{
		this.transform.FindChild("mouseOver").gameObject.SetActive (true);
	}
	void OnMouseExit()
	{
		this.transform.FindChild("mouseOver").gameObject.SetActive (false);
	}
	void OnMouseUpAsButton()
	{
		this.transform.FindChild("mouseOver").gameObject.SetActive (true);
		if(this.name == "StartGame")
		{
			AudioManager.PlayButton();
			AudioManager.StopSound(AudioManager.audioManager.backSound);
			Application.LoadLevel("Yotta_Start_mov");
		}
		if(this.name == "TeamMem")
		{
			AudioManager.PlayButton();
			Vector3 pos = new Vector3(0,0,0);
			Instantiate(teamMem, pos,Quaternion.identity);
			GameObject.Find("TeamMem").GetComponent<BoxCollider2D>().enabled = false;
			GameObject.Find("StartGame").GetComponent<BoxCollider2D>().enabled = false;
		}
	}
}


