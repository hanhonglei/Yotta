using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour {
	GameObject child; 
	GameObject othChild;
	public GameObject teamMem;
	private float timeEllapse = 0;
	private float timeLimit = 5;
	private int increaseAmount = 0;
	void Awake()
	{
		if(this.transform.FindChild("mouseOver"))
		{
			child = this.transform.Find("mouseOver").gameObject;
			child.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		}
		else 
			child = null;
		if(this.name.Contains("Music") || this.name.Contains("Off"))
		{
			if(this.transform.FindChild("mouseClick"))
			{
				othChild = this.transform.FindChild("mouseClick").gameObject;

				if(GlobalVariables.soundOn == 1)
				{
					othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				}
				else
				{
					othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
					this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				}
			}
		}
		else if(this.name.Contains("TeamMem"))
		{
			othChild = this.transform.FindChild("mouseClick").gameObject;
			if(othChild != null)
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}
		else if(this.name.Contains("Start"))
		{
			othChild = this.transform.FindChild("mouseClick").gameObject;
			if(othChild != null)
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}
		else if(this.name.Contains("Exit"))
		{
			othChild = this.transform.FindChild("mouseClick").gameObject;
			if(othChild != null)
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}
		else if(this.name.Contains("ReturntoMain"))
		{
			othChild = this.transform.FindChild("mouseClick").gameObject;
			if(othChild != null)
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		}
		else if(this.name == "hpEsseHead")
		{
			increaseAmount = 1;
			timeLimit = 5;
		}
		else if(this.name.Contains("Pause") || this.name.Contains("Select"))
		{
			if(this.transform.Find("mouseDown"))
			{
				othChild = this.transform.Find("mouseDown").gameObject;
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

			}
		}
		timeEllapse = 0;
	}
	public void OnMouseOver()
	{
		if( child != null)
		{
			child.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

		}
		else if(this.name.Contains("icon"))
		{
			this.GetComponent<SpriteRenderer>().color = Color.green;
			Transform can = this.transform.FindChild("Canvas");
			GameObject tex = can.FindChild("Text").gameObject;
			Text text = tex.transform.GetComponent<Text>();
			text.color = Color.green;
		}

	}
	public void OnMouseExit()
	{
		if (child != null)
		{
			child.GetComponent<SpriteRenderer> ().color = new Color (1.0f, 1.0f, 1.0f, 0.0f);
			this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			if(this.name.Contains("Pause"))
			{
				othChild = this.transform.Find("mouseDown").gameObject;
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			}
		}
		else if (this.name.Contains ("icon"))
		{
			this.GetComponent<SpriteRenderer> ().color = Color.white;
			Transform can = this.transform.FindChild("Canvas");
			GameObject tex = can.FindChild("Text").gameObject;
			Text text = tex.transform.GetComponent<Text>();
			text.color = Color.white;
		}
	}
	/*
	public void OnMouseStay()
	{
	}*/
	public void OnMouseUpAsButton()
	{

		if(child != null)
		{
			//child.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			if(this.transform.name.Contains("BuildingFlag"))
			{
				GameManager.ClearUpgradeUI();
				GameManager.ClearDismantleUI();
		  		BuildManager.Select(this.transform);
			}
			else if(this.name.Contains("Pause"))
			{
				AudioManager.PlayButton();
				othChild = this.transform.Find("mouseDown").gameObject;
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				//othChild = this.transform.Find("general").gameObject;
				//othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				child.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				Time.timeScale = (float)((int)(Time.timeScale +1.0f)%2);
			}
			else if(this.name.Contains ("Select"))
			{
				AudioManager.PlayButton();
				//othChild = this.transform.Find("mouseDown").gameObject;
				//othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

				//child.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

				GameManager.ShowInterfaceSet();
				Time.timeScale = 0.0f;
			}
			else if(this.name.Contains("UpgradeUI"))
			{
				GameManager.ClearBaseBuildings();
				BuildManager.SelectUpgradeUI();
				
			}
			else if(this.name.Contains("Dismantle"))
				BuildManager.SelectDismantleUI();
			else if(this.name.Contains("Replay"))
			{
				AudioManager.PlayButton();
				string name = "Yotta_Level"+GameManager.gameManager.currentLevel.ToString();
				AudioManager.PlayButton();
				Application.LoadLevel(name);
			}
			else if(this.name.Contains("Next"))
			{
				//for now, the level is limited to 2;
				AudioManager.PlayButton();
				int nextlevel = GameManager.gameManager.currentLevel+1;
				if(nextlevel>2)
					nextlevel = 2;
				string name = "Yotta_Level"+nextlevel.ToString();
				Application.LoadLevel(name);
			}
			else if(this.name.Contains("Back"))
			{
				AudioManager.PlayButton();
				//this.transform.parent.gameObject.SetActive(false);
				//Vector3 pos = this.transform.parent.position;
				//this.transform.parent.position= new Vector3(pos.x, pos.y, -2);
				GameManager.ClearInterfaceSet();
				Time.timeScale = 1.0f;

			}
			else if(this.name.Contains("LevSel"))
			{
				AudioManager.StopSound(AudioManager.audioManager.backSound);
				Application.LoadLevel("Yotta_LevelSelect");
			}
			else if(this.name.Contains("TeamMem"))
			{
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				child.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				AudioManager.PlayButton();
				Vector3 pos = new Vector3(0,0,0);
				Instantiate(teamMem, pos,Quaternion.identity);
				GameObject.Find("TeamMem").GetComponent<BoxCollider2D>().enabled = false;
				GameObject.Find("StartGame").GetComponent<BoxCollider2D>().enabled = false;
			}
			else if(this.name.Contains("Start"))
			{
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				child.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				AudioManager.PlayButton();
				AudioManager.StopSound(AudioManager.audioManager.backSound);
				Application.LoadLevel("Yotta_Start_mov");
			}
			else if(this.name.Contains("Exit"))
			{
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				child.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				Application.Quit();
			}
			else if(this.name.Contains("ReturntoMain"))
			{
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
				child.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				AudioManager.PlayButton();
				AudioManager.StopSound(AudioManager.audioManager.backSound);
				Application.LoadLevel("Yotta_Start0");
			}
			else if(this.name.Contains("Button_next"))
			{
				AudioManager.PlayButton();
				GameObject.Find("TeamMem").GetComponent<BoxCollider2D>().enabled = true;
				GameObject.Find("StartGame").GetComponent<BoxCollider2D>().enabled = true;
				GameObject.Destroy(this.transform.parent.gameObject);
			}
		}
		else if(this.name.Contains("Music"))
		{
			AudioManager.PlayButton();
			othChild = this.transform.Find("mouseClick").gameObject;
			int flag = (int)GlobalVariables.soundOn;
			//Color color;//the change with color param can be done later
			if(othChild != null)
			{
				flag = (int)this.GetComponent<SpriteRenderer>().color.a;
				this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, (float)((flag+1)%2));
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, flag*1.0f);
				GameObject otherObj;
				GameObject otherObjChild;
				otherObj = this.transform.parent.Find("SetOff").gameObject;
				otherObj.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, (float)((flag+1)%2));
				otherObjChild = otherObj.transform.Find("mouseClick").gameObject;
				if(otherObjChild != null)
					otherObjChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, flag*1.0f);
			}
			GlobalVariables.soundOn = (flag+1)%2;
		}
		else if (this.name.Contains("Off"))
		{
			AudioManager.PlayButton();
			othChild = this.transform.Find("mouseClick").gameObject;
			int flag = 0;

			//Color color;//the change with color param can be done later
			if(othChild != null)
			{
				flag = (int)this.GetComponent<SpriteRenderer>().color.r;
				this.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, (flag+1)%2*1.0f);
				othChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, flag*1.0f);
				GameObject otherObj;
				GameObject otherObjChild;
				otherObj = this.transform.parent.Find("SetMusic").gameObject;
				otherObj.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, (flag+1)%2*1.0f);
				otherObjChild = otherObj.transform.Find("mouseClick").gameObject;
				if(otherObjChild != null)
					otherObjChild.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, flag*1.0f);
			}
			GlobalVariables.soundOn = (flag+1)%2;
		}
		else if(this.name.Contains("icon"))
		{
			this.GetComponent<SpriteRenderer>().color = Color.green;
			Transform can = this.transform.FindChild("Canvas");
			GameObject tex = can.FindChild("Text").gameObject;
			Text text = tex.transform.GetComponent<Text>();
			text.color = Color.green;
			GameManager.ClearBaseBuildings();
			BuildManager.Select(this.gameObject);

		}
		else if(this.name.Contains("B0"))//building
		{
			UnitTower tower = this.GetComponent<UnitTower>();
			BuildManager.Select(tower);
		}
		else if(this.name.Contains("Esse"))
		{
			AudioManager.PlaySound(AudioManager.audioManager.essenCollectSound,false);
			ScoreControl.EssenceIncrease(increaseAmount);
			Destroy(this.transform.parent.gameObject);
			PoolManager.Unspawn (this.transform.parent.gameObject);
		}

	}
	void Start()
	{

	}
	void Update()
	{
		if(Input.GetButtonDown("Fire1"))
		{
			Vector3 pointer = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//RaycastHit2D hit;
			//if(Physics.Raycast(pointer,out )
			Vector3 dir = new Vector3(0.0f,0.0f, 1.0f);
			if(Physics2D.Raycast(pointer, dir, 5.0f, LayerMask.GetMask("BuildingFlags"))== false
			   && Physics2D.Raycast(pointer, dir, 5.0f, LayerMask.GetMask("Tower"))== false
			   && Physics2D.Raycast(pointer, dir, 5.0f, LayerMask.GetMask("BuildingIcons"))== false
			   && Physics2D.Raycast(pointer, dir, 5.0f, LayerMask.GetMask("UpgradeUI"))== false)
			{
				GameManager.ClearSelection();
			}
			/*-
			if(Physics2D.Raycast(pointer, Vector2.up, 50.0f, LayerMask.GetMask("BuildingFlags"))== true)
			{
				int a = 0;
			}*/
		}
		if(this.name.Contains("Esse"))
		{
			timeEllapse += Time.deltaTime;
			if(timeEllapse > timeLimit)
			{
				Destroy(this.transform.parent.gameObject);
			}
		}

	} 





}
