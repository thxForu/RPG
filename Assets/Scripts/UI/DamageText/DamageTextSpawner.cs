using UnityEngine;

namespace UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {

        [SerializeField] private DamageText damageTextPrefab;

        public void Spawn(float damageAmount)
        {
            DamageText instance = Instantiate(damageTextPrefab,transform);
            instance.SetValue(damageAmount);
        }
    }
}
