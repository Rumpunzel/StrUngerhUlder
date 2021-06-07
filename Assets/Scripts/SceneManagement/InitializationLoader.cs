using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Strungerhulder.Events.ScriptableObjects;

namespace Strungerhulder
{
    /// <summary>
    /// This class is responsible for starting the game by loading the persistent managers scene 
    /// and raising the event to load the Main Menu
    /// </summary>
    public class InitializationLoader : MonoBehaviour
    {
        [SerializeField] private GameSceneSO m_ManagersScene = default;
        [SerializeField] private GameSceneSO m_MenuToLoad = default;

        [Header("Broadcasting on")]
        //[SerializeField] private AssetReference m_MenuLoadChannelAsset = default;
        [SerializeField] private LoadEventChannelSO m_MenuLoadChannel = default;


        private void Start()
        {
            //Load the persistent managers scene
            m_ManagersScene.sceneReference.LoadSceneAsync(LoadSceneMode.Additive, true).Completed += LoadEventChannel;
        }


        private void LoadEventChannel(AsyncOperationHandle<SceneInstance> obj)
        {
            //m_MenuLoadChannelAsset.LoadAssetAsync<LoadEventChannelSO>().Completed += LoadMainMenu;
            LoadMainMenu();
        }

        private void LoadMainMenu()//AsyncOperationHandle<LoadEventChannelSO> obj)
        {
            /*print("Initializer");
            print(m_MenuLoadChannel.GetHashCode());
            print(m_MenuLoadChannel.GetInstanceID());
            print(m_MenuLoadChannel.ToString());
            print(m_MenuLoadChannel.OnLoadingRequested);*/
            m_MenuLoadChannel.RaiseEvent(m_MenuToLoad);
            //print(m_MenuLoadChannel.OnLoadingRequested);
            SceneManager.UnloadSceneAsync(0); // Main is the only scene in BuildSettings, thus it has index 0
        }
    }
}
