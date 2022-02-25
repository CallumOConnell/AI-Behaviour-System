using UnityEngine;

namespace Interaction
{
    [CreateAssetMenu(fileName = "Interaction Input Data", menuName = "InteractionSystem/InputData")]
    public class InteractionInputData : ScriptableObject
    {
        public bool InteractedClicked { get; set; }

        public void ResetInput()
        {
            InteractedClicked = false;
        }
    }
}