using Fighters.Battle;
using Fighters.Models.Fighters;
using Fighters.Tests.TestData;

namespace Fighters.Tests.Battle;

public class BattleResultTests
{
    [Fact]
    public void Victory_Always_SetsVictoryOutcomeAndWinner()
    {
        IFighter winner = FighterBuilder.CreateMock( "Winner" ).Object;

        BattleResult result = BattleResult.Victory( winner );

        Assert.Equal( BattleOutcome.Victory, result.Outcome );
        Assert.Same( winner, result.Winner );
    }

    [Fact]
    public void Stalemate_Always_SetsStalemateOutcomeAndWinner()
    {
        IFighter winner = FighterBuilder.CreateMock( "Winner" ).Object;

        BattleResult result = BattleResult.Stalemate( winner );

        Assert.Equal( BattleOutcome.Stalemate, result.Outcome );
        Assert.Same( winner, result.Winner );
    }

    [Fact]
    public void RoundLimitReached_Always_SetsRoundLimitOutcomeAndWinner()
    {
        IFighter winner = FighterBuilder.CreateMock( "Winner" ).Object;

        BattleResult result = BattleResult.RoundLimitReached( winner );

        Assert.Equal( BattleOutcome.RoundLimitReached, result.Outcome );
        Assert.Same( winner, result.Winner );
    }
}