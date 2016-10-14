using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class Pool
{
	public int ID = -1;
	
	private GameObject prefab;
	private int totalObjCount;
	
	private List<GameObject> available = new List<GameObject> ();
	private List<GameObject> allObject = new List<GameObject> ();
	
	public Pool() {}
	
	public Pool(GameObject obj, int num, int id)
	{
		prefab = obj;
		ID = id;
		PrePopulate (num); //
	}
	public void MatchPopulation(int num) //这里是说要达到num的数量，需要增加多少
	{
		PrePopulate (num - totalObjCount);
	}
	public void PrePopulate(int num) //预生成一些物体，没有具体类型，都是GameObject，并且其没有类别之分
	{
		for(int i=0; i<num; ++i)
		{
			GameObject obj = (GameObject) GameObject.Instantiate(prefab);
			obj.AddComponent<ObjectID>().SetID(ID);
			available.Add(obj);
			allObject.Add(obj);
			
			totalObjCount += 1;
			
			obj.SetActive( false);
		}
	}
	public GameObject Spawn(GameObject obj, Vector3 pos, Quaternion rot)
	{
		GameObject spawnObj;
		
		if(available.Count >0)  //若available中还有备用的已经实例化的，就直接拷贝给spawnObj
		{
			spawnObj = available[0];
			available.RemoveAt(0);
			
			spawnObj.SetActive(true);
			
			Transform tempT = spawnObj.transform;
			
			tempT.position = pos;
			tempT.rotation = rot;
		}
		else                     //若没有，则实例化一个
		{
			spawnObj = (GameObject)GameObject.Instantiate(prefab, pos, rot);
			spawnObj.AddComponent<ObjectID>().SetID(ID);
			totalObjCount += 1;
		}
		return spawnObj;
	}
	public void Unspawn(GameObject obj) //而不生成的时候会将该物体加入available中
	{
		available.Add (obj);
		
		obj.SetActive(false);
	}
	public void UnspawnAll()			//这个就搞不懂了，所谓“都不生成”，是将allObject中所有非空的对象删除
	{
		foreach(GameObject obj in allObject)
			if (obj != null)
				GameObject.Destroy(obj);
	}
	public void HideInHerarchy(Transform t) //层级隐藏
	{
		foreach(GameObject obj in allObject)
			obj.transform.parent = t;
	}
	public GameObject GetPrefab()
	{
		return prefab;
	}
	public int GetTotalCount()
	{
		return totalObjCount;
	}
	public List<GameObject> GetFullList()
	{
		return allObject;
	}
}
[System.Serializable]
public class ObjectID: MonoBehaviour
{
	private int ID = -1;
	public void SetID(int id)
	{
		ID = id;
	}
	public int GetID()
	{
		return ID;
	}
}

[System.Serializable]
public class VisitorPool
{
	//public UnitVisitor unitVisitor;
	public string name;
	public float fullHp;
	public VisitorAttribute visitorAttri;
	public Transform gameObject;

};
public enum VisitorLineInExcel
{
	em_KidLine			=5,
	em_MaleLine			=9,
	em_FemaleLine		=13,
	em_SeniumLine		=17,
	em_AlienLine		=21,
};
[System.Serializable]
public class TowerPool
{
	//public UnitVisitor unitVisitor;
	public string name;
	public int ID;
	public TowerAttribute towerAttri;
	public Transform gameObject;
};
public enum TowerLineInExcel
{
	em_ScenicSpotLine 		= 3,
	em_CateringLine 		= 7,
	em_AccommodationLine 	= 10,
	em_FilmLine 			= 13,
	em_ShoppingLine 		= 16,
	em_AmusementLine 		= 20,
};
public class PoolManager : MonoBehaviour {


	private static List<Pool> poolList = new List<Pool>(); //每个物体对应pool中的一项，每一项都有一个available以及一个allObject的list
	private static bool cumulative = true; //累积的
	
	public static PoolManager poolManager;
	public static Transform thisT;
	
	public static int currentIDCount = 0;
	
	public static void SetCumulativeFlag(bool flag)
	{
		cumulative = flag;
	}
	public static bool GetCumulativeFlag()
	{
		return cumulative;
	}
	public static void New(GameObject obj, int num)
	{
		New (obj, num, cumulative);
	}
	public static void New(Transform obj, int num)
	{
		New (obj.gameObject, num, cumulative);
	}
	public static void New(Transform obj, int num, bool flag)
	{
		New (obj.gameObject, num, flag);
	}
	
