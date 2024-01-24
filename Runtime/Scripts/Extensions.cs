using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HexTecGames.Basics;
using System;
using UnityEngine.UI;
using System.Text;

public static class Extensions
{
    public static Vector3 GetMousePosition(this Camera cam)
    {
        return cam.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Camera.main.transform.position.z));
    }
    public static Vector3 GetMousePosition(this Camera cam, Vector3 offset)
    {
        return cam.ScreenToWorldPoint(Input.mousePosition + offset + new Vector3(0, 0, Camera.main.transform.position.z));
    }
    public static int GetDistance(this int nr1, int nr2)
    {
        return Mathf.Abs(nr1 - nr2);
    }
    public static float GetDistance(this float nr1, float nr2)
    {
        return Mathf.Abs(nr1 - nr2);
    }
    public static float ConvertToFloat(this string text)
    {
        return (float)Convert.ToDouble(text);
    }
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
    public static string RemoveSpaces(this string input)
    {
        var results = input.Split(" ");
        foreach (var result in results)
        {
            result.Trim();
        }
        string output = "";
        foreach (var result in results)
        {
            output += result;
        }
        return output;
    }
    public static T Random<T>(this IList<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return default;
        }
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static int NextIndex<T>(this IList<T> list, int index)
    {
        if (list.Count <= index + 1)
        {
            index = 0;
        }
        else index++;

        return index;
    }

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

    public static T Next<T>(this IList<T> list, ref int index)
    {
        if (list.Count == 0)
        {
            return default;
        }
        index = list.NextIndex(index);
        return list[index];
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

    public static float GetRangeValue(this Vector2 v)
    {
        float value = UnityEngine.Random.Range(v.x, v.y);
        return value;
    }
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
    public static Vector3 Round(this Vector3 v)
    {
        return new Vector3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
    }
    public static Vector2 Round(this Vector2 v)
    {
        return new Vector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static IntValue Find(this List<IntValue> list, ValType type)
    {
        return list.Find(x => x.Type == type);
    }
    public static MaxIntValue Find(this List<MaxIntValue> list, ValType type)
    {
        return list.Find(x => x.Type == type);
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

    public static int WrapIndex(this int index, int change, int maximum)
    {
        //TODO make this work for large increases
        index += change;
        if (index < 0)
        {
            index = maximum;
        }
        else if (index > maximum)
        {
            index = 0;
        }
        return index;
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

}