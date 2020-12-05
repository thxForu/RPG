using UnityEngine;

namespace Combat
{ 
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(weapon);
                Destroy(gameObject);
            }
        }
    }
}
