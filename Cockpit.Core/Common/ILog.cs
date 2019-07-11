using System;

namespace Cockpit.Core.Common
{
    public interface ILog
    {
        void Error(Exception e);
    }
}