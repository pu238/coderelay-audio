class MathUtils
{
    public static double Lerp(double a, double b, double f)
    {
        return a * f + b * (1.0 - f);
    }
}
