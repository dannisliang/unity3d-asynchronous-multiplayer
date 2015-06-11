using UnityEngine;
using System.Collections;

/// <summary>
/// Spawning points.
/// </summary>
[System.Serializable]
public class SpawningPoints : ScriptableObject {
	public string objectName = "Spawned Items In A Screen";
	public SpawnedItem[] spawnedItems;
	public Vector3[] spawnedPositions;
}
