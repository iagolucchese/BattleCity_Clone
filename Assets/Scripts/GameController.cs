using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject playerPrefab;
	public GameObject boundaries;
	public GameObject grid;
	public LivesScoreManager livesScoreManager;
	public EnemySpawner enemySpawnerScript;
	public GameObject playerSpawner;
	public GameObject gameOverText;

	private bool gameStopped = false;

	public void EnemyDestroyed() {
		livesScoreManager.ChangeScore(10); //fixed score at 10 right now
		enemySpawnerScript.enemyKilled();
	}

	public void PlayerDied(){
		livesScoreManager.RemoveLife();
		if (livesScoreManager.lives <= 0) {
			gameOver();
		}
		else {
			SpawnPlayer();
		}
	}

	public void SpawnPlayer() {
		if (playerSpawner)
			Instantiate (playerPrefab, playerSpawner.transform.position, playerSpawner.transform.rotation);
		else
			throw new UnityException("Failed to acquire the player spawn position. Did you set one at the mapfile?");
	}

	public void gameOver() {
		gameOverText.SetActive(true); //shows the game over text
		audio.Stop(); //stops the bgm
		gameStopped = true; //marks the game as stopped
		Time.timeScale = 0f; //time-stops the game
	}

	void Update () {
		if (gameStopped){ //if the game is stopped
			if (Input.GetKey("r")) { //if the R key has been pressed
				Time.timeScale = 1f; //restore time to the game
				Application.LoadLevel(Application.loadedLevelName); //reloads the level
			}
		}
	}
}
