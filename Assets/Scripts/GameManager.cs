using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum GameState
{
	em_Idle,
	em_Started,
	em_Ended
}
public enum GradeLevel
{
	em_null = 0,
	em_S,
	em_A,
	em_B,
	em_C
}
 //[RequireComponent (typeof (ResourceManager))]
 [RequireComponent (typeof (LayerManager))]

public class GameManager : MonoBehaviour {
	public delegate void GameOverHandler(bool win);
	public static event GameOverHandler GameOverE;

	public delegate void ResourceHandler();
	public static event ResourceHandler ResourceE;

	public delegate void LifeHandler();
	public static event LifeHandler LifeE;

	static public GameState gameState = GameState.em_Idle;

	public int playerLife = 10;

	public float sellTowerRefundRatio = 0.5f;
	public static int soundOff = 1;
	[HideInInspector] public LayerManager layerManager;
	//public SpawnManager spawnManager;
	public int currentLevel = 0;
	private int totalWaveCount;
	private int currentWave = 0;
	public GameObject passportUI;
	public GameObject gameoverUI;
	public GameObject scoreUI;
	public GameObject star;
	//public GameObject essenceHead;
	//public GameObject coinHead;

	public GameObject rangeIndicator;
	private float rangeIndicatorWidth = 10.4f;
	private float rangeIndicatorHeight = 5.91f;
	public GameObject upgradeUI;
	public GameObject dismantleUI;
	public GameObject[] baseBuilidngs;
	public GameObject _baseBuildings;
    
	public GameObject buildingFlag;

	public GameObject interfaceSet;

	public static GameManager gameManager;
	float showtime = 0;

	//public float buildingBarWidthModifier = 1.0f;
	//public float buildingBarHeightModifier = 1.0f;
	//public Vector3 buildingBarPosOffset = new Vector3(0.0f, 0.5f, 0.0f);
	Vector2 pos1 = new Vector2(-20.48f,-11.52f);
	Vector2 pos2 = new Vector2(20.48f, 11.52f);
	[HideInInspector] public Vector3 leftBottom; 
	[HideInInspector] public Vector3 rightTop; 


	float gridWidth = 2.0f;
	float gridHeight = 1.5f;
	float intervalx = 0.1f;
	float intervaly = 0.7f;
	
	float minX;
	//float maxX = -1 * minX - gridWidth * 3 - interval * 3;
	float maxY;
	float minY;

