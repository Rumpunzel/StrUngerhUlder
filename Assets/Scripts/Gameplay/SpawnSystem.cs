using System;
using System.Linq;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private int m_DefaultSpawnIndex = 0;

	[Header("Asset References")]
	[SerializeField] private Protagonist m_PlayerPrefab = default;
	[SerializeField] private TransformAnchor m_PlayerTransformAnchor = default;
	[SerializeField] private TransformEventChannelSO m_PlayerInstantiatedChannel = default;
	[SerializeField] private PathAnchor m_PathTaken = default;

	[Header("Scene References")]
	private Transform[] m_SpawnLocations;

	[Header("Scene Ready Event")]
	[SerializeField] private VoidEventChannelSO m_OnSceneReady = default; // Raised when the scene is loaded and set active

	
	private void OnEnable()
	{
		m_OnSceneReady.onEventRaised += SpawnPlayer;
	}

	private void OnDisable()
	{
		m_OnSceneReady.onEventRaised -= SpawnPlayer;
	}

	private void SpawnPlayer()
	{
		GameObject[] spawnLocationsGameObject = GameObject.FindGameObjectsWithTag("SpawnLocation");
		m_SpawnLocations = new Transform[spawnLocationsGameObject.Length];

		for (int i = 0; i < spawnLocationsGameObject.Length; ++i)
			m_SpawnLocations[i] = spawnLocationsGameObject[i].transform;

		Spawn(FindSpawnIndex(m_PathTaken?.Path ?? null));
	}

	private void Reset() => AutoFill();

	/// <summary>
	/// This function tries to autofill some of the parameters of the component, so it's easy to drop in a new scene
	/// </summary>
	[ContextMenu("Attempt Auto Fill")]
	private void AutoFill()
	{
		if (m_SpawnLocations == null || m_SpawnLocations.Length == 0)
			m_SpawnLocations = transform.GetComponentsInChildren<Transform>(true)
								.Where(t => t != this.transform)
								.ToArray();
	}

	private void Spawn(int spawnIndex)
	{
		Transform spawnLocation = GetSpawnLocation(spawnIndex, m_SpawnLocations);
		Protagonist playerInstance = InstantiatePlayer(m_PlayerPrefab, spawnLocation);

		m_PlayerInstantiatedChannel.RaiseEvent(playerInstance.transform); // The CameraSystem will pick this up to frame the player
		m_PlayerTransformAnchor.Transform = playerInstance.transform;
	}

	private Transform GetSpawnLocation(int index, Transform[] spawnLocations)
	{
		if (spawnLocations == null || spawnLocations.Length == 0)
			throw new Exception("No spawn locations set.");

		index = Mathf.Clamp(index, 0, spawnLocations.Length - 1);
		return spawnLocations[index];
	}

	private int FindSpawnIndex(PathSO pathTaken)
	{
		if (pathTaken == null)
			return m_DefaultSpawnIndex;

		int index = Array.FindIndex(m_SpawnLocations, element =>
			element?.GetComponent<LocationEntrance>()?.EntrancePath == pathTaken
		);

		return (index < 0) ? m_DefaultSpawnIndex : index;
	}

	private Protagonist InstantiatePlayer(Protagonist playerPrefab, Transform spawnLocation)
	{
		if (playerPrefab == null)
			throw new Exception("Player Prefab can't be null.");

		Protagonist playerInstance = Instantiate(playerPrefab, spawnLocation.position, spawnLocation.rotation);

		return playerInstance;
	}
}
