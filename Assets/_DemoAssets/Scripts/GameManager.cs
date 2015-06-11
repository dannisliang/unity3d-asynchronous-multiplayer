using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject dragon;
	public int playerScore = 0;
	public bool isPlayerDied = false;
	
	private static GameManager _instance = null;
	public static GameManager Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<GameManager>();
			return _instance;
		}
	}

	// Use this for initialization
	void Start () {
		// FIXME weak code
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
		// FIXME weak code
		dragon.GetComponent<Scroller> ().enabled = true;
		dragon.GetComponentInChildren<Rigidbody> ().useGravity = true;
		dragon.GetComponentInChildren<DragonController> ().enabled = true;
	}

	public void OnDragonGetBall() {
		playerScore += 1;
	}

	public void OnDragonCrashed() {
		isPlayerDied = true;
		Debug.Log ("GameManager::OnDragonCrashed");
	}
}
