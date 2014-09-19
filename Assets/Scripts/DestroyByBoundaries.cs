using UnityEngine;
using System.Collections;

public class DestroyByBoundaries : MonoBehaviour {
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Shot") //only destroys shots that try to leave the scene, so far
			Destroy(other.gameObject);
		if (other.tag == "Player")
			other.transform.rigidbody2D.velocity = Vector2.zero; //stops the player from leaving
	}
}
