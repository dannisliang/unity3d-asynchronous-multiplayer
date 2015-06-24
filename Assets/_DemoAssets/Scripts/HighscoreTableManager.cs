using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighscoreTableManager : MonoBehaviour {

	public GameObject[] highscorePlayers;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < highscorePlayers.Length; i++) {
			highscorePlayers[i].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}	

	public void CreateTable(PlayerData[] topPlayerDatas) {
		for (int i = 0; i < topPlayerDatas.Length; i++) {
			highscorePlayers[i].SetActive(true);
			
			highscorePlayers[i].GetComponent<PlayerHighscore>().SetInfo(topPlayerDatas[i].FacebookName, topPlayerDatas[i].Score);
		}
	}
}
