using UnityEngine;
using TMPro;

namespace Interaction
{
    public class InteractionUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetToolTip(string tooltip) => _text.SetText(tooltip);

        public void SetTooltipActiveState(bool state) => _text.gameObject.SetActive(state);
    }
}