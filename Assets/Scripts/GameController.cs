using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject boundaries;
	public GameObject grid;
	public LivesScoreManager livesScoreManager;
	public EnemySpawner enemySpawner;
	public GameObject playerSpawn;

	public void EnemyDestroyed() {
		livesScoreManager.ChangeScore(10); //fixed score at 10 right now
		enemySpawner.enemyKilled();
	}

	public void PlayerDied(){
		livesScoreManager.RemoveLife();
		if (livesScoreManager.lives <= 0) {
			//TODO: Game over code here
		}
		else {
			SpawnPlayer();
		}
	}

	public void SpawnPlayer() {
		if (playerSpawn)
			Instantiate (playerPrefab, playerSpawn.transform.position, playerSpawn.transform.rotation);
		else
			throw new UnityException("Failed to acquire the player spawn position. Did you set one at the mapfile?");
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