	float startTime;
	void Awake()
	{
		PoolManager.Init ();
		gameManager = this;

		gameState = GameState.em_Idle;

		leftBottom = Camera.main.ScreenToWorldPoint (new Vector3 (0, 0, Camera.main.nearClipPlane));
		rightTop = Camera.main.ScreenToWorldPoint (new Vector3 (Camera.main.pixelWidth, Camera.main.pixelHeight, Camera.main.nearClipPlane));

		minX = leftBottom.x +(gridWidth * 3 + intervalx * 3)*0.5f+0.3f;
		//float maxX = -1 * minX - gridWidth * 3 - interval * 3;
		maxY =  rightTop.y -gridHeight*2-intervaly*2 - 250/100;
		minY = leftBottom.y;
		scoreUI = GameObject.Find ("UI");
		//PoolManager.New (coinHead.transform, 10);
		//PoolManager.New (essenceHead.transform, 3);
	}
	// Use this for initialization
	void Start () {
		//totalWaveCount = SpawnManager.spawnManager.waveList.Length;
		totalWaveCount = GameObject.Find ("SpawnPoint").GetComponent<SpawnManager> ().waveList.Length;
		//SpawnManager.WaveStartSpawnE += WaveStartSpawned;
		//SpawnManager.WaveClearedE += WaveCleared;

		//Unit.ScoreE += DeductLife;
		//UnitTower.DestroyE += TowerDestroy;

		rangeIndicator = (GameObject)Instantiate (rangeIndicator);
		rangeIndicator.transform.parent = this.transform;
		int i = 0;
		_baseBuildings = new GameObject ();
		_baseBuildings.name = "baseBuildings";
		_baseBuildings.transform.parent = this.transform;
		foreach(GameObject buiding in baseBuilidngs)
		{
			baseBuilidngs[i] = (GameObject)Instantiate(buiding);
			i++;
		}
		ArrangeBuilding (this.transform.position);
		UpdateBuilState (baseBuilidngs);
		i = 0;
		foreach(GameObject buiding in baseBuilidngs)
		{
			baseBuilidngs[i].transform.parent = _baseBuildings.transform;
			i++;
		}
		upgradeUI = (GameObject)Instantiate (upgradeUI);
		upgradeUI.transform.parent = this.transform;
		dismantleUI = (GameObject)Instantiate (dismantleUI);
		dismantleUI.transform.parent = this.transform;
		interfaceSet = (GameObject)Instantiate(interfaceSet);
		interfaceSet.transform.parent = this.transform;

		ClearIndicator ();
		ClearBaseBuildings ();
		ClearUpgradeUI ();
		ClearDismantleUI ();
		ClearInterfaceSet();

		startTime = Time.time;
		Time.timeScale = 1;
		//Camera.main.audio.volume = 0.001f;
		//Camera.main.GetComponent<AudioListener>().
		AudioListener.volume = 0.5f;
	}
	void OnDisable()
	{
		/*
		SpawnManager.WaveStartSpawnE -= WaveStartSpawned;
		SpawnManager.WaveClearedE -= WaveCleared;
		//SpawnManager.WaveSpawnedE -= WaveSpawned;
		
		Unit.ScoreE -= DeductLife;
		UnitTower.DestroyE -= TowerDestroy;
		*/
    }
	void DeductLife(int waveID){
		playerLife-=1;
		if(playerLife<=0) playerLife=0;
		
		if(LifeE!=null) LifeE();
		
		if(playerLife==0){
			//game over, player lost
			gameState=GameState.em_Ended;
            if(GameOverE!=null) GameOverE(false);
        }
    }
	void TowerDestroy(UnitTower tower){
		if(BuildManager.selectedTower==tower || BuildManager.selectedTower==null || !BuildManager.selectedTower.thisObj.activeInHierarchy){
			ClearSelection();
        }
    }
	public static int GetPlayerLife(){
		return gameManager.playerLife;
    }
	void WaveStartSpawned(int waveID){
		currentWave+=1;
		
		//if game is not yet started, start it now
		if(gameState==GameState.em_Idle) gameState=GameState.em_Started;
	}
	
	void WaveSpawned(int waveID){
		
	}
	
	void WaveCleared(int waveID){
		//Debug.Log("Wave "+waveID+" has been cleared");
		if(waveID==totalWaveCount-1){
			//game over, player won
			gameState=GameState.em_Ended;
			if(GameOverE!=null) GameOverE(true);
		}
	}
	public static void ShowUpgrade(UnitTower tower)
	{
		gameManager._ShowUpgrade (tower);
	}

	public void _ShowUpgrade(UnitTower tower)
	{
		Vector3 pos;
		if(upgradeUI!= null)
		{
			int upprice = tower.baseTower.upgradeCost;
			Transform can = upgradeUI.gameObject.transform.FindChild("Canvas");
			GameObject tex = can.FindChild("Text").gameObject;
			Text text = tex.transform.GetComponent<Text>();
			text.text = "$" + upprice.ToString ();
			pos = tower.thisT.position;
			float height = upgradeUI.GetComponent<BoxCollider2D>().size.y*upgradeUI.transform.localScale.y;

			if(pos.y+ tower.GetComponent<BoxCollider2D>().size.y+height/2+0.05f < maxY)
			{
				upgradeUI.transform.position = new Vector3(pos.x, pos.y+ tower.GetComponent<BoxCollider2D>().size.y+height/2+0.05f, pos.z);
			}
			else 
			{
				float width = upgradeUI.GetComponent<BoxCollider2D>().size.x*upgradeUI.transform.localScale.x;
				float width2 = tower.GetComponent<BoxCollider2D>().size.x;
				upgradeUI.transform.position = new Vector3(pos.x-width2/2-width/2-0.05f, pos.y, pos.z);
			}
			upgradeUI.GetComponent<Renderer>().enabled = true;
			if(ScoreControl.CheckResourceSuffi(upprice) == true && BuildManager.FindUpgrade(ReadFromExcel.ExtractInt(tower.name)))
			{
				upgradeUI.GetComponent<SpriteRenderer>().color = Color.white;
				text.color = Color.white;
				upgradeUI.GetComponent<BoxCollider2D>().enabled = true;
				Transform child;
				if((child = upgradeUI.transform.FindChild("mouseOver")) != null)
					child.GetComponent<Renderer>().enabled = true;
			}
			else
			{
				upgradeUI.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
				text.color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
				upgradeUI.GetComponent<BoxCollider2D>().enabled = false;
				Transform child;
				if((child = upgradeUI.transform.FindChild("mouseOver")) != null)
					child.GetComponent<Renderer>().enabled = false;
			}
		}

	}
	public static void ShowDismantle(UnitTower tower)
	{
		gameManager._ShowDismantle (tower);
	}
	
