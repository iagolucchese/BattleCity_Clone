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
	private float lastTimeDirectionChanged = 0f;
	private float[] directions;
	private Vector2 newVelocity;
	private RaycastHit2D[] hits;

	// Use this for initialization
	void Start () {
		directions = new float[4]{0f,90f,180f,270f};
		shotContainer = GameObject.FindGameObjectWithTag("ShotContainer");
		newVelocity = new Vector2(0,-1) * moveSpeed; //moves forward again
	}
	
	// Update is called once per frame
	void Update () {
		hits = Physics2D.RaycastAll(shotSpawn.transform.position, shotSpawn.transform.up, 20f); //20f is just a big enough number for anything
		foreach(RaycastHit2D h in hits) {
			if (h.collider && h.collider.gameObject.tag == "Player")
				FireShot();
		}
	}

	void FixedUpdate() {
		rigidbody2D.velocity = newVelocity;
	}

	void FireShot() {
		if (Time.time > nextFire && !existingShot){
			existingShot = (GameObject)Instantiate(shotPrefab, shotSpawn.position, shotSpawn.rotation); //spawns a shot inside the showspawn's position and rotation
			existingShot.transform.parent = shotContainer.transform; //puts the shot into the container
			nextFire = Time.time + fireRate; //adds to the shot delay
		}
	}

	void NewRandomDirection() {
		if (Time.time > lastTimeDirectionChanged+pickRandomDirectionDelay) { //if enough time has passed since last direction pick
			Debug.Log("New direction");
			int newDirection = UnityEngine.Random.Range(0,directions.Length);

			rigidbody2D.velocity = Vector2.zero; //stops moving
			transform.rotation = Quaternion.Euler(0,0,directions[newDirection]); //picks a random direction from the directions's array and sets it

			switch(newDirection){
			case 0: //0 degrees
				newVelocity = new Vector2(-1,0) * moveSpeed; //left
				break;
			
			case 1: //90 degrees
				newVelocity = new Vector2(0,-1) * moveSpeed; //down
				break;
			
			case 2: //180 degrees
				newVelocity = new Vector2(1,0) * moveSpeed; //right
				break;

			case 3: //270 degrees
				newVelocity = new Vector2(0,1) * moveSpeed; //up
				break;
			default:
				newVelocity = Vector2.zero;
				break;
			}
			rigidbody2D.velocity = newVelocity;

			lastTimeDirectionChanged = Time.time; //marks this time as the time this object last picked a new direction
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag == "Wall" || other.collider.tag == "Enemy")
			NewRandomDirection();
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.collider.tag == "Wall" || other.collider.tag == "Enemy")
			NewRandomDirection();
	}

	void OnDestroy(){
		//TODO: insert death animation here

		GameController gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
		if (gameController)
			gameController.EnemyDestroyed();
	}
}
