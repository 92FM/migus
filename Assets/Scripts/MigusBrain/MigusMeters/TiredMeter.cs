public class TiredMeter : MigusMeter
{
  private const float _defaultValue = 0f;
  private const int _defaultMilliseconds = 1000;
  private const float _lazyDecrement = -0.5f;
  private const float _wanderIncrement = 0.5f;
  private const float _danceIncrement = 0.8f;

  public TiredMeter() : base(GetDefaultValue(), _defaultMilliseconds) { }

  private static float GetDefaultValue()
  {
    // TODO: read value from last save
    return _defaultValue;
  }

  // Tired: -lazy, +wander, ++dance

  public void Update(MigusBehaviorEnum state)
  {
    float updateBy = 0f;
    switch (state)
    {
      case MigusBehaviorEnum.Dance:
        updateBy = _danceIncrement;
        break;
      case MigusBehaviorEnum.Lazy:
        updateBy = _lazyDecrement;
        break;
      case MigusBehaviorEnum.Wander:
        updateBy = _wanderIncrement;
        break;
    }

    UpdateValue(updateBy);
  }
}
