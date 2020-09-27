using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour
    {
        [SerializeField] private float weaponRange = 2f;
        private Transform _target;
        private void Update()
        {
            if (_target == null) return;
            
            if (_target != null && !GetIsInRange())
            {
                GetComponent<Mover>().MoveTo(_target.position);   
            }
            else
            {
                GetComponent<Mover>().Stop();
            }
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
