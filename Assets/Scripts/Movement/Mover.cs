using Core;
using Saving;
using UnityEngine;
using UnityEngine.AI;

namespace Movement
{
    public class Mover : MonoBehaviour,IAction, ISaveable 
    {
        [SerializeField] private Transform target;
        [SerializeField] private float maxSpeed = 6f;
        private NavMeshAgent _navMeshAgent;
        private Health _health;

        public void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
        }

        private void Update()
        {
            _navMeshAgent.enabled = !_health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            _navMeshAgent.isStopped = false;

        }

        public void Cancel()
        {
            _navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        { 
            Vector3 velocity = _navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
        
        [System.Serializable]
        struct MoverSaveData
        {
            //public SerializableVector3 position;
            //public SerializableVector3 rotation;
        }
        
        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            SerializableVector3 position = (SerializableVector3) state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}