	public void _ShowDismantle(UnitTower tower)
	{
		Vector3 pos;
		if(dismantleUI!= null)
		{
			int upprice = tower.baseTower.price;
			Transform can = dismantleUI.gameObject.transform.FindChild("Canvas");
			GameObject tex = can.FindChild("Text").gameObject;
			Text text = tex.transform.GetComponent<Text>();
			text.text = "$" + upprice.ToString ();
			pos = tower.thisT.position;
			float height = dismantleUI.GetComponent<BoxCollider2D>().size.y*dismantleUI.transform.localScale.y;
			dismantleUI.transform.position = new Vector3(pos.x, pos.y-height/2-0.05f, pos.z);
			dismantleUI.GetComponent<Renderer>().enabled = true;
			dismantleUI.GetComponent<BoxCollider2D>().enabled = true;
			Transform child;
			if((child = dismantleUI.transform.FindChild("mouseOver")) != null)
				child.GetComponent<Renderer>().enabled = true;
		}
		
	}
	public static void ShowBaseBuilding(Transform trs)
	{
		gameManager._ShowBaseBuilding (trs);
	}
	
	public void _ShowBaseBuilding(Transform trs)
	{
		//_basebuilding
		if(_baseBuildings != null)
		{
			Vector3 pos = trs.position;
			float disy = buildingFlag.GetComponent<BoxCollider2D>().size.y*buildingFlag.transform.localScale.y;
			pos = new Vector3(pos.x, pos.y+ disy+0.05f, pos.z);
			if(pos.y > maxY)
				pos = new Vector3(pos.x, pos.y-(disy+0.05f+gridHeight*2 + intervaly*2), pos.z);
			pos = gameManager.ClampPosition(pos);
			_baseBuildings.transform.position = pos;
			//_baseBuildings.renderer.enabled = true;
			foreach(GameObject building in baseBuilidngs)
				building.GetComponent<Renderer>().enabled = true;
			UpdateBuilState(baseBuilidngs);
		}
		
	}
	public void ArrangeBuilding(Vector3 center) //center bottom
	{
		Vector3 pos;
		int i = 0;

		#region 
		/*
		float gridWidth = 2.0f;
		float gridHeight = 1.5f;
		float intervalx = 0.1f;
		float intervaly = 0.7f;

		float minX = leftBottom.x;
		float maxX = -1 * minX - gridWidth * 3 - intervalx * 3;
		float maxY =  rightTop.y;
		float minY = -1 * maxY + gridHeight * 2 + intervaly * 2;*/
		#endregion


		Vector3 leftTop = new Vector3 (center.x - gridWidth * 1.5f - intervalx, center.y + gridHeight*2 + intervaly*2, center.z);


		foreach(GameObject building in baseBuilidngs)
		{
			pos = new Vector3(leftTop.x+gridWidth*((i%3)*2+1)/2.0f+intervalx*(i%3), leftTop.y-gridHeight*((i/3)*2+1)/2.0f -(i/3+1)*intervaly, leftTop.z);
			building.transform.position = pos;
			building.GetComponent<Renderer>().enabled = true;

			++i;
		}
	}
	public void UpdateBuilState (GameObject[] buildings)
	{
		float ratio = 0.8f;
		foreach(GameObject building in buildings)
		{
			TowerPool tower;
			tower = PoolManager.poolManager.GetTowerParamFromID(ReadFromExcel.ExtractInt(building.name));
			int cost = tower.towerAttri.cost;
			
			Transform can = building.transform.FindChild("Canvas");
			GameObject tex = can.FindChild("Text").gameObject;
			Text text = tex.transform.GetComponent<Text>();
			text.text = "$ "+cost.ToString();
			//tex.GetComponent<Text>().text = "$" + cost.ToString();
			
			if(ScoreControl.CheckResourceSuffi(cost) == false)
			{
				building.GetComponent<SpriteRenderer>().color = new Color(ratio, ratio, ratio, 1.0f);
				text.color = new Color(ratio, ratio, ratio, 1.0f);
				building.GetComponent<BoxCollider2D>().enabled = false;
			}
			else
			{
				building.GetComponent<BoxCollider2D>().enabled = true;
				text.color = Color.white;
				building.GetComponent<SpriteRenderer>().color = Color.white;
			}
		}
	}
	public Vector3 ClampPosition(Vector3 pos)
	{

		pos = new Vector3(Mathf.Clamp(pos.x, minX, -1*minX), Mathf.Clamp(pos.y, minY, maxY), pos.z);
		return pos;
	}
	public static void ShowIndicator(UnitTower tower){
		gameManager._ShowIndicator(tower);
	}
	
