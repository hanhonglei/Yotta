using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//from now on, the following code is in your hand!
//be brave, and stay calm!!
//2014.10.27 18:07



/// <summary>
/// Unit attribute.
/// the basic attribute for the base class Unit
/// Unit is the base class for all the towers
/// includes all the functions and attributes which need to be inherited 
/// </summary>
[System.Serializable]
public class UnitAttribute{
	public float m_fullHp;
	public float m_curHp;

	public Transform m_overlayHp;		//the hp bar on the top of it
	public Transform m_overlayBase;		//the base of the Unit(e.g. tower_base)
	public bool m_alwaysShowOverlay = false;
}
/// <summary>
/// Unit.
/// declare the functions and attributes of Unit
/// </summary>
public class Unit : MonoBehaviour {
	private enum UnitSubClassType{ //specify the type of the gameobject
		None = 0,
		em_Tower,
		em_Visitor,
	};

	//public attribute
	public string unitName;
	public Texture icon;
	public UnitAttribute HpAttribute;

	//protected attribute
	protected Vector3 overlayScaleH; 	//the horizonal life bar scale vector
	protected bool overlayIsVisible = false;
	protected bool isDead = false;
	protected bool isScored = false;

	[HideInInspector] public Transform thisT;			//the current transform
	[HideInInspector] public GameObject thisObj;		//the current gameobject
	public float moveSpeed;								//the move speed of the unit					
	public float rotateSpeed;							//the rotate/turn speed of the unit

	public delegate void DeadHandler( int waveID );
	public static event DeadHandler DeadE;

	public delegate void ScoreHandler( int waveID);
	public static event ScoreHandler ScoreE;
	
	//private
	private UnitSubClassType subClass = UnitSubClassType.None;
	private UnitTower unitT;
	private UnitVisitor unitV; //visitor

	public void SetSubClassID(UnitTower unit)
	{
		unitT = unit;
		subClass = UnitSubClassType.em_Tower;
		moveSpeed = unitT.moveSpeed;
		rotateSpeed = unitT.rotateSpeed;
	}
	public void SetSubClassID( UnitVisitor unit)
	{
		unitV = unit;
		subClass = UnitSubClassType.em_Visitor;
		moveSpeed = unitV.moveSpeed;
		rotateSpeed = unitV.rotateSpeed;
	}

	public virtual void Awake()
	{
		thisT = transform;
		thisObj = gameObject;
		if (HpAttribute.m_overlayHp != null) 
		{
			overlayScaleH = HpAttribute.m_overlayHp.localScale;
		}
		//UnitUtility.DestroyColliderRecursively (thisT);
	}

	// Use this for initialization
	public virtual void Init()
	{
		//HpAttribute.m_curHp = HpAttribute.m_fullHp;

		isDead = false;
		isScored = false;
	}
	public void SetFullHp(float hp)
	{
		HpAttribute.m_fullHp = hp;
		//HpAttribute.m_curHp = HpAttribute.m_fullHp;
	}
	public void SetCurHp(float hp)
	{
		HpAttribute.m_curHp = hp;
	}
	
	public virtual void Start () 
	{
		Init ();
	}
	
	// Update is called once per frame
	public virtual void Update () 
	{

	}

}
