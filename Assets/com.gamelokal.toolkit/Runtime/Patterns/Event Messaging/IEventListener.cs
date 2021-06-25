namespace GameLokal.Toolkit.Pattern
{
    public interface IEventListener<T> : IEventListenerBase
    {
        void OnEvent(T e);
    }
}