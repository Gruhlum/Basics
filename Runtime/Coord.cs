using System;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public struct Coord : IComparable<Coord>
    {
        public int x;
        public int y;
        [HideInInspector] public bool isValid;

        public static readonly Coord zero = new Coord(0, 0);
        public static readonly Coord up = new Coord(0, 1);
        public static readonly Coord down = new Coord(0, -1);
        public static readonly Coord left = new Coord(-1, 0);
        public static readonly Coord right = new Coord(1, 0);
        public static readonly Coord one = new Coord(1, 1);



        public Coord(Vector2 position, bool isValid = true) : this(position.x, position.y, isValid)
        { }
        public Coord(float x, float y, bool isValid = true) : this(Mathf.RoundToInt(x), Mathf.RoundToInt(y), isValid)
        { }
        public Coord(int x, int y, bool isValid = true)
        {
            this.x = x;
            this.y = y;
            this.isValid = isValid;
        }

        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void Set(Coord coord)
        {
            Set(coord.x, coord.y);
        }

        public override bool Equals(object obj)
        {
            return obj is Coord coord &&
                   this.x == coord.x &&
                   this.y == coord.y;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public Coord Normalize(Coord center, int rotation)
        {
            Coord normalized = center + this;
            normalized.Rotate(center, rotation);
            return normalized;
        }
        public void Rotate(Coord center, int rotation)
        {
            this -= center;

            if (rotation > 0)
            {
                for (int i = 0; i < rotation; i++)
                {
                    this = new Coord(this.y, -this.x); // 90° clockwise
                }
            }
            else if (rotation < 0)
            {
                for (int i = 0; i < -rotation; i++)
                {
                    this = new Coord(-this.y, this.x); // 90° counter-clockwise
                }
            }

            this += center;
        }
        public static bool operator ==(Coord coord1, Coord coord2)
        {
            if (coord1.x != coord2.x)
            {
                return false;
            }
            if (coord1.y != coord2.y)
            {
                return false;
            }
            return true;
        }
        public static bool operator !=(Coord coord1, Coord coord2)
        {
            return !(coord1 == coord2);
        }
        public static Coord operator +(Coord coord1, Coord coord2)
        {
            coord1.x += coord2.x;
            coord1.y += coord2.y;
            return coord1;
        }
        public static Coord operator -(Coord coord1, Coord coord2)
        {
            coord1.x -= coord2.x;
            coord1.y -= coord2.y;
            return coord1;
        }
        public static Coord operator *(Coord coord1, Coord coord2)
        {
            coord1.x *= coord2.x;
            coord1.y *= coord2.y;
            return coord1;
        }
        public static Coord operator /(Coord coord1, Coord coord2)
        {
            coord1.x /= coord2.x;
            coord1.y /= coord2.y;
            return coord1;
        }
        public static Coord operator *(Coord coord1, int value)
        {
            coord1.x *= value;
            coord1.y *= value;
            return coord1;
        }
        public static Coord operator /(Coord coord1, int value)
        {
            coord1.x /= value;
            coord1.y /= value;
            return coord1;
        }
        public static Coord operator +(Coord coord1, Vector2 vector)
        {
            coord1.x += Mathf.RoundToInt(vector.x);
            coord1.y += Mathf.RoundToInt(vector.y);
            return coord1;
        }
        public static Coord operator -(Coord coord1, Vector2 vector)
        {
            coord1.x -= Mathf.RoundToInt(vector.x);
            coord1.y -= Mathf.RoundToInt(vector.y);
            return coord1;
        }
        public override string ToString()
        {
            return $"({x}), ({y})";
        }

        public int CompareTo(Coord other)
        {
            int result = this.x.CompareTo(other.x);
            if (result != 0)
            {
                return result;
            }
            else return this.y.CompareTo(other.y);
        }
    }
}