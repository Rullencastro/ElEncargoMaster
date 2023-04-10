//using DG.Tweening;
using UnityEngine;


public class Spinner : MonoBehaviour
{
    public float spinForce = 10f;
    public float maxSpeed = 20f;
    public float frictionCoefficient = 0.1f;
    public Transform spinnerTop;

    private Rigidbody rb;
    private bool isFalling;
    private float timer;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = rb.constraints | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX;
        rb.maxAngularVelocity = maxSpeed;
        rb.AddTorque(transform.up * spinForce,ForceMode.Impulse);
        timer = 3;
    }

    void FixedUpdate()
    {
        float angularVelocity = rb.angularVelocity.magnitude;
        float linearVelocity = rb.velocity.magnitude;
        float speed = angularVelocity + linearVelocity;
        float frictionForce = frictionCoefficient * speed;
        Debug.Log(speed);
        

        if(speed < 15 && speed > 14 && !isFalling)
        {
            Fall();
            isFalling = true;
        }

        Vector3 frictionDirection = -rb.velocity.normalized;
        Vector3 friction = frictionDirection * frictionForce;

        rb.AddForce(friction, ForceMode.Force);
    }

    void Fall()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.AddTorque(transform.forward * 10, ForceMode.Force);
    }

    void Tremble()
    {
        Quaternion newRot = Quaternion.Euler(5,0,0);
        rb.MoveRotation(newRot);
        
    }
}
