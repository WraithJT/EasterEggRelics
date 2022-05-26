using UnityModManagerNet;

namespace EasterEggRelics.Utilities
{
    public class Settings : UnityModManager.ModSettings
    {
        public bool useEasterEggRelics = true;

        public override void Save(UnityModManager.ModEntry modEntry)
        {
            Save(this, modEntry);
        }
    }
}
