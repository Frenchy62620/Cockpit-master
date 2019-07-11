using System;

namespace Cockpit.Core.Plugins.Wiimote
{
    public class Subscribable
    {
        public Subscribable(out Action trigger)
        {
            trigger = OnUpdate;
        }

        private void OnUpdate()
        {
            if (update != null)
                update();
        }

        public event Action update;
    }
}
