using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Strungerhulder.Events.ScriptableObjects;
using Strungerhulder.SceneManagement.ScriptableObjects;

namespace Strungerhulder.SceneManagement
{
    /// <summary>
    /// Allows a "cold start" in the editor, when pressing Play and not passing from the Initialisation scene.
    /// </summary> 
    public class EditorColdStartup : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private GameSceneSO m_ThisSceneSO = default;
        [SerializeField] private GameSceneSO m_PersistentManagersSO = default;
        //[SerializeField] private AssetReference m_NotifyColdStartupChannelAsset = default;
        [SerializeField] private LoadEventChannelSO m_NotifyColdStartupChannel = default;
        [SerializeField] private VoidEventChannelSO m_OnSceneReadyChannel = default;


        private void Start()
        {
            if (!SceneManager.GetSceneByName(m_PersistentManagersSO.sceneReference.editorAsset.name).isLoaded)
                m_PersistentManagersSO.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
        }

        private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        {
            //m_NotifyColdStartupChannelAsset.LoadAssetAsync<LoadEventChannelSO>().Completed += OnNotifyChannelLoaded;
            OnNotifyChannelLoaded();
        }

        private void OnNotifyChannelLoaded()//AsyncOperationHandle<LoadEventChannelSO> obj)
        {
            if (m_ThisSceneSO != null)
            {
                //obj.Result.RaiseEvent(m_ThisSceneSO);
                m_NotifyColdStartupChannel.RaiseEvent(m_ThisSceneSO);
            }
            else
            {
                //Raise a fake scene ready event, so the player is spawned
                m_OnSceneReadyChannel.RaiseEvent();
                //When this happens, the player won't be able to move between scenes because the SceneLoader has no conception of which scene we are in
            }
        }
#endif
    }
}