	public void _ShowIndicator(UnitTower tower){

		float range = tower.baseTower.range;
		if(rangeIndicator != null)
		{
			rangeIndicator.transform.position = tower.thisT.position;
			rangeIndicator.transform.localScale =  new Vector3(range*2/rangeIndicatorWidth, range*2/rangeIndicatorHeight*0.7f,1.0f);
			rangeIndicator.GetComponent<Renderer>().enabled = true;
			showtime = 0;
		}
    }

	public static void ClearIndicator(){
		gameManager._ClearIndicator();
	}
	
	public void _ClearIndicator(){
		if (rangeIndicator != null)
			rangeIndicator.GetComponent<Renderer>().enabled = false;
		//if(upgradeUI != null)
		//	upgradeUI.renderer.enabled = false;
    }

	public static void ClearSelection(){
		BuildManager.selectedTower = null;
		BuildManager.selectedFlag = null;
		//gameManager._ClearIndicator();
		gameManager._ClearBaseBuildings ();
		gameManager._ClearUpgradeUI();
		gameManager._ClearDismantleUI ();
    }

    public static void TowerUpgradeComplete(UnitTower tower){
		if(tower==BuildManager.selectedTower){
			gameManager._ShowIndicator(tower);
        }
    }
	public static  void ClearBaseBuildings()
	{
		gameManager._ClearBaseBuildings ();
	}
	public void _ClearBaseBuildings ()
	{
		if(baseBuilidngs.Length>0 && _baseBuildings != null)
		{
			foreach(GameObject building in baseBuilidngs)
			{
				building.GetComponent<Renderer>().enabled = false;
				building.GetComponent<BoxCollider2D>().enabled = false;
				Transform can = building.transform.FindChild("Canvas");
				GameObject tex = can.FindChild("Text").gameObject;
				Text text = tex.transform.GetComponent<Text>();
				text.text = "";
				text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
			}
		}
	}
	public static void ClearUpgradeUI()
	{
		gameManager._ClearUpgradeUI ();
	}
	public void _ClearUpgradeUI ()
	{
		Transform child;
		if(upgradeUI != null)
		{
			upgradeUI.GetComponent<Renderer>().enabled = false;
			upgradeUI.GetComponent<BoxCollider2D>().enabled = false;
			if((child = upgradeUI.transform.FindChild("mouseOver")) != null)
				child.GetComponent<Renderer>().enabled = false;
			Transform can = upgradeUI.transform.FindChild("Canvas");
			GameObject tex = can.FindChild("Text").gameObject;
			Text text = tex.transform.GetComponent<Text>();
			text.text = "";
			//text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
		}


	}
	public static void ClearDismantleUI()
	{
		gameManager._ClearDismantleUI ();
	}
	public void _ClearDismantleUI ()
	{
		Transform child;
		if(dismantleUI != null)
		{
			dismantleUI.GetComponent<Renderer>().enabled = false;
			dismantleUI.GetComponent<BoxCollider2D>().enabled = false;
			if((child = dismantleUI.transform.FindChild("mouseOver")) != null)
				child.GetComponent<Renderer>().enabled = false;
			Transform can = dismantleUI.transform.FindChild("Canvas");
			GameObject tex = can.FindChild("Text").gameObject;
			Text text = tex.transform.GetComponent<Text>();
			text.text = "";
			//text.color = new Color(text.color.r, text.color.g, text.color.b, 0.0f);
		}
		
		
	}
	public static void ClearInterfaceSet()
	{
		gameManager._ClearInterfaceSet();
	}
	public void _ClearInterfaceSet()
	{
		//Transform child;
		if(interfaceSet != null)
		{
			interfaceSet.GetComponent<Renderer>().enabled = false;
			BoxCollider2D[] boxColliders;
			boxColliders = interfaceSet.GetComponentsInChildren<BoxCollider2D>();
			foreach(BoxCollider2D colli in boxColliders)
			{
				colli.enabled = false;
			}
			Renderer[] renders;
			renders = interfaceSet.GetComponentsInChildren<Renderer>();
			foreach(Renderer render in renders)
				render.enabled = false;

			/*
			interfaceSet.transform.FindChild("SetLevSel").renderer.enabled = false;
			interfaceSet.transform.FindChild("SetBack").renderer.enabled = false;
			interfaceSet.transform.FindChild("Mask").renderer.enabled = false;
			interfaceSet.transform.FindChild("SetHelp").renderer.enabled = false;
			interfaceSet.transform.FindChild("SetMusic").renderer.enabled = false;
			interfaceSet.transform.FindChild("SetOff").renderer.enabled = false;
			interfaceSet.transform.FindChild("SetReplay").renderer.enabled = false;

			//interfaceSet.renderer.enabled = false;
			*/
		}
	}
	public static void ShowInterfaceSet()
	{
		gameManager._ShowInterfaceSet();
	}
	public void _ShowInterfaceSet()
	{
		//Transform child;
		if(interfaceSet != null)
		{
			interfaceSet.GetComponent<Renderer>().enabled = true;
			BoxCollider2D[] boxColliders;
			boxColliders = interfaceSet.GetComponentsInChildren<BoxCollider2D>();
			foreach(BoxCollider2D colli in boxColliders)
			{
				colli.enabled = true;
			}
			Renderer[] renders;
			renders = interfaceSet.GetComponentsInChildren<Renderer>();
			foreach(Renderer render in renders)
				render.enabled = true;
			/*
			//interfaceSet.transform.position = new Vector3(0, 0, 0);
			interfaceSet.transform.FindChild("SetLevSel").renderer.enabled = true;
			interfaceSet.transform.FindChild("SetBack").renderer.enabled = true;
			interfaceSet.transform.FindChild("Mask").renderer.enabled = true;
			interfaceSet.transform.FindChild("SetHelp").renderer.enabled = true;
			interfaceSet.transform.FindChild("SetMusic").renderer.enabled = true;
			interfaceSet.transform.FindChild("SetOff").renderer.enabled = true;
			interfaceSet.transform.FindChild("SetReplay").renderer.enabled = true;
			//interfaceSet.renderer.enabled = true;
			*/
		}
	}
	/*
	public static void GainResource(int val){
		//Debug.Log("gain");
		ResourceManager.GainResource(0, val);
		if(ResourceE!=null) ResourceE();
	}
	
	public static void GainResource(int id, int val){
		//Debug.Log("gain");
		ResourceManager.GainResource(id, val);
		if(ResourceE!=null) ResourceE();
	}
	
	public static void GainResource(int[] val){
		//Debug.Log("gain");
		ResourceManager.GainResource(val);
		if(ResourceE!=null) ResourceE();
	}
	
	public static void SpendResource(int[] val){
		ResourceManager.SpendResource(val);
		if(ResourceE!=null) ResourceE();
	}
	
	public static int GetResourceVal(){
		return ResourceManager.GetResourceVal(0);
	}
	
	public static int GetResourceVal(int id){
		return ResourceManager.GetResourceVal(id);
	}
	
	public static int[] GetAllResourceVal(){
		Resource[] rscList=ResourceManager.GetResourceList();
		
		int[] valList=new int[rscList.Length];
		for(int i=0; i<valList.Length; i++){
			valList[i]=rscList[i].value;
		}
		
		return valList;
	}
	
	public static bool HaveSufficientResource(int[] cost){
		return ResourceManager.HaveSufficientResource(cost);
    }
    
	public static Resource[] GetResourceList(){
        return ResourceManager.GetResourceList();
    }*/
    
