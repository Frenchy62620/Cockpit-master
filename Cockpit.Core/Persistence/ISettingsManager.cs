using System.Collections.Generic;
using Cockpit.Core.Contracts;
using Cockpit.Core.Model;

namespace Cockpit.Core.Persistence
{
    public interface ISettingsManager
    {
        bool Load();
        void Save();
        Settings Settings { get; }
    }
}