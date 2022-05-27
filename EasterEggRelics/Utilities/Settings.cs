using UnityModManagerNet;

namespace RelicsOfTheRighteous.Utilities
{
    public class Settings : UnityModManager.ModSettings
    {
        public bool useRelicsOfTheRighteous = true;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
