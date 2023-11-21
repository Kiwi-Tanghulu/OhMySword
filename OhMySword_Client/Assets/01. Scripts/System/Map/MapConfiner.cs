using UnityEngine;

public class MapConfiner : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Hitbox") == false)
            return;

        if(other.transform.root.TryGetComponent<IDamageable>(out IDamageable id) == false)
            return;

        Debug.Log("fall");
        id?.OnDamage(1, gameObject, other.transform.position);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = Color.green;
            MeshFilter[] filter = GetComponentsInChildren<MeshFilter>();
            if (filter.Length > 0)
            {
                if (filter[0].sharedMesh != null)
                    Gizmos.DrawWireMesh(filter[0].sharedMesh, 0, filter[0].transform.position, filter[0].transform.rotation, filter[0].transform.localScale);
            }
        }
        catch { }
    }

#endif
}
