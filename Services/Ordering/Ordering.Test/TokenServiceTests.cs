using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Ordering.Domain.Entities.Identity;
using Ordering.Infrastructure.Repositories;
using Xunit;

namespace Ordering.Test
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;
        private readonly Mock<IConfiguration> _mockConfiguration;

        public TokenServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["Token:Key"]).Returns("lists hklmm kmls any may secret key");
            _mockConfiguration.Setup(c => c["Token:Issuer"]).Returns("https://localhost:8004/");
            _mockConfiguration.Setup(c => c["Token:Audience"]).Returns("https://localhost:8004/");

            _tokenService = new TokenService(_mockConfiguration.Object);
        }

        [Fact]
        public void CreateTokenAsync_ValidUser_ReturnsValidToken()
        {
            // Arrange
            var user = new AppUser
            {
                Email = "test@example.com",
                DisplayName = "John Doe"
            };

            var expectedClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.DisplayName)
            };

            var expectedKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes("lists hklmm kmls any may secret key")
            );

            var expectedCredentials = new SigningCredentials(
                expectedKey,
                SecurityAlgorithms.HmacSha256
            );

            var expectedTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(expectedClaims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = expectedCredentials,
                Issuer = "https://localhost:8004/",
                Audience = "https://localhost:8004/"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var expectedToken = tokenHandler.CreateToken(expectedTokenDescriptor);
            var expectedTokenString = tokenHandler.WriteToken(expectedToken);

            // Act
            var generatedToken = _tokenService.CreateTokenAsync(user);

            // Assert
            Assert.Equal(expectedTokenString, generatedToken);
        }
    }
}
