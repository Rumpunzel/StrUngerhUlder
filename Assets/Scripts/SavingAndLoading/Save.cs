using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strungerhulder.SavingAndLoading
{
    /// <summary>
    /// This class contains all the variables that will be serialized and saved to a file.<br/>
    /// Can be considered as a save file structure or format.
    /// </summary>
    [Serializable]
    public class Save
    {
        // This is test data, written according to TestScript.cs class
        // This will change according to whatever data that needs to be stored

        // The variables need to be public, else we would have to write trivial getter/setter functions.
        public string m_LocationId;
        public List<SerializedItemStack> m_ItemStacks = new List<SerializedItemStack>();

        public string ToJson() => JsonUtility.ToJson(this);
        public void LoadFromJson(string json) => JsonUtility.FromJsonOverwrite(json, this);
    }
}
