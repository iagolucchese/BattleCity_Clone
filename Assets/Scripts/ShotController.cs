using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {
	public GameController gameController;
	public float shotSpeed = 4f;
	public float selfDestructDelay = 0.001f; //useful for adjusting multi collisions with one shot
	public string killsTagged; //kills anything tagged with this, plus other shots and walls by default
	public GameObject explosionPrefab;

	void Start()
	{
		if (!gameController)
			gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();

		float rotation = (transform.rotation.eulerAngles.z * Mathf.Deg2Rad); //math, cuz unity has euler angles in degrees but Mathf only accepts rad angles *facepalm*
		rigidbody2D.AddForce(new Vector2(Mathf.Sin(-rotation)*100*shotSpeed,Mathf.Cos(-rotation)*100*shotSpeed)); //the sin and cosin functions basically work to return either -1, 0, or 1, to determine the direction of the force
	}

	void OnTriggerEnter2D(Collider2D other) {
		CollisionDetected(other);
	}

	void OnTriggerStay2D(Collider2D other) { //this one is just in case the shot spawns INSIDE an enemy or a wall
		CollisionDetected(other);
	}

	void CollisionDetected(Collider2D other){
		if (other.gameObject.tag == killsTagged || other.gameObject.tag == "PlayerHouse") {
			other.GetComponent<DeathController>().KillCharacter();
			DestroyThis(0f);
		}
		else if(other.tag == "Shot")
			DestroyThis(selfDestructDelay);

		else if (other.tag == "Wall") {
			Destroy(other.gameObject);
			DestroyThis(selfDestructDelay);//this mini-delay allows the shot to also work on anything else nearby, emulating a "splash" effect
		}
		else if(other.tag == "Metal")
			DestroyThis();
	}

	void DestroyThis(float delay = 0f) {
		if (explosionPrefab)
			Instantiate(explosionPrefab,this.transform.position,this.transform.rotation);
		Destroy(this.gameObject,delay);
	}
}
