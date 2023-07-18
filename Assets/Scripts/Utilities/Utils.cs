using System;
using UnityEngine;

public class Utils
{
    public static float EaseOut(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }

}