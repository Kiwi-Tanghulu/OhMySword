using UnityEngine;

public static class ArrayExtensions
{
    public static T PickRandom<T>(this T[] source)
    {
        int randomIndex = Random.Range(0, source.Length);
        if(randomIndex >= source.Length)
            return default(T);
        
        return source[randomIndex];
    }
}