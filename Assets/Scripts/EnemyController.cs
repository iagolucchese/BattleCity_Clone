using UnityEngine;
using System.Collections;
using System;

public class EnemyController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy(){
		//TODO: insert death animation here

		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		if (gameController)
			gameController.GetComponent<EnemySpawner>().enemyKilled(this.gameObject);
		else {
			throw new NullReferenceException("Either the Game Controller is tagged incorrectly, or it got destroyed for some weird reason!");
		}
	}
}
