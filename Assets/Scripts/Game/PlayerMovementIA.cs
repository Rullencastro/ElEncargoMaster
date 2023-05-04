using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementIA : MonoBehaviour
{
    public float movementSpeed;
    public Animator playerAnimation;


    private Rigidbody _rb;
    private bool _isDetectingWall;
    private GameObject _wallDetected;
    private int _wallID;

    private bool isGamePaused;

    private List<Vector2Int> _path;
    private int _currentIndex;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _isDetectingWall = false;
        _path = MazeGenerator.Instance.AStarFindPath();
        _currentIndex = 0;
        StartCoroutine(MoveAlongPath());
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
            return;
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

    private IEnumerator MoveAlongPath()
    {
        playerAnimation.SetBool("IsWalking",true);
        while (_currentIndex < _path.Count - 1)
        {
            Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);
            Vector2 nextPos = _path[_currentIndex + 1];

            Vector3 direction = new Vector3((nextPos.x + 0.5f) - currentPos.x, 0f, (nextPos.y + 0.5f) - currentPos.y);
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 360 * Time.deltaTime);

            
            Vector3 targetPosition = new Vector3(nextPos.x + 0.5f, transform.position.y, nextPos.y + 0.5f);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);
            

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                _currentIndex++;
            }

            yield return null;
        }
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
