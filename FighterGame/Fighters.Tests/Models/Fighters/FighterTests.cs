using Fighters.Models.Armors;
using Fighters.Models.Classes;
using Fighters.Models.Fighters;
using Fighters.Models.Races;
using Fighters.Models.Weapons;
using Moq;

namespace Fighters.Tests.Models.Fighters;

public class FighterTests
{
    private static Mock<IRace> CreateRace(
        int health = 100,
        int damage = 1,
        int armor = 0,
        int initiative = 5
    )
    {
        Mock<IRace> race = new Mock<IRace>();
        race.SetupGet( r => r.Name ).Returns( "TestRace" );
        race.SetupGet( r => r.Health ).Returns( health );
        race.SetupGet( r => r.Damage ).Returns( damage );
        race.SetupGet( r => r.Armor ).Returns( armor );
        race.SetupGet( r => r.Initiative ).Returns( initiative );

        return race;
    }

    private static Mock<IFighterClass> CreateClass( int health = 50, int damage = 5 )
    {
        Mock<IFighterClass> fighterClass = new Mock<IFighterClass>();
        fighterClass.SetupGet( c => c.Name ).Returns( "TestClass" );
        fighterClass.SetupGet( c => c.Health ).Returns( health );
        fighterClass.SetupGet( c => c.Damage ).Returns( damage );

        return fighterClass;
    }

    private static Mock<IWeapon> CreateWeapon( int damage = 10 )
    {
        Mock<IWeapon> weapon = new Mock<IWeapon>();
        weapon.SetupGet( w => w.Name ).Returns( "TestWeapon" );
        weapon.SetupGet( w => w.Damage ).Returns( damage );

        return weapon;
    }

    private static Mock<IArmor> CreateArmor( int armor = 0 )
    {
        Mock<IArmor> piece = new Mock<IArmor>();
        piece.SetupGet( a => a.Name ).Returns( "TestArmor" );
        piece.SetupGet( a => a.Armor ).Returns( armor );

        return piece;
    }

    private static Fighter CreateFighter(
        string name = "Hero",
        IRace? race = null,
        IFighterClass? fighterClass = null,
        IWeapon? weapon = null,
        IArmor? armor = null
    ) => new Fighter(
        name,
        race ?? CreateRace().Object,
        fighterClass ?? CreateClass().Object,
        weapon ?? CreateWeapon().Object,
        armor ?? CreateArmor().Object
    );

    [Fact]
    public void MaxHealth_RaceAndClassHealth_IsSum()
    {
        Fighter fighter = CreateFighter(
            race: CreateRace( health: 100 ).Object,
            fighterClass: CreateClass( health: 50 ).Object
        );

        Assert.Equal( 150, fighter.MaxHealth );
    }

    [Fact]
    public void Damage_RaceClassAndWeapon_IsSum()
    {
        Fighter fighter = CreateFighter(
            race: CreateRace( damage: 1 ).Object,
            fighterClass: CreateClass( damage: 5 ).Object,
            weapon: CreateWeapon( damage: 15 ).Object
        );

        Assert.Equal( 21, fighter.Damage );
    }

    [Fact]
    public void Armor_RaceAndArmor_IsSum()
    {
        Fighter fighter = CreateFighter(
            race: CreateRace( armor: 3 ).Object,
            armor: CreateArmor( armor: 15 ).Object
        );

        Assert.Equal( 18, fighter.Armor );
    }

    [Fact]
    public void Initiative_Always_ComesFromRace()
    {
        Fighter fighter = CreateFighter(
            race: CreateRace( initiative: 9 ).Object
        );

        Assert.Equal( 9, fighter.Initiative );
    }

