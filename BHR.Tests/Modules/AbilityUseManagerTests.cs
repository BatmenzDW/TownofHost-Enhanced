using Xunit;
using BHR.Modules;

namespace BHR.Tests.Modules;
public class AbilityUseManagerTests
{

    public AbilityUseManagerTests()
    {
        AbilityUseManager.Initialize();
    }

    [Fact]
    public void Initialize_ClearsAbilityUseLimits()
    {
        AbilityUseManager.SetAbilityUseLimit(0, 0.5f, rpc: false, log: false);
        AbilityUseManager.Initialize();
        Assert.Equal(float.NaN, AbilityUseManager.GetAbilityUseLimit(0));
    }
}