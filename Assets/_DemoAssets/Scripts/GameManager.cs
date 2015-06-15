using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	/// <summary>
	/// The dragon.
	/// </summary>
	public GameObject dragon = null;
	public GameObject friendDragons = null;

	private PlayerData[] friendPlayerDatas = new PlayerData[5];

	/// <summary>
	/// The player data.
	/// </summary>
	private PlayerData playerData = null;

	/// <summary>
	/// The is player died.
	/// </summary>
	private bool isPlayerDied = false;

	public PlayerData GetPlayerData {
		get {
			return playerData;
		}
	}

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

	void Awake() {
		// create memory for saving player data
		playerData = new PlayerData ();

		//TODO create slots for storing friend's dragons (if there's any)
	}

	// Use this for initialization
	void Start () {
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
		if (dragon != null) {
			dragon.GetComponent<Scroller> ().enabled = false;
			dragon.GetComponentInChildren<DragonController> ().enabled = false;
			dragon.GetComponentInChildren<Rigidbody> ().isKinematic = true;
		}
	}

	/// <summary>
	/// Raises the tap to play event.
	/// </summary>
	public void OnTapToPlay() {
		// FIXME weak code
		if (dragon != null) {
			dragon.GetComponent<Scroller> ().OnTapToPlay ();
			dragon.GetComponentInChildren<DragonController> ().OnTapToPlay ();
			dragon.GetComponentInChildren<Rigidbody> ().useGravity = true;
		}

		if (friendDragons != null) {
			friendDragons.GetComponent<FriendDragonsManager>().OnTapToPlay();
		}
	}

	/// <summary>
	/// Raises the dragon get ball event.
	/// </summary>
	public void OnDragonGetBall(bool isBonusBall) {
		if (isBonusBall) {
			playerData.Score += 10;
			dragon.GetComponent<Scroller> ().OnGetBonusBall();
		} else {
			playerData.Score += 1;
		}
	}

	/// <summary>
	/// Raises the dragon crashed event.
	/// </summary>
	public void OnDragonCrashed() {
		// Get player's replay data
		DragonController controller = dragon.GetComponent<DragonController> ();
		playerData.JumpData = controller.JumpData;
		Debug.Log ("JumpData = " + playerData.JumpData);

		Scroller scroller = dragon.GetComponent<Scroller> ();
		playerData.BonusData = scroller.BonusData;
		Debug.Log ("BonusData = " + scroller.BonusData);

		isPlayerDied = true;
	}

	/// <summary>
	/// Updates the player data to database.
	/// </summary>
	/// <returns><c>true</c>, if player data to database was updated, <c>false</c> otherwise.</returns>
	public bool UpdatePlayerDataToDatabase() {
		return true;
	}

	/// <summary>
	/// Loads the player data from database.
	/// </summary>
	/// <returns><c>true</c>, if player data from database was loaded, <c>false</c> otherwise.</returns>
	public bool LoadPlayerDataFromDatabase() {
		return true;
	}

	/// <summary>
	/// Loads the friend player data from database.
	/// </summary>
	/// <returns><c>true</c>, if friend player data from database was loaded, <c>false</c> otherwise.</returns>
	public bool LoadFriendPlayerDataFromDatabase() {
		return true;
	}


}
