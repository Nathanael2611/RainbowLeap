using UnityEngine;

public class RotationLock : MonoBehaviour
{
    public bool x, y, z;

    void Update()
    {
        Vector3 eulers = this.transform.rotation.eulerAngles;

        if (this.x)
            eulers.x = 0;
        if (this.y)
            eulers.y = 0;
        if (this.z)
            eulers.z = 0;

        this.transform.rotation = Quaternion.Euler(eulers);
    }
}