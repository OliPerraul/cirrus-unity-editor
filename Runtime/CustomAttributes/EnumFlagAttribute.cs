using UnityEngine;
using System; using Cirrus.Objects;

namespace Cirrus.UnityEditorExt
{

    /// <summary>
    /// Display multi-select popup for Flags enum correctly.
    /// </summary>
	[AttributeUsage(AttributeTargets.Field)]
    public class EnumFlagAttribute : PropertyAttribute
    {
    }

}