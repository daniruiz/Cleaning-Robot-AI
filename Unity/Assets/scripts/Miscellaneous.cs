using System;

public static class Miscellaneous
{
    public static string arrayToString<T>(T[] array)
    {
        SetFloatStringFormat();
        string s = "[";
        foreach (T val in array)
            s += val + ", ";
        return s.Remove(s.Length - 2) + "]";
    }

    public static void SetFloatStringFormat() {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)
        System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
    }
}