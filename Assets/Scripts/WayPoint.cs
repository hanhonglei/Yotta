using UnityEngine;
using System.Collections;

public class WayPoint : MonoBehaviour {
	public Transform[] next;
	public bool isDestPoint()
	{
		if(next.Length == 0)
			return true;
		return false;
	}
}
