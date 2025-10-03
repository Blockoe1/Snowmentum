/*****************************************************************************
// File Name : TimerDisplayer.cs
// Author : Jack Fisher
// Creation Date : October 3, 2025
// Last Modified : October 3, 2025
//
// Brief Description : This script pools objects to be spawned in the background,
then will repeatedly choose from those objects at random to be spawned in random intervals.
*****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Snowmentum
{
    

    public class BackgroundObjects : MonoBehaviour

    {
        public Dictionary<string, Queue<GameObject>> poolDictionary;
    }
}
