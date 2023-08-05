using System;
using UnityEngine;

public class MigusBrainOld : MonoBehaviour
{
  private MigusBehaviorEnum _currentState;
  private DateTime _stateEnd;
  private float _eepyCount;
  private float _hungerCount;
  private float _keepWanderChance;
  private float _danceChance;

  private const float KEEP_WANDER_RATIO = 0.95f;
  private const float DANCE_CHANCE_DEFAULT = 0.5f;

  private System.Random _random;

  public MigusBrainOld() 
  {
    _currentState = MigusBehaviorEnum.Lazy;
    _stateEnd = DateTime.Now.AddSeconds(15);
    _eepyCount = 0f;
    _hungerCount = 0f;
    _keepWanderChance = 1f;
    _danceChance = 0.2f;

    _random = new System.Random();
  }
  
  public MigusBehaviorEnum GetCurrentState(bool isMoving)
  {
    // lazy / wander / dance states don't change for at least a few secs
    // lazy: doesn't move, probably not standing
    // wander: standing around, infrequently picks random places to walk to
    // dance: dances in place

    switch (_currentState)
    {
      case MigusBehaviorEnum.Lazy:
      case MigusBehaviorEnum.Wander:
      case MigusBehaviorEnum.Default:
      case MigusBehaviorEnum.Dance:
        SetIdleState(isMoving);
        break;
      case MigusBehaviorEnum.CuriousIdle:
        break;
    }

    return _currentState;
  }

  private void SetIdleState(bool isMoving)
  {
    if (_currentState == MigusBehaviorEnum.Wander && !isMoving)
    {
      _currentState = MigusBehaviorEnum.Default;
      SetStateEnd();
    }
    else if (DateTime.Compare(DateTime.Now, _stateEnd) > 0)
    {
      if (_currentState == MigusBehaviorEnum.Default && KeepWandering())
      {
        _currentState = MigusBehaviorEnum.Wander;
      }
      else
      {
        _keepWanderChance = 1f;
        IdleStateDecision();
        SetStateEnd();
      }      
    }
  }

  private bool KeepWandering()
  {
    _keepWanderChance *= Math.Clamp(KEEP_WANDER_RATIO - _eepyCount, 0, 1);
    return _random.NextDouble() < _keepWanderChance;
  }

  private void IdleStateDecision()
  {
    if (_eepyCount < 0.5f && ShouldDance())
    {
      // TODO: dancing increases eepy at a faster rate
      _danceChance = 0f;
      _currentState = MigusBehaviorEnum.Dance;
    }
    else if (_eepyCount < 0.75f && ShouldWander())
    {
      // TODO: inceases eepy slowly
      _currentState = MigusBehaviorEnum.Wander;
    }
    else
    {
      // TODO: decreases eepy
      _currentState = MigusBehaviorEnum.Lazy;
    }
  }

  private bool ShouldDance()
  {
    _danceChance += 0.00005f;
    return _random.NextDouble() < 
      Math.Clamp(DANCE_CHANCE_DEFAULT * (1 - _eepyCount + 1) + _danceChance, 0, 1);
  }

  private bool ShouldWander()
  {
    return _random.NextDouble() < 0.45f / (0.85 + _eepyCount);
  }

  private void SetStateEnd()
  {
    // Wander, Curious and Hungry states are not time-based states
    int durationSec;
    switch(_currentState)
    {
      case MigusBehaviorEnum.Lazy:
        durationSec = _random.Next(20, 40);
        break;
      case MigusBehaviorEnum.Dance:
        durationSec = _random.Next(10, 20);        
        break;
      case MigusBehaviorEnum.Default:
        durationSec = _random.Next(3, 13);
        break;
      default:
        durationSec = 0;
        break;
    }

    _stateEnd = DateTime.Now.AddSeconds(durationSec);
  }

  #region Accessors
  public void increaseHunger(int increment)
  {
    _hungerCount += increment;
  }

  public void decreaseHunger(int decrement)
  {
    _hungerCount -= decrement;
  }

  public void increaseEepy(int increment)
  {
    _eepyCount += increment;
  }

  public void decreaseEepy(int decrement)
  {
    _eepyCount -= decrement;
  }
  #endregion
}