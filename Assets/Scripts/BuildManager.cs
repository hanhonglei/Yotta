using UnityEngine;
using System.Collections;

[System.Serializable]
public class BuildableInfo
{
	public bool buildable = false;
	public Vector3 position = Vector3.zero;
	public TowerType[] buildableType = null;
	public void BuildSpotInto(){}

	public void BuildSpotInto(Vector3 pos)
	{
		position = pos;
	}
	public void BuildSpotInto(Vector3 pos, TowerType[] bT )
	{
		position = pos;
		buildableType = bT;
	}
}

public class BuildManager : MonoBehaviour {

	public GameObject[] buildingPoints;
	public UnitTower[] towers;
	static private BuildableInfo currentBuildInfo;
	static private int towerCount = 0;

	public static BuildManager buildManager;
	public static GameObject selected;
	
	public static int GetTowerCount()
	{
		return towerCount;
	}
	void Awake()
	{
		buildManager = this;
		/*foreach (UnitTower tower in towers)
		{
			tower.thisObj = tower.gameObject;
			tower.thisT = tower.transform;
		}*/
		towerCount = 0;


	}
	public void BuildingFlags()
	{
		foreach(GameObject point in buildingPoints)
		{
			GameObject obj = PoolManager.Spawn(GameManager.gameManager.buildingFlag, point.transform.position, Quaternion.identity);
			obj.GetComponent<SpriteRenderer>().sortingLayerName = point.GetComponent<SpriteRenderer>().sortingLayerName;
		}
	}
	public static void ClearBuildPoint() //
	{
		currentBuildInfo = null;
	}
	static public bool CheckBuildPoint(Vector3 pointer)
	{
		BuildableInfo buildableInfo = new BuildableInfo ();
		return false;
		//LayerMask mask = 1 << LayerManager.LayerPlatform ();
	}

	private UnitTower[] sampleTower;
	private int currentSampleID = -1;
	public static void InitiateSampleTower()
	{
		buildManager.sampleTower = new UnitTower[buildManager.towers.Length];
		for(int i=0; i<buildManager.towers.Length; ++i)
		{
			GameObject towerObj = (GameObject)Instantiate(buildManager.towers[i].gameObject);
			buildManager.sampleTower[i] = towerObj.GetComponent<UnitTower>();
			//towerObj.SetActiveRecursively(false);
			//UnitUtility.SetAdditivematColorRecursively(towerObj.transform, Color.green);

		}
	}
	public static void ShowSampleTower(int ID)
	{
		buildManager._ShowSampleTower (ID);
	}
	public void _ShowSampleTower(int ID)
	{
		if (currentSampleID == ID || currentBuildInfo == null)
			return;
		if(currentSampleID>0)
		{
			ClearSampleTower();
		}
		currentSampleID = ID;
		sampleTower [ID].thisT.position = currentBuildInfo.position;
		//sampleTower [ID].thisObj.SetActiveRecursively (true);
		//GameManager.ShowIndicator (sampleTower [ID]);
	}
	static public void ClearSampleTower()
	{
		buildManager._ClearSampleTower ();
	}
	public void _ClearSampleTower()
	{
		if (currentSampleID < 0)
			return;
		//sampleTower[currentSampleID].thisObj.SetActiveRecursively (false);
		//GameManager.ClearIndicator ();
		currentSampleID = -1;
	}
	static public BuildableInfo GetBuildInfo()
	{
		return currentBuildInfo;
	}
	static public UnitTower[] GetTowerList()
	{
		return buildManager.towers;
	}
	void Start(){
		PoolManager.New (GameManager.gameManager.buildingFlag, buildingPoints.Length);
		for(int i = 0; i<towers.Length; ++i)
		{
			towers[i].GetComponent<UnitTower>().baseTower = PoolManager.poolManager.towerPool[i].towerAttri;
		}
		foreach(UnitTower tower in towers)
		{
			PoolManager.New(tower.gameObject,1);
		}
		BuildingFlags ();
	}
	
	// Update is called once per frame
	void Update () {

	}	


	public static UnitTower selectedTower;
	public static GameObject selectedFlag;
	public static GameObject selectedIcon;
	
