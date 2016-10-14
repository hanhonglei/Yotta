using UnityEngine;
using System.Collections;


public enum VisitorType
{
	None = 0,
	em_Kid,
	em_Male,
	em_Female,
	em_Senium,
	em_Alien,
};
public enum VisitorLevel
{
	None = 0,
	em_Level1,
	em_Level2,
	em_Level3,
	em_Level4,
};
public enum VisitorEffect
{
	None = -1,
	em_BadEffect,
	em_GoodEffect,
}
[System.Serializable]
public class VisitorAttribute
{
	public int visitorID;
	public float angryLimit = 5;
	public float initHp = 0;
	public float hpDecrease = 0;
	public int essencePoints = 0;
	public float damage = 0;
	public float speedUpRatio=1.5f;
	public float capacity;
	public int happyLimit = 10;
	
	//public bool isActive = false;

	public float normalSpeed;
	public float angrySpeed;
	
	
	public VisitorType visitorType = VisitorType.None;
	public VisitorLevel visitorLevel = VisitorLevel.None;
	public string description = "";
	public string bgDescription = "";
	public float decrement = 1.0f;
	public int defence = 10;
	public float moveReduce = 0.0f;
	public float angryMax = 15.0f;
	//public string preferBuilTp = "";
}

public class UnitVisitor : Unit {
	public VisitorAttribute visitorAttri;
	public VisitorAttribute[] upgradeVisitors = new VisitorAttribute[1];

	public Transform[] destPoint;
	private Transform targetPoint;
	private Vector3 moveDirection;
	//public int visitorID;
	public GameObject childFire = null;
	public GameObject childLove = null;
	public GameObject childDislike = null;
	public GameObject thisCore = null;
	VisitorEffect showEffect = VisitorEffect.None;
	bool effectChanges = false;
	bool angryChange = false;// non angry to angry state
	bool isAngry = false;
	public GameObject essenceHead;
	public AudioClip[] audioEffect;
	// Use this for initialization
	public override void Awake () {
		base.Awake();
		targetPoint = PoolManager.poolManager.GetStartPoint();
		destPoint = PoolManager.poolManager.GetEndPoints();

		moveDirection = targetPoint.position - thisT.position;
		moveDirection.z = 0;
		moveDirection.Normalize ();
		//HpAttribute.m_curHp = 8;
		//HpAttribute.m_fullHp = 30;
		moveSpeed = 2.0f;
		rotateSpeed = 0.3f;
		childFire = thisT.FindChild("Fire").gameObject;
		childFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 28.0f/255, 33.0f/255, 0.0f);
		childLove = thisT.FindChild ("LovingEffects").gameObject;
		childDislike = thisT.FindChild ("DislikeEffects").gameObject;

		if(childFire != null)
			childFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		if( childDislike != null)
			childDislike.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		if(childLove != null)
			childLove.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

		if(moveDirection.x >0)
			FlipSprite(-1);
		else
			FlipSprite(1);

