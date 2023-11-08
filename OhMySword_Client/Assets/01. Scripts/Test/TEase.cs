using UnityEngine;

public class TEase : MonoBehaviour
{
	#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        float devide = 1000;
        Vector3 last = transform.position;

        Gizmos.color = Color.green;
        for(int i = 0; i < devide; i++)
        {
            float theta = i / devide;
            Vector3 current = transform.position + new Vector3(Mathf.Lerp(0f, 1f, theta), Mathf.Lerp(0f, 1f, EaseOutInExpo(theta)));
            Gizmos.DrawLine(last, current);

            last = current;
        }

    }
    #endif

    public float EaseTangent(float t)
    {
        return Mathf.Tan(t * (Mathf.PI / 2f));
    }

    public float EaseOutInExpo(float t) 
    {
        if (t < 0.5f)
        {
            return 0.5f * Mathf.Pow(t * 2, 3);
        }
        else
        {
            return 0.5f * (2 - Mathf.Pow(2 - (t * 2), 3));
        }
    }
}
