using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FacebookUserInfo {
	public string userID = "";
	public string userName = "";
	public string userFriends = "";
	public Image userAvatar = null;

	public void LogInfo() {
		Debug.Log ("User info" + userID + " - " + userName + " - " + userFriends);
	}
}

public class FacebookHelper : MonoBehaviour {
	/// <summary>
	/// The _instance.
	/// </summary>
	private static FacebookHelper _instance = null;
	
	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static FacebookHelper Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<FacebookHelper>();
			return _instance;
		}
	}

	public GameObject PlayerAvatar = null;
	public GameObject PlayerName = null;

	public GameObject facebookPanel = null;
	public Button btnLogin = null;
	public Button btnHighscore = null;
	public Button btnPlay = null;

	private bool isLoggedInSuccessful = false;
	private FacebookUserInfo userInfo;

	public bool IsLoggedInSuccessful {
		get {
			return isLoggedInSuccessful;
		}
	}

	public FacebookUserInfo UserInfo {
		get {
			return userInfo;
		}
	}

	void Awake() {
		if (userInfo == null) {
			userInfo = new FacebookUserInfo ();
		}

		btnLogin.enabled = true;
		btnHighscore.enabled = false;
		btnPlay.enabled = false;

		if (!FB.IsInitialized) {
			FB.Init (SetInit, OnHideUnity);
		}
	}

	// Use this for initialization
	void Start () {
		if (FB.IsLoggedIn) {
			DealWithFacebookLoggedIn ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnBtnLogin() {
		if (!FB.IsLoggedIn) {
			FB.Login ("user_friends, public_profile", LoginCallBack);
		} else {
			btnLogin.enabled = false;
			btnHighscore.enabled = true;
			btnPlay.enabled = true;

			btnLogin.GetComponent<Text>().text = "Log Out Facebook";
		}
	}

	public void OnBtnHighscore() {
	}

	public void OnBtnPlay() {
		// Hide Facebook panel
		facebookPanel.SetActive (false);
	}

	public void OnGameOver() {
		// Show Facebook panel
		facebookPanel.SetActive (true);
	}

	void DealWithFacebookLoggedIn() {
		//Get user's info
		FB.API("me?fields=id,name,friends", Facebook.HttpMethod.GET, GetUserInfoCallback);
	}

	#region Callback Functions
	
	void SetInit() {
		if (FB.IsLoggedIn) {
			Debug.Log("FB logged in");
		}
	}
	
	void OnHideUnity(bool isGameShow) {
		if (!isGameShow)
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
	}
	
	void LoginCallBack(FBResult result) {
		if (FB.IsLoggedIn) {
			Debug.Log("FB is logged in to work");

			DealWithFacebookLoggedIn();

			btnLogin.enabled = false;
		}
		else
		{
			GameManager.Instance.OnPlayerLoginFacebook (false);
		}
	}
	
	void GetUserInfoCallback(FBResult result)
	{
		if (result.Error != null)
		{
			Debug.Log("Get username fail");
			return;
		}
		
		Debug.Log("GetUserInfoCallback: " + result.Text);

		string[] fields = new string[] {"id", "name"};
		Dictionary<string, string> playerProfile = FacebookUtil.DeserializeJSONProfile(result.Text, fields);

		// Set Player's avatar and name
		PlayerAvatar.SetActive(true);
		PlayerName.SetActive(true);
		
		Text username = PlayerName.GetComponent<Text>();
		username.text = "Hello, " + playerProfile["name"];

		userInfo.userID = string.Copy (playerProfile ["id"]);
		userInfo.userName = string.Copy (playerProfile ["name"]);

		List<object> friends = FacebookUtil.DeserializeJSONFriends (result.Text);
		userInfo.userFriends = "";
		
		for (int i = 0; i < friends.Count; i++) {
			var friendDict = ((Dictionary<string,object>)(friends[i]));
			
			string friendID = (string)friendDict["id"];
			userInfo.userFriends += friendID + "|";
		}
		
		//Get avatar
		FB.API(FacebookUtil.GetPictureURL("me", 128, 128), Facebook.HttpMethod.GET, GetAvatarCallback);
	}
	
	void GetAvatarCallback(FBResult result)
	{
		if (result.Error != null) {
			Debug.Log ("Get avatar fail");
			return;
		}

		userInfo.userAvatar = PlayerAvatar.GetComponent<Image> ();
		userInfo.userAvatar.sprite = Sprite.Create (result.Texture, new Rect (0, 0, 128, 128), new Vector2 (0, 0));

		userInfo.LogInfo ();
		
		isLoggedInSuccessful = true;

		GameManager.Instance.OnPlayerLoginFacebook (isLoggedInSuccessful);
		
		btnLogin.enabled = false;
		btnHighscore.enabled = true;
		btnPlay.enabled = true;
		btnLogin.GetComponentInChildren<Text>().text = "Log Out Facebook";
	}
	
	#endregion

}
