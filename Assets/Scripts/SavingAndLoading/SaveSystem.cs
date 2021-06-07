using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Strungerhulder.Events.ScriptableObjects;
using Strungerhulder.Inventory.ScriptableObjects;

namespace Strungerhulder.SavingAndLoading
{
    [CreateAssetMenu(fileName = "SaveSystem", menuName = "Save System/Save System")]
    public class SaveSystem : ScriptableObject
    {
        public string saveFilename = "save.strungerhulder";
        public string backupSaveFilename = "save.strungerhulder.bak";
        public Save saveData = new Save();

        [SerializeField] private LoadEventChannelSO m_LoadLocation = default;
        [SerializeField] private ProtagonistInventory m_ProtagonistInventory;


        private void OnEnable()
        {
            m_LoadLocation.OnLoadingRequested += CacheLoadLocations;
        }

        private void OnDisable()
        {
            m_LoadLocation.OnLoadingRequested -= CacheLoadLocations;
        }


        private void CacheLoadLocations(GameSceneSO locationsToLoad, bool showLoadingScreen)
        {
            LocationSO locationSO = locationsToLoad as LocationSO;

            if (locationSO)
                saveData.m_LocationId = locationSO.Guid;

            SaveDataToDisk();
        }

        public bool LoadSaveDataFromDisk()
        {
            if (FileManager.LoadFromFile(saveFilename, out var json))
            {
                saveData.LoadFromJson(json);
                return true;
            }

            return false;
        }

        public IEnumerator LoadSavedInventory()
        {
            m_ProtagonistInventory.Items.Clear();

            foreach (var serializedItemStack in saveData.m_ItemStacks)
            {
                var loadItemOperationHandle = Addressables.LoadAssetAsync<Item>(serializedItemStack.itemGuid);
                yield return loadItemOperationHandle;

                if (loadItemOperationHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    var itemSO = loadItemOperationHandle.Result;
                    m_ProtagonistInventory.Add(itemSO, serializedItemStack.amount);
                }
            }
        }

        public void SaveDataToDisk()
        {
            saveData.m_ItemStacks.Clear();

            foreach (var itemStack in m_ProtagonistInventory.Items)
            {
                //	saveData._itemStacks.Add(new SerializedItemStack(itemStack.Item.Guid, itemStack.Amount));
            }

            if (FileManager.MoveFile(saveFilename, backupSaveFilename))
            {
                if (FileManager.WriteToFile(saveFilename, saveData.ToJson()))
                    Debug.Log("Save successful");
            }
        }

        public void WriteEmptySaveFile() => FileManager.WriteToFile(saveFilename, "");
    }
}
