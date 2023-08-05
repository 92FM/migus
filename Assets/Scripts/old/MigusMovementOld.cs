using UnityEngine;
using UnityEngine.AI;

public class MigusMovementOld : MonoBehaviour
{
  public Camera Camera;
  public NavMeshAgent MigusAgent;
  public Animator MigusAnimator;
  public Transform TargetDest;
  public Transform Floor;
  public float Speed;
  public float AcceptableAngleRange;
  public MigusBrainOld MigusBrain;

  private float _floorOffset;
  private Vector3 _destinationPoint = Vector3.zero;
  private bool _isRotating;
  private bool _isMoving;

  private void Awake()
  {
    _floorOffset = (Floor.localScale.y / 2) + Floor.position.y;
    _isRotating = false;
    _isMoving = false;
    MigusAgent.updateRotation = false;
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      SetDestination();
    }

    _isMoving = MigusAgent.velocity != Vector3.zero;
    MoveAndRotate();
    AnimatorUpdates();
  }

  private void SetDestination()
  {
    Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

    if (Physics.Raycast(ray, out RaycastHit hitPoint))
    {
      MigusAgent.velocity = Vector3.zero;
      _destinationPoint = new Vector3(hitPoint.point.x, _floorOffset, hitPoint.point.z);
      TargetDest.position = _destinationPoint;
      _isRotating = true;
    }
  }

  private void MoveAndRotate()
  {
    if (_isRotating && Vector3.Angle(TargetDest.position - transform.position, transform.forward) < AcceptableAngleRange)
    {
      _isRotating = false;
      MigusAgent.SetDestination(_destinationPoint);
    }
    else if (_isRotating)
    {
      var targetRotation = Quaternion.LookRotation(TargetDest.transform.position - transform.position);
      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Speed * Time.deltaTime);
    }
  }

  private void AnimatorUpdates()
  {
    MigusAnimator.SetBool("IsWalking", _isMoving);
  }
}
