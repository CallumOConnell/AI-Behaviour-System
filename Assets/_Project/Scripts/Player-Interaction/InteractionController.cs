using Input;
using UnityEngine;

namespace Interaction
{
    public class InteractionController : MonoBehaviour
    {
        [Space, Header("Data")]
        [SerializeField] private InteractionInputData _interactionInputData;
        [SerializeField] private InteractionData _interactionData;

        [Space, Header("Ray Settings")]
        [SerializeField] private float _rayDistance = 0f;
        [SerializeField] private float _raySphereRadius = 0f;
        [SerializeField] private LayerMask _interactableLayer = ~0;

        [Space, Header("UI Settings")]
        [SerializeField] private InteractionUI _interactionUI;

        [Space, Header("Camera Settings")]
        [SerializeField] private Camera _camera;

        private Controls _inputActions;

        private bool _interacting;

        private void Awake() => _inputActions = InputManager.InputActions;

        private void Update()
        {
            CheckForInput();
            CheckForInteractable();
            CheckForInteractableInput();
        }

        private void CheckForInput()
        {
            if (_inputActions != null)
            {
                _interactionInputData.InteractedClicked = _inputActions.Player.Interact.triggered;
            }
        }

        private void CheckForInteractable()
        {
            var ray = new Ray(_camera.transform.position, _camera.transform.forward);

            var hitSomething = Physics.SphereCast(ray, _raySphereRadius, out RaycastHit hitInfo, _rayDistance, _interactableLayer);

            if (hitSomething)
            {
                var interactable = hitInfo.transform.GetComponent<InteractableBase>();

                if (interactable != null)
                {
                    _interactionUI.SetToolTip(interactable.ToolTip);
                    _interactionUI.SetTooltipActiveState(true);

                    if (_interactionData.IsEmpty())
                    {
                        _interactionData.Interactable = interactable;
                    }
                    else
                    {
                        if (!_interactionData.IsSameInteractable(interactable))
                        {
                            _interactionData.Interactable = interactable;
                        }
                    }
                }
            }
            else
            {
                _interactionData.ResetData();
                _interactionUI.SetTooltipActiveState(false);
            }

            Debug.DrawRay(ray.origin, ray.direction * _rayDistance, hitSomething ? Color.green : Color.red);
        }

        private void CheckForInteractableInput()
        {
            if (_interactionData.IsEmpty()) return;

            if (_interactionInputData.InteractedClicked)
            {
                _interacting = true;
            }

            if (_interacting)
            {
                if (!_interactionData.Interactable.IsInteractable) return;

                _interactionData.Interact();
                _interactionUI.SetTooltipActiveState(false);
                _interacting = false;
            }
        }
    }
}