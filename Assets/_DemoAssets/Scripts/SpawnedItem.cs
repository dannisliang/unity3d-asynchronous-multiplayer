using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpawnedItem : ScriptableObject {
	public string objectName = "New Item";
	public GameObject itemPrefab;
}