	public static void Select(UnitTower tower){  //选中当前的塔
		buildManager._Select (tower);
	}
	public UnitTower _Select(UnitTower tower)
	{
		selectedTower = tower;
		GameManager.ShowIndicator(selectedTower);
		GameManager.ShowUpgrade (selectedTower);
		GameManager.ShowDismantle (selectedTower);
		//gameManager._ShowUpgrades (selectedTower);
		return selectedTower;
	}
	public static void Select(Transform flag)  //选中当前的buildingflag，在当前位置上建造初始建筑物
	{
		buildManager._Select (flag);
	}
	public GameObject _Select(Transform flag)
	{
		selectedFlag = flag.gameObject;
		AudioManager.PlaySound(AudioManager.audioManager.basetowerShowSound,false);
		GameManager.ShowBaseBuilding (selectedFlag.transform);
		return selectedFlag;
	}
	public static void Select(GameObject icon) //选中的是图标 
	{
		buildManager._Select (icon);
	}
	public GameObject _Select(GameObject icon)
	{
		int id = ReadFromExcel.ExtractInt (icon.name);
		foreach(UnitTower tower in towers)
		{
			if(ReadFromExcel.ExtractInt(tower.name) == id)
			{
				//Vector3 pos = new Vector3 (0, 0, 0);
				GameObject obj;
				obj = PoolManager.Spawn (tower.gameObject, selectedFlag.transform.position, Quaternion.identity);
				ScoreControl.CoinDecrease(tower.baseTower.cost);
				obj.GetComponent<SpriteRenderer>().sortingLayerName = selectedFlag.GetComponent<SpriteRenderer>().sortingLayerName;
				//GameManager.ShowIndicator(obj.GetComponent<UnitTower>());
				//GameManager.ClearBaseBuildings();
				//buildManager.ClearBuildPoint();
				AudioManager.PlayTowerBuilding();
				PoolManager.Unspawn(selectedFlag);
				selectedFlag = null;
				break;
			}
		}

		//PoolManager.Spawn(
		selectedIcon = icon;

		return selectedIcon;
	}
	public static void SelectUpgradeUI()
	{
		buildManager._SelectUpgradeUI ();
	}

	public void _SelectUpgradeUI()
	{
		int id = selectedTower.GetTowerID();
		UnitTower upgradeTower;
		upgradeTower = FindUpgrade (id);
		if(upgradeTower != null)
		{
			GameObject obj;
			PoolManager.Unspawn (selectedTower.transform);
			GameManager.ClearIndicator();
			obj = PoolManager.Spawn (upgradeTower.transform,selectedTower.transform.position, Quaternion.identity);
			ScoreControl.CoinDecrease(selectedTower.baseTower.upgradeCost);
			obj.GetComponent<SpriteRenderer>().sortingLayerName = selectedTower.GetComponent<SpriteRenderer>().sortingLayerName;
			AudioManager.PlayTowerBuilding();
			GameManager.ClearUpgradeUI();
			GameManager.ClearDismantleUI ();
			selectedTower = null;
		}
		if(upgradeTower == null)
		{
			GameManager.ClearUpgradeUI();
			GameManager.ClearDismantleUI ();
			selectedTower = null;
		}

	}
	public static UnitTower FindUpgrade(int id)
	{
		return buildManager._FindUpgrade (id);
	}
	public UnitTower _FindUpgrade(int id)
	{
		int upid = id + 1;
		foreach(UnitTower tower in towers)
		{
			if(upid == ReadFromExcel.ExtractInt(tower.name))
			{
				return tower;
			}
		}
		return null;
	}
	public static void SelectDismantleUI()
	{
		buildManager._SelectDismantleUI ();
	}
	public void _SelectDismantleUI()
	{
		GameObject obj;
		PoolManager.Unspawn (selectedTower.transform);
		AudioManager.PlayTowerSold();
		GameManager.ClearIndicator();
		ScoreControl.CoinIncrease (selectedTower.baseTower.price);

		obj = PoolManager.Spawn (GameManager.gameManager.buildingFlag,selectedTower.transform.position, Quaternion.identity);
		obj.GetComponent<SpriteRenderer>().sortingLayerName = selectedTower.GetComponent<SpriteRenderer>().sortingLayerName;

		//GameManager.ClearUpgradeUI();
		//GameManager.ClearDismantleUI ();
		//selectedTower = null;
		GameManager.ClearSelection ();
	}

}