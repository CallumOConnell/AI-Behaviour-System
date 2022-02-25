using UnityEngine;

namespace Interaction
{
    [CreateAssetMenu(fileName = "Interaction Data", menuName = "InteractionSystem/InteractionData")]
    public class InteractionData : ScriptableObject
    {
        public InteractableBase Interactable { get; set; }

        public void Interact()
        {
            Interactable.OnInteract();
            ResetData();
        }

        public bool IsSameInteractable(InteractableBase newInteractable) => Interactable == newInteractable;
        public bool IsEmpty() => Interactable == null;
        public void ResetData() => Interactable = null;
    }
}