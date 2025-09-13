using UnityEngine;

namespace TLS.Progression
{
    // Attach to Tower: auto-enable abilities when unlocked
    public class AbilityActivator : MonoBehaviour
    {
        public bool enableSeekerOnUnlock = true;
        public bool enableMultishotOnUnlock = true;
        public bool enableAoEEMIOnUnlock = true;

        private void Start()
        {
            TryEnable<TLS.Abilities.Seeker>(UnlocksService.Ability.Seeker, enableSeekerOnUnlock);
            TryEnable<TLS.Abilities.Multishot>(UnlocksService.Ability.Multishot, enableMultishotOnUnlock);
            TryEnable<TLS.Abilities.AoEEMI>(UnlocksService.Ability.AoEEMI, enableAoEEMIOnUnlock);
        }

        private void TryEnable<T>(UnlocksService.Ability a, bool shouldEnable) where T : MonoBehaviour
        {
            var comp = GetComponent<T>();
            bool unlocked = UnlocksService.IsUnlocked(a);
            if (!shouldEnable) unlocked = false;
            if (unlocked)
            {
                if (comp == null) comp = gameObject.AddComponent<T>();
                comp.enabled = true;
            }
            else
            {
                if (comp != null) comp.enabled = false;
            }
        }
    }
}

