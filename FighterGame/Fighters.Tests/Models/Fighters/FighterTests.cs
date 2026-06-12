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

    [Fact]
    public void FighterStats_RaceClassWeaponArmor_IsSum()
    {
        // Arrange
        IFighter fighter = new Fighter(
            "Hero",
            CreateRace( health: 100, damage: 10, armor: 5, initiative: 5 ).Object,
            CreateClass( health: 50, damage: 5 ).Object,
            CreateWeapon( damage: 15 ).Object,
            CreateArmor( armor: 15 ).Object
        );

        // Act
        int currentHp = fighter.CurrentHealth;
        int actualHp = fighter.MaxHealth;
        int actualDamage = fighter.Damage;
        int actualArmor = fighter.Armor;
        int actualInitiative = fighter.Initiative;

        // Assert
        Assert.True( fighter.IsAlive );
        Assert.Equal( 150, actualHp );
        Assert.Equal( actualHp, currentHp );
        Assert.Equal( 30, actualDamage );
        Assert.Equal( 20, actualArmor );
        Assert.Equal( 5, actualInitiative );
    }

    [Theory]
    [InlineData( "" )]
    [InlineData( "   " )]
    [InlineData( null )]
    public void Constructor_EmptyName_ThrowsArgumentException( string? name )
    {
        // Act, Assert
        Assert.Throws<ArgumentException>( () => new Fighter(
                name!,
                CreateRace().Object,
                CreateClass().Object,
                CreateWeapon().Object,
                CreateArmor().Object
            )
        );
    }

    [Fact]
    public void Constructor_NullRace_ThrowsArgumentNullException()
    {
        // Act, Assert
        Assert.Throws<ArgumentNullException>( () => new Fighter(
                "Hero",
                null!,
                CreateClass().Object,
                CreateWeapon().Object,
                CreateArmor().Object
            )
        );
    }

    [Fact]
    public void Constructor_NullClass_ThrowsArgumentNullException()
    {
        // Act, Assert
        Assert.Throws<ArgumentNullException>( () => new Fighter(
                "Hero",
                CreateRace().Object,
                null!,
                CreateWeapon().Object,
                CreateArmor().Object
            )
        );
    }

    [Fact]
    public void Constructor_NullWeapon_ThrowsArgumentNullException()
    {
        // Act, Assert
        Assert.Throws<ArgumentNullException>( () => new Fighter(
                "Hero",
                CreateRace().Object,
                CreateClass().Object,
                null!,
                CreateArmor().Object
            )
        );
    }

    [Fact]
    public void Constructor_NullArmor_ThrowsArgumentNullException()
    {
        // Act, Assert
        Assert.Throws<ArgumentNullException>( () => new Fighter(
                "Hero",
                CreateRace().Object,
                CreateClass().Object,
                CreateWeapon().Object,
                null!
            )
        );
    }

    [Fact]
    public void TakeDamage_PositiveAmount_ReducesHealthByExactAmount()
    {
        // Arrange
        Fighter sut = new Fighter(
            "Hero",
            CreateRace( health: 100 ).Object,
            CreateClass( health: 50 ).Object,
            CreateWeapon().Object,
            CreateArmor().Object
        );

        // Act
        sut.TakeDamage( 30 );

        // Assert
        Assert.Equal( 150 - 30, sut.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_MoreThanHealth_ClampsToZeroAndDies()
    {
        // Arrange
        Fighter sut = new Fighter(
            "Hero",
            CreateRace( health: 100 ).Object,
            CreateClass( health: 1 ).Object,
            CreateWeapon().Object,
            CreateArmor().Object
        );

        // Act
        sut.TakeDamage( 99999 );

        // Assert
        Assert.Equal( 0, sut.CurrentHealth );
        Assert.False( sut.IsAlive );
    }

    [Fact]
    public void TakeDamage_ExactlyHealth_DropsToZeroAndDies()
    {
        // Arrange
        Fighter sut = new Fighter(
            "Hero",
            CreateRace( health: 50 ).Object,
            CreateClass( health: 50 ).Object,
            CreateWeapon().Object,
            CreateArmor().Object
        );

        // Act
        sut.TakeDamage( 100 );

        // Assert
        Assert.Equal( 0, sut.CurrentHealth );
        Assert.False( sut.IsAlive );
    }

    [Fact]
    public void TakeDamage_ZeroDamage_KeepsHealth()
    {
        // Arrange
        Fighter sut = new Fighter(
            "Hero",
            CreateRace( health: 100 ).Object,
            CreateClass( health: 100 ).Object,
            CreateWeapon().Object,
            CreateArmor().Object
        );

        int before = sut.CurrentHealth;

        // Act
        sut.TakeDamage( 0 );

        // Assert
        Assert.Equal( before, sut.CurrentHealth );
    }

    [Fact]
    public void TakeDamage_NegativeAmount_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        Fighter sut = new Fighter(
            "Hero",
            CreateRace( health: 50 ).Object,
            CreateClass( health: 50 ).Object,
            CreateWeapon().Object,
            CreateArmor().Object
        );

        // Act, Assert
        Assert.Throws<ArgumentOutOfRangeException>( () => sut.TakeDamage( -1 ) );
    }
}