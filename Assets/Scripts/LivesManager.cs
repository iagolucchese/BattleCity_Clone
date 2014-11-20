using UnityEngine;
using System.Collections;

public class LivesManager : MonoBehaviour {

	public GUITexture[] lifeIcons;
	public int lifeCount;

	// Use this for initialization
	void Start () {
		if (lifeIcons.Length > 0)
			lifeCount = lifeIcons.Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
