using System;

public static class Miscellaneous
{
    public static String arrayToString<T>(T[] array)
    {
        String s = "";
        foreach (T val in array)
            s += val + ", ";
        return s;
    }
}
