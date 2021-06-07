using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Strungerhulder.Events.ScriptableObjects;
using Strungerhulder.SavingAndLoading;
using Strungerhulder.SceneManagement.ScriptableObjects;

namespace Strungerhulder.SceneManagement
{
    /// <summary>
    /// This class contains the function to call when play button is pressed
    /// </summary>
    public class StartGame : MonoBehaviour
    {
        [SerializeField] private GameSceneSO m_LocationsToLoad;
        [SerializeField] private bool m_ShowLoadScreen = default;
        [SerializeField] private SaveSystem m_SaveSystem = default;

        [Header("Broadcasting on ")]
        [SerializeField] private LoadEventChannelSO m_StartGameEvent = default;

        [Header("Listening to")]
        [SerializeField] private VoidEventChannelSO m_StartNewGameEvent = default;
        [SerializeField] private VoidEventChannelSO m_ContinueGameEvent = default;


        private bool m_HasSaveData;


        private void Start()
        {
            m_HasSaveData = m_SaveSystem.LoadSaveDataFromDisk();
            m_StartNewGameEvent.onEventRaised += StartNewGame;
            m_ContinueGameEvent.onEventRaised += ContinuePreviousGame;
        }

        private void OnDestroy()
        {
            m_StartNewGameEvent.onEventRaised -= StartNewGame;
            m_ContinueGameEvent.onEventRaised -= ContinuePreviousGame;
        }


        private void StartNewGame()
        {
            m_HasSaveData = false;
            m_SaveSystem.WriteEmptySaveFile();
            //Start new game
            m_StartGameEvent.RaiseEvent(m_LocationsToLoad, m_ShowLoadScreen);
        }

        private void ContinuePreviousGame() => StartCoroutine(LoadSaveGame());

        private void OnResetSaveDataPress() => m_HasSaveData = false;

        private IEnumerator LoadSaveGame()
        {
            yield return StartCoroutine(m_SaveSystem.LoadSavedInventory());

            var locationGuid = m_SaveSystem.saveData.m_LocationId;
            var asyncOperationHandle = Addressables.LoadAssetAsync<LocationSO>(locationGuid);
            yield return asyncOperationHandle;

            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                LocationSO locationSO = asyncOperationHandle.Result;
                m_StartGameEvent.RaiseEvent(locationSO, m_ShowLoadScreen);
            }
        }
    }
}
