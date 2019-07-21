﻿using UnityEngine;
using Verse;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
using RimWorld;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace TorannMagic
{
    public static class TM_Calc
    {
        public static bool IsRobotPawn(Pawn pawn)
        {
            bool flag_Core = pawn.RaceProps.IsMechanoid;
            bool flag_AndroidTiers = (pawn.def.defName == "Android1Tier" || pawn.def.defName == "Android2Tier" || pawn.def.defName == "Android3Tier" || pawn.def.defName == "Android4Tier" || pawn.def.defName == "Android5Tier" || pawn.def.defName == "M7Mech" || pawn.def.defName == "MicroScyther");
            bool flag_Androids = pawn.RaceProps.FleshType.defName == "ChJDroid" || pawn.def.defName == "ChjAndroid";
            bool isRobot = flag_Core || flag_AndroidTiers || flag_Androids;
            return isRobot;
        }

        public static bool IsUndead(Pawn pawn)
        {
            if (pawn != null)
            {
                bool flag_Hediff = false;
                if (pawn.health != null && pawn.health.hediffSet != null)
                {
                    if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadStageHD"), false))
                    {
                        flag_Hediff = true;
                    }
                    Hediff hediff = null;
                    for (int i = 0; i < pawn.health.hediffSet.hediffs.Count; i++)
                    {
                        hediff = pawn.health.hediffSet.hediffs[i];
                        if (hediff.def.defName.Contains("ROM_Vamp"))
                        {
                            flag_Hediff = true;
                        }
                    }
                }
                bool flag_DefName = false;
                if (pawn.def.defName == "SL_Runner" || pawn.def.defName == "SL_Peon" || pawn.def.defName == "SL_Archer" || pawn.def.defName == "SL_Hero")
                {
                    flag_DefName = true;
                }
                bool flag_Trait = false;
                if (pawn.story != null && pawn.story.traits != null)
                {
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Undead))
                    {
                        flag_Trait = true;
                    }
                }
                bool isUndead = flag_Hediff || flag_DefName || flag_Trait;
                return isUndead;
            }
            return false;
        }

        public static bool IsUndeadNotVamp(Pawn pawn)
        {
            if (pawn != null)
            {
                bool flag_Hediff = false;
                if (pawn.health != null)
                {
                    if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadAnimalHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_LichHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_UndeadStageHD"), false))
                    {
                        flag_Hediff = true;
                    }
                }
                bool flag_DefName = false;
                if (pawn.def.defName == "SL_Runner" || pawn.def.defName == "SL_Peon" || pawn.def.defName == "SL_Archer" || pawn.def.defName == "SL_Hero")
                {
                    flag_DefName = true;
                }
                bool flag_Trait = false;
                if (pawn.story != null && pawn.story.traits != null)
                {
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Undead))
                    {
                        flag_Trait = true;
                    }
                }
                bool isUndead = flag_Hediff || flag_DefName || flag_Trait;
                return isUndead;
            }
            return false;
        }

        public static bool HasHateHediff(Pawn pawn)
        {
            if(pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_HateHD_I"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_HateHD_II"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_HateHD_III"), false) ||
                pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_HateHD"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_HateHD_IV"), false) || pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_HateHD_V"), false))
            {
                return true;
            }
            return false;
        }

        public static bool IsMightUser(Pawn pawn)
        {
            if (pawn != null)
            {
                bool flag_Hediff = false;
                if (pawn.health != null)
                {
                    if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_MightUserHD"), false))
                    {
                        flag_Hediff = true;
                    }
                }
                bool flag_Need = false;
                if (pawn.needs != null)
                {
                    List<Need> needs = pawn.needs.AllNeeds;
                    for (int i = 0; i < needs.Count; i++)
                    {
                        if (needs[i].def.defName == "TM_Stamina")
                        {
                            flag_Need = true;
                        }
                    }
                }
                bool flag_Trait = false;
                if (pawn.story != null && pawn.story.traits != null)
                {
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Bladedancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Gladiator) || pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || 
                        pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Sniper) || pawn.story.traits.HasTrait(TorannMagicDefOf.Ranger) || pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Psionic) ||
                        pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Monk))
                    { 
                        flag_Trait = true;
                    }
                }
                bool isMightUser = flag_Hediff || flag_Trait || flag_Need;
                return isMightUser;
            }
            return false;
        }

        public static bool IsMagicUser(Pawn pawn)
        {
            if (pawn != null)
            {
                bool flag_Hediff = false;
                if (pawn.health != null)
                {
                    if (pawn.health.hediffSet.HasHediff(HediffDef.Named("TM_MagicUserHD"), false))
                    {
                        flag_Hediff = true;
                    }
                }
                bool flag_Need = false;
                if (pawn.needs != null)
                {
                    List<Need> needs = pawn.needs.AllNeeds;
                    for (int i = 0; i < needs.Count; i++)
                    {
                        if (needs[i].def.defName == "TM_Mana")
                        {
                            flag_Need = true;
                        }
                    }
                }
                bool flag_Trait = false;
                if (pawn.story != null && pawn.story.traits != null)
                {
                    if (pawn.story.traits.HasTrait(TorannMagicDefOf.Technomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Geomancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Warlock) || 
                        pawn.story.traits.HasTrait(TorannMagicDefOf.Succubus) || pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless) || pawn.story.traits.HasTrait(TorannMagicDefOf.InnerFire) || 
                        pawn.story.traits.HasTrait(TorannMagicDefOf.HeartOfFrost) || pawn.story.traits.HasTrait(TorannMagicDefOf.StormBorn) || pawn.story.traits.HasTrait(TorannMagicDefOf.Arcanist) || 
                        pawn.story.traits.HasTrait(TorannMagicDefOf.Paladin) || pawn.story.traits.HasTrait(TorannMagicDefOf.Summoner) || pawn.story.traits.HasTrait(TorannMagicDefOf.Druid) || 
                        (pawn.story.traits.HasTrait(TorannMagicDefOf.Necromancer) || pawn.story.traits.HasTrait(TorannMagicDefOf.Lich)) || pawn.story.traits.HasTrait(TorannMagicDefOf.Priest) || 
                        pawn.story.traits.HasTrait(TorannMagicDefOf.TM_Bard) || pawn.story.traits.HasTrait(TorannMagicDefOf.Chronomancer))
                    {
                        flag_Trait = true;
                    }
                    if(pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                    {
                        return false;
                    }
                }                
                bool isMagicUser = flag_Hediff || flag_Trait || flag_Need;
                return isMagicUser;
            }
            return false;
        }

        public static bool IsPawnInjured(Pawn targetPawn, float minInjurySeverity = 0)
        {
            float injurySeverity = 0;
            using (IEnumerator<BodyPartRecord> enumerator = targetPawn.health.hediffSet.GetInjuredParts().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    BodyPartRecord rec = enumerator.Current;
                    IEnumerable<Hediff_Injury> arg_BB_0 = targetPawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                    Func<Hediff_Injury, bool> arg_BB_1;
                    arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                    foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                    {
                        bool flag5 = current.CanHealNaturally() && !current.IsPermanent();
                        if (flag5)
                        {
                            injurySeverity += current.Severity;
                        }
                    }
                }
            }
            return injurySeverity > minInjurySeverity;
        }

        public static List<Hediff> GetPawnAfflictions(Pawn targetPawn)
        {
            List<Hediff> afflictionList = new List<Hediff>();
            afflictionList.Clear();
            using (IEnumerator<Hediff> enumerator = targetPawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Hediff rec = enumerator.Current;
                    if (rec.def.isBad && rec.def.makesSickThought)
                    {
                        afflictionList.Add(rec);
                    }
                }
            }
            return afflictionList;
        }

        public static List<Hediff> GetPawnAddictions(Pawn targetPawn)
        {
            List<Hediff> addictionList = new List<Hediff>();
            addictionList.Clear();
            using (IEnumerator<Hediff_Addiction> enumerator = targetPawn.health.hediffSet.GetHediffs<Hediff_Addiction>().GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Hediff_Addiction rec = enumerator.Current;
                    if (rec.Chemical.addictionHediff != null)
                    {
                        addictionList.Add(rec);
                    }
                }
            }
            return addictionList;
        }

        public static Vector3 GetVector(IntVec3 from, IntVec3 to)
        {
            Vector3 heading = (to - from).ToVector3();
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            return direction;
        }

        public static Vector3 GetVector(Vector3 from, Vector3 to)
        {
            Vector3 heading = (to - from);
            float distance = heading.magnitude;
            Vector3 direction = heading / distance;
            return direction;
        }

        public static Pawn FindNearbyOtherPawn(Pawn pawn, int radius)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                {
                    if (targetPawn != pawn && !targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        pawnList.Add(targetPawn);
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyPawn(Pawn pawn, int radius)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                {
                    if (!targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        pawnList.Add(targetPawn);
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyFactionPawn(Pawn pawn, Faction faction, int radius)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                {
                    if (targetPawn.Faction == faction && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        pawnList.Add(targetPawn);
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyMage(Pawn pawn, int radius, bool inCombat)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed && targetPawn.Faction != null)
                {
                    if(inCombat)
                    {
                        if (pawn != targetPawn && targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            CompAbilityUserMagic targetComp = targetPawn.GetComp<CompAbilityUserMagic>();
                            if (targetComp != null && targetComp.IsMagicUser && !targetComp.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                    }                    
                    else
                    {
                        if (pawn != targetPawn && !targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            CompAbilityUserMagic targetComp = targetPawn.GetComp<CompAbilityUserMagic>();
                            if (targetComp != null && targetComp.IsMagicUser && !targetComp.Pawn.story.traits.HasTrait(TorannMagicDefOf.Faceless))
                            {
                                pawnList.Add(targetPawn);                                
                            }
                        }
                    }                    
                }
                targetPawn = null;
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyFighter(Pawn pawn, int radius, bool inCombat)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (pawn != targetPawn && targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed && targetPawn.Faction != null)
                {
                    if (inCombat)
                    {
                        if (targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            CompAbilityUserMight targetComp = targetPawn.GetComp<CompAbilityUserMight>();
                            if (targetComp != null && targetComp.IsMightUser)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                    }
                    else
                    {
                        if (pawn != targetPawn && !targetPawn.HostileTo(pawn.Faction) && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                        {
                            CompAbilityUserMight targetComp = targetPawn.GetComp<CompAbilityUserMight>();
                            if (targetComp != null && targetComp.IsMightUser)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                    }
                }
                targetPawn = null;
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyInjuredPawn(Pawn pawn, int radius, float minSeverity)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !TM_Calc.IsUndead(targetPawn))
                {
                    if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        float injurySeverity = 0;
                        using (IEnumerator<BodyPartRecord> enumerator = targetPawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                BodyPartRecord rec = enumerator.Current;
                                IEnumerable<Hediff_Injury> arg_BB_0 = targetPawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                                Func<Hediff_Injury, bool> arg_BB_1;
                                arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                                foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                                {
                                    bool flag5 = current.CanHealNaturally() && !current.IsPermanent();
                                    if (flag5)
                                    {
                                        injurySeverity += current.Severity;
                                    }                                        
                                }                                
                            }
                        }
                        if (minSeverity != 0)
                        {
                            if (injurySeverity >= minSeverity)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                        else
                        {
                            if (injurySeverity != 0)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyInjuredPawnOther(Pawn pawn, int radius, float minSeverity)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !TM_Calc.IsUndead(targetPawn))
                {
                    if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius && targetPawn != pawn)
                    {
                        float injurySeverity = 0;
                        using (IEnumerator<BodyPartRecord> enumerator = targetPawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                BodyPartRecord rec = enumerator.Current;
                                IEnumerable<Hediff_Injury> arg_BB_0 = targetPawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                                Func<Hediff_Injury, bool> arg_BB_1;
                                arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                                foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                                {
                                    bool flag5 = current.CanHealNaturally() && !current.IsPermanent();
                                    if (flag5)
                                    {
                                        injurySeverity += current.Severity;
                                    }
                                }
                            }
                        }
                        if (minSeverity != 0)
                        {
                            if (injurySeverity >= minSeverity)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                        else
                        {
                            if (injurySeverity != 0)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyPermanentlyInjuredPawn(Pawn pawn, int radius, float minSeverity)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !TM_Calc.IsUndead(targetPawn))
                {
                    if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        float injurySeverity = 0;
                        using (IEnumerator<BodyPartRecord> enumerator = targetPawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                BodyPartRecord rec = enumerator.Current;
                                IEnumerable<Hediff_Injury> arg_BB_0 = targetPawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                                Func<Hediff_Injury, bool> arg_BB_1;
                                arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                                foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                                {
                                    bool flag5 = !current.CanHealNaturally() && current.IsPermanent();
                                    if (flag5)
                                    {
                                        injurySeverity += current.Severity;
                                    }
                                }
                            }
                        }
                        if (minSeverity != 0)
                        {
                            if (injurySeverity >= minSeverity)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                        else
                        {
                            if (injurySeverity != 0)
                            {
                                pawnList.Add(targetPawn);
                            }
                        }
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyAfflictedPawn(Pawn pawn, int radius, List<string> validAfflictionDefnames)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed)
                {
                    if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        using (IEnumerator<Hediff> enumerator = targetPawn.health.hediffSet.GetHediffs<Hediff>().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Hediff rec = enumerator.Current;
                                for(int j =0; j < validAfflictionDefnames.Count; j++)
                                {
                                    if (rec.def.defName.Contains(validAfflictionDefnames[j]))
                                    {
                                        pawnList.Add(targetPawn);
                                    }
                                }                                    
                                
                            }
                        }
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyAddictedPawn(Pawn pawn, int radius, List<string> validAddictionDefnames)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed)
                {
                    if (targetPawn.IsColonist && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        using (IEnumerator<Hediff_Addiction> enumerator = targetPawn.health.hediffSet.GetHediffs<Hediff_Addiction>().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                Hediff_Addiction rec = enumerator.Current;
                                for (int j = 0; j < validAddictionDefnames.Count; j++)
                                {
                                    if (rec.Chemical.defName.Contains(validAddictionDefnames[j]))
                                    {
                                        pawnList.Add(targetPawn);
                                    }
                                }

                            }
                        }
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static Pawn FindNearbyEnemy(Pawn pawn, int radius)
        {
            return FindNearbyEnemy(pawn.Position, pawn.Map, pawn.Faction, radius, 0);
        }

        public static Pawn FindNearbyEnemy(IntVec3 position, Map map, Faction faction, float radius, float minRange)
        {
            List<Pawn> mapPawns = map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                {
                    if (targetPawn.Position != position && targetPawn.HostileTo(faction) && (position - targetPawn.Position).LengthHorizontal <= radius && (position - targetPawn.Position).LengthHorizontal > minRange)
                    {
                        pawnList.Add(targetPawn);
                        targetPawn = null;
                    }
                    else
                    {
                        targetPawn = null;
                    }
                }                
            }
            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static List<Pawn> FindPawnsNearTarget(Pawn pawn, int radius, IntVec3 targetCell, bool hostile)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed && !targetPawn.Downed)
                {
                    if (targetPawn != pawn && (targetCell - targetPawn.Position).LengthHorizontal <= radius)
                    {
                        if (hostile && targetPawn.HostileTo(pawn.Faction))
                        {
                            pawnList.Add(targetPawn);
                        }
                        else if(!hostile && !targetPawn.HostileTo(pawn.Faction))
                        {
                            pawnList.Add(targetPawn);
                        }
                    }
                    targetPawn = null;                    
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList;
            }
            else
            {
                return null;
            }
        }

        public static bool HasLoSFromTo(IntVec3 root, LocalTargetInfo targ, Thing caster, float minRange, float maxRange)
        {
            float range = (targ.Cell - root).LengthHorizontal;
            if (targ.HasThing && targ.Thing.Map != caster.Map)
            {
                return false;
            }
            if (range <= minRange || range >= maxRange)
            {
                return false;
            }
            CellRect cellRect = (!targ.HasThing) ? CellRect.SingleCell(targ.Cell) : targ.Thing.OccupiedRect();
            if (caster is Pawn)
            {
                if (GenSight.LineOfSight(caster.Position, targ.Cell, caster.Map, skipFirstCell: true))
                {
                    return true;
                }
                List<IntVec3> tempLeanShootSources = new List<IntVec3>();
                ShootLeanUtility.LeanShootingSourcesFromTo(root, cellRect.ClosestCellTo(root), caster.Map, tempLeanShootSources);
                for (int i = 0; i < tempLeanShootSources.Count; i++)
                {
                    IntVec3 intVec = tempLeanShootSources[i];
                    if (GenSight.LineOfSight(intVec, targ.Cell, caster.Map, skipFirstCell: true))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if(GenSight.LineOfSight(root, targ.Cell, caster.Map, skipFirstCell: true))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Thing> FindNearbyDamagedBuilding(Pawn pawn, int radius)
        {
            List<Thing> mapBuildings = pawn.Map.listerBuildingsRepairable.RepairableBuildings(pawn.Faction);
            List<Thing> buildingList = new List<Thing>();
            Building building= null;
            buildingList.Clear();
            for (int i = 0; i < mapBuildings.Count; i++)
            {
                building = mapBuildings[i] as Building;
                if (building != null && (building.Position - pawn.Position).LengthHorizontal <= radius && building.def.useHitPoints && building.HitPoints != building.MaxHitPoints)
                {
                    if (pawn.Drafted && building.def.designationCategory == DesignationCategoryDefOf.Security || building.def.building.ai_combatDangerous)
                    {
                        buildingList.Add(building);
                    }
                    else if(!pawn.Drafted)
                    {
                        buildingList.Add(building);
                    }
                }
                building = null;                
            }

            if (buildingList.Count > 0)
            {
                return buildingList;
            }
            else
            {
                return null;
            }
        }

        public static Thing FindNearbyDamagedThing(Pawn pawn, int radius)
        {
            List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
            List<Thing> pawnList = new List<Thing>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && targetPawn.Spawned && !targetPawn.Dead && !targetPawn.Destroyed)
                {
                    //Log.Message("evaluating targetpawn " + targetPawn.LabelShort);
                    //Log.Message("pawn faction is " + targetPawn.Faction);
                    //Log.Message("pawn position is " + targetPawn.Position);
                    //Log.Message("pawn is robot: " + TM_Calc.IsRobotPawn(targetPawn));
                    if (targetPawn.Faction != null && targetPawn.Faction == pawn.Faction && (pawn.Position - targetPawn.Position).LengthHorizontal <= radius && TM_Calc.IsRobotPawn(targetPawn))
                    {
                        float injurySeverity = 0;
                        using (IEnumerator<BodyPartRecord> enumerator = targetPawn.health.hediffSet.GetInjuredParts().GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                BodyPartRecord rec = enumerator.Current;
                                IEnumerable<Hediff_Injury> arg_BB_0 = targetPawn.health.hediffSet.GetHediffs<Hediff_Injury>();
                                Func<Hediff_Injury, bool> arg_BB_1;
                                arg_BB_1 = ((Hediff_Injury injury) => injury.Part == rec);

                                foreach (Hediff_Injury current in arg_BB_0.Where(arg_BB_1))
                                {
                                    bool flag5 = !current.IsPermanent();
                                    if (flag5)
                                    {
                                        injurySeverity += current.Severity;
                                    }
                                }
                            }
                        }
                        
                        if (injurySeverity != 0)
                        {
                            pawnList.Add(targetPawn as Thing);
                        }
                    }
                    targetPawn = null;                    
                }
            }

            List<Thing> buildingList = TM_Calc.FindNearbyDamagedBuilding(pawn, radius);
            if (buildingList != null)
            {
                for (int i = 0; i < buildingList.Count; i++)
                {
                    pawnList.Add(buildingList[i]);
                }
            }

            if (pawnList.Count > 0)
            {
                return pawnList.RandomElement();
            }
            else
            {
                return null;
            }
        }

        public static List<Pawn> FindAllPawnsAround(Map map, IntVec3 center, float radius, Faction faction = null, bool sameFaction = false)
        {
            List<Pawn> mapPawns = map.mapPawns.AllPawnsSpawned;
            List<Pawn> pawnList = new List<Pawn>();
            Pawn targetPawn = null;
            pawnList.Clear();
            for (int i = 0; i < mapPawns.Count; i++)
            {
                targetPawn = mapPawns[i];
                if (targetPawn != null && !targetPawn.Dead && !targetPawn.Destroyed)
                {
                    if (faction != null && !sameFaction)
                    {
                        if ((targetPawn.Position - center).LengthHorizontal <= radius)
                        {
                            if (targetPawn.Faction != null)
                            {
                                if (targetPawn.Faction != faction)
                                {
                                    pawnList.Add(targetPawn);
                                    targetPawn = null;
                                }
                                else
                                {
                                    targetPawn = null;
                                }
                            }
                            else
                            {
                                pawnList.Add(targetPawn);
                                targetPawn = null;
                            }
                        }
                        else
                        {
                            targetPawn = null;
                        }
                    }
                    else if(faction != null && sameFaction)
                    {
                        if (targetPawn.Faction != null && targetPawn.Faction == faction && (targetPawn.Position - center).LengthHorizontal <= radius)
                        {
                            pawnList.Add(targetPawn);
                            targetPawn = null;
                        }
                        else
                        {
                            targetPawn = null;
                        }
                    }
                    else
                    {
                        if((targetPawn.Position - center).LengthHorizontal <= radius)
                        {
                            pawnList.Add(targetPawn);
                            targetPawn = null;
                        }
                        else
                        {
                            targetPawn = null;
                        }
                    }
                }
            }
            if (pawnList.Count > 0)
            {
                return pawnList;
            }
            else
            {
                return null;
            }
        }

        public static float GetArcaneResistance(Pawn pawn, bool includePsychicSensitivity)
        {
            float resistance = 0;
            CompAbilityUserMagic compMagic = pawn.GetComp<CompAbilityUserMagic>();
            if(compMagic != null)
            {
                resistance += (compMagic.arcaneRes - 1);
            }

            CompAbilityUserMight compMight = pawn.GetComp<CompAbilityUserMight>();
            if(compMight != null)
            {
                resistance += (compMight.arcaneRes - 1);
            }

            if (includePsychicSensitivity && resistance == 0f)
            {
                resistance += (1 - pawn.GetStatValue(StatDefOf.PsychicSensitivity, false))/2;
            }

            if (pawn.health != null && pawn.health.capacities != null)
            {
                resistance += (pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness) - 1);
            }

            return resistance;
        }

        public static float GetSpellPenetration(Pawn pawn)
        {
            float penetration = 0;
            CompAbilityUserMagic compMagic = pawn.GetComp<CompAbilityUserMagic>();
            if (compMagic != null)
            {
                penetration += (compMagic.arcaneDmg - 1);
            }

            CompAbilityUserMight compMight = pawn.GetComp<CompAbilityUserMight>();
            if (compMight != null)
            {
                penetration += (compMight.mightPwr - 1);
            }

            if (pawn.health != null && pawn.health.capacities != null)
            {
                penetration += (pawn.health.capacities.GetLevel(PawnCapacityDefOf.Consciousness) - 1);
            }

            return penetration;
        }

        public static float GetSpellSuccessChance(Pawn caster, Pawn victim, bool usePsychicSensitivity = true)
        {
            float successChance;
            float penetration = TM_Calc.GetSpellPenetration(caster);
            float resistance = TM_Calc.GetArcaneResistance(victim, usePsychicSensitivity);
            successChance = 1f + penetration - resistance;
            return successChance;
        }

        public static List<ThingDef> GetAllRaceBloodTypes()
        {
            List<ThingDef> bloodTypes = new List<ThingDef>();
            bloodTypes.Clear();

            IEnumerable<ThingDef> enumerable = from def in DefDatabase<ThingDef>.AllDefs
                                                  where (def.race != null && def.race.BloodDef != null)
                                                  select def;

            foreach (ThingDef current in enumerable)
            {
                bloodTypes.AddDistinct(current.race.BloodDef);                
            }

            //for(int i =0; i< bloodTypes.Count; i++)
            //{
            //    Log.Message("blood type includes " + bloodTypes[i].defName);
            //}
            return bloodTypes;
        }

        public static T Clone<T>(T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("The type must be serializable.", "source");
            }

            // Don't serialize a null object, simply return the default for that object
            if (System.Object.ReferenceEquals(source, null))
            {
                return default(T);
            }

            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream();
            using (stream)
            {
                formatter.Serialize(stream, source);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(stream);
            }
        }

        //Rand.Chance(((settingsRef.baseFighterChance * 6) + (settingsRef.baseMageChance * 6) + (8 * settingsRef.advFighterChance) + (16 * settingsRef.advMageChance)) / (allTraits.Count))
        public static float GetRWoMTraitChance()
        {
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            List<TraitDef> allTraits = DefDatabase<TraitDef>.AllDefsListForReading;
            float chance = ((settingsRef.baseFighterChance * 6) + (settingsRef.baseMageChance * 6) + (8 * settingsRef.advFighterChance) + (16 * settingsRef.advMageChance)) / (allTraits.Count);
            return Mathf.Clamp01(chance);
        }
        
        public static float GetMagePrecurserChance()
        {
            float chance = 0f;
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            chance = (settingsRef.baseMageChance * 6) / ((settingsRef.baseFighterChance * 6) + (settingsRef.baseMageChance * 6) + (8 * settingsRef.advFighterChance) + (16 * settingsRef.advMageChance));
            chance *= GetRWoMTraitChance();
            return chance;
        }

        public static float GetFighterPrecurserChance()
        {
            float chance = 0f;
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            chance = (settingsRef.baseFighterChance * 6) / ((settingsRef.baseFighterChance * 6) + (settingsRef.baseMageChance * 6) + (8 * settingsRef.advFighterChance) + (16 * settingsRef.advMageChance));
            chance *= GetRWoMTraitChance();
            return chance;
        }

        public static float GetMageSpawnChance()
        {
            float chance = 0f;
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            chance = (settingsRef.advMageChance * 16) / ((settingsRef.baseFighterChance * 6) + (settingsRef.baseMageChance * 6) + (8 * settingsRef.advFighterChance) + (16 * settingsRef.advMageChance));
            chance *= GetRWoMTraitChance();
            return chance;
        }

        public static float GetFighterSpawnChance()
        {
            float chance = 0f;
            ModOptions.SettingsRef settingsRef = new ModOptions.SettingsRef();
            chance = (settingsRef.advFighterChance * 8) / ((settingsRef.baseFighterChance * 6) + (settingsRef.baseMageChance * 6) + (8 * settingsRef.advFighterChance) + (16 * settingsRef.advMageChance));
            chance *= GetRWoMTraitChance();
            return chance;
        }

        public static Area GetSpriteArea()
        {
            Area spriteArea = null;
            List<Area> allAreas = Find.CurrentMap.areaManager.AllAreas;
            if (allAreas != null && allAreas.Count > 0)
            {
                for (int i = 0; i < allAreas.Count; i++)
                {
                    if(allAreas[i].Label == "earth sprites")
                    {
                        spriteArea = allAreas[i];
                    }
                }
            }
            if(spriteArea == null)
            {
                Area_Allowed newArea = null;
                if(Find.CurrentMap.areaManager.TryMakeNewAllowed(out newArea))
                {
                    newArea.SetLabel("earth sprites");                    
                }
            }
            return spriteArea;
        }

        public static List<Apparel> GetNecroticOrbs(Pawn pawn)
        {
            List<Apparel> orbs = new List<Apparel>();
            orbs.Clear();
            if (pawn.Map != null)
            {
                List<Pawn> mapPawns = pawn.Map.mapPawns.AllPawnsSpawned;
                for (int i = 0; i < mapPawns.Count; i++)
                {
                    if (mapPawns[i].RaceProps.Humanlike && mapPawns[i].apparel != null && mapPawns[i].Faction == pawn.Faction && mapPawns[i].apparel.WornApparelCount > 0)
                    {
                        List<Apparel> apparelList = mapPawns[i].apparel.WornApparel;
                        for (int j = 0; j < apparelList.Count; j++)
                        {
                            if (apparelList[j].def == TorannMagicDefOf.TM_Artifact_NecroticOrb)
                            {
                                orbs.Add(apparelList[j]);
                            }
                        }
                    }
                }
            }
            else if (pawn.ParentHolder.ToString().Contains("Caravan"))
            {
                foreach (Pawn current in pawn.holdingOwner)
                {
                    if (current != null)
                    {
                        if (current.RaceProps.Humanlike && current.Faction == pawn.Faction && current.apparel != null && current.apparel.WornApparelCount > 0)
                        {
                            List<Apparel> apparelList = current.apparel.WornApparel;
                            for (int j = 0; j < apparelList.Count; j++)
                            {
                                if (apparelList[j].def == TorannMagicDefOf.TM_Artifact_NecroticOrb)
                                {
                                    orbs.Add(apparelList[j]);
                                }
                            }
                        }
                    }
                }
            }
            return orbs;
        }
    }
}
