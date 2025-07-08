using System.Reflection;
using System;
using System.IO;
using System.Text;
using Mastermind;
using Xunit;

namespace Mastermind.Tests
{
    public class GameplayTests
    {
        [Theory]
        [InlineData("1234", true)]
        [InlineData("1123", false)]
        [InlineData("8765", true)]
        [InlineData("9879", false)]
        [InlineData("abcd", false)]
        [InlineData("123", false)]
        [InlineData("12345", false)]
        public void IsValidGuess_ValidatesProperly(string guess, bool expected)
        {
            MethodInfo method = typeof(Gameplay).GetMethod("IsValidGuess", BindingFlags.NonPublic | BindingFlags.Static);
            bool result = (bool)method.Invoke(null, new object[] { guess });
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1234", "1234", 4, 0)]
        [InlineData("1234", "4321", 0, 4)]
        [InlineData("1234", "1243", 2, 2)]
        [InlineData("1234", "5678", 0, 0)]
        [InlineData("1234", "1203", 2, 1)]
        public void EvaluateGuess_ReturnsCorrectCounts(string code, string guess, int expectedPos, int expectedMisplaced)
        {
            Gameplay game = new Gameplay();
            PropertyInfo codeProp = typeof(Gameplay).GetProperty("Code", BindingFlags.NonPublic | BindingFlags.Instance);
            codeProp.SetValue(game, code);

            MethodInfo method = typeof(Gameplay).GetMethod("EvaluateGuess", BindingFlags.NonPublic | BindingFlags.Instance);
            var result = ((int, int))method.Invoke(game, new object[] { guess });

            Assert.Equal(expectedPos, result.Item1);
            Assert.Equal(expectedMisplaced, result.Item2);
        }

        [Fact]
        public void Setup_ValidArguments_ReturnsTrue()
        {
            Gameplay game = new Gameplay();
            string[] args = new[] { "-c", "1234", "-t", "5" };
            bool result = game.Setup(args);
            Assert.True(result);
        }

        [Fact]
        public void Setup_InvalidCode_ReturnsFalse()
        {
            Gameplay game = new Gameplay();
            string[] args = new[] { "-c", "1123" }; // Duplicate digits
            bool result = game.Setup(args);
            Assert.False(result);
        }

        [Fact]
        public void Setup_HelpFlag_ReturnsFalse()
        {
            Gameplay game = new Gameplay();
            string[] args = new[] { "--help" };
            bool result = game.Setup(args);
            Assert.False(result);
        }

        [Fact]
        public void GenerateSecretCode_ReturnsUniqueFourDigitsBetween0and8()
        {
            MethodInfo method = typeof(Gameplay).GetMethod("GenerateSecretCode", BindingFlags.NonPublic | BindingFlags.Static);

            for (int i = 0; i < 100; i++)
            {
                string code = (string)method.Invoke(null, null);
                Assert.Equal(4, code.Length);
                Assert.All(code, c => Assert.InRange(c - '0', 0, 8));
                Assert.Equal(4, new HashSet<char>(code).Count);
            }
        }
    }
}