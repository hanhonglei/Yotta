using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LayerManager : MonoBehaviour {
	
	public static bool initiated = false;
	public int layerVisitor = 30;
	public int layerTower = 29;
	public int layerTowerUp = 31;
	public int layerOverlay = 25; //CCat
	
	public static LayerManager layerManager;
	
	void Awake(){
		layerManager = this;
		GameManager gameManager = gameObject.GetComponent<GameManager>();
		if(gameManager != null)
			gameManager.layerManager = this;
	}
	
	public static void Init()
	{
		if(layerManager == null)
		{
			GameObject obj = new GameObject();
			obj.name = "LayerManager";
			
			layerManager = obj.AddComponent<LayerManager>();
			
		}
	}
	
	public static int LayerVistor()
	{
		return layerManager.layerVisitor;
	}
	public static int LayerTower()
	{
		return layerManager.layerTower;
	}
	public static int LayerTowerUp()
	{
		return layerManager.layerTowerUp;
	}
	
	public static int LayerOverlay()
	{
		return layerManager.layerOverlay;
	}
	
}

