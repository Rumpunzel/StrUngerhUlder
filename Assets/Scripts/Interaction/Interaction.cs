using UnityEngine;

namespace Strungerhulder.Interactions
{
    public class Interaction
    {
        public InteractionType type;
        public GameObject interactableObject;

        public Interaction(InteractionType t, GameObject obj)
        {
            type = t;
            interactableObject = obj;
        }
    }
}
