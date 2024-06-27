using Data;

namespace Services.Progress
{
    public interface IPersistentProgressService
    {
        PlayerProgress PlayerProgress { get; set; }
    }
}