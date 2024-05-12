
using System;

public class SnapToGrid
{ 
    public float snap(float position_point, float grid_length)
    {
        float a = (position_point / grid_length);
        return MathF.Round(a, 2) * grid_length;
    }

}