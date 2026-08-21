using Hazel;
using System;
using System.Text.RegularExpressions;
using BHR.Modules;
using BHR.Modules.ChatManager;
using BHR.Modules.Rpc;
using BHR.Patches;
using BHR.Roles.Coven;
using BHR.Roles.Double;
using BHR.Roles.Impostor;
using UnityEngine;
using static BHR.Translator;
using static BHR.Utils;

namespace BHR.Roles.Crewmate;

internal class JudgeOld : RoleBase
{
    //===========================SETUP================================\\
    public override CustomRoles Role => CustomRoles.JudgeOld;
    private const int Id = 10700;
    public override CustomRoles ThisRoleBase => CustomRoles.Crewmate;
    public override Custom_RoleType ThisRoleType => Custom_RoleType.CrewmateKilling;
    //==================================================================\\

    public static OptionItem TrialLimitPerMeeting;
    private static OptionItem TrialLimitPerGame;
    private static OptionItem CanTrialMadmate;
    private static OptionItem CanTrialCharmed;
    private static OptionItem CanTrialSidekick;
    private static OptionItem CanTrialInfected;
    private static OptionItem CanTrialContagious;
    private static OptionItem CanTrialEnchanted;
    private static OptionItem CanTrialCrewKilling;
    private static OptionItem CanTrialNeutralB;
    private static OptionItem CanTrialNeutralP;
    private static OptionItem CanTrialNeutralK;
    private static OptionItem CanTrialNeutralE;
    private static OptionItem CanTrialNeutralC;
    private static OptionItem CanTrialNeutralA;
    private static OptionItem CanTrialCoven;
    private static OptionItem CanTrialAdmired;

    private static readonly Dictionary<byte, int> TrialLimitMeeting = [];
    private static readonly Dictionary<byte, int> TrialLimitGame = [];