	//The flag is to indicate whether the number should be covered or added
	public static void New(GameObject obj, int num, bool flag) //创建带有flag标记的obj物体num个
	{
		//check if the object is existed in the list
		int objExist = CheckIfObjectExist (obj);
		
		if(objExist>=0)
		{
			//if obj existed, use the existing pool
			if(flag)
				poolList[objExist].PrePopulate(num); //added---若当前类型的物体已经存在，就在对应的队列中增加gameobject
			else
				poolList[objExist].MatchPopulation(num); //covered
			
			poolList[objExist].HideInHerarchy(thisT);
		}
		else 
		{
			//if obj doesnot exit, create an empty slot in the poolList and
			//insert the newly created pool
			//this is to prevent other object being registered into the same
			//ID if hte other object is created while instiating this object
			
			//increase ID, since this particular ID will be assigned to this pool
			currentIDCount +=1;				//如果该物体类型不在poolList中，就在poolList中新增加一个项目
			
			//create a dummy pool to occupy the list
			poolList.Add(new Pool()); //这里是先增加一项，然后再新建一项，占用这个位置，而所谓的new Pool()会被回收机制回收
			
			//create a new pool and add to the list and slot it into the 
			//appropriate slot of the poolList
			Pool newPool = new Pool(obj, num, currentIDCount-1);
			poolList[newPool.ID] = newPool;
			
			poolList[newPool.ID].HideInHerarchy(thisT);
		}
	}
	//check to see if the new object exist
	private static int CheckIfObjectExist(GameObject obj)
	{
		int match = 0;
		foreach(Pool pool in poolList)
		{
			if(obj == pool.GetPrefab())
				return match;
			match += 1;
		}
		return -1;
	}
	//check if an object is tagged with an id
	private static int CheckIfObjectIsTagged(GameObject obj) //检查该物体是否被标记上了id
	{
		ObjectID objectID = obj.GetComponent<ObjectID>();
		if(objectID == null)
			return -1;
		else
			return objectID.GetID ();
	}
	public static GameObject Spawn(GameObject obj)
	{
		return Spawn (obj, Vector3.zero, Quaternion.identity);
	}
	public static GameObject Spawn(Transform obj)
	{
		return Spawn (obj.gameObject, Vector3.zero, Quaternion.identity);
	}
	public static GameObject Spawn (Transform obj, Vector3 pos, Quaternion rot)
	{
		return Spawn (obj.gameObject, pos, rot);
	}
	public static GameObject Spawn(GameObject obj, Vector3 pos, Quaternion rot)
	{
		GameObject spawnObj;
		
		int ID = CheckIfObjectExist (obj);
		
		if(ID == -1)
		{
			return (GameObject)Instantiate (obj, pos, rot);
		}
		else
			spawnObj = poolList[ID].Spawn(obj, pos, rot);
		spawnObj.transform.parent = null;
		
		return spawnObj;
	}
	public static void Unspawn(Transform obj)
	{
		Unspawn (obj.gameObject);
	}
	public static void Unspawn(GameObject obj)
	{
		int ID = CheckIfObjectIsTagged (obj);
		
		obj.transform.parent = thisT;
		if(ID == -1)
		{
			obj.SetActive( false);
		}
		else
			poolList[ID].Unspawn (obj);  //how could this happened????? I can't understand!!!  CCat
	}
	//delayed unspawn
	public static void Unspawn(Transform obj, float duration)
	{
		poolManager.StartCoroutine (poolManager.UnspawnTimed (obj.gameObject, duration));
		
	}
	public static void Unspawn(GameObject obj, float duration)
	{
		poolManager.StartCoroutine (poolManager.UnspawnTimed (obj, duration));
	}
	private IEnumerator UnspawnTimed(GameObject obj, float duration)
	{
		yield return new WaitForSeconds (duration);
		Unspawn (obj);
	}
	public static void Init()
	{
		ClearAll ();


		currentIDCount = 0;


		//no meed for this, cause the poolmanager is added to the gameManager object
	/*	GameObject obj = new GameObject ();
		obj.name = "PoolManager";
		
		poolManager = obj.AddComponent<PoolManager> ();
		thisT = obj.transform;*/
	}
	
	public static void ClearAll()
	{
		foreach(Pool pool in poolList)
		{
			pool.UnspawnAll();
		}
		poolList = new List<Pool> ();
	}
	public static List<GameObject> GetList(GameObject obj)
	{
		int ID = CheckIfObjectExist (obj);
		
		if(ID>=0)
			return poolList[ID].GetFullList();
		else
			return new List<GameObject>();
	}


