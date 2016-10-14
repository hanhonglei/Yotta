using UnityEngine;
using System.Collections;

public enum BonusType
{
	em_None = 0,
	em_Coin,
	em_Essence,
};

public class Bonus : MonoBehaviour {
	private float timeEllapse = 0;
	private float timeLimit = 5;
	private int increaseAmount = 0;
	private BonusType bonusType = BonusType.em_None;
	void Awake()
	{
		if(this.name == "CoinHead")
		{
			bonusType = BonusType.em_Coin;
			increaseAmount = 0;
			timeLimit = 1;
		}
		else if(this.name == "hpEsseHead")
		{
			bonusType = BonusType.em_Essence;
			increaseAmount = 1;
			timeLimit = 5;
		}
		timeEllapse = 0;
	}
	void Update()
	{
		//this.gameObject.GetComponent<Collider2D>().enabled = true;  //check this later on

		timeEllapse += Time.deltaTime;
		if(timeEllapse > timeLimit)
		{
			//PoolManager.Unspawn(this.transform.parent.gameObject);
			//this.transform.parent.gameObject.SetActive (false);
			Destroy(this.transform.parent.gameObject);
		}

	}

	void OnMouseOver()
	{
		if(bonusType == BonusType.em_Essence)
			ScoreControl.EssenceIncrease(increaseAmount);
		//this.transform.parent.gameObject.SetActive (false);
		Destroy(this.transform.parent.gameObject);
		//Destroy(this.gameObject);
		PoolManager.Unspawn (this.transform.parent.gameObject);
	}
	/*
	void OnBecameInvisible()
	{
		if(bonusType == BonusType.em_Coin )
			PoolManager.Unspawn(this.gameObject);
	}*/
	void UnspawnCoin()
	{
		//PoolManager.Unspawn (this.transform.parent.gameObject);
		//this.transform.parent.gameObject.SetActive (false);
		if(this.bonusType == BonusType.em_Coin)
			Destroy(this.transform.parent.gameObject);
	}
}
