using UnityEngine;
using System;
using System.Collections;

public class FriendScroller : MonoBehaviour {
	
	public float ScrollSpeedNormal = 2.0f;
	public float ScrollSpeedBonusMax = 3.0f;
	
	private float scrollSpeed = 2.0f;
	private float scrollSpeedBonus = 1.0f;
	private float scrollTimeTotal = 0.0f;
	
	// FIXME change bonusData to private when testing done
	public string bonusData = "";
	private float[] bonusTimestamps;
	private int currentBonusTimestampIndex = 0;
	
	public string BonusData {
		get {
			return bonusData;
		}
		set {
			bonusData = string.Copy (value);
		}
	}
	
	void Awake() {
		enabled = false;
		scrollTimeTotal = 0.0f;
		ExtractBonusData ();
	}
	
	// Use this for initialization
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		scrollTimeTotal += Time.deltaTime;

		if (bonusTimestamps != null && currentBonusTimestampIndex < bonusTimestamps.Length) {
			if (scrollTimeTotal >= bonusTimestamps[currentBonusTimestampIndex]) {
				scrollSpeedBonus = ScrollSpeedBonusMax;

				currentBonusTimestampIndex++;
			}
		}
		
		if (scrollSpeedBonus > 1.0f) {
			scrollSpeedBonus = Mathf.Lerp (scrollSpeedBonus, 1.0f, Time.deltaTime);
		}
		
		scrollSpeed = scrollSpeedBonus * ScrollSpeedNormal;
		
		transform.Translate (Vector3.right * Time.deltaTime * scrollSpeed);
	}
	
	public void OnTapToPlay() {
		enabled = true;
		scrollTimeTotal = 0.0f;
	}
	
	/// <summary>
	/// Raises the dragon killed event.
	/// </summary>
	public void OnDragonKilled() {
		Reset ();
	}
	
	/// <summary>
	/// Reset this instance.
	/// </summary>
	private void Reset() {
		enabled = false;
		scrollTimeTotal = 0.0f;
	}

	public void ExtractBonusData() {
		char[] delimiters = new char[] { '|' };
		string[] bonusTimestampSt = bonusData.Split (delimiters, StringSplitOptions.RemoveEmptyEntries);
		
		if (bonusTimestampSt.Length > 1) {
			Debug.Log ("bonusTimestampSt.Length = " + bonusTimestampSt.Length);
			bonusTimestamps = new float[bonusTimestampSt.Length];
			
			for (int i = 0; i < bonusTimestamps.Length; i++) {
				float timestamp = float.Parse (bonusTimestampSt [i]);
				bonusTimestamps [i] = timestamp;
			}
		}
	}
}
