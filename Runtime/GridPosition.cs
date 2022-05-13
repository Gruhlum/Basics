using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Exile
{
	[System.Serializable]
	public struct GridPosition
	{
		public int x;
		public int y;

        public GridPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return x + "/" + y;
        }

        public static bool operator ==(GridPosition p1, GridPosition p2)
        {
            if ((object)p1 == null)
                return (object)p2 == null;

            return p1.Equals(p2);
        }
        public static bool operator !=(GridPosition p1, GridPosition p2)
        {
            return !(p1 == p2);
        }

    }
}