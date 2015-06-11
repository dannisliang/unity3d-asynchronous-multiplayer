using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	/// <summary>
	/// The dragon.
	/// </summary>
	public GameObject dragon;

	/// <summary>
	/// The player score.
	/// </summary>
	public int playerScore = 0;

	/// <summary>
	/// The is player died.
	/// </summary>
	public bool isPlayerDied = false;

	/// <summary>
	/// The _instance.
	/// </summary>
	private static GameManager _instance = null;

	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
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

	/// <summary>
	/// Kills the dragon.
	/// </summary>
	void KillDragon() {
	}

	/// <summary>
	/// Raises the tap to play event.
	/// </summary>
	public void OnTapToPlay() {
		// FIXME weak code
		dragon.GetComponent<Scroller> ().enabled = true;
		dragon.GetComponentInChildren<Rigidbody> ().useGravity = true;
		dragon.GetComponentInChildren<DragonController> ().enabled = true;
	}

	/// <summary>
	/// Raises the dragon get ball event.
	/// </summary>
	public void OnDragonGetBall() {
		playerScore += 1;
	}

	/// <summary>
	/// Raises the dragon crashed event.
	/// </summary>
	public void OnDragonCrashed() {
		isPlayerDied = true;
		Debug.Log ("GameManager::OnDragonCrashed");
	}
}
