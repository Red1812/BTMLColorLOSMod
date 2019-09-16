using System.Collections.Generic;
using BattleTech;
using BattleTech.UI;
using Harmony;
using UnityEngine;
using static BTMLColorLOSMod.BTMLColorLOSMod;
using HBS.Math;

namespace BTMLColorLOSMod
{
    [HarmonyPatch(typeof(BattleTech.UI.WeaponRangeIndicators), "DrawLine")]
    public static class WeaponRangeIndicators_DrawLine_Patch
    {
        static bool Prefix(
            ref Vector3 position,
            ref Quaternion rotation,
            ref bool isPositionLocked,
            ref AbstractActor selectedActor,
            ref ICombatant target,
            ref bool usingMultifire,
            ref bool isLocked,
            ref bool isMelee,
            WeaponRangeIndicators __instance)
        {
            CombatHUD HUD = (CombatHUD)ReflectionHelper.GetPrivateProperty(__instance, "HUD");
            if (__instance.DEBUG_showLOSLines)
            {
                __instance.GetDebugDrawer().DrawLines(selectedActor, HUD.SelectionHandler.ActiveState, target);
            }
            LineRenderer line =
                (LineRenderer)ReflectionHelper.InvokePrivateMethode(__instance, "getLine", new object[] { });
            Vector3 vector = Vector3.Lerp(position, position + selectedActor.HighestLOSPosition, __instance.sourceLaserDestRatio);
            Vector3 vector2 = Vector3.Lerp(target.CurrentPosition, target.TargetPosition, __instance.targetLaserDestRatio);
            AbstractActor abstractActor = target as AbstractActor;
            if (isMelee)
            {
                line.startWidth = __instance.LOSWidthBegin;
                line.endWidth = __instance.LOSWidthEnd;
                line.material = __instance.MaterialInRange;
                line.startColor = __instance.LOSLockedTarget;
                line.endColor = __instance.LOSLockedTarget;
                line.positionCount = 2;
                line.SetPosition(0, vector);
                Vector3 vector3 = vector - vector2;
                vector3.Normalize();
                vector3 *= __instance.LineEndOffset;
                vector2 += vector3;
                line.SetPosition(1, vector2);
                ReflectionHelper.InvokePrivateMethode(__instance, "SetEnemyTargetable", new object[] { target, true });
                List<AbstractActor> allActors = selectedActor.Combat.AllActors;
                allActors.Remove(selectedActor);
                allActors.Remove(abstractActor);
                PathNode pathNode;
                Vector3 attackPosition;
                float num;
                selectedActor.Pathing.GetMeleeDestination(abstractActor, allActors, out pathNode, out attackPosition, out num);
                HUD.InWorldMgr.ShowAttackDirection(HUD.SelectedActor, abstractActor, HUD.Combat.HitLocation.GetAttackDirection(attackPosition, target), vector2.y, MeleeAttackType.Punch, 0);
            }
            FiringPreviewManager.PreviewInfo previewInfo = HUD.SelectionHandler.ActiveState.FiringPreview.GetPreviewInfo(target);

            AttackDirection direction = HUD.Combat.HitLocation.GetAttackDirection(position, target);
            bool status = false;
            Color chosenColor = __instance.LOSInRange;
            if (direction == AttackDirection.FromFront && ModSettings.Direct.Active) { chosenColor = ModSettings.Direct.Color; status = true; }
            if ((direction == AttackDirection.FromLeft || direction == AttackDirection.FromRight) && ModSettings.Side.Active) { chosenColor = ModSettings.Side.Color; status = true; }
            if (direction == AttackDirection.FromBack && ModSettings.Back.Active) { chosenColor = ModSettings.Back.Color; status = true; }
            if ((target.UnitType == UnitType.Turret || target.UnitType == UnitType.Building) && status) { chosenColor = ModSettings.Direct.Color; }

            if (previewInfo.availability == FiringPreviewManager.TargetAvailability.NotSet)
            {
                Debug.LogError("Error - trying to draw line with no FiringPreviewManager availability!");
            }
            bool flag = HUD.SelectionHandler.ActiveState.SelectionType != SelectionType.Sprint || HUD.SelectedActor.CanShootAfterSprinting;
            bool flag2 = !isPositionLocked && previewInfo.availability != FiringPreviewManager.TargetAvailability.BeyondMaxRange && previewInfo.availability != FiringPreviewManager.TargetAvailability.BeyondRotation;
            if (flag && (previewInfo.IsCurrentlyAvailable || flag2))
            {
                if (usingMultifire)
                {
                    if (target == HUD.SelectedTarget)
                    {
                        line.startColor = (line.endColor = __instance.LOSMultiTargetKBSelection);
                    }
                    else if (isLocked)
                    {
                        line.startColor = (line.endColor = __instance.LOSLockedTarget);
                    }
                    else
                    {
                        line.startColor = (line.endColor = __instance.LOSUnlockedTarget);
                    }
                }
                else
                {
                    float shotQuality = (float)ReflectionHelper.InvokePrivateMethode(__instance, "GetShotQuality", new object[] { selectedActor, position, rotation, target });
                    Color endColor = Color.Lerp(Color.clear, __instance.LOSInRange, shotQuality);
                    line.startColor = (line.endColor = endColor);
                }
                line.material = __instance.MaterialInRange;
                if (previewInfo.HasLOF)
                {
                    line.positionCount = 2;
                    line.SetPosition(0, vector);
                    Vector3 vector4 = vector - vector2;
                    vector4.Normalize();
                    vector4 *= __instance.LineEndOffset;
                    vector2 += vector4;
                    if (previewInfo.LOFLevel == LineOfFireLevel.LOFClear)
                    {
                        if (target == HUD.SelectionHandler.ActiveState.FacingEnemy)
                        {
                            if (status)
                            {
                                float shotQuality = (float)ReflectionHelper.InvokePrivateMethode(__instance,
                                    "GetShotQuality", new object[] { selectedActor, position, rotation, target });
                                line.material.color = Color.white;
                                line.endColor = line.startColor = Color.Lerp(Color.clear, chosenColor, shotQuality);
                            }
                            line.startWidth =
                                __instance.LOSWidthBegin * __instance.LOSWidthFacingTargetMultiplier;
                            line.endWidth = __instance.LOSWidthEnd * __instance.LOSWidthFacingTargetMultiplier;
                        }
                        else
                        {
                            if (status)
                            {

                                float shotQuality = (float)ReflectionHelper.InvokePrivateMethode(__instance,
                                    "GetShotQuality", new object[] { selectedActor, position, rotation, target });
                                line.material.color = Color.white;
                                line.endColor = line.startColor = Color.Lerp(Color.clear, chosenColor, shotQuality);
                                if (direction == AttackDirection.FromLeft || direction == AttackDirection.FromRight)
                                {
                                    if (ModSettings.Side.Dashed)
                                    {
                                        line.material = __instance.MaterialOutOfRange;
                                        line.material.color = line.endColor;
                                    }
                                    line.startWidth = line.endWidth = ModSettings.Side.Thickness;
                                }
                                else if (direction == AttackDirection.FromBack)
                                {
                                    if (ModSettings.Back.Dashed)
                                    {
                                        line.material = __instance.MaterialOutOfRange;
                                        line.material.color = line.endColor;
                                    }
                                    line.startWidth = line.endWidth = ModSettings.Back.Thickness;
                                }
                                else
                                {
                                    if (ModSettings.Side.Dashed)
                                    {
                                        line.material = __instance.MaterialOutOfRange;
                                        line.material.color = line.endColor;
                                    }
                                    line.startWidth = line.endWidth = ModSettings.Direct.Thickness;
                                }
                            }
                        }
                        line.SetPosition(1, vector2);
                    }
                    else
                    {
                        if (target == HUD.SelectionHandler.ActiveState.FacingEnemy)
                        {
                            line.startWidth = __instance.LOSWidthBegin * __instance.LOSWidthFacingTargetMultiplier;
                            line.endWidth = __instance.LOSWidthBegin * __instance.LOSWidthFacingTargetMultiplier;
                        }
                        else
                        {
                            line.startWidth = __instance.LOSWidthBegin;
                            line.endWidth = __instance.LOSWidthBegin;
                        }
                        Vector3 vector5 = previewInfo.collisionPoint;
                        vector5 = Vector3.Project(vector5 - vector, vector2 - vector) + vector;
                        line.SetPosition(1, vector5);
                        if (ModSettings.ObstructedAttackerSide.Active)
                        {
                            line.material.color = Color.white;
                            line.startColor = line.endColor = ModSettings.ObstructedAttackerSide.Color;
                            if (ModSettings.ObstructedAttackerSide.Colorside && direction != AttackDirection.ToProne)
                            {
                                line.startColor = line.endColor = chosenColor;
                            }
                            line.startWidth = line.endWidth = ModSettings.ObstructedAttackerSide.Thickness;
                            if (ModSettings.ObstructedAttackerSide.Dashed)
                            {
                                line.material = __instance.MaterialOutOfRange;
                                line.material.color = line.endColor;
                            }
                        }
                        LineRenderer line2 =
                            (LineRenderer)ReflectionHelper.InvokePrivateMethode(__instance, "getLine", new object[] { });
                        line2.positionCount = 2;
                        line2.startWidth = __instance.LOSWidthBlocked;
                        line2.endWidth = __instance.LOSWidthBlocked;
                        line2.material = __instance.MaterialInRange;
                        if (ModSettings.ObstructedTargetSide.Active)
                        {
                            line2.material.color = Color.white;
                            line2.startColor = line2.endColor = ModSettings.ObstructedTargetSide.Color;
                            line2.startWidth = line2.endWidth = ModSettings.ObstructedTargetSide.Thickness;
                            if (ModSettings.ObstructedTargetSide.Dashed)
                            {
                                line2.material = __instance.MaterialOutOfRange;
                                line2.material.color = line2.endColor;
                            }
                        }
                        else
                        {
                            line2.startColor = line2.endColor = __instance.LOSBlocked;
                            line2.startWidth = line2.endWidth = __instance.LOSWidthBlocked;
                        }
                        line2.SetPosition(0, vector5);
                        line2.SetPosition(1, vector2);
                        GameObject coverIcon = (GameObject)ReflectionHelper.InvokePrivateMethode(__instance, "getCoverIcon", new object[] { });
                        if (!coverIcon.activeSelf)
                        {
                            coverIcon.SetActive(true);
                        }
                        coverIcon.transform.position = vector5;
                    }
                }
                else
                {
                    if (ModSettings.Indirect.Active)
                    {
                        float shotQuality = (float)ReflectionHelper.InvokePrivateMethode(__instance,
                            "GetShotQuality", new object[] { selectedActor, position, rotation, target });
                        Color couleur = ModSettings.Indirect.Color;
                        if (ModSettings.Indirect.Colorside)
                        {
                            couleur = chosenColor;
                        }
                        Color color6 = Color.Lerp(Color.clear, couleur, shotQuality);
                        if (ModSettings.Indirect.Dashed)
                        {
                            line.material = __instance.MaterialOutOfRange;
                            line.material.color = color6;
                            line.startWidth = line.endWidth = ModSettings.Indirect.Thickness;
                        }
                        else
                        {
                            line.material.color = Color.white;
                            line.endColor = line.startColor = color6;
                        }
                    }
                    int num2 = Mathf.Min(Mathf.Max((int)(NvMath.SqrMagnitudeXZ(vector, vector2) / (__instance.IndirectFireSegFreq * __instance.IndirectFireSegFreq)), __instance.IndirectFireMinPoints), __instance.IndirectFireMaxPoints);
                    Vector3[] pointsForArcDodgeBuildings = WeaponRangeIndicators.GetPointsForArcDodgeBuildings(num2, __instance.IndirectFireMinArcHeight, vector, vector2, HUD.Combat.MapMetaData, __instance.IndirectFireBuffer, __instance.IndirectFireMaxArcHeight, __instance.IndirectFireCheckFreq, false);
                    line.positionCount = num2;
                    line.SetPositions(pointsForArcDodgeBuildings);
                }
                ReflectionHelper.InvokePrivateMethode(__instance, "SetEnemyTargetable", new object[] { target, true });
                if (abstractActor != null)
                {
                    HUD.InWorldMgr.ShowAttackDirection(HUD.SelectedActor, abstractActor, HUD.Combat.HitLocation.GetAttackDirection(position, target), vector2.y, MeleeAttackType.NotSet, HUD.InWorldMgr.NumWeaponsTargeting(target));
                }
            }
            else
            {
                line.positionCount = 2;
                line.SetPosition(0, vector);
                line.SetPosition(1, vector2);
                line.startColor = (line.endColor = __instance.LOSOutOfRange);
                line.material = __instance.MaterialOutOfRange;
                ReflectionHelper.InvokePrivateMethode(__instance, "SetEnemyTargetable", new object[] { target, false });
            }
            return false;
        }
    }
}