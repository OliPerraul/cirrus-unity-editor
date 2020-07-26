using System;
using System.Collections.Generic;
//using Cirrus.Editor.ThirdParty.UniLinq;
using System.Text;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
#endif

namespace Cirrus.UnityEditorExt
{

    //Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
    //Modified by: Sebastian Lague

    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class SerializePropertyAttribute : PropertyAttribute
    {
        public SerializePropertyAttribute()
        {

        }
    }
}

#if UNITY_EDITOR

namespace Cirrus.UnityEditorExt
{
    [CustomPropertyDrawer(typeof(SerializePropertyAttribute))]
    class SerializePropertyAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int pFrom = property.displayName.IndexOf("<");
            int pTo = property.displayName.LastIndexOf(">");

            string result = property.displayName.Substring(pFrom + 1, pTo - 1 - pFrom);

            EditorGUI.PropertyField(
                position,
                property,
                new GUIContent(result));
        }
    }
}

#endif