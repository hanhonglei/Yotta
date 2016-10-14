using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


//stay focus, everything will be fine!!
//stay calm, and you will be better than it!!



public enum TowerType
{
	None = 0,
	em_ScenicSpot,			//A级景点
	em_Catering,			//小餐馆
	em_Accommodation,		//小旅店
	em_Film,				//小剧场
	em_Shopping,			//便利店
	em_Amusement,			//滑梯公园
};
public enum TowerLevel
{
	None = 0,
	em_Level1,
	em_Level2,
	em_Level3,
	em_Level4,
};

[System.Serializable]
public class TowerAttribute 			// 
{
	public int cost=10;						//价格
	//public int[] costs=new int[1];
//	public int[] incomes=new int[1];
	public int price = 0;					//售价
	public int fixCost = 0;					//修理费
//	public int[] repareCosts = new int[1];

//	public float damage=5;
	public float cooldown=1;
//	public int clipSize=5;
	[HideInInspector]
	public float reloadDuration=1;
	public float range=10;
	public float minRange=0;
	public float maxRange = 10;
	public float attackInterval = 0.5f;
	public int durability = 0;

	[HideInInspector]
	public float buildDuration = 1;
	[HideInInspector]
	public float stunDuration = 0;
	//public float influProbability = 1;

	public Transform attractObject;  ////the effects which the tower attracts the visitor
	public Transform turretObject;	//the turret tower
	public Transform baseObject;	//the base point(building point)

	//public float hpIncrease = 0;

	public TowerType towerType = TowerType.None;
	public TowerLevel towerLevel  = TowerLevel.None;
	public string description = "Description for the Tower.";

	public int upgradeCost = 100;
}


public class UnitTower : Unit {
	public TowerAttribute baseTower; 									//base attribute
	public TowerAttribute[] upgradeTowers = new TowerAttribute[1];		//upgrade

	public float turretMaxAngle = 0;
	public float turretMaxRange = 0;

	private Unit target;
	//private float curTargetDist = 0.0f;
	
	public AudioClip attractSound;
	public AudioClip reloadSound;
	public AudioClip buildSound;

	public float timeEllapse = 0;

	private int towerID = -1;
	public void SetTowerID(int ID){ towerID = ID;}
	public int GetTowerID(){ return towerID;}
	
	private bool isCount = false;


	public GameObject coinHead;
	public GameObject childRange;

	private float circleWidth = 10.4f;
	private float circleHeight = 5.91f;

	public override void Awake()     
	{ 
		base.Awake ();
	}


	// Use this for initialization
	public override void Start () {
		base.Start ();
		towerID = ReadFromExcel.ExtractInt(thisObj.name);
		InitTower(towerID);
		//Debug.Log(baseTower.range);
		thisT.FindChild("unitRange").transform.localScale = new Vector3((baseTower.range-1)*2/6.44f, (baseTower.range-1)*2/5.32f,1.0f);
		childRange = thisT.FindChild ("unitRange").gameObject;
		childRange.transform.localScale = new Vector3((baseTower.range)*2/circleWidth, (baseTower.range)*2/circleHeight*0.7f,1.0f);
		//childRange.AddComponent<CircleCollider2D> ().radius = baseTower.range;
		//childRange.GetComponent<CircleCollider2D> ().isTrigger = true;
	}

	public void InitTower(int ID)  //the level is not used here
	{
		TowerPool tgameObject;
		tgameObject = PoolManager.poolManager.GetTowerParamFromID(ID);
		base.unitName = tgameObject.name;
		baseTower = tgameObject.towerAttri;
		baseTower.range = baseTower.maxRange;
	}
	
	bool ScanForTarget()
	{//scan for the targets, if the timeLimit is over, then charge the targets in its influence area with certain probablity
		Collider2D[] colliders;
		//colliders.Initialize();
		//Vector3 pos = new Vector3(thisT.position.x, thisT.position.y+thisObj.GetComponent<BoxCollider2D>().size.y/2.0f, thisT.position.z);
		Vector3 pos = new Vector3(thisT.position.x, thisT.position.y, thisT.position.z);
		colliders= Physics2D.OverlapCircleAll(pos, baseTower.range,LayerMask.GetMask("Visitor"));
		float hpIncrease;
		UnitVisitor tempVisitor;
		Vector3 tempPos;
		float yratio = 2.0408163f; // 1/(0.7^0.7)
		Vector3 dir;
		if(colliders.Length > 0)
		{
			isCount = true;
			if(baseTower.stunDuration*5>= baseTower.attackInterval)
			//if(baseTower.stunDuration >= baseTower.timeLimit)
			{
				foreach(Collider2D collider in colliders )
				{
					dir = collider.transform.position - thisT.position;
					if(dir.x*dir.x+dir.y*dir.y*yratio < baseTower.range*baseTower.range)
					{
						tempVisitor = collider.GetComponent<UnitVisitor>();
						hpIncrease = PoolManager.GetPref(towerID,tempVisitor.visitorAttri.visitorID);
						tempVisitor.TakeRecover(hpIncrease*(1-tempVisitor.visitorAttri.decrement));
						ScoreControl.CoinIncrease((int)(hpIncrease*tempVisitor.visitorAttri.capacity));
						tempPos = thisT.position;
						tempPos.y += thisT.GetComponent<BoxCollider2D>().size.y/2;
						Instantiate (coinHead, tempPos, Quaternion.identity);
						//PoolManager.Spawn(GameManager.gameManager.coinHead,tempPos,Quaternion.identity);
						//Debug.Log(hpIncrease*tempVisitor.visitorAttri.capacity);
					}
				}
				baseTower.stunDuration = 0;
			}
			return true;
		}
		else
		{
			isCount = false;
			return false;
		}
	}

	// Update is called once per frame*/
	public override void Update () 
	{
		base.Update();
		timeEllapse += Time.deltaTime;
		if(isCount)
		{
			baseTower.stunDuration += Time.deltaTime;
		}
		//if (timeEllapse > 1.0f)
			//childRange.SetActive (false);
			//thisT.FindChild("unitRange").gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		ScanForTarget();
	}	
}


































