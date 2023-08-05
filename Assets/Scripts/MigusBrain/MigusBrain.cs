using System;
using UnityEngine;
using UnityEngine.AI;

public class MigusBrain : MonoBehaviour
{
  public MigusBehaviorEnum CurrentState { get; private set; }
  public float FloorOffset { get; private set; }
  public Vector3 DestCoordinates { get; private set; }
  public Quaternion DestRotation { get; private set; }

  public Transform Floor;
  public Camera Camera;
  public float WalkRadius = 1f;

  private DateTime _stateExpiresOn;

  private void Awake()
  {
    CurrentState = MigusBehaviorEnum.Default;
    FloorOffset = (Floor.localScale.y / 2) + Floor.position.y;
    _stateExpiresOn = DateTime.Now;
  }

  private void Update()
  {
    if (Input.GetMouseButtonDown(0))
    {
      Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

      if (Physics.Raycast(ray, out RaycastHit hitPoint))
      {
        DestCoordinates = new Vector3(hitPoint.point.x, FloorOffset, hitPoint.point.z);

        SetRotation();
        _stateExpiresOn = DateTime.Now.AddSeconds(5);
      }
    }
    else if (DateTime.Now.CompareTo( _stateExpiresOn ) > 0)
    {
      Vector2 randomDirection2 = UnityEngine.Random.insideUnitCircle * WalkRadius;
      Vector3 randomDirection3 = new Vector3(randomDirection2.x, FloorOffset, randomDirection2.y);
      NavMeshHit hit;
      NavMesh.SamplePosition(randomDirection3, out hit, WalkRadius, 1);
      DestCoordinates = hit.position;

      SetRotation();
      _stateExpiresOn = DateTime.Now.AddSeconds(8);
    }

    // if new leek
      // if hungry
      // else if ignore
        // if absolute ignore
      // else, curious
      // update leek list (each leek ID'd)
    // else
      // case wander
        // if near leek
        // else keep walking to dest
      // case lazy
        // if time up, run keepLazy check
          // if keep lazy, keep lazy
          // else, get new idle state
        // else, keep lazy
      // case dance
        // if time up, default
        // else, keep dance
      // case default
        // if time up, get new idle state
        // else, keep default
  }

  private void SetRotation()
  {
    float randomAngle = UnityEngine.Random.Range(0f, 360f);
    DestRotation = Quaternion.Euler(DestRotation.eulerAngles.x, randomAngle, DestRotation.eulerAngles.z);
  }

  // Curious Logic

  // Hungry Logic

  // Ignore logic
  // if absolute ignore, do nothing + return
  // if wander, lazy, or idle, look for IgnoredLookTime
  // ignore chance is higher while already curious

  // Dance Logic
  // play music + anim
  // ignore all else until dance is over
  // increment dance counter within the last 6 min

  // Default Logic
  // Stand around with a rotation within 60 deg of previous direction facing

  // Lazy logic
  // Play sit animation + extra anims
  // Do not interrupt extra anims when time is over

  // Wander logic
  // pick random point on map and walk to it
  // add distance walked to list of wanders within the last 3 mins

  // NOTE: No animator/navmesh/transform/rotation changes in here!!!!
}
