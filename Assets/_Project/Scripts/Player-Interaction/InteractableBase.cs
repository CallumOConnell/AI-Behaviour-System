using UnityEngine;

namespace Interaction
{
    public class InteractableBase : MonoBehaviour, IInteractable
    {
        [Space, Header("Interacable Settings")]

        [SerializeField] private bool _isInteractable = true;

        [SerializeField] private string _tooltip = "";

        public bool IsInteractable => _isInteractable;

        public string ToolTip => _tooltip;

        public virtual void OnInteract() {}
    }
}