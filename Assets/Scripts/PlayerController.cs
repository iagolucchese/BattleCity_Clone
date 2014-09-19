using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	private GameObject boundaries;
	private GridController grid;

	public float playerSizeInPixels;

	public float speed, fireRate; //the player's move speed (higher = faster), and the fire rate (lower = faster)
	private GameObject existingShot; //holds the shot that was already fired, for fire rate limiting purposes
	private float nextFire; //a variable that tells the game how long in total should it wait to let the player fire again

	public GUIText debugText; //reference a GUItext element

	public GameObject shot; //the actual shot Prefab
	public Transform shotSpawn; //the GameObject that is used as a positional reference for all shots to spawn; it is declared as a Transform, 
								//so unity grabs the Transform property of whatever you put here

	private Vector2 oldMovementAxis; //for alignment 'snapping' uses
	private float stepSizeX, stepSizeY; //defining the size of the "step" of the player object, kind of like saying he moves in units of this variable
	public float stepSmoothness = 6; //the higher, the smooth each step is, but too high and the player might have trouble with the controls

	// Use this for initialization
	void Start () 
	{
		boundaries = GameObject.FindWithTag("Boundaries");
		grid = GameObject.FindWithTag("Grid").GetComponent<GridController>();
	
		transform.localScale = new Vector3(transform.localScale.x * playerSizeInPixels,transform.localScale.y * playerSizeInPixels,transform.localScale.z);

		oldMovementAxis = new Vector2(0.0f, 0.0f);
		stepSizeX = ((boundaries.transform.localScale.x * 2) / grid.numberOfColumns) / stepSmoothness;
		stepSizeY = ((boundaries.transform.localScale.y * 2) / grid.numberOfLines) / stepSmoothness;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//spawns the shot
		if (Input.GetButton("Fire1") && Time.time > nextFire && !existingShot) {
			//if (shotSpawn.renderer.bounds.Intersects(boundaries.renderer.bounds)) { //checking whether the shot spawned in the games boundaries
				existingShot = (GameObject)Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
				nextFire = Time.time + fireRate;

				//audio.Play(); //plays the shot's sound, if there's any
			//}
		}
	}
	
	void FixedUpdate() //Used for physics code
	{
		float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = Input.GetAxis ("Horizontal");
		if (moveHorizontal != 0 && moveVertical != 0) moveVertical = 0; //prevents diagonal movement, swaps it with horizontal movement

		FlipPlayer (moveHorizontal, moveVertical);
		rigidbody2D.velocity = new Vector2 (moveHorizontal, moveVertical) * speed;

		oldMovementAxis = new Vector2(moveHorizontal,moveVertical);

	}

	void FlipPlayer(float x, float y)
	{
		/* start of the code that align the player with the walls; sort of like snapping, but along one axis only; happened on the classic too */
		if (oldMovementAxis.x == 0 && x != 0)
		{
			//align along the Y axis
			/*
			float playerPositionFactor_Line = transform.position.y/(boundaries.transform.localScale.y/grid.numberOfLines); //formula to get an aproximate position of the player, relative to the background grid
			playerPositionFactor_Line = Mathf.Min();
			transform.position = new Vector3(transform.position.x,playerPositionFactor_Line,transform.position.z);
			*/
		}
		else if (oldMovementAxis.y == 0 && y != 0)
		{
			//align along the X axis

		}

		//performs rotations based on the direction the player is going; also, keep in mind Euler angles are degree based
		if (x > 0)
			transform.rotation = Quaternion.Euler(0.0f,0.0f,270.0f);
		else if (x < 0)
			transform.rotation = Quaternion.Euler(0.0f,0.0f,90.0f);
		else
			if (y > 0)
				transform.rotation = Quaternion.Euler(0.0f,0.0f,0.0f);
			else if (y < 0)
				transform.rotation = Quaternion.Euler(0.0f,0.0f,180.0f);		 
	}
}
