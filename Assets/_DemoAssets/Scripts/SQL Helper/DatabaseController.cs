using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DatabaseController : MonoBehaviour {
	public string insertQueryURL = "http://localhost/phpmyadmin/unity_test/insert_player_data.php?";
	public string updateQueryURL = "http://localhost/phpmyadmin/unity_test/update_player_data.php?";
	public string deleteQueryURL = "http://localhost/phpmyadmin/unity_test/delete_player_data.php?";
	public string selectQueryURL = "http://localhost/phpmyadmin/unity_test/load_player_data.php";

	private List<PlayerData> topPlayers = new List<PlayerData> ();
	
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

	public List<PlayerData> TopPlayers {
		get {
			return topPlayers;
		}
	}

	void Awake() {
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
		string requestURL = insertQueryURL + "new_fb_id=" + WWW.EscapeURL(playerData.FacebookID)
			+ "&new_fb_name=" + WWW.EscapeURL(playerData.FacebookName)
			+ "&new_fb_friends=" + WWW.EscapeURL(playerData.FacebookFriends)
			+ "&new_score=" + playerData.Score
			+ "&new_jump_data=" + WWW.EscapeURL(playerData.JumpData)
			+ "&new_bonus_data=" + WWW.EscapeURL(playerData.BonusData);
		
		Debug.Log("requestURL = " + requestURL);
		
		
		WWW webRequest = new WWW(requestURL);
		
		yield return webRequest;
		
		if (webRequest.error != null) {
			Debug.Log ("Save request error: " + webRequest.error);
		} else {
			Debug.Log (webRequest.text);
			Debug.Log("InsertPlayerDataRoutine successful!");
		}
	}
	
	IEnumerator UpdatePlayerDataRoutine(PlayerData playerData)
	{
		playerData.LogAllInfos ();

		string requestURL = updateQueryURL + "fb_id=" + WWW.EscapeURL(playerData.FacebookID)
			+ "&new_fb_name=" + WWW.EscapeURL(playerData.FacebookName)
			+ "&new_fb_friends=" + WWW.EscapeURL(playerData.FacebookFriends)
			+ "&new_score=" + playerData.Score
			+ "&new_jump_data=" + WWW.EscapeURL(playerData.JumpData)
			+ "&new_bonus_data=" + WWW.EscapeURL(playerData.BonusData);
		
		Debug.Log("requestURL = " + requestURL);
		
		
		WWW webRequest = new WWW(requestURL);
		
		yield return webRequest;
		
		if (webRequest.error != null) {
			Debug.Log ("Save request error: " + webRequest.error);
		} else {
			Debug.Log("UpdatePlayerDataRoutine successful!");
		}
	}
	
	IEnumerator DeletePlayerDataRoutine(PlayerData playerData)
	{
		string requestURL = deleteQueryURL + "new_fb_id=" + WWW.EscapeURL(playerData.FacebookID);
		
		Debug.Log("requestURL = " + requestURL);
		
		
		WWW webRequest = new WWW(requestURL);
		
		yield return webRequest;
		
		if (webRequest.error != null) {
			Debug.Log ("Save request error: " + webRequest.error);
		} else {
			Debug.Log(webRequest.text);
		}
	}
	
	IEnumerator LoadPlayerDataRoutine()
	{
		WWW webRequest = new WWW(selectQueryURL);
		
		yield return webRequest;
		
		if (webRequest.error != null)
		{
			Debug.Log("Load request error: " + webRequest.error);
		}
		else
		{
			DeserializePlayerDatas(webRequest.text);
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

	private bool DeserializePlayerDatas(string returnedText) {
		char[] rowDelimiters = new char[] { '\n' };
		string[] rows = returnedText.Split (rowDelimiters, StringSplitOptions.RemoveEmptyEntries);

		if (rows != null && rows.Length > 0) {
			char[] colDelimiters = new char[] { ';' };

			for (int i = 0; i < rows.Length; i++) {
				string[] cols = rows[i].Split(colDelimiters, StringSplitOptions.RemoveEmptyEntries);

				if (cols != null && cols.Length > 5) {
					PlayerData pData = new PlayerData();

					pData.FacebookID = cols[0];
					pData.FacebookName = cols[1];
					pData.FacebookFriends = cols[2];
					pData.Score = int.Parse(cols[3]);
					pData.JumpData = cols[4];
					pData.BonusData = cols[5];

					//Debug.Log (pData.FacebookID + "\t" + pData.FacebookName + "\t" + pData.FacebookFriends + "\t" + pData.Score + "\t" + pData.JumpData + "\t" + pData.BonusData);

					// add player data to managed list
					topPlayers.Add(pData);
				}
			}
		} else {
			return false;
		}

		return true;
	}
}
