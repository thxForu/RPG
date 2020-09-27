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
        private Transform _target;
        private static readonly int Attack1 = Animator.StringToHash("attack");

        private float _timeSinceLastAttack = 0;
        private void Update()
        {
            _timeSinceLastAttack += Time.deltaTime;
            
            if (_target == null) return;
            
            if (_target != null && !GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.position);   
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            if (_timeSinceLastAttack > timeBetweenAttacks)
            {
                //This will trigger the Hit() event
                GetComponent<Animator>().SetTrigger(Attack1);
                _timeSinceLastAttack = 0;
               
            }
        }
        //Animation Event
        private void Hit()
        {
            Health healthComponent = _target.GetComponent<Health>();
            healthComponent.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange()
        {
            bool isInRange = Vector3.Distance(transform.position, _target.position) < weaponRange;
            return isInRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            _target = combatTarget.transform;
        }

        public void Cancel()
        {
            _target = null;
        }

        
    }
}
