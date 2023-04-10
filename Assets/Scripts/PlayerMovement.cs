using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Movement();
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Desactivar la propiedad isKinematic para permitir el movimiento debido a las colisiones
        _rb.isKinematic = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Reactivar la propiedad isKinematic para evitar recibir fuerzas externas
        _rb.isKinematic = true;
    }

    private void Movement()
    {
        float movimientoHorizontal = Input.GetAxisRaw("Horizontal");
        float movimientoVertical = Input.GetAxisRaw("Vertical");

        Vector3 movimiento = new Vector3(movimientoHorizontal, 0 , movimientoVertical).normalized;

        _rb.MovePosition(transform.position += movimiento * (movementSpeed * Time.fixedDeltaTime));
        
        if (movimiento.magnitude > 0)
            Rotate(movimiento);
    }

    private void Rotate(Vector3 movement)
    {
        Quaternion rotacionObjetivo = Quaternion.LookRotation(movement, Vector3.up);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, 360 * Time.fixedDeltaTime));
    }
}
