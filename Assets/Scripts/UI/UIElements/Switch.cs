using System;
using UnityEngine;
using UnityEngine.UI;

public class Switch
{
    Rect rect;
    GUIStyle background;
    GUIStyle knob;

    float animValue = 0f;

    float animSpeed = 2f;

    public Switch(Rect rect, GUIStyle background, GUIStyle knob)
    {
        this.rect = rect;
        this.background = background;
        this.knob = knob;
    }

    public bool render(bool state)
    {
        if (GUI.Button(rect, "", background))
        {
            state = !state;
        }
        checkAndUpdateAnimValue(state);



        GUI.Label(new Rect(rect.x + (rect.width - rect.height) * Utils.EaseOut(0f, 1f, animValue), rect.y, rect.height, rect.height), "", knob);
        return state;
    }


    private void checkAndUpdateAnimValue(bool state)
    {
        if (state)
        {
            if (animValue < 1f)
            {
                animValue += Time.deltaTime * animSpeed;
            }
            else
            {
                animValue = 1f;
            }
        }
        else
        {
            if (animValue > 0f)
            {
                animValue -= Time.deltaTime * animSpeed;
            }
            else
            {
                animValue = 0f;
            }
        }
    }
}

