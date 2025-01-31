using UnityEngine;

public static class Extensions
{
    public static bool IsClose(this MonoBehaviour gameObject, GameObject other, float magnitude = 1)
    {
        return (gameObject.transform.position - other.transform.position).magnitude < magnitude;
    }


    public static bool IsInObject(this Vector3 vector, GameObject other)
    {
        var transform = other.transform;

        if (vector.x < transform.position.x - transform.lossyScale.x / 2) return false;
        
        if (vector.x > transform.position.x + transform.lossyScale.x / 2) return false;

        if (vector.z < transform.position.z - transform.lossyScale.z / 2) return false;
        
        if (vector.z > transform.position.z + transform.lossyScale.z / 2) return false;

        return true;
    }
    
}