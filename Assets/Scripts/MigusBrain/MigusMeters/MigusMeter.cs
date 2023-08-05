using System;

public abstract class MigusMeter
{
  /// <summary>
  /// How many milliseconds pass before meter value is updated by time-dependent setters.
  /// </summary>
  private int _timeUnit;

  /// <summary>
  /// Stores when meter value was last updated by time-dependent setters.
  /// </summary>
  private DateTime _lastUpdated;

  private float _value;
  public float Value
  {
    get => _value;
    private set => _value = Math.Clamp(value, 0, 100);
  }

  public MigusMeter(float value, int timeUnit)
  {
    _value = value;
    _timeUnit = timeUnit;
    _lastUpdated = DateTime.Now;
  }

  public void UpdateValue(float valueChange)
  {
    if (CanUpdate())
    {
      _value += valueChange;
      _lastUpdated = DateTime.Now;
    }
  }

  /// <summary>
  /// Determines whether enough time has passed to allow time-dependent setters to update meter value.
  /// </summary>
  /// <returns></returns>
  private bool CanUpdate()
  {
    return DateTime.Compare(_lastUpdated.AddMilliseconds(_timeUnit), DateTime.Now) > 0;
  }
}
