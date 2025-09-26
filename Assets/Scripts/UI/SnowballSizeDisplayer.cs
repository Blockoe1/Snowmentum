/*****************************************************************************
// File Name : SnowballSizeDisplayer.cs
// Author : Jack Fisher
// Creation Date : September 24, 2025
// Last Modified : September 24, 2025
//
// Brief Description : This script displays a measurement of the size of the snowball in the UI
*****************************************************************************/
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Snowmentum
{
    public class SnowballSizeDisplayer : MonoBehaviour
    {
        private float sizeForText;
        [SerializeField] private TMP_Text snowballSizeText;
        [SerializeField] private int displayedDigits = 5;
        [SerializeField] private int postfixDigits = 1;

        // Update is called once per frame
        void Update()
        {
            //I set this float that will be rounded to the nearest integer for the display of the text. 
            //This does mean that the text displays you as 4meters in size when you are at 3.6 meters, for example, but I am hoping that is fine
            sizeForText = SnowballSize.Value;
            sizeForText = Mathf.Round(sizeForText * 100);
            snowballSizeText.text = AddDigits(displayedDigits, sizeForText, postfixDigits);
        }

        private static string AddDigits(int digits, float sizeValue, int postfixDigits)
        {
            string sizeString = sizeValue.ToString("F0"); 

            // Pad with leading zeros
            for (int i = sizeString.Length; i < digits; i++)
            {
                sizeString = "0" + sizeString;
            }

            // Add postfix zeros
            for (int i = 0; i < postfixDigits; i++)
            {
                sizeString = sizeString + "cm";
            }

            return sizeString;
        }
    }
}
