using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHighscore : MonoBehaviour {

	public GameObject playerName = null;
	public GameObject playerScore = null;

	// Use this for initialization
	void Start () {
	}
		
	// Update is called once per frame
	void Update () {

	}

	public void SetInfo(string name, int score) {
		playerName.GetComponent<Text> ().text = string.Copy (name);
		playerScore.GetComponent<Text> ().text = string.Copy ("" + score);
	}
}
