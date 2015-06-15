using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DatabaseController : MonoBehaviour {
	public string insertQueryURL = "http://localhost/phpmyadmin/insert_player_data.php?";
	public string updateQueryURL = "http://localhost/phpmyadmin/update_player_data.php?";
	public string deleteQueryURL = "http://localhost/phpmyadmin/delete_player_data.php?";
	public string selectQueryURL = "http://localhost/phpmyadmin/select_player_data.php";
	
	public string secretKey = "unity_test_db_key";
	public PlayerData[] top10Players;
	public Text textObject;
	
	private static DatabaseController _instance = null;
	
	/// <summary>
	/// Gets the instance.
	/// </summary>
	/// <value>The instance.</value>
	public static DatabaseController Instance {
		get {
			if (_instance == null)
				_instance = GameObject.FindObjectOfType<DatabaseController>();
			return _instance;
		}
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void InsertPlayerData(PlayerData playerData) {
		if (!playerData.FacebookID.Equals ("")) {
			StartCoroutine (InsertPlayerDataRoutine (playerData));
		}
	}
	
	public void UpdatePlayerData(PlayerData playerData) {
		if (!playerData.FacebookID.Equals ("")) {
			StartCoroutine (UpdatePlayerDataRoutine (playerData));
		}
	}
	
	public void DeletePlayerData(PlayerData playerData) {
		if (!playerData.FacebookID.Equals ("")) {
			StartCoroutine (DeletePlayerDataRoutine (playerData));
		}
	}
	
	public void LoadPlayerData() {
		StartCoroutine(LoadPlayerDataRoutine());
	}
	
	IEnumerator InsertPlayerDataRoutine(PlayerData playerData)
	{
		string requestURL = insertQueryURL + "new_fb_id=" + playerData.FacebookID
			+ "&new_fb_name=" + playerData.FacebookName
				+ "&new_fb_friends=" + playerData.FacebookFriends
				+ "&new_score=" + playerData.Score
				+ "&new_replay_data=" + playerData.ReplayData;
		
		Debug.Log("requestURL = " + requestURL);
		
		
		WWW webRequest = new WWW(requestURL);
		
		yield return webRequest;
		
		if (webRequest.error != null) {
			Debug.Log ("Save request error: " + webRequest.error);
		} else {
			Debug.Log(webRequest.ToString());
		}
	}
	
	IEnumerator UpdatePlayerDataRoutine(PlayerData playerData)
	{
		string requestURL = updateQueryURL + "fb_id=" + playerData.FacebookID
			+ "&new_fb_name=" + playerData.FacebookName
				+ "&new_fb_friends=" + playerData.FacebookFriends
				+ "&new_score=" + playerData.Score
				+ "&new_replay_data=" + playerData.ReplayData;
		
		Debug.Log("requestURL = " + requestURL);
		
		
		WWW webRequest = new WWW(requestURL);
		
		yield return webRequest;
		
		if (webRequest.error != null) {
			Debug.Log ("Save request error: " + webRequest.error);
		} else {
			Debug.Log(webRequest.ToString());
		}
	}
	
	IEnumerator DeletePlayerDataRoutine(PlayerData playerData)
	{
		string requestURL = deleteQueryURL + "new_fb_id=" + playerData.FacebookID;
		
		Debug.Log("requestURL = " + requestURL);
		
		
		WWW webRequest = new WWW(requestURL);
		
		yield return webRequest;
		
		if (webRequest.error != null) {
			Debug.Log ("Save request error: " + webRequest.error);
		} else {
			Debug.Log(webRequest.ToString());
		}
	}
	
	IEnumerator LoadPlayerDataRoutine()
	{
		textObject.text = "Loading Scores ...";
		
		WWW webRequest = new WWW(selectQueryURL);
		
		yield return webRequest;
		
		if (webRequest.error != null)
		{
			Debug.Log("Load request error: " + webRequest.error);
		}
		else
		{
			Debug.Log(webRequest.text);
			//display score on GUI
			textObject.text = webRequest.text;
		}
	}
	
	public string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);
		
		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);
		
		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";
		
		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}
		
		return hashString.PadLeft(32, '0');
	}
}
