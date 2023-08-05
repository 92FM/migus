using System;

public static class MBFormulas
{
  // Wander chance
  // Lazy Chance + duration
  // Keep lazing chance
  // Dance Chance
  // Default Chance (if not wander, lazy or dance, then default) + duration
  // Hungry/Curious chance
  // Eat after curious chance
  // Ignore chance + absolute ignore

  // if leeks and (wander, default, or lazy (lower chance if lazy))...
    // if hungryChance = true, hungry
    // else if ignoreChance = true...
      // if absolute ignore, do nothing
      // else, look at leek for timeDefault * (1 - ignoreChance)
    // else, curious
      // if curious end + hungryChanceEnd = true, eat
  // else if (wander, default, or lazy)...
    // if lazyChance = true, lazy
      // higher chance to lazy if already being lazy
    // else...
      // decide between wander+++, default++ and dance+

  public static float HungryChance(float hungerValue)
  {
    if (hungerValue < 30)
    {
      return 0;
    }
    if (hungerValue > 80)
    {
      return 1;
    }

    return (float)Math.Clamp(hungerValue * 0.9, 0, 1);
  }

  public static float IgnoreChance(int leeksSeen)
  {
    double chance = Math.Min(leeksSeen - 5, 0) * 0.09;
    return (float)Math.Clamp(chance, 0, 1);
  }
  public static float IgnoredLookTime(float ignoreChance)
  {
    return 2.5f * (1 - ignoreChance);
  }

  public static float EatAfterCuriousChance(float hungerValue)
  {
    throw new NotImplementedException();
  }

  public static float LazyChance(float tiredValue)
  {
    throw new NotImplementedException();
  }

  // Semi-random roulette that picks between default, wander and dance states
  public static float IdleRoulette(float tiredValue, int numOfTimesWander, int numOfTimesDance)
  {
    // the more times wandered in the last 3 minutes, the less likely to wander again
    // the more times danced in the last 6 minutes, the less likely to dance again
    // default chance boosted by tired value

    throw new NotImplementedException();
  }
}
