using UnityEngine;
using System.Collections;

[System.Serializable]
public class AudioObject{
	public AudioSource source;//An AudioSource is attached to a GameObject for playing back sounds in a 3D environment.
	public bool inUse = false;
	public Transform thisT;

	public AudioObject(AudioSource src, Transform t)
	{
		source = src;
		thisT = t;
	}
}
public class AudioManager : MonoBehaviour {
	private static AudioObject[] audioObject;
	public static AudioManager audioManager;

	public float minFallOffRange = 10;
	public AudioClip[] musicList;
	public bool playMusic = true;
	private bool shuffle = false;
	private int currentTrackID = 0;
	private AudioSource musicSource;

	public AudioClip waveClearedSound;
	public AudioClip gameWonSound;
	public AudioClip gameLostSound;
	public AudioClip starShowSound;
	public AudioClip backSound;
	public AudioClip essenCollectSound;

	public AudioClip towerBuildingSound;
	public AudioClip towerBuiltSound;//for now, the building sound and the built sound is same because of the stunding is 0
	public AudioClip towerSoldSound;
	public AudioClip basetowerShowSound;
	public AudioClip buttonSound;
	public AudioClip buttonSound2;
	private GameObject thisObj;
	private Transform thisT;

	public static void PlayGameWonSound()
	{
		if(audioManager.gameWonSound != null)
			PlaySound(audioManager.gameWonSound,false);
	}
	public static void PlayGameLostSound()
	{
		if(audioManager.gameLostSound != null)
			PlaySound (audioManager.gameLostSound,false);
	}
	void Awake()
	{
		audioManager = this;
		thisObj = gameObject;
		if(playMusic && musicList != null && musicList.Length>0)
		{
			GameObject obj = new GameObject();
			obj.name = "MusicSource";
			obj.transform.position = GameManager.gameManager.transform.position;
			obj.transform.parent = GameManager.gameManager.transform;
			musicSource = obj.AddComponent<AudioSource>();
			musicSource.loop = false;
			musicSource.playOnAwake = false;

			StartCoroutine(MusicRoutine());
		}
		audioObject = new AudioObject[20];
		for(int i=0; i< audioObject.Length; ++i)
		{
			GameObject obj = new GameObject();
			obj.name = "AudioSource";
			AudioSource src = obj.AddComponent<AudioSource>();
			src.playOnAwake = false;
			src.loop = false;

			src.minDistance = minFallOffRange;
			Transform t = obj.transform;
			t.parent = thisObj.transform;

			audioObject[i] = new AudioObject(src, t);
		}

	}

	public static void Init()
	{
		if(audioManager == null)
		{
			GameObject objParent = new GameObject();
			objParent.name = "AudioManager";
			audioManager = objParent.AddComponent<AudioManager>();
		}
	}

	public IEnumerator MusicRoutine()
	{
		while(true)
		{
			if(shuffle)
				musicSource.clip = musicList[Random.Range(0, musicList.Length)];
			else
			{
				musicSource.clip = musicList[currentTrackID];
				currentTrackID += 1;
				if(currentTrackID == musicList.Length)
					currentTrackID = 0;

			}
			musicSource.Play();
			yield return new WaitForSeconds(musicSource.clip.length);
		}
	}
	// Use this for initialization
	void Start () {
		if (backSound != null)
			PlaySound (backSound,true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private static int GetUnusedAudioObject()
	{
		for(int i=0; i<audioObject.Length; ++i)
		{
			if(!audioObject[i].inUse)
				return i;
		}
		return 0;
	}
	//3D sound as a position is given
	public static void PlaySound(AudioClip clip, Vector3 pos)
	{
		if(audioManager == null)
			Init ();
		int ID = GetUnusedAudioObject ();
		audioObject [ID].inUse = true;
		audioObject [ID].thisT.position = pos;
		audioObject [ID].source.clip = clip;
		audioObject [ID].source.Play ();

		float duration = audioObject [ID].source.clip.length;
		audioManager.StartCoroutine (audioManager.ClearAudioObject (ID, duration));
	}
	public static void StopSound(AudioClip clip)
	{
		int ID = GetUsedClipID (clip);
		audioObject[ID].source.Stop();
	}
	public static int GetUsedClipID(AudioClip clip)
	{
		int id = -1;
		foreach(AudioObject obj in audioObject)
		{
			if(audioObject[++id].source.clip == clip )
				return id;
		}
		
		return id;
	}
	public static void PlaySound(AudioClip clip,bool loop)
	{
		if(audioManager == null)
			Init();
		int ID = GetUnusedAudioObject ();
		audioObject [ID].inUse = true;
		//audioObject [ID].thisT.position = pos;
		audioObject [ID].source.clip = clip;
		audioObject [ID].source.Play ();

		if(loop == false)
		{
			float duration = audioObject [ID].source.clip.length;
			audioManager.StartCoroutine (audioManager.ClearAudioObject (ID, duration));
			audioObject [ID].source.loop = false;
		}
		else 
			audioObject [ID].source.loop = true;
	}
	private static IEnumerator SoundRoutine2D(int ID, float duration)
	{
		while(duration>0)
		{
			audioObject[ID].thisT.position = GameManager.gameManager.transform.position;
			yield return null;
		}
		audioManager.StartCoroutine (audioManager.ClearAudioObject (ID, 0));
	}
	private IEnumerator ClearAudioObject(int ID, float duration)
	{
		yield return new WaitForSeconds(duration);
		audioObject [ID].inUse = false;
	}
	public static void PlayTowerBuilding()
	{
		if(audioManager.towerBuildingSound != null)
			PlaySound (audioManager.towerBuildingSound,false);
	}
	public static void PlayTowerBuilt()
	{
		if(audioManager.towerBuiltSound != null)
			PlaySound(audioManager.towerBuiltSound,false);
	}
	public static void PlayTowerSold()
	{
		if(audioManager.towerSoldSound != null)
			PlaySound(audioManager.towerSoldSound,false);
	}
	public static void PlayButton()
	{
		if(audioManager.buttonSound != null)
			PlaySound(audioManager.buttonSound,false);
	}
	public static void PlayButton2()
	{
		if(audioManager.buttonSound2 != null)
			PlaySound(audioManager.buttonSound2, false);
	}
}






















