namespace Observer
{
    public interface IObserver
    {
        void OnNotify(Subject sub);
    }
}