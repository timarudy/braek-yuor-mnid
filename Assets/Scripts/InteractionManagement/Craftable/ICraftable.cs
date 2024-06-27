namespace InteractionManagement.Craftable
{
    public interface ICraftable
    {
        CraftableType CraftableName { get; }
        void DestroySelf();
    }
}