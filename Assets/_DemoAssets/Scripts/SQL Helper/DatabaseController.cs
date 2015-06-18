using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DatabaseController : MonoBehaviour {
	public string insertQueryURL = "http://54.172.187.82/unitytest/insert_player_data.php?";
	public string updateQueryURL = "http://54.172.187.82/unitytest/update_player_data.php?";
	public string deleteQueryURL = "http://54.172.187.82/unitytest/delete_player_data.php?";
	public string selectQueryURL = "http://54.172.187.82/unitytest/load_player_data.php?";

	private bool isLoading = false;
	
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
	
	public void LoadPlayerData(string playerFacebookID) {
		StartCoroutine(LoadPlayerDataRoutine(playerFacebookID));
	}
	
	public void DeletePlayerData(string playerFacebookID) {
		if (!playerFacebookID.Equals ("")) {
			StartCoroutine (DeletePlayerDataRoutine (playerFacebookID));
		}
	}
	
	IEnumerator InsertPlayerDataRoutine(PlayerData playerData)
	{
		string requestURL = insertQueryURL + "new_fb_id=" + WWW.EscapeURL(playerData.FacebookID)
			+ "&new_fb_name=" + WWW.EscapeURL(playerData.FacebookName)
			+ "&new_fb_friends=" + WWW.EscapeURL(playerData.FacebookFriends)
			+ "&new_score=" + playerData.Score
			+ "&new_jump_data=" + WWW.EscapeURL(playerData.JumpData)
			+ "&new_bonus_data=" + WWW.EscapeURL(playerData.BonusData);
		
		//Debug.Log("requestURL = " + requestURL);
		
		
		WWW webRequest = new WWW(requestURL);
		
		yield return webRequest;
		
		GameManager.Instance.OnUpdatePlayerDataFinish(webRequest.error == null);
	}
	
	IEnumerator LoadPlayerDataRoutine(string playerFacebookID)
	{
		string requestURL = selectQueryURL + "fb_id=" + WWW.EscapeURL(playerFacebookID);
		Debug.Log("requestURL = " + requestURL);

		WWW webRequest = new WWW(requestURL);
		
		yield return webRequest;
		
		GameManager.Instance.OnLoadPlayerDatasFinish(webRequest.error == null, webRequest.text);
	}
	
	IEnumerator DeletePlayerDataRoutine(string playerFacebookID)
	{
		string requestURL = deleteQueryURL + "new_fb_id=" + WWW.EscapeURL(playerFacebookID);
		
		Debug.Log("requestURL = " + requestURL);
		
		
		WWW webRequest = new WWW(requestURL);
		
		yield return webRequest;
		
		GameManager.Instance.OnDeletePlayerDataFinish(webRequest.error == null);
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