	//
	//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
	//The following is the process of param reading from the filess
	//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
	//

	//public Transform[] visitorObjs;
	public VisitorPool[] visitorPool;  //have problems
	public TowerPool[] towerPool;
	public static float[,] prefTable = new float[20,21];
	public static int[,] effectTable = new int[20, 21];
	public Transform[] wayPoints;
	//static PoolManager poolManager;
	// Use this for initialization
	//to restore the baselist
	public static string menuPool = null ;
	//public static GameObject[] visitorPool;
	//public static GameObject[] towerPool;



	private List<GameObject> available = new List<GameObject> ();
	private List<GameObject> allObject = new List<GameObject> ();


	/*
	void Start () {
		InitParamPool();
		//InitTower();
	}
	*/
	void Awake()
	{
		poolManager = this;
		InitParamPool ();
    }
	void Start()
	{
		//InitParamPool();
	}
	private void InitParamPool()
	{
		ReadFromExcel.readXLS(Application.dataPath+"/YOTTA_param_V1.2.xls","Visitor");
		for(int i=0; i<visitorPool.Length; ++i)
		{
			//visitorList[i].gameObject = visitorObjs[i];
			visitorPool[i].visitorAttri.visitorID =  ReadFromExcel.ExtractInt(visitorPool[i].gameObject.name);
			InitVisitor(visitorPool[i], visitorPool[i].visitorAttri.visitorID);
		}
		ReadFromExcel.readXLS(Application.dataPath+"/YOTTA_param_V1.2.xls","Tower's ATK");
		InitPrefTable();
		ReadFromExcel.readXLS(Application.dataPath+"/YOTTA_param_V1.2.xls","VisitorEffect");
		InitEffectTable ();
		ReadFromExcel.readXLS(Application.dataPath+"/YOTTA_param_V1.2.xls","Tower");
		for(int i=0; i<towerPool.Length; ++i)
		{
			//visitorList[i].gameObject = visitorObjs[i];
			towerPool[i].ID =  ReadFromExcel.ExtractInt(towerPool[i].gameObject.name);
			InitTower(towerPool[i], towerPool[i].ID);
		}
	}
	public void InitVisitor(VisitorPool visitorPool, int ID)
	{
		int row,col;
		row = col = 0;
		
		visitorPool.visitorAttri.visitorType = (VisitorType)(ID/100);
		visitorPool.visitorAttri.visitorLevel = (VisitorLevel)(ID%100);
		switch(visitorPool.visitorAttri.visitorType)
		{
		case VisitorType.em_Kid:
			row = (int)VisitorLineInExcel.em_KidLine;
			break;
		case VisitorType.em_Male:
			row = (int)VisitorLineInExcel.em_MaleLine;
			break;
		case VisitorType.em_Female:
			row = (int)VisitorLineInExcel.em_FemaleLine;
			break;
		case VisitorType.em_Senium:
			row = (int)VisitorLineInExcel.em_SeniumLine;
			break;
		case VisitorType.em_Alien:
			row = (int)VisitorLineInExcel.em_AlienLine;
			break;
		}
		row = row +(int)visitorPool.visitorAttri.visitorLevel -1;
		
		col = 2;
		visitorPool.name = ReadFromExcel.getStringFromRowColm(row,col);
		col = 5;
		visitorPool.visitorAttri.description = ReadFromExcel.getStringFromRowColm(row, col++);
		visitorPool.visitorAttri.bgDescription = ReadFromExcel.getStringFromRowColm(row, col++);
		visitorPool.visitorAttri.initHp = ReadFromExcel.getFloatFromRowColm(row, col++);
		visitorPool.fullHp = ReadFromExcel.getFloatFromRowColm(row, col++);
		col = 10;
		visitorPool.visitorAttri.normalSpeed = ReadFromExcel.getFloatFromRowColm(row, col++);
		visitorPool.visitorAttri.capacity = ReadFromExcel.getFloatFromRowColm(row, col++);
		visitorPool.visitorAttri.damage = ReadFromExcel.getFloatFromRowColm(row, col++);
		col++;
		visitorPool.visitorAttri.decrement = ReadFromExcel.getFloatFromRowColm(row, col++);
		visitorPool.visitorAttri.defence = ReadFromExcel.getIntFromRowColm(row, col++);
		visitorPool.visitorAttri.moveReduce = ReadFromExcel.getFloatFromRowColm(row, col++);
		visitorPool.visitorAttri.angryLimit = ReadFromExcel.getFloatFromRowColm(row, col++);
		visitorPool.visitorAttri.angryMax = ReadFromExcel.getFloatFromRowColm(row, col++);
		visitorPool.visitorAttri.speedUpRatio = ReadFromExcel.getFloatFromRowColm(row, col++);
		visitorPool.visitorAttri.happyLimit = ReadFromExcel.getIntFromRowColm(row, col++);
		visitorPool.visitorAttri.essencePoints = ReadFromExcel.getIntFromRowColm(row, col++);

		visitorPool.visitorAttri.angrySpeed = visitorPool.visitorAttri.normalSpeed * visitorPool.visitorAttri.speedUpRatio;
	}
	public void InitTower(TowerPool towerPool, int ID)
	{
		int row,col;
		row = col = 0;
		
		towerPool.towerAttri.towerType = (TowerType)(ID/100);
		towerPool.towerAttri.towerLevel = (TowerLevel)(ID%100);
		
		switch(towerPool.towerAttri.towerType)
		{
		case TowerType.em_ScenicSpot:
			row = (int)(TowerLineInExcel.em_ScenicSpotLine);
			break;
		case TowerType.em_Catering:
			row = (int)TowerLineInExcel.em_CateringLine;
			break;
		case TowerType.em_Accommodation:
			row = (int)TowerLineInExcel.em_AccommodationLine;
			break;
		case TowerType.em_Film:	
			row = (int)TowerLineInExcel.em_FilmLine;
			break;
		case TowerType.em_Shopping:
			row = (int)TowerLineInExcel.em_ShoppingLine;
			break;
		case TowerType.em_Amusement:
			row = (int)TowerLineInExcel.em_AmusementLine;
			break;
		}
		row = row + (int)towerPool.towerAttri.towerLevel-1;
		
		col = 2;
		towerPool.name = ReadFromExcel.getStringFromRowColm(row,col);
		col = 5;
		towerPool.towerAttri.description = ReadFromExcel.getStringFromRowColm(row,col++);
		towerPool.towerAttri.cost = ReadFromExcel.getIntFromRowColm(row,col++);
		
		//col++;
		towerPool.towerAttri.upgradeCost = ReadFromExcel.getIntFromRowColm(row, col++);
		towerPool.towerAttri.price = ReadFromExcel.getIntFromRowColm(row, col++);
		towerPool.towerAttri.fixCost = ReadFromExcel.getIntFromRowColm(row, col++);
		towerPool.towerAttri.durability = ReadFromExcel.getIntFromRowColm(row, col++);
		towerPool.towerAttri.minRange = ReadFromExcel.getFloatFromRowColm(row, col++);
		towerPool.towerAttri.maxRange = ReadFromExcel.getFloatFromRowColm(row, col++);
		col++;
		towerPool.towerAttri.attackInterval = ReadFromExcel.getFloatFromRowColm(row, col++);
		towerPool.towerAttri.range = towerPool.towerAttri.maxRange;
		//towerPool.towerAttri.influProbability = ReadFromExcel.getFloatFromRowColm(row,col++);
	}