    public override void SetupCustomOption()
    {
        Options.SetupRoleOptions(Id, TabGroup.CrewmateRoles, CustomRoles.JudgeOld);
        TrialLimitPerMeeting = IntegerOptionItem.Create(Id + 10, "JudgeOldTrialLimitPerMeeting", new(1, 30, 1), 1, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld])
            .SetValueFormat(OptionFormat.Times);
        TrialLimitPerGame = IntegerOptionItem.Create(Id + 25, "JudgeOldTrialLimitPerGame", new(1, 30, 1), 1, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld])
            .SetValueFormat(OptionFormat.Times);
        CanTrialMadmate = BooleanOptionItem.Create(Id + 12, "JudgeOldCanTrialMadmate", true, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialCharmed = BooleanOptionItem.Create(Id + 16, "JudgeOldCanTrialCharmed", true, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialSidekick = BooleanOptionItem.Create(Id + 19, "JudgeOldCanTrialSidekick", true, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialInfected = BooleanOptionItem.Create(Id + 20, "JudgeOldCanTrialInfected", true, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialContagious = BooleanOptionItem.Create(Id + 21, "JudgeOldCanTrialContagious", true, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialEnchanted = BooleanOptionItem.Create(Id + 24, "JudgeOldCanTrialEnchanted", true, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialCrewKilling = BooleanOptionItem.Create(Id + 13, "JudgeOldCanTrialnCrewKilling", true, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialNeutralB = BooleanOptionItem.Create(Id + 14, "JudgeOldCanTrialNeutralB", false, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialNeutralP = BooleanOptionItem.Create(Id + 27, "JudgeOldCanTrialNeutralP", false, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialNeutralE = BooleanOptionItem.Create(Id + 17, "JudgeOldCanTrialNeutralE", false, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialNeutralC = BooleanOptionItem.Create(Id + 18, "JudgeOldCanTrialNeutralC", false, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialNeutralK = BooleanOptionItem.Create(Id + 15, "JudgeOldCanTrialNeutralK", true, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialNeutralA = BooleanOptionItem.Create(Id + 22, "JudgeOldCanTrialNeutralA", true, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialCoven = BooleanOptionItem.Create(Id + 23, "JudgeOldCanTrialCoven", true, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
        CanTrialAdmired = BooleanOptionItem.Create(Id + 26, "JudgeOldCanTrialAdmired", true, TabGroup.CrewmateRoles, false).SetParent(Options.CustomRoleSpawnChances[CustomRoles.JudgeOld]);
    }
    public override void Init()
    {
        TrialLimitMeeting.Clear();
        TrialLimitGame.Clear();
    }
    public override void Add(byte playerId)
    {
        TrialLimitMeeting[playerId] = TrialLimitPerMeeting.GetInt();
        TrialLimitGame[playerId] = TrialLimitPerGame.GetInt();
        playerId.SetAbilityUseLimit(TrialLimitPerGame.GetInt());
    }
    public override void Remove(byte playerId)
    {
        TrialLimitMeeting.Remove(playerId);
        TrialLimitGame.Remove(playerId);
    }
    public override void OnReportDeadBody(PlayerControl party, NetworkedPlayerInfo dinosaur)
    {
        if (!_Player) return;

        TrialLimitMeeting[_Player.PlayerId] = TrialLimitPerMeeting.GetInt();

        if (TrialLimitGame[_Player.PlayerId] <= TrialLimitPerMeeting.GetInt())
        {
            _Player.SetAbilityUseLimit(TrialLimitGame[_Player.PlayerId]);
        }
        else
        {
            _Player.SetAbilityUseLimit(TrialLimitPerMeeting.GetInt());
        }
    }
    public override void AfterMeetingTasks()
    {
        if (!_Player) return;
        _Player.SetAbilityUseLimit(TrialLimitGame[_Player.PlayerId]);
    }

    public static void TrialCommand(PlayerControl player, string commandKey, string text, string[] args)
    {
        if (!AmongUsClient.Instance.AmHost)
        {
            ChatCommands.RequestCommandProcessingFromHost(text, commandKey);
            return;
        }
        
        if (player.Is(CustomRoles.JudgeOld))
            TrialMsg(player, text);
        else if (player.Is(CustomRoles.Councillor))
            Councillor.MurderMsg(player, text);
    }

    public static bool TrialMsg(PlayerControl pc, string msg, bool isUI = false)
    {
        var originMsg = msg;

        if (!AmongUsClient.Instance.AmHost) return false;
        if (!GameStates.IsMeeting || pc == null || GameStates.IsExilling) return false;
        if (!pc.Is(CustomRoles.JudgeOld)) return false;

        msg = msg.ToLower().TrimStart().TrimEnd();

        if (!pc.IsAlive())
        {
            pc.ShowInfoMessage(isUI, GetString("JudgeOldDead"));
            return true;
        }

        if (!MsgToPlayerAndRole(msg, out byte targetId, out string error))
        {
            SendMessage(error, pc.PlayerId);
            return true;
        }
        var target = GetPlayerById(targetId);
        if (target != null)
        {
            Logger.Info($"{pc.GetNameWithRole()} try trial {target.GetNameWithRole()}", "JudgeOld");
            bool judgeSuicide = true;
            if (TrialLimitMeeting[pc.PlayerId] < 1)
            {
                pc.ShowInfoMessage(isUI, GetString("JudgeOldTrialMaxMeetingMsg"));
                return true;
            }
            if (pc.GetAbilityUseLimit() < 1)
            {
                pc.ShowInfoMessage(isUI, GetString("JudgeOldTrialMaxGameMsg"));
            }
            if (target.Is(CustomRoles.VoodooMaster) && VoodooMaster.Dolls[target.PlayerId].Count > 0)
            {
                target = GetPlayerById(VoodooMaster.Dolls[target.PlayerId].Where(x => GetPlayerById(x).IsAlive()).ToList().RandomElement());
                SendMessage(string.Format(GetString("VoodooMasterTargetInMeeting"), target.GetRealName()), Utils.GetPlayerListByRole(CustomRoles.VoodooMaster).First().PlayerId);
            }
            if (Jailer.IsTarget(target.PlayerId))
            {
                pc.ShowInfoMessage(isUI, GetString("CanNotTrialJailed"), ColorString(GetRoleColor(CustomRoles.Jailer), GetString("Jailer").ToUpper()));
                return true;
            }
            if (pc.PlayerId == target.PlayerId)
            {
                pc.ShowInfoMessage(isUI, GetString("JudgeOld_LaughToWhoTrialSelf"), ColorString(Color.cyan, GetString("MessageFromKPD")));
                goto SkipToPerform;
            }
            if (target.Is(CustomRoles.NiceMini) && Mini.Age < 18)
            {
                pc.ShowInfoMessage(isUI, GetString("GuessMini"));
                return true;
            }
            if (target.Is(CustomRoles.PunchingBag))
            {
                pc.ShowInfoMessage(isUI, GetString("EradicatePunchingBag"));
                return true;
            }

            if (target.Is(CustomRoles.Rebound))
            {
                Logger.Info($"{pc.GetNameWithRole()} judged {target.GetNameWithRole()}, judge sucide = true because target rebound", "JudgeOldTrialMsg");
                judgeSuicide = true;
            }
            else if (target.Is(CustomRoles.Onbound))
            {
                pc.ShowInfoMessage(isUI, GetString("GuessOnbound"));
                return true;
            }
            else if (target.Is(CustomRoles.Solsticer))
            {
                pc.ShowInfoMessage(isUI, GetString("GuessSolsticer"));
                return true;
            }
            else if (target.Is(CustomRoles.Admired) && !CanTrialAdmired.GetBool()) judgeSuicide = true;
            else if (target.IsTransformedNeutralApocalypse()) judgeSuicide = true;
            else if (Medic.IsProtected(target.PlayerId) && !Medic.GuesserIgnoreShield.GetBool())
            {
                pc.ShowInfoMessage(isUI, GetString("GuessShielded"));
                return true;
            }
            else if (Guardian.CannotBeKilled(target))
            {
                pc.ShowInfoMessage(isUI, GetString("GuessGuardianTask"));
                return true;
            }
            else if (pc.IsAnySubRole(x => x.IsConverted())) judgeSuicide = false;
            else if (target.Is(CustomRoles.Trickster)) judgeSuicide = true;
            else if (target.Is(CustomRoles.Rascal)) judgeSuicide = false;
            else if (target.Is(CustomRoles.Narc)) judgeSuicide = true;
            else if ((target.Is(CustomRoles.Sidekick) || target.Is(CustomRoles.Recruit)) && CanTrialSidekick.GetBool()) judgeSuicide = false;
            else if ((target.GetCustomRole().IsMadmate() || target.Is(CustomRoles.Madmate)) && CanTrialMadmate.GetBool()) judgeSuicide = false;
            else if (target.Is(CustomRoles.Infected) && CanTrialInfected.GetBool()) judgeSuicide = false;
            else if (target.Is(CustomRoles.Contagious) && CanTrialContagious.GetBool()) judgeSuicide = false;
            else if (target.Is(CustomRoles.Charmed) && CanTrialCharmed.GetBool()) judgeSuicide = false;
            else if (target.Is(CustomRoles.Enchanted) && CanTrialEnchanted.GetBool()) judgeSuicide = false;
            else if (target.GetCustomRole().IsCrewKiller() && CanTrialCrewKilling.GetBool()) judgeSuicide = false;
            else if (target.GetCustomRole().IsNK() && CanTrialNeutralK.GetBool()) judgeSuicide = false;
            else if (target.GetCustomRole().IsNB() && CanTrialNeutralB.GetBool()) judgeSuicide = false;
            else if (target.GetCustomRole().IsNP() && CanTrialNeutralP.GetBool()) judgeSuicide = false;
            else if (target.GetCustomRole().IsNE() && CanTrialNeutralE.GetBool()) judgeSuicide = false;
            else if (target.GetCustomRole().IsNC() && CanTrialNeutralC.GetBool()) judgeSuicide = false;
            else if (target.GetCustomRole().IsNA() && CanTrialNeutralA.GetBool()) judgeSuicide = false;
            else if (target.GetCustomRole().IsCoven() && CanTrialCoven.GetBool()) judgeSuicide = false;
            else if (target.GetCustomRole().IsImpostor()) judgeSuicide = false;
            else
            {
                Logger.Warn("Impossibe to reach here!", "JudgeOldTrial");
                judgeSuicide = true;
            }

        SkipToPerform:
            var dp = judgeSuicide ? pc : target;
            target = dp;

            string Name = dp.GetRealName();

            TrialLimitMeeting[pc.PlayerId]--;
            TrialLimitGame[pc.PlayerId]--;
            pc.RpcRemoveAbilityUse();

            if (!GameStates.IsProceeding)
                _ = new LateTask(() =>
                {
                    dp.SetDeathReason(PlayerState.DeathReason.Trialed);
                    dp.SetRealKiller(pc);
                    GuessManager.RpcGuesserMurderPlayer(dp);

                    Main.PlayersDiedInMeeting.Add(dp.PlayerId);
                    MurderPlayerPatch.AfterPlayerDeathTasks(pc, dp, true);

                    _ = new LateTask(() => { SendMessage(string.Format(GetString("JudgeOld_TrialKill"), Name), 255, ColorString(GetRoleColor(CustomRoles.JudgeOld), GetString("JudgeOld_TrialKillTitle")), true); }, 0.6f, "Guess Msg");

                }, 0.2f, "Trial Kill");
        }
        return true;
    }
    private static bool MsgToPlayerAndRole(string msg, out byte id, out string error)
    {
        if (msg.StartsWith("/")) msg = msg.Replace("/", string.Empty);

        Regex r = new("\\d+");
        MatchCollection mc = r.Matches(msg);
        string result = string.Empty;
        for (int i = 0; i < mc.Count; i++)
        {
            result += mc[i];
        }

        if (int.TryParse(result, out int num))
        {
            id = Convert.ToByte(num);
        }
        else
        {
            id = byte.MaxValue;
            error = GetString("JudgeOld_TrialHelp");
            return false;
        }

        PlayerControl target = GetPlayerById(id);
        if (target == null || target.Data.IsDead || !target.IsAlive())
        {
            error = GetString("JudgeOld_TrialNull");
            return false;
        }

        error = string.Empty;
        return true;
    }

    private static void SendRPC(byte targetId)
    {
        RoleRpcs.SendJudgeOld(PlayerControl.LocalPlayer.NetId, targetId);
    }
    public static void ReceiveRPC_Custom(MessageReader reader, PlayerControl pc)
    {
        byte targetId = reader.ReadByte();

        TrialMsg(pc, $"/tl {targetId}", true);
    }

    private static void JudgeOnClick(byte targetId /*, MeetingHud __instance*/)
    {
        Logger.Msg($"Click: ID {targetId}", "Judge UI");
        var target = targetId.GetPlayer();
        if (target == null || !target.IsAlive() || !GameStates.IsVoting) return;
        if (AmongUsClient.Instance.AmHost) TrialMsg(PlayerControl.LocalPlayer, $"/tl {targetId}", true);
        else SendRPC(targetId);
    }

    public override string NotifyPlayerName(PlayerControl seer, PlayerControl target, string TargetPlayerName = "", bool IsForMeeting = false)
        => IsForMeeting && seer.IsAlive() && target.IsAlive() ? ColorString(GetRoleColor(CustomRoles.JudgeOld), target.PlayerId.ToString()) + " " + TargetPlayerName : "";
    public override string PVANameText(PlayerVoteArea pva, PlayerControl seer, PlayerControl target)
        => seer.IsAlive() && target.IsAlive() ? ColorString(GetRoleColor(CustomRoles.JudgeOld), target.PlayerId.ToString()) + " " + pva.NameText.text : "";

    [HarmonyPatch(typeof(MeetingHud), nameof(MeetingHud.Start))]
    class StartMeetingPatch
    {
        public static void Postfix(MeetingHud __instance)
        {
            if (PlayerControl.LocalPlayer.Is(CustomRoles.JudgeOld) && PlayerControl.LocalPlayer.IsAlive())
                CreateJudgeButton(__instance);
        }
    }
    public static void CreateJudgeButton(MeetingHud __instance)
    {
        foreach (var pva in __instance.playerStates)
        {
            var pc = GetPlayerById(pva.TargetPlayerId);
            if (pc == null || !pc.IsAlive()) continue;

            GameObject template = pva.Buttons.transform.Find("CancelButton").gameObject;
            GameObject targetBox = UnityEngine.Object.Instantiate(template, pva.transform);
            targetBox.name = "ShootButton";
            targetBox.transform.localPosition = new Vector3(-0.35f, 0.03f, -1.31f);
            SpriteRenderer renderer = targetBox.GetComponent<SpriteRenderer>();
            renderer.sprite = CustomButton.Get("JudgeIcon");
            PassiveButton button = targetBox.GetComponent<PassiveButton>();
            button.OnClick.RemoveAllListeners();
            button.OnClick.AddListener((UnityEngine.Events.UnityAction)(() => JudgeOnClick(pva.TargetPlayerId/*, __instance*/)));
        }
    }
}
