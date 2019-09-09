using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NumericalExtentions
{
    /// <summary>
    /// Returns value remapped from the value range to a new nonlinear range.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="valueRangeMin"></param>
    /// <param name="valueRangeMax"></param>
    /// <param name="curve"></param>
    /// <returns></returns>
    public static float Remap(
        this float value,
        float valueRangeMin,
        float valueRangeMax,
        AnimationCurve curve)
    {
        return curve.Evaluate(Remap(value, valueRangeMin, valueRangeMax));
    }

    /// <summary>
    /// Returns value remapped from the value range to a new range.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="valueRangeMin">The minimum in the current range the value is in.</param>
    /// <param name="valueRangeMax">The maximum in the current range the value is in.</param>
    /// <param name="newRangeMin">The minimum in the new range, default is 0.</param>
    /// <param name="mewRangeMax">The maximum in the new range, default is 1.</param>
    /// <returns></returns>
    public static float Remap(
        this float value,
        float valueRangeMin,
        float valueRangeMax,
        float newRangeMin = 0f,
        float mewRangeMax = 1f)
    {
        return newRangeMin + (value - valueRangeMin) * (mewRangeMax - newRangeMin) / (valueRangeMax - valueRangeMin);
    }

}
