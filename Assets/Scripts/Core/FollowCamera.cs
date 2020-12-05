using UnityEngine;

namespace Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] private Transform target;

        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}

