using RPG.Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float weaponDamage = 5f;
        
        private Health _target;
        private float _timeSinceLastAttack = Mathf.Infinity;
        
        private static readonly int AttackT = Animator.StringToHash("attack");
        private static readonly int StopAttackT = Animator.StringToHash("stopAttack");

        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;
            if (_target.IsDead()) return;
            
            if (_target != null && !GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.transform.position);  
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
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
            _target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            bool isInRange = Vector3.Distance(transform.position, _target.transform.position) < weaponRange;
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
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger(AttackT);
            GetComponent<Animator>().SetTrigger(StopAttackT);
        }
    }
}
