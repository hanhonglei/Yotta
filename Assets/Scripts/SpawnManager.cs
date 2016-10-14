using UnityEngine;
using System.Collections;

//imagine that you have 25 waves, and in each wave, there are 10 subwaves,that is the meaning
//just like the "protectradish"
//finally knew it, the wavelist is the total big waves in each level, the subwaves in each wave of the
//wavelist is 
[System.Serializable]
public class SubWave{
	public GameObject m_gameObject;
	public int m_num=1;//the num of gameobjects
	public float m_delay=0.0f;
	public float m_interval = 0.0f;
	[HideInInspector]
	public int m_ID=0;
}
[System.Serializable]
public class Wave{//if a class is public ,will its members be shown on the inspector?
	[HideInInspector]
	public GameObject m_gameObject;//maybe this is an empty object which used to connect with a prefab
	public SubWave[] m_subWaves = new SubWave[1];//the number 1 here has some defect
	[HideInInspector]
	public int m_num = 0; //the num of objects in all subwaves
	[HideInInspector]
	public float m_interval = 1.0f;
	public float m_waveInterval = 20.0f;
	
}

public class SpawnManager : MonoBehaviour {
	public GameObject subwaveObject;
	//public GameObject baseList;
	//private Wave[] waveList;//the whole subwaves
	[HideInInspector] public int allnum = 0;
	public Wave[] waveList;
	private int waveIndex = 0; 
	public float xoffset = 0.1f;
	public float yoffset = 0.1f;
	// Use this for initialization
	public static SpawnManager spawnManager;
	void Start () {
		foreach(Wave wave in waveList)
		{
			foreach (SubWave subWave in wave.m_subWaves) //calculate the whole num of the objects int the game 
			{
				wave.m_num += subWave.m_num;
			}
			allnum += wave.m_num;
		}
		//	Instantiate(baseList,new Vector3(-30,0),Quaternion.identity);
		StartCoroutine("SpawnWave");
	}
	
	void Update () {
	}
	
	IEnumerator SpawnWave()
	{
		//int i = 0;
		foreach (Wave wave in waveList) 
		{
			//yield return new WaitForSeconds (wave.m_waveInterval);
			++ waveIndex;
			StartCoroutine("SpawnSubWave",wave);
			yield return new WaitForSeconds(wave.m_waveInterval);
		}
	}
	
	IEnumerator SpawnSubWave(Wave wave)
	{
		//int i = 0;
		foreach (SubWave subWave in wave.m_subWaves) 
		{
			yield return new WaitForSeconds (subWave.m_delay);
			while (subWave.m_num>0 ) 
			{
				SpawnObject(subWave.m_gameObject);
				subWave.m_num--;
				wave.m_num--;
				yield return new WaitForSeconds (subWave.m_interval);
			}
			yield return new WaitForSeconds(wave.m_interval);
		}
	}
	
	void SpawnObject(GameObject obj)
	{
		//GameObject tempObject;
		Vector3 offSet = new Vector3(Random.Range(-xoffset,xoffset),Random.Range(-yoffset,yoffset),0);
		Vector3 subwaveObjectPos = transform.position+offSet;
		subwaveObjectPos.z =0;
		Instantiate(obj, subwaveObjectPos, Quaternion.identity);
		//tempObject = (GameObject)Instantiate(subwaveObject, subwaveObjectPos, Quaternion.identity);
	}
	
	
	IEnumerator SystemWait(float seconds)
	{
		yield return new WaitForSeconds (seconds);
	}
	public int GetWholeWaves()
	{
		return waveList.Length;
	}
	public int GetCurrWave()
	{
		return waveIndex;
	}
}
