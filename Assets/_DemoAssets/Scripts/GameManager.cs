using UnityEngine;
using System.Collections;

public class GameManager : ScriptableObject {

	public static GameManager GetInstance() {
		return CreateInstance<GameManager> ();
	}

	public GameObject dragon;
	public int playerScore = 0;
	public bool isPlayerDied = false;

	// Use this for initialization
	void Start () {
		dragon.GetComponent<Scroller> ().enabled = false;
		dragon.GetComponentInChildren<Rigidbody> ().useGravity = false;
		dragon.GetComponentInChildren<DragonController> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isPlayerDied) {
			KillDragon();

			isPlayerDied = false;
		}
	}
	
	void KillDragon() {
	}

	public void OnTapToPlay() {
		dragon.GetComponent<Scroller> ().enabled = true;
		dragon.GetComponentInChildren<Rigidbody> ().useGravity = true;
		dragon.GetComponentInChildren<DragonController> ().enabled = true;
	}

	public void OnDragonGetBall() {
		playerScore += 1;
		Debug.Log ("GameManager::OnDragonGetBall " + playerScore);
	}

	public void OnDragonCrashed() {
		isPlayerDied = true;
		Debug.Log ("GameManager::OnDragonCrashed");
	}
}
