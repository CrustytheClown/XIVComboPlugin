using Dalamud.Game.ClientState.JobGauge.Types;

namespace XIVComboExpandedPlugin.Combos;

internal static class BLM
{
    public const byte ClassID = 7;
    public const byte JobID = 25;

    public const uint
        Fire = 141,
        Blizzard = 142,
        Thunder = 144,
        Fire2 = 147,
        Transpose = 149,
        Fire3 = 152,
        Thunder3 = 153,
        Blizzard3 = 154,
        Scathe = 156,
        Freeze = 159,
        Flare = 162,
        LeyLines = 3573,
        Sharpcast = 3574,
        Blizzard4 = 3576,
        Fire4 = 3577,
        BetweenTheLines = 7419,
        Thunder4 = 7420,
        Despair = 16505,
        UmbralSoul = 16506,
        Xenoglossy = 16507,
        Blizzard2 = 25793,
        HighFire2 = 25794,
        HighBlizzard2 = 25795,
        Paradox = 25797;

    public static class Buffs
    {
        public const ushort
            Thundercloud = 164,
            Firestarter = 165,
            Swiftcast = 167,
            LeyLines = 737,
            Sharpcast = 867,
            Triplecast = 1211,
            EnhancedFlare = 2960;
    }

    public static class Debuffs
    {
        public const ushort
            Thunder = 161,
            Thunder3 = 163;
    }

    public static class Levels
    {
        public const byte
            Fire3 = 35,
            Blizzard3 = 35,
            Freeze = 40,
            Thunder3 = 45,
            Flare = 50,
            Sharpcast = 54,
            Blizzard4 = 58,
            Fire4 = 60,
            BetweenTheLines = 62,
            Despair = 72,
            UmbralSoul = 76,
            Xenoglossy = 80,
            HighFire2 = 82,
            HighBlizzard2 = 82,
            EnhancedSharpcast2 = 88,
            Paradox = 90;
    }

    public static class MpCosts
    {
        public const ushort
            Fire2 = 3000,
            Flare = 800;
    }
}

internal class BlackFireBlizzard4 : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BlmAny;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == BLM.Blizzard4)
        {
            var gauge = GetJobGauge<BLMGauge>();

            if (IsEnabled(CustomComboPreset.BlackSpellsUmbralSoulFeature))
            {
                if (level >= BLM.Levels.UmbralSoul && gauge.InUmbralIce && !HasTarget())
                    return BLM.UmbralSoul;
            }
        }

        if (actionID == BLM.Fire4 || actionID == BLM.Blizzard4)
        {
            var gauge = GetJobGauge<BLMGauge>();

            if (IsEnabled(CustomComboPreset.BlackEnochianFeature))
            {
                if (gauge.InAstralFire)
                {
                    if (IsEnabled(CustomComboPreset.BlackEnochianDespairFeature))
                    {
                        if (level >= BLM.Levels.Despair && LocalPlayer?.CurrentMp < 2400)
                            return BLM.Despair;
                    }

                    if (IsEnabled(CustomComboPreset.BlackEnochianNoSyncFeature) || level >= BLM.Levels.Fire4)
                        return BLM.Fire4;

                    return BLM.Fire;
                }

                if (gauge.InUmbralIce)
                {
                    if (IsEnabled(CustomComboPreset.BlackEnochianNoSyncFeature) || level >= BLM.Levels.Blizzard4)
                        return BLM.Blizzard4;

                    return BLM.Blizzard;
                }
            }
        }

        return actionID;
    }
}

internal class BlackTranspose : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BlackTransposeUmbralSoulFeature;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == BLM.Transpose)
        {
            var gauge = GetJobGauge<BLMGauge>();

            if (level >= BLM.Levels.UmbralSoul && gauge.IsEnochianActive && gauge.InUmbralIce)
                return BLM.UmbralSoul;
        }

        return actionID;
    }
}

internal class BlackUmbralSoul : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BlackUmbralSoulTransposeFeature;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == BLM.UmbralSoul)
        {
            var gauge = GetJobGauge<BLMGauge>();

            if (level < BLM.Levels.UmbralSoul || (gauge.IsEnochianActive && gauge.InAstralFire))
                return BLM.Transpose;
        }

        return actionID;
    }
}

internal class BlackLeyLines : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BlackLeyLinesFeature;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == BLM.LeyLines)
        {
            if (level >= BLM.Levels.BetweenTheLines && HasEffect(BLM.Buffs.LeyLines))
                return BLM.BetweenTheLines;
        }

        return actionID;
    }
}

internal class BlackFire : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BlackFireFeature;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == BLM.Fire)
        {
            var gauge = GetJobGauge<BLMGauge>();

            if (level >= BLM.Levels.Paradox && gauge.IsParadoxActive && gauge.InUmbralIce)
                return BLM.Paradox;

            if (level >= BLM.Levels.Fire3)
            {
                if (IsEnabled(CustomComboPreset.BlackFireOption))
                {
                    if (gauge.AstralFireStacks < 3)
                        return BLM.Fire3;
                }

                if (IsNotEnabled(CustomComboPreset.BlackFireOption2))
                {
                    if (!gauge.InAstralFire)
                        return BLM.Fire3;
                }

                if (HasEffect(BLM.Buffs.Firestarter))
                    return BLM.Fire3;
            }
        }

        return actionID;
    }
}