   /* static public float GetSellTowerRefundRatio(){
        return gameManager.sellTowerRefundRatio;
    }*/
    bool isOnce = true;
	// Update is called once per frame
	float countEnd = 0.0f;
	void Update () {
		if(Time.time-startTime > 20)
		{
			Collider2D[] colliders = Physics2D.OverlapAreaAll(pos1, pos2,LayerMask.GetMask("Visitor"));
			if(colliders.Length == 0)
			{
				Animator ani = scoreUI.GetComponent<Animator>();

				StartCoroutine(SystemWait(1.0f));
				if(ani.GetBool("isDisappear") == true)
				{
					countEnd += Time.deltaTime;
					if( isOnce)
					{
						PrePassDisplay();
					}
					if(countEnd >8.0f)
						Time.timeScale = 0.0f;
				}
				ani.SetBool("isDisappear", true);
				ani.SetBool("isShowup", false);
				//Application.LoadLevel("Yotta_Start");
			}
		}
		if(rangeIndicator.GetComponent<Renderer>().isVisible == true)
		{
			showtime += Time.deltaTime;
			if(showtime > 1.0f)
				ClearIndicator();

		}
		//if(Input.GetKeyDown(KeyCode.A))
		//	Application.LoadLevel("Yotta_Start");
		if (Input.GetKey ("escape")) 
		{
			Application.Quit();
		}
		else if(Input.GetKey("a"))
			Application.LoadLevel("Yotta_Start0");
		else if(Input.GetKey("b"))
			Application.LoadLevel("Yotta_LevelSelect");
		else if(Input.GetKey("c"))
			Application.LoadLevel("Yotta_Level1");
		else if(Input.GetKey("d"))
			Application.LoadLevel("Yotta_Level2");
	}
	IEnumerator SystemWait(float seconds)
	{
		yield return new WaitForSeconds (seconds);
	}
	public void GameOverDisplay()
	{
		Time.timeScale = 0.0f;
		GameObject overUI= (GameObject)Instantiate (gameoverUI, this.transform.position, Quaternion.identity);
		AudioManager.StopSound(AudioManager.audioManager.backSound);
		AudioManager.PlayGameLostSound ();
	}
	public void PrePassDisplay()
	{
		GameObject passUI = (GameObject)Instantiate(passportUI, this.transform.position, Quaternion.identity);
		GradeLevel grade = GradeJudge(ScoreControl.angryParam,ScoreControl.essenceParam);
		AudioManager.StopSound(AudioManager.audioManager.backSound);
		AudioManager.PlayGameWonSound ();
		ShowGrade(passUI,grade);
		isOnce = false;
    }
    public GradeLevel GradeJudge (int angry, int essence)
	{
		int num = GameObject.Find ("SpawnPoint").GetComponent<SpawnManager> ().allnum;
		if(angry ==0 && essence >= num*0.7f)
			return GradeLevel.em_S;
		else if(angry <=3 && essence >= num*0.3f)
			return GradeLevel.em_A;
		else if(angry <=7 && essence >= num*0.1f)
			return GradeLevel.em_B;
		else if(angry <=9 && essence >= 0)
			return GradeLevel.em_C;
		else
			return GradeLevel.em_null;


	}
	public void ShowGrade(GameObject passUI, GradeLevel grade)
	{
		Vector3 pos;
		switch(grade)
		{
		case GradeLevel.em_S:
			pos = passUI.transform.FindChild("Star1").transform.position;
			StartCoroutine(GenerateStars(pos,1.0f));
			pos = passUI.transform.FindChild("Star2").transform.position;
			StartCoroutine(GenerateStars(pos,2.0f));
			pos = passUI.transform.FindChild("Star3").transform.position;
			StartCoroutine(GenerateStars(pos,3.0f));
			break;
		case GradeLevel.em_A:
			pos = passUI.transform.FindChild("Star1").transform.position;
			StartCoroutine(GenerateStars(pos,1.0f));
			pos = passUI.transform.FindChild("Star2").transform.position;
			StartCoroutine(GenerateStars(pos,2.0f));
			break;
		case GradeLevel.em_B:
			pos = passUI.transform.FindChild("Star1").transform.position;
			StartCoroutine(GenerateStars(pos,1.0f));
			break;
		case GradeLevel.em_C:
			break;
		default:
			break;
		}
	}
	IEnumerator GenerateStars(Vector3 pos, float duration)
	{
		yield return new WaitForSeconds(duration);
		AudioManager.PlaySound(AudioManager.audioManager.starShowSound,false);
		Instantiate(star, pos, Quaternion.identity);


	}
}
