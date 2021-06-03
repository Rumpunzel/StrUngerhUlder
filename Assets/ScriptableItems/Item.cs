using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    [CreateAssetMenu(menuName = "Resource", fileName = "ResourceName.asset")]
    [System.Serializable]
    public class Item : ScriptableObject
    {
        public string itemName;
        public Image icon;
        public GameObject physicalRepresentation;

        public float weight = 1f;
    }
}
