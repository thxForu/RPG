using System;
using Control;
using Core;
using UnityEngine;
using UnityEngine.Playables;

namespace Cinematic
{
    public class CinematicControlRemover : MonoBehaviour
    {
        private GameObject player;

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }
        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
        } 
        private void DisableControl(PlayableDirector pd)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector pd)
        {
            player.GetComponent<PlayerController>().enabled = true;
            print("EnableControl");
        }
    }
}
