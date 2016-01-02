﻿using Autofac.Extras.Moq;
using ParkingATHWeb.Business.Providers;
using ParkingATHWeb.Business.Services;
using ParkingATHWeb.Business.Tests.Base;
using ParkingATHWeb.DataAccess;
using ParkingATHWeb.DataAccess.Common;
using ParkingATHWeb.DataAccess.Repositories;
using ParkingATHWeb.Resolver.Mappings;
using ParkingATHWeb.Shared.Enums;
using SharpTestsEx;
using Xunit;

namespace ParkingATHWeb.Business.Tests.Services
{
    public class TokenServiceTests : BusinessTestBase
    {
        private TokenService _sut;
        private readonly AutoMock _mock = AutoMock.GetLoose();

        public TokenServiceTests()
        {
            //Before
            InitContext();

        }

        [Fact]
        public async void WhenTokenIsCreated_DeleteItByTypeAndSecureToken_ThenTokenDontExists()
        {
            //Before
            var changePasswordTokenCreateResult = _sut.Create(TokenType.ResetPasswordToken);
            InitContext();
            //Act
            var data = await _sut.DeleteTokenBySecureTokenAndTypeAsync(changePasswordTokenCreateResult.Result.SecureToken, changePasswordTokenCreateResult.Result.TokenType);

            //Then

            var checkIfExists = await _sut.GetTokenBySecureTokenAndTypeAsync(changePasswordTokenCreateResult.Result.SecureToken, changePasswordTokenCreateResult.Result.TokenType);
            checkIfExists.IsValid.Should().Be.False();
            checkIfExists.ValidationErrors.Should().Have.Count.EqualTo(1);
            data.IsValid.Should().Be.True();
            changePasswordTokenCreateResult.IsValid.Should().Be.True();
        }

        [Fact]
        public async void WhenTokenIsCreated_GetItFromDbBySecTokenAndType_ThenTokenEquals()
        {
            //Before
            var changePasswordTokenCreateResult = _sut.Create(TokenType.ResetPasswordToken);

            //Act
            var data = await _sut.GetTokenBySecureTokenAndTypeAsync(changePasswordTokenCreateResult.Result.SecureToken, changePasswordTokenCreateResult.Result.TokenType);

            //Then
            data.IsValid.Should().Be.True();
            data.Result.TokenType.Should().Be.EqualTo(changePasswordTokenCreateResult.Result.TokenType);
            data.Result.SecureToken.Should().Be.EqualTo(changePasswordTokenCreateResult.Result.SecureToken);
            changePasswordTokenCreateResult.IsValid.Should().Be.True();
        }

        [Fact]
        public void WhenTokenIsCreated_EncryptIt_ThenDecryptedDataEquals()
        {
            //Before
            var changePasswordTokenCreateResult = _sut.Create(TokenType.ResetPasswordToken);
            var encryptedTokenData = changePasswordTokenCreateResult.Result.BuildEncryptedToken();

            //Act
            var data = _sut.GetDecryptedData(System.Net.WebUtility.UrlDecode(encryptedTokenData));

            //Then
            data.IsValid.Should().Be.True();
            data.Result.TokenType.Should().Be.EqualTo(changePasswordTokenCreateResult.Result.TokenType);
            data.Result.SecureToken.Should().Be.EqualTo(changePasswordTokenCreateResult.Result.SecureToken);
            changePasswordTokenCreateResult.IsValid.Should().Be.True();
        }

        [Fact]
        public void WhenTokenTypeIsProvided_CreateToken_ThenValidToDateIsCorrrect()
        {
            //Act
            var browserTokenCreateResult = _sut.Create(TokenType.ViewInBrowserToken);
            var changePasswordTokenCreateResult = _sut.Create(TokenType.ResetPasswordToken);

            //Then
            browserTokenCreateResult.IsValid.Should().Be.True();
            changePasswordTokenCreateResult.IsValid.Should().Be.True();

            var changePasswordValidDate = TokenValidityTimeProvider.GetValidToDate(TokenType.ResetPasswordToken);

            browserTokenCreateResult.Result.ValidTo.Should().Be.EqualTo(null);
            changePasswordTokenCreateResult.Result.ValidTo.Should().Be.IncludedIn(changePasswordValidDate.Value.AddMinutes(-1), changePasswordValidDate);
        }



        [Fact]
        public async void WhenTokenTypeIsProvided_CreateToken_ThenValidToDateIsCorrrect_Async()
        {
            //Act
            var browserTokenCreateResult = await _sut.CreateAsync(TokenType.ViewInBrowserToken);
            var changePasswordTokenCreateResult = await _sut.CreateAsync(TokenType.ResetPasswordToken);

            //Then
            browserTokenCreateResult.IsValid.Should().Be.True();
            changePasswordTokenCreateResult.IsValid.Should().Be.True();

            var changePasswordValidDate = TokenValidityTimeProvider.GetValidToDate(TokenType.ResetPasswordToken);

            browserTokenCreateResult.Result.ValidTo.Should().Be.EqualTo(null);
            changePasswordTokenCreateResult.Result.ValidTo.Should().Be.IncludedIn(changePasswordValidDate.Value.AddMinutes(-1), changePasswordValidDate);
        }

        private void InitContext()
        {
            _mock.Mock<IDatabaseFactory>().Setup(x => x.Get()).Returns(GetContext());
            var repository = _mock.Create<TokenRepository>();
            var uow = _mock.Create<UnitOfWork>();

            _sut = new TokenService(uow, repository);
            BackendMappingProvider.InitMappings();
        }
    }
}