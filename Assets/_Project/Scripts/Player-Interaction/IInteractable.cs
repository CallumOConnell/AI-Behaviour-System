namespace Interaction
{
    public interface IInteractable
    {
        bool IsInteractable { get; }

        string ToolTip { get; }

        void OnInteract();
    }
}