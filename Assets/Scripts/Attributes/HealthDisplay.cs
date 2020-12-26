using System;
using UnityEngine;
using UnityEngine.UI;

namespace Attributes
{
    public class HealthDisplay: MonoBehaviour
    {
        private Health health;
        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}", health.GetHealthPoints(), health.GetMaxHealthPoints());
        }
    }
}