﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour {

	public GameObject enemyContainer;
	public List<GameObject> listOfEnemyPrefabs;
	public List<GameObject> listOfEnemySpawners;

	public float enemySpawnDelay = 2.0f;
	public int enemyLimit = 4;
	private int enemyCount = 0;
	private float timeToSpawn = 0;

	// Use this for initialization
	void Start () {
		timeToSpawn = enemySpawnDelay;
	}
	
	// Update is called once per frame
	void Update () {
		if (timeToSpawn <= Time.deltaTime && enemyCount < enemyLimit){ //if enough time has passed, spawn an enemy
			GameObject randomSpawner = listOfEnemySpawners[Random.Range(0,listOfEnemySpawners.Count)];
									
			GameObject newEnemy = Instantiate(listOfEnemyPrefabs[Random.Range(0,listOfEnemyPrefabs.Count)],randomSpawner.transform.position,Quaternion.Euler(Vector3.zero)) as GameObject; //creates the new enemy gameObject
			newEnemy.transform.parent = enemyContainer.transform; //puts it under the container for enemies

			enemyCount++;
			timeToSpawn += enemySpawnDelay;
		}
		timeToSpawn -= Time.deltaTime;
	}

	public void enemyKilled(GameObject enemy){
		timeToSpawn += enemySpawnDelay; //trying to prevent instant respawn after you kill an enemy
		Debug.Log("Enemy Killed!");
		enemyCount--;
	}
}