internal class BlackBlizzard : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BlmAny;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == BLM.Blizzard || actionID == BLM.Blizzard3)
        {
            var gauge = GetJobGauge<BLMGauge>();

            if (IsEnabled(CustomComboPreset.BlackSpellsUmbralSoulFeature))
            {
                if (level >= BLM.Levels.UmbralSoul && gauge.InUmbralIce && !HasTarget())
                    return BLM.UmbralSoul;
            }

            if (IsEnabled(CustomComboPreset.BlackBlizzardFeature))
            {
                if (level >= BLM.Levels.Paradox && gauge.IsParadoxActive)
                {
                    if (gauge.InUmbralIce || (!IsEnabled(CustomComboPreset.BlackBlizzardParadoxOption) && LocalPlayer?.CurrentMp >= 1600))
                        return BLM.Paradox;
                }

                if (level >= BLM.Levels.Blizzard3)
                    return BLM.Blizzard3;

                return BLM.Blizzard;
            }
        }

        return actionID;
    }
}

internal class BlackFreezeFlare : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BlmAny;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == BLM.Freeze)
        {
            var gauge = GetJobGauge<BLMGauge>();

            if (IsEnabled(CustomComboPreset.BlackSpellsUmbralSoulFeature))
            {
                if (level >= BLM.Levels.UmbralSoul && gauge.InUmbralIce && !HasTarget())
                    return BLM.UmbralSoul;
            }
        }

        if (actionID == BLM.Freeze || actionID == BLM.Flare)
        {
            var gauge = GetJobGauge<BLMGauge>();

            if (IsEnabled(CustomComboPreset.BlackFreezeFlareFeature))
            {
                if (level >= BLM.Levels.Freeze && gauge.InUmbralIce)
                    return BLM.Freeze;

                if (level >= BLM.Levels.Flare && gauge.InAstralFire)
                    return BLM.Flare;
            }
        }

        return actionID;
    }
}

internal class BlackFire2 : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BlackFire2Feature;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == BLM.Fire2 || actionID == BLM.HighFire2)
        {
            var gauge = GetJobGauge<BLMGauge>();

            if (IsEnabled(CustomComboPreset.BlackFireBlizzard2Option))
            {
                if (gauge.AstralFireStacks < 3)
                    return actionID;
            }

            if (level >= BLM.Levels.Flare && gauge.InAstralFire)
            {
                // Lv 50 rotation without Umbral Hearts
                if (LocalPlayer?.CurrentMp < BLM.MpCosts.Fire2 + BLM.MpCosts.Flare)
                    return BLM.Flare;

                // Standard AoE rotation Fire2 until 1 Umbral Heart, followed by 2 Flare
                if (gauge.UmbralHearts == 1 || (gauge.UmbralHearts == 0 && HasEffect(BLM.Buffs.EnhancedFlare)))
                    return BLM.Flare;

                if (IsEnabled(CustomComboPreset.BlackFire2TriplecastOption))
                {
                    int triplecasts = FindEffect(BLM.Buffs.Triplecast)?.StackCount ?? 0;

                    // (Umbral Ice) Fire2 -> Triplecast -> Fire2 -> Swiftcast -> Flare -> Flare -> Manafont -> Flare
                    if (gauge.UmbralHearts > 0 && triplecasts == 2)
                        return BLM.Flare;

                    if (triplecasts == 1)
                        return BLM.Flare;
                }
            }
        }

        return actionID;
    }
}

internal class BlackBlizzard2 : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BlmAny;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == BLM.Blizzard2 || actionID == BLM.HighBlizzard2)
        {
            var gauge = GetJobGauge<BLMGauge>();

            if (IsEnabled(CustomComboPreset.BlackSpellsUmbralSoulFeature))
            {
                if (level >= BLM.Levels.UmbralSoul && gauge.InUmbralIce && !HasTarget())
                    return BLM.UmbralSoul;
            }

            if (IsEnabled(CustomComboPreset.BlackBlizzard2Feature))
            {
                if (IsEnabled(CustomComboPreset.BlackFireBlizzard2Option))
                {
                    if (gauge.UmbralIceStacks < 3)
                        return actionID;
                }

                if (level >= BLM.Levels.Freeze && gauge.InUmbralIce)
                    return BLM.Freeze;
            }
        }

        return actionID;
    }
}

internal class BlackScathe : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BlackScatheFeature;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == BLM.Scathe)
        {
            var gauge = GetJobGauge<BLMGauge>();

            if (level >= BLM.Levels.Xenoglossy && gauge.PolyglotStacks > 0)
                return BLM.Xenoglossy;
        }

        return actionID;
    }
}

internal class BlackThunder : CustomCombo
{
    protected internal override CustomComboPreset Preset { get; } = CustomComboPreset.BlackThunderFeature;

    protected override uint Invoke(uint actionID, uint lastComboMove, float comboTime, byte level)
    {
        if (actionID == BLM.Thunder3 || actionID == BLM.Thunder4)
        {
            if (IsEnabled(CustomComboPreset.BlackThunderDelayOption) && this.IsThunderCastRecently(lastComboMove))
                return actionID;

            if (level >= BLM.Levels.EnhancedSharpcast2)
            {
                if (HasCharges(BLM.Sharpcast) && !HasEffect(BLM.Buffs.Sharpcast))
                    return BLM.Sharpcast;
            }
            else if (level >= BLM.Levels.Sharpcast)
            {
                if (IsOffCooldown(BLM.Sharpcast))
                    return BLM.Sharpcast;
            }
        }

        return actionID;
    }

    private bool IsThunderCastRecently(uint lastComboMove)
    {
        bool isGcdOnCooldown = IsOnCooldown(BLM.Thunder3); // shared with Thunder4
        return isGcdOnCooldown && (lastComboMove == BLM.Thunder3 || lastComboMove == BLM.Thunder4);
    }
}
