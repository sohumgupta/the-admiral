using UnityEngine;

/// <summary>
/// Class for destroying game objects
/// </summary>
public static class Transforms
{
    /// <summary>
    /// Destroying game objects
    /// </summary>
    /// <param name="t">Transform for which children we want to destroy</param>
    /// <param name="destroyImmediately">Boolean for if destroy immediately</param>
    public static void DestroyChildren(this Transform t, bool destroyImmediately = true)
    {
        if (destroyImmediately)
        {
            while (t.childCount != 0)
            {
                MonoBehaviour.DestroyImmediate(t.GetChild(0).gameObject);
            }
        } else
        {
            while (t.childCount != 0)
            {
                MonoBehaviour.Destroy(t.GetChild(0).gameObject);
            }
        }
    }
}
