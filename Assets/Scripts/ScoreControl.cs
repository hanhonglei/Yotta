using UnityEngine;
using System.Collections;

public class ScoreControl : MonoBehaviour {

	static public int angryParam;
	static public int coinParam;
	static public int essenceParam;
	static private int angryLimit;
	static private int waveLimit;
	static private int currWave;

	static public float angryBarW;
	static public float angryBarH;

	static public Transform thisWaves;
	static public Transform thisAngry;
	static public Transform thisCoin;
	static public Transform thisHpEssence;
	static public Transform thisAngryBar;
	// Use this for initialization
	void Awake()
	{
		thisWaves = this.transform.FindChild("WavesParam");
		thisAngry = this.transform.FindChild("AngryParam");
		thisCoin = this.transform.FindChild("CoinParam");
		thisHpEssence = this.transform.FindChild("HpEsseParam");
		thisAngryBar = this.transform.FindChild("AngryBar");

		angryParam = 0;
		coinParam = 600;
		essenceParam = 0;
		angryLimit = 10;
		waveLimit = GameObject.Find("SpawnPoint").GetComponent<SpawnManager>().waveList.Length;
		currWave = 0;
		
		angryBarW = 67;
		angryBarH = 20;
		UpdateUI();
	}
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		currWave = GameObject.Find("SpawnPoint").GetComponent<SpawnManager>().GetCurrWave() ;
		UpdateUI();
	}
	static public void UpdateAngryBar()
	{
		GUITexture guiTexture;
		guiTexture = thisAngryBar.GetComponent<GUITexture>();
		guiTexture.pixelInset = new Rect(guiTexture.pixelInset.x, guiTexture.pixelInset.y,
		                                 angryParam*angryBarW/angryLimit, guiTexture.pixelInset.height );
		//guiTexture.color = Color.red;
	}

	static private void UpdateUI()
	{
		thisCoin.GetComponent<GUIText>().text = coinParam.ToString();
		thisWaves.GetComponent<GUIText>().text = currWave.ToString() + "/"+waveLimit.ToString();
		thisAngry.GetComponent<GUIText>().text = angryParam.ToString() + "/" + angryLimit.ToString();
		thisHpEssence.GetComponent<GUIText>().text = essenceParam.ToString();
		UpdateAngryBar();
	}
	static public int CoinIncrease(int amount)
	{
		coinParam += amount;
		UpdateUI();
		return coinParam;
	}
	static public int CoinDecrease(int amount)
	{
		if(coinParam < amount)
		{
			Debug.Log("Not enough coin!!");
			return coinParam;
		}

		coinParam -= amount;
		UpdateUI();
		return coinParam;
	}
	static public int AngryIncrease(int amount)
	{
		angryParam += amount;
		if(angryParam >= angryLimit)
		{
			Animator ani = GameManager.gameManager.scoreUI.GetComponent<Animator>();
			//GameManager::clearall();

			if(ani.GetBool("isDisappear") == true)
			{
				GameObject.Find("GameManager").GetComponent<GameManager>().GameOverDisplay();
				Time.timeScale = 0.0f;
			}
			//Application.LoadLevel("Yotta_Start");
			ani.SetBool("isDisappear", true);
			ani.SetBool("isShowup", false);
			//return angryParam;
		}
		UpdateUI();
		//UpdateAngryBar();
		return angryParam;
	}
	static public int EssenceIncrease(int amount)
	{
		essenceParam += amount;
		return essenceParam;
	}
	static public bool CheckResourceSuffi(int cost)
	{
		if(coinParam-cost < 0)
			return false;
		else
			return true;
	}
}













