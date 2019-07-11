namespace Cockpit.Core.Persistence
{
    public class PersistanceManager : IPersistanceManager
    {
        private readonly ISettingsManager settingsManager;

        public PersistanceManager(ISettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        public bool Load()
        {
            var result = settingsManager.Load();

            return result;
        }

        public void Save()
        {
            settingsManager.Save();
        }
    }
}
