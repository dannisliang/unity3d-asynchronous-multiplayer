using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	/// <summary>
	/// The dragon prefab.
	/// </summar>
	public UnityEngine.Object friendDragonPrefab;

	public GameObject btnTapToPlay = null;
	public GameObject btnTouchToContinue = null;
	public GameObject mainCanvas = null;

	/// <summary>
	/// The dragon.
	/// </summary>
	public GameObject dragon = null;
	private List<GameObject> friendDragons = new List<GameObject> ();

	/// <summary>
	/// The player data.
	/// </summary>
	private FacebookUserInfo playerFacebookInfo = null;
	private PlayerData playerDataSaved = null;
	private PlayerData playerData = null;
	private List<PlayerData> topPlayerDatas = new List<PlayerData> ();

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
		if (playerData == null) {
			playerData = new PlayerData ();
		}

		btnTouchToContinue.SetActive (false);
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

	void CreateFriendDragons() {
		Debug.Log ("CreateFriendDragons: topPlayerDatas.Count = " + topPlayerDatas.Count);
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
			if (pData.FacebookID.Equals(playerData.FacebookID)) {
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
			dragon.GetComponentInChildren<Rigidbody> ().isKinematic = true;
			dragon.GetComponent<Scroller> ().enabled = false;
			dragon.GetComponent<DragonController> ().enabled = false;
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
		}

		foreach (var friendDragon in friendDragons) {
			friendDragon.GetComponent<FriendScroller> ().OnTapToPlay ();
			friendDragon.GetComponent<FriendDragonController> ().OnTapToPlay ();
		}
	}

	/// <summary>
	/// Raises the touch to continue event.
	/// </summary>
	public void OnTouchToContinue() {
		// Notify Facebook helper instance
		FacebookHelper.Instance.OnGameOver ();
		
		// Reset game
		ResetGameScene ();
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

	public void OnPlayerLoginFacebook(bool isSuccessful) {
		if (playerData == null) {
			playerData = new PlayerData();
		}

		if (isSuccessful) {
			playerFacebookInfo = FacebookHelper.Instance.UserInfo;

			playerData.FacebookID = playerFacebookInfo.userID;
			playerData.FacebookName = playerFacebookInfo.userName;
			playerData.FacebookFriends = playerFacebookInfo.userFriends;
			
			playerData.LogAllInfos ();

			// Update player's facebook info (name, friends) to database
			UpdatePlayerFacebookInfoToDatabase();
		} else {
			playerData.FacebookID = "";
			playerData.FacebookName = "Anonymous Player";
			playerData.FacebookFriends = "";
		}
	}

	/// <summary>
	/// Loads the player data from database.
	/// </summary>
	/// <returns><c>true</c>, if player data from database was loaded, <c>false</c> otherwise.</returns>
	public void LoadPlayerDatasFromDatabase() {
		DatabaseController.Instance.LoadPlayerData (playerData.FacebookID);
	}

	public void UpdatePlayerFacebookInfoToDatabase() {
		DatabaseController.Instance.UpdatePlayerInfo (playerData);
	}
	
	/// <summary>
	/// Updates the player data to database.
	/// </summary>
	/// <returns><c>true</c>, if player data to database was updated, <c>false</c> otherwise.</returns>
	public void UpdatePlayerDataToDatabase() {
		// Check if player has not logged into Facebook
		if (!FacebookHelper.Instance.IsLoggedInSuccessful) {
			OnUpdatePlayerDataFinish(false);
			return;
		}
		
		if (playerDataSaved == null || playerDataSaved.Score < playerData.Score) {
			if (playerDataSaved != null) {
				playerDataSaved.Score = playerData.Score;
				playerDataSaved.JumpData = playerData.JumpData;
				playerDataSaved.BonusData = playerData.BonusData;
			}

			// Insert/update player data to database.
			DatabaseController.Instance.InsertPlayerData (playerData);
		} else {
			OnUpdatePlayerDataFinish(false);
		}
	}

	public void DeletePlayerDatasFromDatabase() {
		DatabaseController.Instance.DeletePlayerData (playerData.FacebookID);
	}

	public void OnLoadPlayerDatasFinish(bool isSuccess, string returnedString) {
		if (isSuccess) {
			DeserializePlayerDatas (returnedString);

			CreateFriendDragons();
		}

		Debug.Log ("OnLoadPlayerDatasFinish " + isSuccess);
		Debug.Log ("returnedString = " + returnedString);
	}

	public void OnUpdatePlayerDataFinish(bool isSuccess) {
		btnTouchToContinue.SetActive (true);
	}

	public void OnUpdatePlayerInfoFinish(bool isSuccess) {
		// Load player's datas from database
		LoadPlayerDatasFromDatabase ();
	}

	public void OnDeletePlayerDataFinish(bool isSuccess) {
	}

	private void ResetGameScene() {
		Vector3 translateVec = new Vector3 (-dragon.transform.position.x, 0, 0);
		
		// Reset dragon to start position
		dragon.transform.Translate (translateVec);
		dragon.GetComponent<DragonController> ().Reset ();
		dragon.GetComponent<Scroller> ().Reset ();

		// Reset player data
		playerData.Score = 0;
		playerData.JumpData = "";
		playerData.BonusData = "";

		// Reset friend dragons position
		foreach (var friendDragon in friendDragons) {
			friendDragon.transform.Translate(translateVec);
			friendDragon.GetComponent<FriendDragonController> ().Reset ();
			friendDragon.GetComponent<FriendScroller> ().Reset ();

			if (playerDataSaved != null) {
				if (friendDragon.GetComponentInChildren<TextMesh>().text.Equals("You")) {
					friendDragon.GetComponent<FriendDragonController> ().JumpData = playerDataSaved.JumpData;
					friendDragon.GetComponent<FriendDragonController> ().ExtractJumpData();

					friendDragon.GetComponent<FriendScroller> ().BonusData = playerDataSaved.BonusData;
					friendDragon.GetComponent<FriendScroller> ().ExtractBonusData();
				}
			}
		}

		// Reset green balls
		GameObject[] greenBalls = GameObject.FindGameObjectsWithTag ("GreenBall");
		foreach (var greenBall in greenBalls) {
			greenBall.GetComponentInChildren<MeshRenderer>().enabled = true;
			greenBall.GetComponentInChildren<Collider>().enabled = true;
		}

		// Reset bonus balls
		GameObject[] bonusBalls = GameObject.FindGameObjectsWithTag ("BonusBall");
		foreach (var bonusBall in bonusBalls) {
			bonusBall.GetComponentInChildren<MeshRenderer>().enabled = true;
			bonusBall.GetComponentInChildren<Collider>().enabled = true;
		}

		btnTapToPlay.SetActive (true);
	}
	
	private bool DeserializePlayerDatas(string returnedText) {
		char[] rowDelimiters = new char[] { '\n' };
		string[] rows = returnedText.Split (rowDelimiters, System.StringSplitOptions.RemoveEmptyEntries);
		
		if (rows != null && rows.Length > 0) {
			char[] colDelimiters = new char[] { ';' };
			
			for (int i = 0; i < rows.Length; i++) {
				string[] cols = rows[i].Split(colDelimiters, System.StringSplitOptions.RemoveEmptyEntries);
				
				if (cols != null && cols.Length > 5) {
					PlayerData pData = new PlayerData();
					
					pData.FacebookID = cols[0];
					pData.FacebookName = cols[1];
					pData.FacebookFriends = cols[2];
					pData.Score = int.Parse(cols[3]);
					pData.JumpData = cols[4];
					pData.BonusData = cols[5];
					
					pData.LogAllInfos();
					
					// add player data to managed list
					topPlayerDatas.Add(pData);
				}
			}
		} else {
			return false;
		}
		
		Debug.Log ("DeserializePlayerDatas: topPlayers.Count = " + topPlayerDatas.Count);
		
		return true;
	}
}
