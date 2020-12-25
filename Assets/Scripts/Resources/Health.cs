using System;
using Core;
using GameDevTV.Utils;
using Saving;
using Stats;
using UnityEngine;
using UnityEngine.Events;

namespace Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float regenerationPercentage = 70;

        [SerializeField] private TakeDamageEvent takeDamage;

        
        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {
            
        }
        LazyValue<float> healthPoint;
        private static readonly int DieTrigger = Animator.StringToHash("die");
        
        private bool isDead;

        private BaseStats stats;
        private void Awake()
        {
            stats = GetComponent<BaseStats>();

            healthPoint = new LazyValue<float>(GetInitialHealth);
            
        }

        private void Start()
        {
            healthPoint.ForceInit();
        }

        private float GetInitialHealth()
        {
            return stats.GetStat(Stat.Health);
        }

        private void OnEnable()
        {
            stats.onLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            stats.onLevelUp -= RegenerateHealth;
        }


        public bool IsDead()
        {
            return isDead;
        }
        public void TakeDamage(GameObject instigator, float damage)
        {
            print(gameObject.name +" took damage: " + damage);
            healthPoint.value = Mathf.Max(healthPoint.value - damage, 0);
            if (healthPoint.value == 0)
            {
                Die();
                AwardExperience(instigator);
            }
            else
            {
                takeDamage.Invoke(damage);
            }
        }

        public float GetHealthPoints()
        {
            return healthPoint.value;
        }

        public float GetMaxHealthPoints()
        {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return 100 * (healthPoint.value / GetComponent<BaseStats>().GetStat(Stat.Health));
        }
        private void RegenerateHealth()
        {
            float regenHealthPoints = GetComponent<BaseStats>().GetStat(Stat.Health) * (regenerationPercentage/100);
            healthPoint.value = Mathf.Max(healthPoint.value, regenHealthPoints);
        }

        private void Die()
        {
            if (isDead) return;
            isDead = true;
            GetComponent<Animator>().SetTrigger(DieTrigger);
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
        

        private void AwardExperience(GameObject instigator)
        {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience is null) return;
            
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        public object CaptureState()
        {
            return healthPoint;
        }

        public void RestoreState(object state)
        {
            healthPoint.value = (float) state;
            if (healthPoint.value <= 0)
            {
                Die();
            }
        }
    }
}