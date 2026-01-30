using UnityEngine;

namespace HexTecGames.Basics
{
    public class InlineSOAttribute : PropertyAttribute
    {
        public bool ShowOnlyAttributedFields { get; }

        public InlineSOAttribute(bool showOnlyAttributedFields = false)
        {
            ShowOnlyAttributedFields = showOnlyAttributedFields;
        }
    }

    public class InlineSOFieldAttribute : PropertyAttribute { }
}
