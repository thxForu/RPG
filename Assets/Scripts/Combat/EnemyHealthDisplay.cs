using System;
using Resources;
using UnityEngine;
using UnityEngine.UI;

namespace Combat
{
    public class EnemyHealthDisplay: MonoBehaviour
    {
        private Fighter fighter;
        private Text healthLabile;
        
        private void Awake()
        {
            healthLabile = GetComponent<Text>();
            fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {

            if (fighter.GetTarget() == null)
            {
                healthLabile.text = "N/A";
                return;
            }
            Health health = fighter.GetTarget();
            healthLabile.text = String.Format("{0:0}%", health.GetPercentage());
        }
    }
}