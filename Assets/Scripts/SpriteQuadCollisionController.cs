using UnityEngine;
using System.Collections;

public class SpriteQuadCollisionController : MonoBehaviour {
	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Shot") {
			Destroy(other.gameObject);

			Destroy(this.gameObject); //really shouldn't do this
		}
	} 

	void OnCollisionEnter2D(Collision2D coll) {

	}
}