    [Fact]
    public void Constructor_NewFighter_StartsAtFullHealth()
    {
        Fighter fighter = CreateFighter(
            race: CreateRace( health: 100 ).Object,
            fighterClass: CreateClass( health: 20 ).Object
        );

        Assert.Equal( fighter.MaxHealth, fighter.CurrentHealth );
        Assert.True( fighter.IsAlive );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_EmptyName_ThrowsArgumentException( string? name )
    {
        Assert.Throws<ArgumentException>( () => CreateFighter( name: name! ) );
    }

    [Fact]
    public void Constructor_NullRace_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>( () => new Fighter(
                "Hero",
                null!,
                CreateClass().Object,
                CreateWeapon().Object,
                CreateArmor().Object
            )
        );

    [Fact]
    public void Constructor_NullClass_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>( () => new Fighter(
                "Hero",
                CreateRace().Object,
                null!,
                CreateWeapon().Object,
                CreateArmor().Object
            )
        );

    [Fact]
    public void Constructor_NullWeapon_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>( () => new Fighter(
                "Hero",
                CreateRace().Object,
                CreateClass().Object,
                null!,
                CreateArmor().Object
            )
        );

    [Fact]
    public void Constructor_NullArmor_ThrowsArgumentNullException() =>
        Assert.Throws<ArgumentNullException>( () => new Fighter(
                "Hero",
                CreateRace().Object,
                CreateClass().Object,
                CreateWeapon().Object,
                null!
            )
        );

    [Fact]
    public void TakeDamage_PositiveAmount_ReducesHealthByExactAmount()
    {
        Fighter fighter = CreateFighter(
            race: CreateRace( health: 100 ).Object,
            fighterClass: CreateClass( health: 50 ).Object
        );

        int before = fighter.CurrentHealth;

        fighter.TakeDamage( 30 );

        Assert.Equal( before - 30, fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_MoreThanHealth_ClampsToZeroAndDies()
    {
        Fighter fighter = CreateFighter();

        fighter.TakeDamage( 99999 );

        Assert.Equal( 0, fighter.CurrentHealth );
        Assert.False( fighter.IsAlive );
    }

    [Fact]
    public void TakeDamage_ExactlyHealth_DropsToZeroAndDies()
    {
        Fighter fighter = CreateFighter(
            race: CreateRace( health: 100 ).Object,
            fighterClass: CreateClass( health: 0 ).Object
        );

        fighter.TakeDamage( 100 );

        Assert.Equal( 0, fighter.CurrentHealth );
        Assert.False( fighter.IsAlive );
    }

    [Fact]
    public void TakeDamage_ZeroDamage_KeepsHealth()
    {
        Fighter fighter = CreateFighter();
        int before = fighter.CurrentHealth;

        fighter.TakeDamage( 0 );

        Assert.Equal( before, fighter.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_NegativeAmount_ThrowsArgumentOutOfRangeException()
    {
        Fighter fighter = CreateFighter();

        Assert.Throws<ArgumentOutOfRangeException>( () => fighter.TakeDamage( -1 ) );
    }

    [Fact]
    public void Description_Always_ContainsNamesAndHealth()
    {
        Mock<IRace> race = CreateRace( health: 100 );
        race.SetupGet( r => r.Name ).Returns( "Human" );
        Mock<IFighterClass> fighterClass = CreateClass( health: 50 );
        fighterClass.SetupGet( c => c.Name ).Returns( "Knight" );
        Mock<IWeapon> weapon = CreateWeapon();
        weapon.SetupGet( w => w.Name ).Returns( "Sword" );
        Mock<IArmor> armor = CreateArmor();
        armor.SetupGet( a => a.Name ).Returns( "Plate Armor" );

        string description = CreateFighter(
            race: race.Object,
            fighterClass: fighterClass.Object,
            weapon: weapon.Object,
            armor: armor.Object
        ).Description;

        Assert.Contains( "Hero", description );
        Assert.Contains( "Human", description );
        Assert.Contains( "Knight", description );
        Assert.Contains( "Sword", description );
        Assert.Contains( "Plate Armor", description );
        Assert.Contains( "150 / 150", description );
    }
}