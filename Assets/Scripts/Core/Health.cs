using RPG.Saving;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float healthPoint = 100f;
        private static readonly int DieTrigger = Animator.StringToHash("die");
        
        private bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }
        public void TakeDamage(float damage)
        {

            healthPoint = Mathf.Max(healthPoint - damage, 0);
            if (healthPoint == 0)
            {
                Die();
            }
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger(DieTrigger);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState()
        {
            return healthPoint;
        }

        public void RestoreState(object state)
        {
            healthPoint = (float) state;
            if (healthPoint == 0)
            {
                Die();
            }
        }
    }
}