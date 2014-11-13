using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public GameObject shotContainer;
	public GameObject shotPrefab; //the actual shot Prefab
	public Transform shotSpawn; //the GameObject that is used as a positional reference for all shots to spawn; it is declared as a Transform, 
								//so unity grabs the Transform property of whatever you put here

	//public float playerScale = 1; //currently a multiplier for the player size, not very useful yet
	public float moveSpeed = 2f, fireRate = 0.2f; //the player's move speed (higher = faster), and the fire rate (lower = faster)
	private float nextFire; //tells the game how long in total should it wait to let the player fire again
	private GameObject existingShot; //holds the shot that was already fired, for fire rate limiting purposes

	//public GUIText debugText; //debug text placeholder

	void Start () 
	{
		//transform.localScale = new Vector3(transform.localScale.x * playerScale,transform.localScale.y * playerScale,transform.localScale.z);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//spawns the shot
		if (Input.GetButton("Fire1")) //if the player pressed the button, and the shot's delay has passed, AND there is no other shot in the scene
			FireShot ();
	}
	
	void FixedUpdate() //Used for physics code
	{
		float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = Input.GetAxis ("Horizontal");
		if (moveHorizontal != 0 && moveVertical != 0) moveVertical = 0; //prevents diagonal movement, swaps it with horizontal movement

		FlipPlayer (moveHorizontal, moveVertical);
		rigidbody2D.velocity = new Vector2 (moveHorizontal, moveVertical) * moveSpeed;
	}

	void FireShot() {
		if (Time.time > nextFire && !existingShot){
			existingShot = (GameObject)Instantiate(shotPrefab, shotSpawn.position, shotSpawn.rotation); //spawns a shot inside the showspawn's position and rotation
			existingShot.transform.parent = shotContainer.transform; //puts the shot into the container
			nextFire = Time.time + fireRate; //adds to the shot delay
			//audio.Play(); //plays the shot's sound, if there's any
		}
	}

	void FlipPlayer(float x, float y)
	{
		//performs rotations based on the direction the player is going; also, keep in mind Euler angles are degrees, not radians
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