	// Update is called once per frame
	void Update () {
	}
	public VisitorPool GetVisitorParamFromID(int ID)
	{
		for(int i=0; i<visitorPool.Length; ++i)
			if(visitorPool[i].visitorAttri.visitorID == ID)
				return visitorPool[i];
		return visitorPool[0];
	}
	public TowerPool GetTowerParamFromID(int ID)
	{
		for(int i=0; i<towerPool.Length; ++i)
			if(towerPool[i].ID == ID)
				return towerPool[i];
		return towerPool[0];
	}

	public static void InitPrefTable()
	{
		//readXLS(Application.dataPath + "/Yotta_param.xls","Visitor");
		for(int i=0; i< 20; ++i)
		{
			for(int j=0; j<21; ++j)
			{
				prefTable[i,j] = ReadFromExcel.getFloatFromRowColm(i+5,j+6);
			}
		}
	}
	public static void InitEffectTable()
	{
		for(int i=0; i<20; ++i)
		{
			for(int j=0; j<21; ++j)
			{
				effectTable[i,j] = ReadFromExcel.getIntFromRowColm(j+6, i+5);
			}
		}
	}
	public static float GetPref(int towerID, int visitorID)
	{
		int row, col;
		row = col = 0;
		VisitorType visitorType = (VisitorType)(visitorID/100);
		TowerType towerType = (TowerType)(towerID/100);
		switch(visitorType)
		{
		case VisitorType.em_Kid:
			row = (int)VisitorLineInExcel.em_KidLine;
			break;
		case VisitorType.em_Male:
			row = (int)VisitorLineInExcel.em_MaleLine;
			break;
		case VisitorType.em_Female:
			row = (int)VisitorLineInExcel.em_FemaleLine;
			break;
		case VisitorType.em_Senium:
			row = (int)VisitorLineInExcel.em_SeniumLine;
			break;
		case VisitorType.em_Alien:
			row = (int)VisitorLineInExcel.em_AlienLine;
			break;
		}
		row = row + visitorID%100-1-(int)VisitorLineInExcel.em_KidLine;

		switch(towerType)
		{
		case TowerType.em_ScenicSpot:
			col = (int)(TowerLineInExcel.em_ScenicSpotLine);
			break;
		case TowerType.em_Catering:
			col = (int)TowerLineInExcel.em_CateringLine;
			break;
		case TowerType.em_Accommodation:
			col = (int)TowerLineInExcel.em_AccommodationLine;
			break;
		case TowerType.em_Film:	
			col = (int)TowerLineInExcel.em_FilmLine;
			break;
		case TowerType.em_Shopping:
			col = (int)TowerLineInExcel.em_ShoppingLine;
			break;
		case TowerType.em_Amusement:
			col = (int)TowerLineInExcel.em_AmusementLine;
			break;
		}
		col = col + (int)towerID%100-(int)(TowerLineInExcel.em_ScenicSpotLine)-1;
		return prefTable[row,col];
	}
	public static int GetEffect(int towerID, int visitorID)  ///重复了啊啊啊啊啊！！need to recheck!!
	{
		int row, col;
		row = col = 0;
		VisitorType visitorType = (VisitorType)(visitorID/100);
		TowerType towerType = (TowerType)(towerID/100);
		switch(visitorType)
		{
		case VisitorType.em_Kid:
			row = (int)VisitorLineInExcel.em_KidLine;
			break;
		case VisitorType.em_Male:
			row = (int)VisitorLineInExcel.em_MaleLine;
			break;
		case VisitorType.em_Female:
			row = (int)VisitorLineInExcel.em_FemaleLine;
			break;
		case VisitorType.em_Senium:
			row = (int)VisitorLineInExcel.em_SeniumLine;
			break;
		case VisitorType.em_Alien:
			row = (int)VisitorLineInExcel.em_AlienLine;
			break;
		}
		row = row + visitorID%100-1-(int)VisitorLineInExcel.em_KidLine;
		
		switch(towerType)
		{
		case TowerType.em_ScenicSpot:
			col = (int)(TowerLineInExcel.em_ScenicSpotLine);
			break;
		case TowerType.em_Catering:
			col = (int)TowerLineInExcel.em_CateringLine;
			break;
		case TowerType.em_Accommodation:
			col = (int)TowerLineInExcel.em_AccommodationLine;
			break;
		case TowerType.em_Film:	
			col = (int)TowerLineInExcel.em_FilmLine;
			break;
		case TowerType.em_Shopping:
			col = (int)TowerLineInExcel.em_ShoppingLine;
			break;
		case TowerType.em_Amusement:
			col = (int)TowerLineInExcel.em_AmusementLine;
			break;
		}
		col = col + (int)towerID%100-(int)(TowerLineInExcel.em_ScenicSpotLine)-1;
		return effectTable[row,col];
	}
	/*public static Transform GetStartPoint()
	{
		return poolManager._GetStartPoint();
	}*/
	public Transform GetStartPoint()
	{
		if(wayPoints.Length != 0)
			return wayPoints[0];
		else 
			return null;
	}
	/*public static Transform[] GetEndPoints()
	{
		return poolManager._GetEndPoints();
	}*/
	public Transform[] GetEndPoints()
	{
		int count = 0;
		for(int i=0; i<wayPoints.Length; ++i)
		{
			if(wayPoints[i].GetComponent<WayPoint>().next.Length == 0)
				count ++;
		}
		Transform[] endPoints = new Transform[count];
		int j=0;
		for(int i=wayPoints.Length-1; i>=0; --i)
		{
			if(wayPoints[i].GetComponent<WayPoint>().next.Length == 0)
				endPoints[j++] = wayPoints[i];
			if( j>= count)
				break;
		}
		return endPoints;
	}
}
