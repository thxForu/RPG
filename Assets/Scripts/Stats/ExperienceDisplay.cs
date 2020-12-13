using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stats
{
    public class ExperienceDisplay: MonoBehaviour
    {
        private Experience experience;
        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
        }

        private void Update()
        {
            GetComponent<Text>().text = String.Format("{0:0}", experience.GetPoints());
        }
    }
}