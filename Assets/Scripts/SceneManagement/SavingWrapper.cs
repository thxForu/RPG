using System;
using System.Collections;
//using RPG.Saving;
using RPT.SceneManagement;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string defaultSaveFile = "save";
        [SerializeField] private float fadeInTime = 0.3f;

        private void Awake()
        {
            //StartCoroutine(LoadLastScene());
        }

        private IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            //yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            yield return fader.FadeIn(fadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
            }
            
        }

        public void Load()
        {
            //GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
        public void Save()
        {
            //GetComponent<SavingSystem>().Save(defaultSaveFile);
        }
        public void Delete()
        {
            //GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }
    }
}
