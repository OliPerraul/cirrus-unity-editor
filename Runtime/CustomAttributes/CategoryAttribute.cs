﻿using System; using Cirrus.Objects;
using System.Collections.Generic;
//using Cirrus.Editor.ThirdParty.UniLinq;
using System.Text;
using UnityEngine;

namespace Cirrus.UnityEditorExt
{
    [AttributeUsage(AttributeTargets.All)]
    public class CategoryAttribute : Attribute
    {
        public string category { get; private set; }

        public CategoryAttribute(string category)
        {
            this.category = category;
        }

        public override string ToString()
        {
            return category;
        }
    }
}
