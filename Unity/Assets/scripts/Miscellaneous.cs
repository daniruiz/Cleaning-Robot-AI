using System;

public static class Miscellaneous
{
    public static String arrayToString<T>(T[] array)
    {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";

        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
        String s = "[";
        foreach (T val in array)
            s += val + ", ";
        return s.Remove(s.Length - 2) + "]";
    }
}
