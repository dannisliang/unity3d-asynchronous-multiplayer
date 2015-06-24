using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerData {
	#region properties
	private string	_fbID = "";
	private string	_fbName = "";
	private string	_fbFriends = "";
	private int		_score = 0;
	private string	_jumpData = "";
	private string	_bonusData = "";

	public string FacebookID {
		get {
			return _fbID;
		}
		set {
			_fbID = string.Copy (value);
		}
	}
	
	public string FacebookName {
		get {
			return _fbName;
		}
		set {
			_fbName = string.Copy (value);
		}
	}
	
	public string FacebookFriends {
		get {
			return _fbFriends;
		}
		set {
			_fbFriends = string.Copy (value);
		}
	}
	
	public int Score {
		get {
			return _score;
		}
		set {
			_score = value;
		}
	}
	
	public string JumpData {
		get {
			return _jumpData;
		}
		set {
			_jumpData = string.Copy (value);
		}
	}

	public string BonusData {
		get {
			return _bonusData;
		}
		set {
			_bonusData = string.Copy (value);
		}
	}

	public void LogAllInfos() {
		Debug.Log ("ID="+ _fbID + "\t" + "Name="+ _fbName + "\t" + "Friends="+ _fbFriends + "\t" + "Score="+ _score + "\t" + "Jump="+ _jumpData + "\t" + "Bonus="+ _bonusData);
	}

	#endregion properties
}
