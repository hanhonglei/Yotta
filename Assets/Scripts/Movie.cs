using UnityEngine;
using System.Collections;

public class Movie : MonoBehaviour {
	public MovieTexture movTexture;
	public GameObject audio;
	// Use this for initialization
	void Start () {
		GetComponent<Renderer>().material.mainTexture = movTexture;
		///renderer= movTexture.audioClip;
		movTexture.loop = false;
		movTexture.Play();
		audio.GetComponent<AudioSource>().GetComponent<AudioSource>().Play();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButton("Fire1"))
		{
			if(movTexture.isPlaying == true)
			{
				movTexture.Stop();
				audio.GetComponent<AudioSource>().GetComponent<AudioSource>().Stop();
				Application.LoadLevel("Yotta_LevelSelect");
			}
		}
		if(movTexture.isPlaying == false)
			Application.LoadLevel("Yotta_LevelSelect");
	}
}
