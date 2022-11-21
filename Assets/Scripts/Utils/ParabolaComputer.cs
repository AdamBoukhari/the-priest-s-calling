using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Code provenant du lien suivant: https://gist.github.com/ditzel/68be36987d8e7c83d48f497294c66e08
public class ParabolaComputer
{
    public static Vector2 GetRBImpulse(Vector2 start, Vector2 end, float time)
    {
        float xVel = (end.x - start.x) / time;
        float yVel = (float)(end.y - start.y - 0.5 * Physics2D.gravity.y * Mathf.Pow(time, 2)) / time;

        return new Vector2(xVel, yVel);
    }
}
