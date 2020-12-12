using UnityEngine;

namespace Resources
{
    public class Experience : MonoBehaviour
    {
        [SerializeField] private float experiencePoints;

        public void GainExperience(float experience)
        {
            experiencePoints += experience;
        }
    }
}