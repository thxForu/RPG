﻿using Core;
using Movement;
using Resources;
using Saving;
using UnityEngine;

namespace Combat
{
    public class Fighter : MonoBehaviour, IAction, ISaveable
    {
        
        [SerializeField] private float timeBetweenAttacks = 1f;
        [SerializeField] private Transform rightHandTransform;
        [SerializeField] private Transform leftHandTransform;
        [SerializeField] private Weapon defaultWeapon;
        
        private Health _target;
        private Weapon _currentWeapon;
        private float _timeSinceLastAttack = Mathf.Infinity;
        
        private static readonly int AttackT = Animator.StringToHash("attack");
        private static readonly int StopAttackT = Animator.StringToHash("stopAttack");

        private void Start()
        {
            if (_currentWeapon == null)
            {
                EquipWeapon(defaultWeapon);
            }
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
        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
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
            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, _target, gameObject);
            }
            else
            {
                _target.TakeDamage(gameObject, _currentWeapon.GetDamage());
            }
        }

        private void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            bool isInRange = Vector3.Distance(transform.position, _target.transform.position) < _currentWeapon.GetRange();
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

       public object CaptureState()
       {
           return _currentWeapon.name;
       }

       public void RestoreState(object state)
       {
           string weaponName = (string) state;
           Weapon weapon = UnityEngine.Resources.Load<Weapon>(weaponName);
           EquipWeapon(weapon);
       }
    }
}
