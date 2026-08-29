
namespace BHR.Modules;

public interface IPlayerControl
{
    byte PlayerId { get; }
    CustomRoles GetCustomRole();
    static PlayerControl LocalPlayer { get; }
}

public sealed class ModdedPlayerControl(PlayerControl player) : IPlayerControl
{
    private readonly PlayerControl player = player;

    public byte PlayerId => player.PlayerId;
    public CustomRoles GetCustomRole() => player.GetCustomRole();
    public static PlayerControl LocalPlayer => PlayerControl.LocalPlayer;


    public static implicit operator PlayerControl(ModdedPlayerControl mpc) => mpc.player;
    public static implicit operator ModdedPlayerControl(PlayerControl pc) => new(pc);
}