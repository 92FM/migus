public class HungerMeter : MigusMeter
{
  private const float _defaultValue = 0f;
  private const int _defaultMilliseconds = 1000;
  private const float _eatDecrement = -18.0f;
  private const float _hungerIncrement = 0.7f;

  public HungerMeter() : base(GetDefaultValue(), _defaultMilliseconds) { }

  private static float GetDefaultValue()
  {
    // TODO: read value from last save
    return _defaultValue;
  }

  // Hunger: -eat, +time, ++leek noticed no eat

  // TODO: keep track of leeks seen
  public void Update(bool didEat, float newLeekIncrement)
  {
    float updateBy = _hungerIncrement + newLeekIncrement;

    if (didEat)
    {
      updateBy += _eatDecrement;
    }

    UpdateValue(updateBy);
  }
}
