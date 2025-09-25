using UnityEngine;

namespace HexTecGames.Basics
{
    public enum ButtonMode
    {
        Always,
        PlayModeOnly,
        EditModeOnly
    }

    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class InspectorButtonAttribute : PropertyAttribute
    {
        public ButtonMode Mode { get; }
        public object DefaultValue { get; }
        public bool HasDefaultValue { get; }

        public InspectorButtonAttribute(ButtonMode mode = ButtonMode.Always)
        {
            Mode = mode;
        }
        public InspectorButtonAttribute(object defaultValue, ButtonMode mode = ButtonMode.Always) : this(mode)
        {
            DefaultValue = defaultValue;
            HasDefaultValue = true;
        }
    }
}