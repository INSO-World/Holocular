using System;
using UnityEngine;
using UnityEngine.UI;

public class Slider
{
    public static float render(Rect rect, float state, float start, float end, GUIStyle sliderEmpty, GUIStyle sliderFilled, GUIStyle sliderKnob)
    {
        float activeFactor = 1f / (end - start) * (end - state);

        GUI.Label(new Rect(rect.x, rect.y + rect.height / 2 - 2, rect.width * (1f - activeFactor), 4), "", sliderFilled);
        GUI.Label(new Rect(rect.x + rect.width * (1f - activeFactor), rect.y + rect.height / 2 - 2, rect.width * activeFactor, 4), "", sliderEmpty);
        GUI.Label(new Rect(rect.x + rect.width * (1f - activeFactor) - rect.height / 2, rect.y, rect.height, rect.height), "", sliderKnob);
        return GUI.HorizontalSlider(rect, state, start, end, new GUIStyle(), new GUIStyle());
    }

}

