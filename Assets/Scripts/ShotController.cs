using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour {
	public float shotSpeed = 4f;
	public float selfDestructDelay = 0.001f; //useful for adjusting multi collisions with one shot

	void Start()
	{
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
		//gameObject.GetComponent<BoxCollider2D>().size += new Vector2(0.2f,0f); //this is basically an "explosion collision", by making the box bigger when it collides with something

		if (other.tag == "Enemy") {
			Destroy(other.gameObject);
			Destroy(this.gameObject);
		}
		if (other.tag == "Wall") {
			Destroy(other.gameObject);
			
			Destroy(this.gameObject,selfDestructDelay);//through this delay in destroy, i intend to take out anything that immediatly collides with the shot, but hopefully not anything behind it
		}
		if (other.tag == "Player") {
			Destroy(other.gameObject);
			Destroy(this.gameObject);
		}
	}

	void OnDestroy() {
		//TODO: Insert shot explosion animation here
	}
}
