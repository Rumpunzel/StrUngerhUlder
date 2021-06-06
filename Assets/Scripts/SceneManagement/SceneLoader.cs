using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

/// <summary>
/// This class manages the scene loading and unloading.
/// </summary>
public class SceneLoader : MonoBehaviour
{
	[SerializeField] private GameSceneSO m_GameplayScene = default;

	[Header("Load Events")]
	[SerializeField] private LoadEventChannelSO m_LoadLocation = default;
	[SerializeField] private LoadEventChannelSO m_LoadMenu = default;
	[SerializeField] private LoadEventChannelSO m_ColdStartupLocation = default;

	[Header("Broadcasting on")]
	[SerializeField] private BoolEventChannelSO m_ToggleLoadingScreen = default;
	[SerializeField] private VoidEventChannelSO m_OnSceneReady = default;

	private AsyncOperationHandle<SceneInstance> m_LoadingOperationHandle;
	private AsyncOperationHandle<SceneInstance> m_GameplayManagerLoadingOpHandle;

	//Parameters coming from scene loading requests
	private GameSceneSO m_SceneToLoad;
	private GameSceneSO m_CurrentlyLoadedScene;
	private bool m_ShowLoadingScreen;

	private SceneInstance m_GameplayManagerSceneInstance = new SceneInstance();


	private void OnEnable()
	{
		print("Scene Loader");
        print(m_LoadMenu.GetHashCode());
        print(m_LoadMenu.GetInstanceID());
        print(m_LoadMenu.ToString());
		print(m_LoadMenu.OnLoadingRequested);
		m_LoadLocation.OnLoadingRequested += LoadLocation;
		m_LoadMenu.OnLoadingRequested += LoadMenu;
		print(m_LoadMenu.OnLoadingRequested);
#if UNITY_EDITOR
		m_ColdStartupLocation.OnLoadingRequested += LocationColdStartup;
#endif
	}

	private void OnDisable()
	{
		m_LoadLocation.OnLoadingRequested -= LoadLocation;
		m_LoadMenu.OnLoadingRequested -= LoadMenu;
		
#if UNITY_EDITOR
		m_ColdStartupLocation.OnLoadingRequested -= LocationColdStartup;
#endif
	}

#if UNITY_EDITOR
	/// <summary>
	/// This special loading function is only used in the editor, when the developer presses Play in a Location scene, without passing by Initialisation.
	/// </summary>
	private void LocationColdStartup(GameSceneSO currentlyOpenedLocation, bool showLoadingScreen)
	{
		m_CurrentlyLoadedScene = currentlyOpenedLocation;

		if (m_CurrentlyLoadedScene.sceneType == GameSceneSO.GameSceneType.Location)
		{
			//Gameplay managers is loaded synchronously
			m_GameplayManagerLoadingOpHandle = m_GameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
			m_GameplayManagerLoadingOpHandle.WaitForCompletion();
			m_GameplayManagerSceneInstance = m_GameplayManagerLoadingOpHandle.Result;

			StartGameplay();
		}
	}
#endif

	/// <summary>
	/// This function loads the location scenes passed as array parameter
	/// </summary>
	private void LoadLocation(GameSceneSO locationToLoad, bool showLoadingScreen)
	{
		print("Loading Location");
		m_SceneToLoad = locationToLoad;
		m_ShowLoadingScreen = showLoadingScreen;

		//In case we are coming from the main menu, we need to load the Gameplay manager scene first
		if (m_GameplayManagerSceneInstance.Scene == null
			|| !m_GameplayManagerSceneInstance.Scene.isLoaded)
		{
			m_GameplayManagerLoadingOpHandle = m_GameplayScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true);
			m_GameplayManagerLoadingOpHandle.Completed += OnGameplayMangersLoaded;
		}
		else
			UnloadPreviousScene();
	}

	private void OnGameplayMangersLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		m_GameplayManagerSceneInstance = m_GameplayManagerLoadingOpHandle.Result;

		UnloadPreviousScene();
	}

	/// <summary>
	/// Prepares to load the main menu scene, first removing the Gameplay scene in case the game is coming back from gameplay to menus.
	/// </summary>
	private void LoadMenu(GameSceneSO menuToLoad, bool showLoadingScreen)
	{
		m_SceneToLoad = menuToLoad;
		m_ShowLoadingScreen = showLoadingScreen;

		//In case we are coming from a Location back to the main menu, we need to get rid of the persistent Gameplay manager scene
		if (m_GameplayManagerSceneInstance.Scene != null
			&& m_GameplayManagerSceneInstance.Scene.isLoaded)
			Addressables.UnloadSceneAsync(m_GameplayManagerLoadingOpHandle, true);

		UnloadPreviousScene();
	}

	/// <summary>
	/// In both Location and Menu loading, this function takes care of removing previously loaded scenes.
	/// </summary>
	private void UnloadPreviousScene()
	{
		if (m_CurrentlyLoadedScene != null) //would be null if the game was started in Initialisation
		{
			if (m_CurrentlyLoadedScene.sceneReference.OperationHandle.IsValid())
			{
				//Unload the scene through its AssetReference, i.e. through the Addressable system
				m_CurrentlyLoadedScene.sceneReference.UnLoadScene();
			}
#if UNITY_EDITOR
			else
			{
				//Only used when, after a "cold start", the player moves to a new scene
				//Since the AsyncOperationHandle has not been used (the scene was already open in the editor),
				//the scene needs to be unloaded using regular SceneManager instead of as an Addressable
				SceneManager.UnloadSceneAsync(m_CurrentlyLoadedScene.sceneReference.editorAsset.name);
			}
#endif
		}

		LoadNewScene();
	}

	/// <summary>
	/// Kicks off the asynchronous loading of a scene, either menu or Location.
	/// </summary>
	private void LoadNewScene()
	{
		if (m_ShowLoadingScreen)
			m_ToggleLoadingScreen.RaiseEvent(true);

		m_LoadingOperationHandle = m_SceneToLoad.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true, 0);
		m_LoadingOperationHandle.Completed += OnNewSceneLoaded;
	}

	private void OnNewSceneLoaded(AsyncOperationHandle<SceneInstance> obj)
	{
		//Save loaded scenes (to be unloaded at next load request)
		m_CurrentlyLoadedScene = m_SceneToLoad;
		SetActiveScene();

		if (m_ShowLoadingScreen)
			m_ToggleLoadingScreen.RaiseEvent(false);
	}

	/// <summary>
	/// This function is called when all the scenes have been loaded
	/// </summary>
	private void SetActiveScene()
	{
		Scene scene = ((SceneInstance)m_LoadingOperationHandle.Result).Scene;
		SceneManager.SetActiveScene(scene);

		//LightProbes.TetrahedralizeAsync();

		StartGameplay();
	}

	private void StartGameplay() => m_OnSceneReady.RaiseEvent(); //Spawn system will spawn the Protagonist

	private void ExitGame()
	{
		Application.Quit();
		Debug.Log("Exit!");
	}
}
