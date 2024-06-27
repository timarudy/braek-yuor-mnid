namespace Services.LevelAccess
{
    public interface ICodeAccessChecker
    {
        int CodeDecryptorsCompleted { get; set; }
        bool IsPigRescued { get; set; }
        int ImagesGuessed { get; set; }
        bool CheckAccess();
    }
}