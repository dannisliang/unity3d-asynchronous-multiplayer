using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public readonly int MaxFriendDragon = 4;

	/// <summary>
	/// The dragon prefab.
	/// </summar>
	public UnityEngine.Object friendDragonPrefab;

	public GameObject btnTapToPlay = null;
	public GameObject btnTouchToContinue = null;
	public GameObject mainCanvas = null;
	public GameObject highscoreTable = null;

	/// <summary>
	/// The dragon.
	/// </summary>
	public GameObject dragon = null;
	private GameObject[] friendDragons = null;

	/// <summary>
	/// The player data.
	/// </summary>
	private FacebookUserInfo playerFacebookInfo = null;
	private PlayerData playerDataSaved = null;
	private PlayerData playerData = null;
	private PlayerData[] topPlayerDatas = null;

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
	}

	// Use this for initialization
	void Start () {
		btnTouchToContinue.SetActive (false);
		btnTapToPlay.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (isPlayerDied) {
			KillDragon();

			isPlayerDied = false;
		}
	}

	void CreateFriendDragons() {
		int playerDataCount = topPlayerDatas.Length;

		friendDragons = new GameObject[MaxFriendDragon];
		for (int i = 0; i < MaxFriendDragon; i++) {
			PlayerData pData = topPlayerDatas[i];
			GameObject friendDragon = (GameObject) Instantiate(friendDragonPrefab, Vector3.zero, Quaternion.identity);

			if (i < playerDataCount) {
				// Set data for controller component
				FriendDragonController controller = friendDragon.GetComponent<FriendDragonController> ();
				controller.JumpData = pData.JumpData;
				controller.ExtractJumpData();

				// Set data for scroller component
				FriendScroller scroller = friendDragon.GetComponent<FriendScroller> ();
				scroller.BonusData = pData.BonusData;
				scroller.ExtractBonusData();

				friendDragon.GetComponentInChildren<TextMesh>().text = pData.FacebookName;
				
				// Cached your data on SQL server
				if (pData.FacebookID.Equals(playerData.FacebookID)) {
					playerDataSaved = pData;
					
					if (friendDragon != null) {
						friendDragon.GetComponentInChildren<TextMesh>().text = "You";
					}
				}
			}
			else {
				friendDragon.SetActive(false);
			}

			friendDragons[i] = friendDragon;
		}
	}
	
	void UpdateFriendDragonsInfo() {
		int playerDataCount = topPlayerDatas.Length;
		
		for (int i = 0; i < MaxFriendDragon; i++) {
			PlayerData pData = topPlayerDatas[i];
			GameObject friendDragon = friendDragons[i];
			
			if (i < playerDataCount) {
				friendDragon.SetActive(true);

				// Set data for controller component
				FriendDragonController controller = friendDragon.GetComponent<FriendDragonController> ();
				controller.JumpData = pData.JumpData;
				controller.ExtractJumpData();
				
				// Set data for scroller component
				FriendScroller scroller = friendDragon.GetComponent<FriendScroller> ();
				scroller.BonusData = pData.BonusData;
				scroller.ExtractBonusData();
				
				friendDragon.GetComponentInChildren<TextMesh>().text = pData.FacebookName;
				
				// Cached your data on SQL server
				if (pData.FacebookID.Equals(playerData.FacebookID)) {
					playerDataSaved = pData;
					
					if (friendDragon != null) {
						friendDragon.GetComponentInChildren<TextMesh>().text = "You";
					}
				}
			}
			else {
				friendDragon.SetActive(false);
			}
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
		if (friendDragons != null) {
			foreach (var friendDragon in friendDragons) {
				friendDragon.GetComponent<FriendScroller> ().OnTapToPlay ();
				friendDragon.GetComponent<FriendDragonController> ().OnTapToPlay ();
			}
		}

		// FIXME weak code
		if (dragon != null) {
			dragon.GetComponent<Scroller> ().OnTapToPlay ();
			dragon.GetComponent<DragonController> ().OnTapToPlay ();
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
		
		highscoreTable.GetComponent<HighscoreTableManager> ().CreateTable (topPlayerDatas);
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

		Scroller scroller = dragon.GetComponent<Scroller> ();
		playerData.BonusData = scroller.BonusData;

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
			btnTouchToContinue.SetActive(true);
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
			btnTouchToContinue.SetActive(true);
		}
	}

	public void DeletePlayerDatasFromDatabase() {
		DatabaseController.Instance.DeletePlayerData (playerData.FacebookID);
	}

	public void OnLoadPlayerDatasFinish(bool isSuccess, string returnedString) {
		bool isReplay = true;

		if (isSuccess) {
			DeserializePlayerDatas (returnedString);

			if (friendDragons == null) {
				isReplay = false;
				CreateFriendDragons();
				btnTapToPlay.SetActive(true);
			}
			else {
				UpdateFriendDragonsInfo();
			}
		}

		btnTouchToContinue.SetActive (isReplay);
	}

	public void OnUpdatePlayerDataFinish(bool isSuccess) {
		if (isSuccess) {
			// Update top player data
			LoadPlayerDatasFromDatabase ();
		} else {
			btnTouchToContinue.SetActive(true);
		}
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
		if (friendDragons != null) {
			foreach (var friendDragon in friendDragons) {
				if (friendDragon.activeSelf) {
					friendDragon.transform.Translate (translateVec);
					friendDragon.GetComponent<FriendDragonController> ().Reset ();
					friendDragon.GetComponent<FriendScroller> ().Reset ();
				}
			}
		}

		// Reset green balls
		GameObject[] greenBalls = GameObject.FindGameObjectsWithTag ("GreenBall");
		if (greenBalls != null) {
			foreach (var greenBall in greenBalls) {
				greenBall.GetComponentInChildren<MeshRenderer> ().enabled = true;
				greenBall.GetComponentInChildren<Collider> ().enabled = true;
			}
		}

		// Reset bonus balls
		GameObject[] bonusBalls = GameObject.FindGameObjectsWithTag ("BonusBall");
		if (bonusBalls != null) {
			foreach (var bonusBall in bonusBalls) {
				bonusBall.GetComponentInChildren<MeshRenderer> ().enabled = true;
				bonusBall.GetComponentInChildren<Collider> ().enabled = true;
			}
		}

		btnTapToPlay.SetActive (true);
	}
	
	private bool DeserializePlayerDatas(string returnedText) {
		char[] rowDelimiters = new char[] { '\n' };
		string[] rows = returnedText.Split (rowDelimiters, System.StringSplitOptions.RemoveEmptyEntries);

		if (rows != null && rows.Length > 0) {
			topPlayerDatas = new PlayerData[rows.Length];

			char[] colDelimiters = new char[] { ';' };
			
			for (int i = 0; i < rows.Length; i++) {
				string[] cols = rows[i].Split(colDelimiters, System.StringSplitOptions.RemoveEmptyEntries);

				PlayerData pData = topPlayerDatas[i] = new PlayerData();
				
				pData.FacebookID = cols[0];
				pData.FacebookName = cols[1];
				pData.FacebookFriends = cols[2];
				pData.Score = int.Parse(cols[3]);
				pData.JumpData = cols[4];
				pData.BonusData = cols[5];

				if (pData.FacebookID.Equals(playerData.FacebookID)) {
					playerDataSaved = pData;
				}
				
				pData.LogAllInfos();
			}
		} else {
			return false;
		}
		
		return true;
	}
}
