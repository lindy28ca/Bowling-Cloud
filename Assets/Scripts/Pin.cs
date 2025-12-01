using UnityEngine;

public class Pin : MonoBehaviour
{
    private Quaternion startRot;

    void Start()
    {
        startRot = transform.rotation;
    }

    public bool Cayo()
    {
        if (transform.position.y < 0.1f) return true;

        return Vector3.Angle(Vector3.up, transform.up) > 20f;
    }

    public void Resetear(Vector3 pos)
    {
        gameObject.SetActive(true);

        transform.position = pos;
        transform.rotation = startRot;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;
    }
}
