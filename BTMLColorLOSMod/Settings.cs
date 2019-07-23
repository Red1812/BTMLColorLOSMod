using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using InControl;
using UnityEngine;



namespace BTMLColorLOSMod
{
    // the properties in in here are to be a deserializer step from JSON format
    // into the C# variables used by the mod.
    public class Settings
    {
        public LineSetting direct = new LineSetting();
        public LineSetting Direct => direct;

        public LineSetting indirect = new LineSetting();
        public LineSetting Indirect => indirect;

        public LineSetting side = new LineSetting();
        public LineSetting Side => side;

        public LineSetting back = new LineSetting();
        public LineSetting Back => back;

        public LineSetting obstructedAttackerSide = new LineSetting();
        public LineSetting ObstructedAttackerSide => obstructedAttackerSide;

        public LineSetting obstructedTargetSide = new LineSetting();
        public LineSetting ObstructedTargetSide => obstructedTargetSide;

        public KeyBindingSetting nextColorKeyBinding;
        public KeyBindingSetting NextColorKeyBinding => nextColorKeyBinding;

        public bool debug = false;

        #region deprecated settings
        #region DirectLineOfFire

        // The color to be patched into the game for the coloring direct Line of Fire (LOF)'s
        // Defaults to magenta if the user gives us funky data so it should stand out
        public float[] directLineOfFireColor
        {
            set => Direct.Colors.Add(SettingsColorHelper.ColorFromValues(value[0], value[1], value[2], value[3]));
        }

        // Controls whether we change the color of the near part of an obstructed LOF line.
        public bool directLineOfFireActive
        {
            set => Direct.active = value;
        }

        #endregion

        #region IndirectLineOfFireArcOptions

        // The color to be patched into the game for coloring indirect lines of fire
        // Defaults to magenta if the user gives us funky data so it should stand out
        public float[] indirectLineOfFireArcColor
        {
            set => Indirect.Colors.Add(SettingsColorHelper.ColorFromValues(value[0], value[1], value[2], value[3]));
        }

        // Controls whether we set the Indirect Line Of Fire Line to Dashed (true: dashed, false: solid)
        public bool indirectLineOfFireArcDashed
        {
            set => Indirect.dashed = value;
        }

        // Controls the thickness of the Indirect Line of Fire when dashed
        public float indirectLineOfFireArcDashedThicknessMultiplier
        {
            set => Indirect.thickness = value;
        }

        // Controls whether we change anything about the Indirect Line of Fire arc style.
        public bool indirectLineOfFireArcActive
        {
            set => Indirect.active = value;
        }

        #endregion

        #region obstructedLineOfFireAttackerSide

        // The color to be patched into the game for the coloring obstructed Line of Fire (LOF)'s part between
        // the shooter and obstruction
        // Defaults to magenta if the user gives us funky data so it should stand out
        public float[] obstructedLineOfFireAttackerSideColor
        {
            set => ObstructedAttackerSide.Colors.Add(SettingsColorHelper.ColorFromValues(value[0], value[1], value[2], value[3]));
        }

        // Controls whether we change the color of the near part of an obstructed LOF line.
        public bool obstructedLineOfFireAttackerSideActive
        {
            set => ObstructedAttackerSide.active = value;
        }

        #endregion

        #region obstructedLineOfFireTargetSide

        // The color to be patched into the game for the coloring obstructed Line of Fire (LOF)'s part between
        // the obstruction and target.
        // Defaults to magenta if the user gives us funky data so it should stand out
        public float[] obstructedLineOfFireTargetSideColor
        {
            set => ObstructedTargetSide.Colors.Add(SettingsColorHelper.ColorFromValues(value[0], value[1], value[2], value[3]));
        }

        // Controls whether we change the color of the far part of an obstructed LOF line.
        public bool obstructedLineOfFireTargetSideActive
        {
            set => ObstructedTargetSide.active = value;
        }

        // Controls the thickness of the obstructed LOF target side line.
        public float obstructedLineOfFireTargetSiteThicknessMultiplier
        {
            set => ObstructedTargetSide.thickness = value;
        }

        #endregion

        #endregion
    }
}