using UnityEngine;
using System.Collections;
using System;

public class EnemyController : MonoBehaviour {

	public float moveSpeed = 2f, fireRate = 0.2f; //the enemy's move speed (higher = faster), and the fire rate (lower = faster)
	private GameObject existingShot; //holds the shot that was already fired, for fire rate limiting purposes
	private float nextFire; //a variable that tells the game how long in total should it wait to let the player fire again

	public GameObject shotPrefab; //the actual shot Prefab
	public Transform shotSpawn; /* the GameObject that is used as a positional reference for all shots to spawn; it is declared as a Transform, so unity grabs the Transform property of whatever you put here */

	/* variables for the enemy AI */
	public float randomDirectionDelay = 0.5f; //minimum amount of time the enemy bumps into a wall before he changes direction

	private RaycastHit2D raycast;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		raycast = Physics2D.Raycast (this.transform.position, this.transform.up, 20f, 9); //20f is just a big enough number for anything, and 9 is the player layer
		if (raycast)
			ShootForward ();

	}

	void ShootForward() {

	}

	void OnDestroy(){
		//TODO: insert death animation here

		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		if (gameController)
			gameController.GetComponent<EnemySpawner>().enemyKilled(this.gameObject);
		else {
			//throw new NullReferenceException("Either the Game Controller is tagged incorrectly, or it got destroyed for some weird reason!");
		}
	}
}
