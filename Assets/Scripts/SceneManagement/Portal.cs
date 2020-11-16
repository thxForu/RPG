using System;

using System.Collections;
using System.Collections.Generic;
using RPT.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int sceneToLoad = -1;
    [SerializeField] private DestinationIdentifier destination;
    [SerializeField] private float fadeOutTime = 1f;
    [SerializeField] private float fadeItTime = 1f;
    [SerializeField] private float fadeWaitTime = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        if (sceneToLoad <0)
        {
            Debug.LogError("Scene to load not set.");
            yield break;
        }

        DontDestroyOnLoad(gameObject);
        
        Fader fader = FindObjectOfType<Fader>();
        
        yield return fader.FadeOut(fadeOutTime);
        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        Portal otherPortal = GetOtherPortal();
        UpdatePlayer(otherPortal);
        
        yield return new WaitForSeconds(fadeWaitTime);
        yield return fader.FadeIn(fadeItTime);

        Destroy(gameObject);
    }

    private Portal GetOtherPortal()
    {
        foreach (var portal in FindObjectsOfType<Portal>())
        {
            if (portal == this) continue;
            if (portal.destination != destination) continue;
            return portal;
        }

        return null;
    }
    private void UpdatePlayer(Portal otherPortal)
    {
        GameObject player = GameObject.FindWithTag("Player");
        player.GetComponent<NavMeshAgent>().Warp(otherPortal.spawnPoint.position);
        player.transform.rotation = otherPortal.spawnPoint.rotation;
    }
}

enum DestinationIdentifier
{
    A,B,C,D,E,F,J
}
