using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour {
	// Use this for initialization
	void Start () {
		this.transform.FindChild("mouseOver").gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
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
		if(this.name == "Level1")
		{
			AudioManager.PlayButton2();
			AudioManager.StopSound(AudioManager.audioManager.backSound);
			Application.LoadLevel("Yotta_Level1");
		}
		if(this.name == "Level2")
		{
			AudioManager.PlayButton2();
			AudioManager.StopSound(AudioManager.audioManager.backSound);
			Application.LoadLevel("Yotta_Level2");
		}
		if(this.name == "Button_next")
		{
			AudioManager.PlayButton();
			GameObject.Find("TeamMem").GetComponent<BoxCollider2D>().enabled = true;
			GameObject.Find("StartGame").GetComponent<BoxCollider2D>().enabled = true;
			GameObject.Destroy(this.transform.parent.gameObject);
		}
	}
}

