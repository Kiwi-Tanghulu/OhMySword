using UnityEngine;

public class TMeshGizmo : MonoBehaviour
{
	[SerializeField] Color color;

    #if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        try
        {
            Gizmos.color = color;
            if(TryGetComponent<SkinnedMeshRenderer>(out SkinnedMeshRenderer renderer))
            {
                if(renderer.sharedMesh != null)
                    Gizmos.DrawWireMesh(renderer.sharedMesh, 0, transform.position, transform.rotation, transform.localScale);
            }
        }catch {}
    }

    #endif
}
