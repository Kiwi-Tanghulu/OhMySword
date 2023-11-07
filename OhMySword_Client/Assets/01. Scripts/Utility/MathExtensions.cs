using System;
using UnityEngine;

public static class MathExtensions 
{
    /// <summary>
    /// callback?.Invoke(digit, number, index_total);
    /// </summary>
	public static void ForEachDigit(this int source, Action<int, int, int> callback)
    {
        int i = 0;
        int origin = source;
        int cursor = (int)MathF.Pow(10, origin.ToString().Length - 1);
        while (cursor > 0)
        {
            int number = origin / cursor;
            for (int j = 0; j < number; j++, i++)
                callback?.Invoke(cursor, number, i);

            origin %= cursor;
            cursor /= 10;
        }
    }

    /// <summary>
    /// callback?.Invoke(digit, number, index_total);
    /// </summary>
	public static void ForEachDigit(this ushort source, Action<ushort, ushort, int> callback)
    {
        int i = 0;
        ushort origin = source;
        ushort cursor = (ushort)MathF.Pow(10, origin.ToString().Length - 1);
        while (cursor > 0)
        {
            ushort number = (ushort)(origin / cursor);
            for (int j = 0; j < number; j++, i++)
                callback?.Invoke(cursor, number, i);

            origin %= cursor;
            cursor /= 10;
        }
    }
}
