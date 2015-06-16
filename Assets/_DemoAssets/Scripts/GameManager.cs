using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	/// <summary>
	/// The dragon prefab.
	/// </summary>
	public Object friendDragonPrefab;

	/// <summary>
	/// The dragon.
	/// </summary>
	public GameObject dragon = null;
	public GameObject friendDragonsObject = null;

	private List<GameObject> friendDragons = new List<GameObject> ();

	/// <summary>
	/// The player data.
	/// </summary>
	private PlayerData playerDataSaved = null;
	private PlayerData playerData = null;
	private List<PlayerData> topPlayerDatas;

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
		
		// TODO Login Facebook and get these informations
		if (true) {
			playerData.FacebookID = "123456";
			playerData.FacebookName = "Trung Ngo";
			playerData.FacebookFriends = "123450|123452|123453|123454|123455";
			
			playerData.LogAllInfos();
		}

		//TODO create slots for storing friend's dragons (if there's any)
		StartCoroutine (LoadPlayerDatas ());

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

	IEnumerator LoadPlayerDatas() {
		DatabaseController.Instance.LoadPlayerData ();

		yield return null;

		topPlayerDatas = DatabaseController.Instance.TopPlayers;

		CreateFriendDragons ();
	}

	void CreateFriendDragons() {
		foreach (var pData in topPlayerDatas) {
			//Debug.Log (pData.FacebookID + "\t" + pData.FacebookName + "\t" + pData.FacebookFriends + "\t" + pData.Score + "\t" + pData.JumpData + "\t" + pData.BonusData);

			GameObject friendDragon = (GameObject) Instantiate(friendDragonPrefab, Vector3.zero, Quaternion.identity);

			// Set data for controller component
			FriendDragonController controller = friendDragon.GetComponent<FriendDragonController> ();
			controller.JumpData = pData.JumpData;
			controller.ExtractJumpData();

			// Set data for scroller component
			FriendScroller scroller = friendDragon.GetComponent<FriendScroller> ();
			scroller.BonusData = pData.BonusData;
			scroller.ExtractBonusData();
			
			// Cached your data on SQL server
			if (pData.FacebookID == playerData.FacebookID) {
				playerDataSaved = pData;
				friendDragon.GetComponentInChildren<TextMesh>().text = "You";
			}
			else
				friendDragon.GetComponentInChildren<TextMesh>().text = pData.FacebookName;

			friendDragons.Add (friendDragon);
		}
	}

	/// <summary>
	/// Kills the dragon.
	/// </summary>
	void KillDragon() {
		if (dragon != null) {
			dragon.GetComponent<Scroller> ().enabled = false;
			dragon.GetComponent<DragonController> ().enabled = false;
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
			dragon.GetComponent<DragonController> ().OnTapToPlay ();
			dragon.GetComponentInChildren<Rigidbody> ().useGravity = true;
		}

		foreach (var friendDragon in friendDragons) {
			friendDragon.GetComponent<FriendScroller> ().OnTapToPlay ();
			friendDragon.GetComponent<FriendDragonController> ().OnTapToPlay ();
			friendDragon.GetComponentInChildren<Rigidbody> ().useGravity = true;
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
		Debug.Log ("Last JumpData = " + playerData.JumpData);

		Scroller scroller = dragon.GetComponent<Scroller> ();
		playerData.BonusData = scroller.BonusData;
		Debug.Log ("Last BonusData = " + scroller.BonusData);

		isPlayerDied = true;

		UpdatePlayerDataToDatabase ();
	}

	/// <summary>
	/// Updates the player data to database.
	/// </summary>
	/// <returns><c>true</c>, if player data to database was updated, <c>false</c> otherwise.</returns>
	public bool UpdatePlayerDataToDatabase() {
		// TODO Check if player has not logged into Facebook
		if (false) {
			return false;
		} else {
			// Check if 
		}

		if (playerDataSaved == null || playerDataSaved.Score < playerData.Score) {
			// Insert/update player data to database.
			DatabaseController.Instance.InsertPlayerData (playerData);
		}

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
