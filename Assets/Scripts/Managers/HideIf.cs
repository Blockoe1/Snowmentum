/*****************************************************************************
// File Name : HideIfTrueAttribute.cs
// Author : Brandon Koederitz
// Creation Date : 10/15/2025
// Last Modified : 10/15/2025
//
// Brief Description : Experimental custom property attribute to hide a field if a bool field value is true.
*****************************************************************************/
using UnityEngine;

namespace Snowmentum
{
    public class HideIf : PropertyAttribute
    {
        private readonly string boolFieldName;

        #region Properties
        public string BoolFieldName => boolFieldName;
        #endregion

        public HideIf(string boolFieldName)
        {
            this.boolFieldName = boolFieldName;
        }
    }
}
