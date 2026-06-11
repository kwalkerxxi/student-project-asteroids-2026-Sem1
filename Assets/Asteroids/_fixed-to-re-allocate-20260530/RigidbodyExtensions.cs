using UnityEngine;

public static class RigidbodyExtensions
{
    public static void ClampLinearVelocity(this Rigidbody rigidbodyToClamp, float maxSpeed)
    {
        if(rigidbodyToClamp.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            rigidbodyToClamp.linearVelocity = rigidbodyToClamp.linearVelocity.normalized * maxSpeed;
        }
    }

    public static void ClampAngularVelocity(this Rigidbody rb, float maxAngularSpeed)
    {
        if(rb.angularVelocity.sqrMagnitude > maxAngularSpeed * maxAngularSpeed)
        {
            rb.angularVelocity = rb.angularVelocity.normalized * maxAngularSpeed;
        }
    }
}
