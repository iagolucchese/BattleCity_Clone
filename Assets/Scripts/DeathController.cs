using UnityEngine;
using System.Collections;

public class DeathController : MonoBehaviour {

	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
	}

	public void KillCharacter() {
		if (this.gameObject.tag == "Player")
			gameController.PlayerDied();
		else if (this.gameObject.tag == "Enemy")
			gameController.EnemyDestroyed();
		else if (this.gameObject.tag == "PlayerHouse")
			gameController.gameOver();
		//TODO: Call death animation here

		Destroy (this.gameObject);
	}
}
