using UnityEngine;
using System.Collections;

public class LivesScoreManager : MonoBehaviour {

	public GUITexture[] lifeIcons;
	public GUIText scoreText;

	public int lives;
	public int score;

	public void ChangeScore(int scoreChange) {
		score += scoreChange;
		scoreText.text = score.ToString();
	}

	public void RemoveLife() {
		lives--;
		lifeIcons[lives].enabled = false;
	}

	void Start () {
		if (lifeIcons.Length > 0)
			lives = lifeIcons.Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}