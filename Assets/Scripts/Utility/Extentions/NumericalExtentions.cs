using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NumericalExtentions
{

    /// <summary>
    /// Shorthand for value = Mathf.Clamp01(value). 
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float Clamped01(this float value)
    {
        return Mathf.Clamp01(value);
    }
    /// <summary>
    /// Shorthand for value = Mathf.Clamp(value,minValue,maxValue). 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="minValue"></param>
    /// <param name="maxValue"></param>
    /// <returns></returns>
    public static float Clamped(this float value, float minValue,float maxValue)
    {
        return Mathf.Clamp(value, minValue, maxValue);
    }

    /// <summary>
    /// Functionally the same as Mathf.Lerp, returns a value linearly interpolated between 0 and 1. 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="valueRangeMin">The minimum in the current range the value is in.</param>
    /// <param name="valueRangeMax">The maximum in the current range the value is in.</param>
    /// <returns></returns>
    public static float Remap01(this float value,float valueRangeMin, float valueRangeMax)
    {
        return 0f + (value - valueRangeMin) * (1f - 0f) / (valueRangeMax - valueRangeMin);
        // newRangeMin + (value - valueRangeMin) * (mewRangeMax - newRangeMin) / (valueRangeMax - valueRangeMin);
    }

    /// <summary>
    /// Return a value evaluated from a curve based on a interpolation between 0 and 1 based on the given value.
    /// </summary>
    /// <param name="value">The given value.</param>
    /// <param name="valueRangeMin">The minimum in the current range the value is in.</param>
    /// <param name="valueRangeMax">The maximum in the current range the value is in.</param>
    /// <param name="curve">The curve that is evaluated.</param>
    /// <returns></returns>
    public static float Remap(this float value, float valueRangeMin, float valueRangeMax, AnimationCurve curve)
    {
        return curve.Evaluate(Remap01(value, valueRangeMin, valueRangeMax));
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
        float newRangeMin,
        float mewRangeMax)
    {
        return newRangeMin + (value - valueRangeMin) * (mewRangeMax - newRangeMin) / (valueRangeMax - valueRangeMin);
    }

}
