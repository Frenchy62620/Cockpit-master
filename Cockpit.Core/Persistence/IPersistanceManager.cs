namespace Cockpit.Core.Persistence
{
    public interface IPersistanceManager
    {
        bool Load();
        void Save();
    }
}