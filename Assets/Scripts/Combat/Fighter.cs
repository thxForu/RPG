using System;
using System.Collections.Generic;
using Attributes;
using Core;
using GameDevTV.Utils;
using Movement;
using Saving;
using Stats;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider
    {
        
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private WeaponConfig defaultWeaponConfig;
        
        private Health _target;
        LazyValue<WeaponConfig> _currentWeapon;
        private float _timeSinceLastAttack = Mathf.Infinity;
        
        private static readonly int AttackT = Animator.StringToHash("attack");
        private static readonly int StopAttackT = Animator.StringToHash("stopAttack");

        private void Awake()
        {
            _currentWeapon = new LazyValue<WeaponConfig>(SetupDefaultWeapon);
        }

        

        private void Start()
        {
            _currentWeapon.ForceInit();
        }
        
        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;
            if (_target.IsDead()) return;
            
            if (_target != null && !GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.transform.position, 1f);  
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }
        private WeaponConfig SetupDefaultWeapon()
        {
            AttachWeapon(defaultWeaponConfig);
            return defaultWeaponConfig;
        }
        public void EquipWeapon(WeaponConfig weaponConfig)
        {
            _currentWeapon.value = weaponConfig;
            AttachWeapon(weaponConfig);
        }

        private void AttachWeapon(WeaponConfig weaponConfig)
        {
            Animator animator = GetComponent<Animator>();
            weaponConfig.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        public Health GetTarget()
        {
            return _target;
        }
        private void AttackBehavior()
        {
            transform.LookAt(_target.transform);
            if (_timeSinceLastAttack > timeBetweenAttacks)
            {
                //This will trigger the Hit() event
                TriggerAttack();
                _timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger(StopAttackT);
            GetComponent<Animator>().SetTrigger(AttackT);
        } 

        //Animation Event
        private void Hit()
        {
            if (_target == null) return;

            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (_currentWeapon.value.HasProjectile())
            {
                _currentWeapon.value.LaunchProjectile(rightHandTransform, leftHandTransform, _target, gameObject, damage);
            }
            else
            {
                _target.TakeDamage(gameObject, damage);
            }
        }

        private void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            bool isInRange = Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.value.GetRange();
            return isInRange;
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        public bool CanAttack(GameObject combatTarget) 
        {
            if (combatTarget == null) return false;
            
            Health targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead();
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
            GetComponent<Mover>().Cancel();
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger(AttackT);
            GetComponent<Animator>().SetTrigger(StopAttackT);
        }
        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {

            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.GetDamage();
                
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.GetPercentageBonus();
            }
        }

        public object CaptureState()
       {
           return _currentWeapon.value.name;
       }

       public void RestoreState(object state)
       {
           string weaponName = (string) state;
           WeaponConfig weaponConfig = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
           EquipWeapon(weaponConfig);
       }
    }
}
