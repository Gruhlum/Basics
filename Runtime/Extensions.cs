using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HexTecGames.Basics;
using System;
using UnityEngine.UI;
using System.Text;
using System.IO;
using UnityEngine.EventSystems;

public static class Extensions
{
    /// <summary>
    /// Converts the current mouse position to a world position.
    /// </summary>
    /// <param name="cam">The camera that will do the conversion.</param>
    /// <returns></returns>
    public static Vector3 GetMousePosition(this Camera cam)
    {
        return cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z));
    }
    /// <summary>
    /// Converts the current mouse position to a world position.
    /// </summary>
    /// <param name="cam">The camera that will do the conversion.</param>
    /// <param name="offset">An additional offset that will be added the the current mouse position.</param>
    /// <returns></returns>
    public static Vector3 GetMousePosition(this Camera cam, Vector3 offset)
    {
        return cam.ScreenToWorldPoint(Input.mousePosition + offset + new Vector3(0, 0, -Camera.main.transform.position.z));
    }
    public static int LoopValue(this int value, int max)
    {
        value %= max;
        if (value < 0)
        {
            value += max;
        }
        return value;
    }
    /// <summary>
    /// <code>return Mathf.Abs(numb1 - numb2);</code>
    /// </summary>
    /// <param name="numb1"></param>
    /// <param name="numb2"></param>
    /// <returns>The distance between the 2 values.</returns>
    public static int GetDistance(this int numb1, int numb2)
    {
        return Mathf.Abs(numb1 - numb2);
    }
    /// <summary>
    /// <code>return Mathf.Abs(numb1 - numb2);</code>
    /// </summary>
    /// <param name="numb1"></param>
    /// <param name="numb2"></param>
    /// <returns>The distance between the 2 values.</returns>
    public static float GetDistance(this float numb1, float numb2)
    {
        return Mathf.Abs(numb1 - numb2);
    }
    /// <summary>
    /// Converts a string to a float.
    /// </summary>
    public static float ConvertToFloat(this string text)
    {
        return (float)Convert.ToDouble(text);
    }

    #region Layout Components
    public static void CopyData(this HorizontalOrVerticalLayoutGroup reciever, HorizontalOrVerticalLayoutGroup sender)
    {
        reciever.padding = sender.padding;
        reciever.childAlignment = sender.childAlignment;

        reciever.spacing = sender.spacing;
        reciever.reverseArrangement = sender.reverseArrangement;

        reciever.childControlHeight = sender.childControlHeight;
        reciever.childForceExpandHeight = sender.childForceExpandHeight;
        reciever.childForceExpandHeight = sender.childForceExpandHeight;

        reciever.childControlWidth = sender.childControlWidth;
        reciever.childForceExpandWidth = sender.childForceExpandWidth;
        reciever.childForceExpandWidth = sender.childForceExpandWidth;
    }
    public static void CopyData(this LayoutElement reciever, LayoutElement sender)
    {
        reciever.layoutPriority = sender.layoutPriority;
        reciever.ignoreLayout = sender.ignoreLayout;

        reciever.minHeight = sender.minHeight;
        reciever.flexibleHeight = sender.flexibleHeight;
        reciever.preferredHeight = sender.preferredHeight;

        reciever.minWidth = sender.minWidth;
        reciever.flexibleWidth = sender.flexibleWidth;
        reciever.preferredWidth = sender.preferredWidth;
    }
    public static void CopyData(this LayoutElement reciever, LayoutElement sender, Orientation orientation)
    {
        if (orientation == Orientation.Vertical)
        {
            reciever.minHeight = sender.minHeight;
            reciever.flexibleHeight = sender.flexibleHeight;
            reciever.preferredHeight = sender.preferredHeight;
        }
        else
        {
            reciever.minWidth = sender.minWidth;
            reciever.flexibleWidth = sender.flexibleWidth;
            reciever.preferredWidth = sender.preferredWidth;
        }

        reciever.layoutPriority = sender.layoutPriority;
        reciever.ignoreLayout = sender.ignoreLayout;
    }
    #endregion

    #region Image and SpriteRenderer
    public static void AddAlpha(this Image img, float alpha)
    {
        Color col = img.color;
        col.a += alpha;
        img.color = col;
    }
    public static void AddAlpha(this Image img, int alpha)
    {
        Color col = img.color;
        col.a += alpha / 255f;
        img.color = col;
    }
    public static void SetAlpha(this Image img, float alpha)
    {
        Color col = img.color;
        col.a = alpha;
        img.color = col;
    }
    public static void SetAlpha(this Image img, int alpha)
    {
        Color col = img.color;
        col.a = alpha / 255f;
        img.color = col;
    }
    public static void AddAlpha(this SpriteRenderer sr, float alpha)
    {
        Color col = sr.color;
        col.a += alpha;
        sr.color = col;
    }
    public static void AddAlpha(this SpriteRenderer sr, int alpha)
    {
        Color col = sr.color;
        col.a += alpha / 255f;
        sr.color = col;
    }
    public static void SetAlpha(this SpriteRenderer sr, float alpha)
    {
        Color col = sr.color;
        col.a = alpha;
        sr.color = col;
    }
    public static void SetAlpha(this SpriteRenderer sr, int alpha)
    {
        Color col = sr.color;
        col.a = alpha / 255f;
        sr.color = col;
    }
    #endregion
    public static bool IsSameColor(this Color32 c1, Color32 c2)
    {
        if (c1.r == c2.r && c1.b == c2.b && c1.g == c2.g)
        {
            return true;
        }
        return false;
    }
    public static IEnumerator Fade(this CanvasGroup canvasGroup, float startAlpha, float endAlpha, float speed = 1f)
    {
        if (speed <= 0)
        {
            yield break;
        }
        float timer = 0;
        while (timer < 1f)
        {
            timer += Time.deltaTime * speed;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timer);
            yield return null;
        }
    }
    public static IEnumerator FadeOut(this CanvasGroup canvasGroup, float speed = 1f)
    {
        yield return Fade(canvasGroup, canvasGroup.alpha, 0, speed);
    }
    public static IEnumerator FadeIn(this CanvasGroup canvasGroup, float speed = 1f)
    {
        yield return Fade(canvasGroup, 0, 1, speed);
    }


    #region Lists
    /// <summary>
    /// Uses UnityEngine.Random to return a random element from a list.
    /// </summary>
    /// <param name="list">The list to take the element from.</param>
    /// <returns>A random element.</returns>
    public static T Random<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return default;
        }
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
    /// <summary>
    /// Returns a random selection of items from a list.
    /// </summary>
    /// <param name="list">The list from which the items are taken.</param>
    /// <param name="items">The total number of items that should be retrieved.</param>
    /// <returns>A list of random elements</returns>
    public static List<T> Random<T>(this IList<T> list, int items)
    {
        List<T> results = new List<T>();

        for (int i = 0; i < items; i++)
        {
            results.Add(list.Random());
        }
        return results;
    }
    /// <summary>
    /// Returns a random selection of unique items from a list.
    /// </summary>
    /// <param name="list">The list from which the items are taken.</param>
    /// <param name="items">The total number of unique items that should be retrieved.</param>
    /// <returns>A list of random, unique elements</returns>
    public static List<T> RandomUnique<T>(this IList<T> list, int items)
    {
        List<T> results = new List<T>();

        List<int> indexes = new List<int>(list.Count);

        for (int i = 0; i < list.Count; i++)
        {
            indexes.Add(i);
        }

        int totalItems = Mathf.Min(items, list.Count);

        for (int i = 0; i < totalItems; i++)
        {
            int index = indexes.Random();
            results.Add(list[index]);
            indexes.Remove(index);
        }
        return results;
    }
    /// <summary>
    /// Shuffles the items into a random order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Increments the supplied index and returns 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static int NextIndex<T>(this IList<T> list, int index)
    {
        if (list.Count > index + 1)
        {
            index++;
        }
        else index = 0;

        return index;
    }

    /// <summary>
    /// Returns the next object in a list.
    /// Loops back to 0 once it reaches the end of the list.
    /// </summary>
    /// <param name="obj">The current object.</param>
    public static T Next<T>(this IList<T> list, T obj)
    {
        if (list.Count == 0)
        {
            return default;
        }
        int index = list.IndexOf(obj);
        index = list.NextIndex(index);
        return list[index];
    }

    /// <summary>
    /// Returns the element by the specified index from a list and increments it.
    /// Loops back to 0 once it reaches the end of the list.
    /// </summary>
    /// <param name="index">Index of the element that should be retrieved.</param>
    public static T Next<T>(this IList<T> list, ref int index)
    {
        if (index < 0)
        {
            index = 0;
        }
        if (list.Count == 0)
        {
            return default;
        }
        index = list.NextIndex(index);
        return list[index];
    }
    #endregion

    public static bool DetectUIObject<T>(this GraphicRaycaster raycaster, out T obj) where T : Component
    {
        return raycaster.DetectUIObject(EventSystem.current, out obj);
    }
    public static bool DetectUIObject<T>(this GraphicRaycaster raycaster, EventSystem eventSys, out T obj) where T : Component
    {
        var m_PointerEventData = new PointerEventData(eventSys);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(m_PointerEventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent(out T t))
            {
                obj = t;
                return true;
            }
        }
        obj = null;
        return false;
    }
    public static RaycastResult? DetectAnyUIObject(this GraphicRaycaster raycaster, EventSystem eventSys)
    {
        var m_PointerEventData = new PointerEventData(eventSys);
        m_PointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(m_PointerEventData, results);

        if (results.Count > 0)
        {
            return results[0];
        }
        else return null;
    }
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = cos * tx - sin * ty;
        v.y = sin * tx + cos * ty;
        return v;
    }

    /// <summary>
    /// Rotates a vector by a specified amount of degrees.
    /// </summary>
    /// <param name="v">The vector that will be rotated.</param>
    /// <param name="degrees">The amount the vector will be rotated.</param>
    /// <returns>The rotated vector.</returns>
    public static Vector3 Rotate(this Vector3 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = cos * tx - sin * ty;
        v.y = sin * tx + cos * ty;
        return v;
    }
    public static Vector3Int Round(this Vector3 v)
    {
        return new Vector3Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
    }
    public static Vector2Int Round(this Vector2 v)
    {
        return new Vector2Int(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static void UpdateShapeToSprite(this PolygonCollider2D collider)
    {
        collider.UpdateShapeToSprite(collider.GetComponent<SpriteRenderer>().sprite);
    }
    public static void UpdateShapeToSprite(this PolygonCollider2D collider, Sprite sprite)
    {
        if (collider != null && sprite != null)
        {
            collider.pathCount = sprite.GetPhysicsShapeCount();

            List<Vector2> path = new List<Vector2>();

            for (int i = 0; i < collider.pathCount; i++)
            {
                path.Clear();
                sprite.GetPhysicsShape(i, path);
                collider.SetPath(i, path.ToArray());
            }
        }
    }

    public static Sprite TextureToSprite(this Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(new Vector2(0, 0), new Vector2(texture.width, texture.height)), Vector2.zero);
    }

    public static int WrapIndex(this int index, int change, int length)
    {
        int number = index + change;
        number = number % length;

        if (number < 0)
        {
            number += length;
        }
        return number;
    }

    public static string CapitalizeFirstLetter(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return null;
        }
        StringBuilder builder = new StringBuilder();
        builder.Append(input.Substring(0, 1).ToUpper());
        if (input.Length > 1)
        {
            builder.Append(input.Substring(1, input.Length - 1));
        }
        return builder.ToString();
    }
    public static string RemoveInvalidSymbols(this string name)
    {
        List<char> results = Path.GetInvalidFileNameChars().ToList();
        results.AddRange(Path.GetInvalidPathChars().ToList());
        for (int i = name.Length - 1; i >= 0; i--)
        {
            if (results.Any(x => x == name[i]))
            {
                results.RemoveAt(i);
            }
        }
        return name;
    }
    public static string RemoveSpaces(this string input)
    {
        return input.Replace(" ", string.Empty);
    }
    /// <summary>
    /// Compares a string to a list of other strings and returns a unique string.
    /// </summary>
    /// <param name="name">The string to test</param>
    /// <param name="names">The other strings to test against</param>
    /// <returns>The supplied string if it is unique, otherwise a number will be added at the end to make it unique.</returns>
    public static string GetUniqueName(this string name, IEnumerable<string> names)
    {
        if (!names.Any(x => x == name))
        {
            return name;
        }
        else
        {
            int index = 1;
            string combinedName = $"{name} {index}";
            while (names.Any(x => x == combinedName))
            {
                index++;
                combinedName = $"{name} {index}";
            }

            return combinedName;
        }
    }

    public static void Enqueue<T>(this Queue<T> queue, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            queue.Enqueue(item);
        }
    }

}