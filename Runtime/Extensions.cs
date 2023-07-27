using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using HexTecGames.Basics;

public static class Extensions
{
    public static Vector2 GetMousePosition(this Camera cam)
    {
        return (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Camera.main.transform.position.z));
    }
    public static void ChangeAlpha(this SpriteRenderer sr, float alpha)
    {
        Color col = sr.color;
        col.a = alpha;
        sr.color = col;
    }
    public static void ChangeAlpha(this SpriteRenderer sr, int alpha)
    {
        Color col = sr.color;
        col.a = alpha / 255f;
        sr.color = col;
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
}