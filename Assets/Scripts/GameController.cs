﻿using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject playerObject;
	public GameObject boundaries;
	public GameObject grid;

	public EnemySpawner enemySpawner;


	// Use this for initialization
	void Start () {
		enemySpawner = gameObject.GetComponent<EnemySpawner>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}