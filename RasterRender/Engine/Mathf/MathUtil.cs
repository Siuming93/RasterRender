using System;
using System.Collections.Generic;

namespace RasterRender.Engine.Mathf
{
    public class MathUtil
    {
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a)*t;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        public static float Clamp01(float value)
        {
            if ((double)value < 0.0)
                return 0.0f;
            if ((double)value > 1.0)
                return 1f;
            return value;
        }

        private static Dictionary<int, float> _cos = new Dictionary<int, float>();
        public static float Fast_Cos(float angle)
        {
            int index = (int)(angle + 0.5f);
            if (!_cos.ContainsKey(index % 360))
            {
                _cos[index % 360] = (float)Math.Cos(index);
            }

            return _cos[index % 360];
        }

        private static Dictionary<int, float> _sin = new Dictionary<int, float>();
        public static float Fast_Sin(float angle)
        {
            int index = (int)(angle + 0.5f);
            if (!_sin.ContainsKey(index % 360))
            {
                _sin[index % 360] = (float)Math.Sin(index);
            }

            return _sin[index % 360];
        }

        public static int Round(float a)
        {
            return (int) (a + 0.5f);
        }
    }
}
