﻿using System.Collections.Generic;
using Autofac.Extras.Moq;
using Moq;
using ParkingATHWeb.Business.Providers.Email;
using ParkingATHWeb.Business.Tests.Base;
using ParkingATHWeb.Shared.Enums;
using SharpTestsEx;
using Xunit;

namespace ParkingATHWeb.Business.Tests.Providers
{
    public class EmailContentProviderTests:BusinessTestBase
    {
        private readonly Mock<EmailContentProvider> _sut;
        private readonly AutoMock _mock = AutoMock.GetLoose();

        public EmailContentProviderTests()
        {
            //Before
            _sut = new Mock<EmailContentProvider> {CallBase = true};
            _sut.Setup(x => x.GetValidTemplateString(EmailType.ResetPassword)).Returns(EmailBodyChangeReset);
            _sut.Setup(x => x.GetValidTemplateString(EmailType.Register)).Returns(EmailBodyRegister);
            _sut.Setup(x => x.GetLayoutTemplate()).Returns(TestLayoutRegister);
        }

        [Fact]
        public void WhenSpecifiedRegisterEmailType_ThenCorrectLayoutIsTaken_AndCorrectBodyIsInsertedIntoLayout()
        {
            //Act
            var emailBody = _sut.Object.GetEmailBody(EmailType.Register, CustomParameters);

            //Then
            emailBody.Should().Be.EqualTo(FullEmailRegister);
            emailBody.Should().Not.Contain("{{BodyHtml}}");
            emailBody.Should().Not.Contain("{{UserName}}");
        }

        [Fact]
        public void WhenSpecifiedResetPasswordEmailType_ThenCorrectLayoutIsTaken_AndCorrectBodyIsInsertedIntoLayout()
        {
            //Act
            var emailBody = _sut.Object.GetEmailBody(EmailType.ResetPassword, CustomParameters);

            //Then
            emailBody.Should().Be.EqualTo(FullEmailChangeReset);
            emailBody.Should().Not.Contain("{{BodyHtml}}");
            emailBody.Should().Not.Contain("{{TekstLayoutPlz}}");
        }
    }
}
