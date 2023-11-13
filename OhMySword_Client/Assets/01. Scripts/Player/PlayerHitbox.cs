using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Body_Hitable") && other.TryGetComponent<IDamageable>(out IDamageable obj))
        {
            obj.OnDamage(1, transform.root.gameObject, transform.position);
        }
    }
}
