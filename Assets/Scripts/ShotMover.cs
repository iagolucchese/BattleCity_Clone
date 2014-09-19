using UnityEngine;
using System.Collections;

public class ShotMover : MonoBehaviour {
	public float speed;
	
	void Start()
	{
		float rotation = (transform.rotation.eulerAngles.z * Mathf.Deg2Rad);
		rigidbody2D.AddForce(new Vector2(Mathf.Sin(-rotation)*100*speed,Mathf.Cos(-rotation)*100*speed));
	}
}
