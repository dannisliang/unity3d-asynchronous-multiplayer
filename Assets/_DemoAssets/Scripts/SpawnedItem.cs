using UnityEngine;
using System.Collections;

/// <summary>
/// Spawned item.
/// </summary>
[System.Serializable]
public class SpawnedItem : ScriptableObject {
	public string objectName = "New Item";
	public GameObject itemPrefab;
}
