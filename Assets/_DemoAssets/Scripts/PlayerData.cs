using UnityEngine;
using System.Collections;

public class PlayerData {
	#region properties
	private string	_fbID = "";
	private string	_fbName = "";
	private string	_fbFriends = "";
	private int		_score = 0;
	private string	_replayData = "";

	public string FacebookID {
		get {
			return _fbID;
		}
		set {
			_fbID = value;
		}
	}
	
	public string FacebookName {
		get {
			return _fbName;
		}
		set {
			_fbID = _fbName;
		}
	}
	
	public string FacebookFriends {
		get {
			return _fbFriends;
		}
		set {
			_fbFriends = value;
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
	
	public string ReplayData {
		get {
			return _replayData;
		}
		set {
			_replayData = value;
		}
	}

	#endregion properties
}
