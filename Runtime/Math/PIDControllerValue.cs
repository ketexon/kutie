using UnityEngine;

public class PIDControllerValue : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] public Kutie.PIDVector3Controller Controller;
    [SerializeField] float forceScale = 2;
    [SerializeField] Rigidbody rb;

    void FixedUpdate()
    {
        Controller.TargetValue = target.position;
        Controller.CurrentValue = rb.position;

        Vector3 force = Controller.Update(UnityEngine.Time.fixedDeltaTime) * forceScale;
        rb.AddForce(force);
    }
}
