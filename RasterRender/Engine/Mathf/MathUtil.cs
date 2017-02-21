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
    }
}
