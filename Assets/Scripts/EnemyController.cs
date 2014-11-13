using UnityEngine;
using System.Collections;
using System;

public class EnemyController : MonoBehaviour {

	public GameObject shotPrefab; //the actual shot Prefab
	public Transform shotSpawn; /* the GameObject that is used as a positional reference for all shots to spawn; it is declared as a Transform, so unity grabs the Transform property of whatever you put here */
	public GameObject shotContainer;

	public float moveSpeed = 2f, fireRate = 0.2f; //the enemy's move speed (higher = faster), and the fire rate (lower = faster)
	private GameObject existingShot; //holds the shot that was already fired, for fire rate limiting purposes
	private float nextFire; //a variable that tells the game how long in total should it wait to let the player fire again
	
	/* variables for the enemy AI */
	public float pickRandomDirectionDelay = 0.5f; //minimum amount of time after the enemy bumps into a wall and before he changes direction
	private Vector2 direction = new Vector2(0,0);

	private RaycastHit2D[] hits;

	// Use this for initialization
	void Start () {
		shotContainer = GameObject.FindGameObjectWithTag("ShotContainer");
	}
	
	// Update is called once per frame
	void Update () {
		hits = Physics2D.RaycastAll(shotSpawn.transform.position, shotSpawn.transform.up, 20f); //20f is just a big enough number for anything
		foreach(RaycastHit2D h in hits) {
			if (h.collider && h.collider.gameObject.tag == "Player")
				FireShot();
		}
	}

	void FireShot() {
		if (Time.time > nextFire && !existingShot){
			existingShot = (GameObject)Instantiate(shotPrefab, shotSpawn.position, shotSpawn.rotation); //spawns a shot inside the showspawn's position and rotation
			existingShot.transform.parent = shotContainer.transform; //puts the shot into the container
			nextFire = Time.time + fireRate; //adds to the shot delay
		}
	}

	void NewRandomDirection() {
		//TODO: Randomizes a different direction
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag = "Wall")
			NewRandomDirection();
	}

	void OnDestroy(){
		//TODO: insert death animation here

		GameObject gameController = GameObject.FindGameObjectWithTag("GameController");
		if (gameController)
			gameController.GetComponent<EnemySpawner>().enemyKilled(this.gameObject);
	}
}
