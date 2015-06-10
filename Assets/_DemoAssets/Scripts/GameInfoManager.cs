using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameInfoManager : MonoBehaviour {

	public Text scoreText = null;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (scoreText != null) {
			scoreText.text = "Score " + GameManager.GetInstance ().playerScore;
		}
	}
}
