using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public Animator playerAnimation;


    private Rigidbody _rb;
    private bool _isDetectingWall;
    private GameObject _wallDetected;
    private int _wallID;

    private bool isGamePaused;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _isDetectingWall = false;
    }

    private void Update()
    {
        if (!IsGamePaused())
        {
            WallToDeleteDetection();
            DeleteWall();
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!IsGamePaused())
        {
            Movement();
        }
        else
        {
            DeadbyGameOver();
            _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Ray r = new Ray(transform.position, transform.forward);
        Gizmos.DrawRay(r);
    }

    private void Movement()
    {
        float movimientoHorizontal = Input.GetAxisRaw("Horizontal");
        float movimientoVertical = Input.GetAxisRaw("Vertical");

        Vector3 movimiento = new Vector3(movimientoHorizontal, 0 , movimientoVertical).normalized;

        _rb.MovePosition(transform.position += movimiento * (movementSpeed * Time.fixedDeltaTime));
        

        if (movimiento.magnitude > 0)
        {
            playerAnimation.SetBool("IsWalking", true);
            _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            Rotate(movimiento);
        }
        else
        {
            playerAnimation.SetBool("IsWalking", false);
            _rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
    }

    private void Rotate(Vector3 movement)
    {
        Quaternion rotacionObjetivo = Quaternion.LookRotation(movement, Vector3.up);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotacionObjetivo, 360 * Time.fixedDeltaTime));
    }

    private void WallToDeleteDetection()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
        if(Physics.Raycast(origin,transform.forward,out RaycastHit hit, 1))
        {
            if(hit.collider.CompareTag("Wall")) {
                bool isDifferentWall = _wallDetected != null ? _wallDetected.GetInstanceID() != hit.collider.gameObject.GetInstanceID() : false;
                if (!_isDetectingWall)
                {
                    _wallDetected = hit.collider.gameObject;
                    _wallDetected.GetComponent<WallInteraction>().WallDetected();
                    _wallID = _wallDetected.GetInstanceID();
                    _isDetectingWall = true;
                }
                else if(_isDetectingWall && isDifferentWall)
                {
                    _wallDetected.GetComponent<WallInteraction>().WallNoDetected();
                    _wallDetected = hit.collider.gameObject;
                    _wallDetected.GetComponent<WallInteraction>().WallDetected();
                    _wallID = _wallDetected.GetInstanceID();
                }
            }
            else
            {
                if (_isDetectingWall)
                {
                    _wallDetected.GetComponent<WallInteraction>().WallNoDetected();
                    _isDetectingWall = false;
                }
            }
        }
        else
        {
            if(_isDetectingWall) 
            {
                _wallDetected.GetComponent<WallInteraction>().WallNoDetected();
                _isDetectingWall= false;
            }
        }
    }

    public bool IsGamePaused()
    {
        return GameManager.Instance.GameStatus();
    }

    public void DeadbyGameOver()
    {
        playerAnimation.SetTrigger("Die");
    }


    private void DeleteWall()
    {
        if (Input.GetKeyDown(KeyCode.E) && _wallDetected != null)
        {
            _wallDetected.GetComponent<WallInteraction>().BreakWall();
            _wallDetected = null;
            _isDetectingWall = false;
            GameManager.Instance.DestroyWall();
        }
    }
}