	//	UpdateOverlayBar();
	}
	public override void Start()
	{
		base.Start();
		//ReadFromExcel.readXLS(Application.dataPath+"/YOTTA_param.xls","Visitor");
		visitorAttri.visitorID = ReadFromExcel.ExtractInt(thisObj.name);
		InitVisitor(visitorAttri.visitorID);

	}
	private void InitVisitor(int ID)
	{
		VisitorPool visitorPool;

		visitorPool = PoolManager.poolManager.GetVisitorParamFromID(ID);	
		visitorAttri = visitorPool.visitorAttri;
		base.unitName = visitorPool.name;
		visitorAttri = visitorPool.visitorAttri;

		base.HpAttribute.m_curHp = visitorAttri.initHp;
		base.HpAttribute.m_fullHp = visitorPool.fullHp;
		base.moveSpeed = visitorAttri.normalSpeed;
		UpdateOverlayBar();
	}
	// Update is called once per frame
	public override void Update () {
		base.Update();
		if(targetPoint != null)
		{
			DamageEllipse(Time.deltaTime);
			if (Vector3.Distance (thisT.position, targetPoint.position) < 0.5f)
			{
				if((targetPoint = Findnext(targetPoint))!=null)
				{
					thisObj.GetComponent<SpriteRenderer>().sortingLayerName = targetPoint.GetComponent<SpriteRenderer>().sortingLayerName;
					SpriteRenderer[] spriteRenderers;
					spriteRenderers = thisObj.GetComponentsInChildren<SpriteRenderer>();
					foreach(SpriteRenderer spriteRender in spriteRenderers)
					{
						spriteRender.sortingLayerName = targetPoint.GetComponent<SpriteRenderer>().sortingLayerName;
					}
					moveDirection = targetPoint.position - thisT.position;
					moveDirection.z = 0;
					moveDirection.Normalize();
				}
			}
		}

		UpdateState();
		if(moveDirection.x >0)
			FlipSprite(-1);
		else
			FlipSprite(1);
		ChangeDirection();
	}

	void ChangeDirection()
	{
		Vector3 target = moveDirection * moveSpeed + thisT.position;
		target = Vector3.Lerp (thisT.position, target, Time.deltaTime);
		GetComponent<Rigidbody2D>().MovePosition(new Vector2(target.x, target.y));
	}

	void UpdateState()
	{
		if(HpAttribute.m_curHp < visitorAttri.angryLimit)
		{
			SpriteRenderer spriteRender;
			spriteRender = thisCore.GetComponent<SpriteRenderer>();
			spriteRender.color = new Color(1.0f, 28.0f/255, 33.0f/255, 1.0f);
			childFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 28.0f/255, 33.0f/255, 1.0f);
			//childFire.SetActive(true);
			moveSpeed = visitorAttri.angrySpeed;
			int index = ((int)this.visitorAttri.visitorType - 1) * 3;
			if(isAngry != true)
			{
				angryChange = true;
				isAngry = true;
				if( angryChange && audioEffect[index+2]!= null)
					AudioManager.PlaySound(audioEffect[index+2],false);

			}
			else
				angryChange = false;

		}
		else
		{
			SpriteRenderer spriteRender;
			spriteRender = thisCore.GetComponent<SpriteRenderer>();
			childFire.GetComponent<SpriteRenderer>().color = new Color(1.0f, 28.0f/255, 33.0f/255, 0.0f);
			//childFire.SetActive(false); //SetActive(false) of cloned object will delete this object
			spriteRender.color = Color.white;
			moveSpeed = visitorAttri.normalSpeed;
			isAngry = false;
			angryChange = false;
			int index = ((int)this.visitorAttri.visitorType - 1) * 3;
			AudioManager.StopSound(audioEffect[index+2]);

		}
		UpdateOverlayBar();
		ShowEffect(ScanForTower ());
	}
	

	Transform Findnext(Transform point)
	{
		for(int i=0; i<destPoint.Length; ++i)
		{
			if(point.position == destPoint[i].position)
				return null;
		}
		Transform[] wayPoints = point.GetComponent<WayPoint> ().next;
		int index = Random.Range (0, wayPoints.Length);
		return wayPoints [index];
	}
	void UpdateOverlayBar()
	{
		SpriteRenderer spriteRender;
		spriteRender = HpAttribute.m_overlayHp.GetComponent<SpriteRenderer>();
		spriteRender.color = Color.Lerp (Color.yellow, Color.red, 1 - (HpAttribute.m_curHp-visitorAttri.angryLimit) /(HpAttribute.m_fullHp-visitorAttri.angryLimit));
		HpAttribute.m_overlayHp.localScale = new Vector3 ((HpAttribute.m_curHp-visitorAttri.angryMax) /(HpAttribute.m_fullHp-visitorAttri.angryMax), 1, 1);//
	}
	public void TakeRecover(float increaseAmount)
	{
		HpAttribute.m_curHp += increaseAmount; 

		if (HpAttribute.m_curHp> HpAttribute.m_fullHp)
		{
			Vector3 pos = HpAttribute.m_overlayHp.position;
			//Transform childEssence = GameManager.gameManager.essenceHsead.transform.FindChild("hpEsseHead");
			Transform childEssence = essenceHead.transform.FindChild("hpEsseHead");
			if( childEssence != null)
			{
				Vector2 size = childEssence.GetComponent<BoxCollider2D>().size;
				float horiDist = 2;
				float vertexDist = 1.5f;//here comes from the inspector of the animation
				float minX = GameManager.gameManager.leftBottom.x;
				float maxX = -minX-horiDist-size.x;
				float minY = GameManager.gameManager.leftBottom.y;
				float maxY = -minY - vertexDist - size.y;
				pos = new Vector3(Mathf.Clamp(pos.x,minX,maxX), Mathf.Clamp(pos.y, minY, maxY), pos.z);
				Instantiate(essenceHead, pos, Quaternion.identity);
				//PoolManager.Spawn(GameManager.gameManager.essenceHead,pos,Quaternion.identity);
				HpAttribute.m_curHp = visitorAttri.initHp;
			}
		}
		UpdateOverlayBar ();
	}

	public void TakeDamage( float damage )
	{
		HpAttribute.m_curHp -= damage;
		if (HpAttribute.m_curHp < visitorAttri.angryMax)
			HpAttribute.m_curHp = visitorAttri.angryMax;
	
		UpdateOverlayBar ();
	}

	void DamageEllipse(float time)
	{
		TakeDamage( visitorAttri.moveReduce*time );
	}

	void Die()
	{
		HpAttribute.m_curHp = 0.0f;
	}
	void OnBecameInvisible()
	{
		//have some problems
		if(this != null)
		{
			if(this.HpAttribute.m_curHp < this.visitorAttri.angryLimit && targetPoint == null )
				ScoreControl.AngryIncrease(1);
			thisObj.SetActive(false);
		}

	}
	void FlipSprite(int x)
	{
		thisCore.transform.localScale = new Vector3(x, 1,1);
	}
	/*
	public int isPreferable(TowerType type)
	{
		int dividePos = 0;
		for(int i=visitorAttri.preferBuilTp.Length-1; i>=0; --i)
		{
			if((int)type == (visitorAttri.preferBuilTp[i]-'0') && i>dividePos)
				return 1;
			if(visitorAttri.preferBuilTp[i] == '0')
			{
				dividePos = i;
				continue;
			}
			if((int)type == (visitorAttri.preferBuilTp[i]-'0') && i<dividePos)
				return -1;

		}
		return 0;
	}
	*/
	VisitorEffect ScanForTower()
	{
		//Judge the effect
		//use the raycast method
		//repeat the number of the tower times
		//each time, 1. get the whole tower information
		//2. raycast a ray from the current position of the visitor to the tower's position
		//3. the distance is the tower's range
		//4. if hit, then add the current tower to the list of coveredTowers
		//5. check if this is the favorable tower
		//6. ShowFavorble and ClearHateful when : a. covered by a favorable tower, b. the hp is larger than the angrylimit
		//7. ShowHateful and ClearFavorable when : a. covered by no favorable tower, b. covered by a hateful tower
		//8. ClearFavorable and ClearHateful when there is no coveredTower or the coveredTower is all null
		//9. keep in mind that the range is not a circle.
		Collider2D[] towerColliders;
		//this step could be replaced by the "PoolManager.allobject" method, recheck it later
		VisitorEffect tempEffect;
		tempEffect = showEffect;
		showEffect = VisitorEffect.None;

		towerColliders = Physics2D.OverlapAreaAll(GameManager.gameManager.leftBottom, GameManager.gameManager.rightTop, 
		                                          LayerMask.GetMask("Tower")); //need to be reviewed CCat


		//---------------the things that already known
		float yratio = 2.0408163f; // 1/(0.7^0.7)
		float range;
		int i = 0;
		foreach(Collider2D towerColli in towerColliders)
		{
			UnitTower tower = towerColli.GetComponent<UnitTower>();
			range = tower.baseTower.range;
			Vector3 dir = towerColli.transform.position - thisT.position;
			//RaycastHit2D hit;
			//if((hit = Physics2D.Raycast(thisT.position, dir, tower.baseTower.range, LayerMask.GetMask("Tower")))!= false)
			if(dir.x*dir.x+dir.y*dir.y*yratio <= range*range)
			{
				if(i == 0)
					showEffect = VisitorEffect.em_BadEffect;
				VisitorEffect visEffect = (VisitorEffect)PoolManager.GetEffect(ReadFromExcel.ExtractInt(tower.name), ReadFromExcel.ExtractInt(this.name));
				if(visEffect == VisitorEffect.em_GoodEffect)
				{
					if(HpAttribute.m_curHp < visitorAttri.angryLimit)
					{
						visEffect = VisitorEffect.None;
					}
				}
				if(visEffect != VisitorEffect.None)
					showEffect = showEffect | visEffect;
				else
					showEffect = VisitorEffect.None;
				//showEffect = showEffect | visEffect; //need to be checked
				++i;
			}


		}
		if(tempEffect == showEffect)
		{
			effectChanges = false;
		}
		else 
			effectChanges = true;
		return showEffect;
	}
	void ShowEffect(VisitorEffect effect)
	{
		//AudioClip clip;
		int index = ((int)this.visitorAttri.visitorType - 1) * 3;
		//AudioClip clip = audioEffect[(int)this.visitorAttri.visitorType]
		switch(effect)
		{
		case VisitorEffect.em_GoodEffect:
			/*
			childLove.renderer.enabled = true;
			childDislike.renderer.enabled = false;*/
			childLove.GetComponent<SpriteRenderer>().color = Color.white;
			childDislike.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			if( effectChanges && audioEffect[index]!= null)
				AudioManager.PlaySound(audioEffect[index],false);
			break;
		case VisitorEffect.em_BadEffect:
			childDislike.GetComponent<SpriteRenderer>().color = Color.white;
			childLove.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			if( effectChanges && audioEffect[index+1]!= null)
				AudioManager.PlaySound(audioEffect[index+1],false);
			break;
		case VisitorEffect.None:
			childLove.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			childDislike.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
			break;
		}
	}


}
