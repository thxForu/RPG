using UnityEngine;

namespace Core
{
    public class PeristentObjectSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject persistentPrefab;
        
        
        private static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;
            
            SpawnPersistentObjects();
            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(persistentPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }   
}
