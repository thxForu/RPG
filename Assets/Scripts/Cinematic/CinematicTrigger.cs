using UnityEngine;
using UnityEngine.Playables;

namespace Cinematic
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool alreadyTriggered = false;
        private void OnTriggerEnter(Collider other)
        {
            if (!alreadyTriggered && other.CompareTag("Player"))
            {
                alreadyTriggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
