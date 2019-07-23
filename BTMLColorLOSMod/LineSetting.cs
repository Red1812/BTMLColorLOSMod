using System.Collections.Generic;
using UnityEngine;

namespace BTMLColorLOSMod
{
    public class LineSetting
    {
        public bool active = false;
        public bool Active => active;

        public bool colorside = false;
        public bool Colorside => colorside;

        public float[][] colors
        {
            set
            {
                Colors.Clear();
                foreach (var colorValues in value)
                {
                    Colors.Add(SettingsColorHelper.ColorFromValues(colorValues[0], colorValues[1], colorValues[2], colorValues[3]));
                }
            }
        }

        public List<Color> Colors = new List<Color>();

        private int currentColorIndex = 0;

        public bool dashed = false;
        public bool Dashed => dashed;

        public float thickness = 1.0f;
        public float Thickness => thickness;

        public void NextColor()
        {
            currentColorIndex = (currentColorIndex + 1) % Colors.Count;
        }

        public Color Color => Colors[currentColorIndex];
    }
}