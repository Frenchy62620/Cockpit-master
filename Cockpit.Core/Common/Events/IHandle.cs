namespace Cockpit.Core.Common.Events
{
    public interface IHandle<in T> where T : class
    {
        void Handle(T message);
    }
}
