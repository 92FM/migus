using UnityEngine;
using UnityEngine.AI;

public class MigusMovement : MonoBehaviour
{
  [SerializeField]
  private bool _isMoving;
  [SerializeField]
  private bool _isRotatingArrival;
  [SerializeField]
  private bool _isRotatingDeparture;

  public MigusBrain MigusBrain;
  public NavMeshAgent MigusAgent;
  public Animator MigusAnimator;
  public float Speed;
  public float AcceptableAngleRange;

  private Vector3 _destinationPoint = Vector3.zero;
  private Quaternion _destinationRotation = Quaternion.identity;

  void Awake()
  {
    _isRotatingArrival = false;
    _isRotatingDeparture = false;
    _isMoving = false;
    MigusAgent.updateRotation = false;
  }

  void Update()
  {
    // Get new destination + ending rotation from brain
      // if new dest + rot is different from old, update destination. else, nothing

    if (_destinationPoint != MigusBrain.DestCoordinates)
    {
      _isRotatingDeparture = true;
    }
    _destinationPoint = MigusBrain.DestCoordinates;

    // check if destination has been reached (do nothing, else IsMoving = false)
    _isMoving = MigusAgent.velocity != Vector3.zero;

    if (_destinationRotation != MigusBrain.DestRotation && !_isMoving)
    {
      _isRotatingArrival = true;
      _destinationRotation = MigusBrain.DestRotation;
    }

    RotationUpdates();
    AnimatorUpdates();
  }

  private void RotationUpdates()
  {
    // check if rotation needs to happen...
      // if not facing destination, rotate
      // else if IsMoving == false and not facing destRotation, rotate
      // else IsRotating = false

    if (_isMoving)
    {
      return;
    }

    if (_isRotatingDeparture)
    {
      if (Vector3.Angle(_destinationPoint - transform.position, transform.forward) < AcceptableAngleRange)
      {
        _isRotatingDeparture = false;
        MigusAgent.SetDestination(_destinationPoint);
      }
      else
      {
        var targetRotation = Quaternion.LookRotation(_destinationPoint - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Speed * Time.deltaTime);
      }
    }
    else if (_isRotatingArrival)
    {
      if (Quaternion.Angle(transform.rotation, MigusBrain.DestRotation) < AcceptableAngleRange)
      {
        _isRotatingArrival = false;
      }
      else
      {
        transform.rotation = Quaternion.Slerp(transform.rotation, MigusBrain.DestRotation, Speed * Time.deltaTime);
        Debug.Log(MigusBrain.DestRotation.eulerAngles);
      }
    }
  }

  private void AnimatorUpdates()
  {
    MigusAnimator.SetBool("IsWalking", _isMoving);
    // play anim based on state
      // default
      // walking/wandering
      // lazy
      // curious idle
      // dance
      // eat
  }
}
