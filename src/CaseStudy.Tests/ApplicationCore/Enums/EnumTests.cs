using ApplicationCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CaseStudy.Tests.ApplicationCore.Enums
{
    public class EnumTests
    {
        #region CultureEnum

        [Theory]
        [InlineData(Culture.Collaborate)]
        [InlineData(Culture.Compete)]
        [InlineData(Culture.Control)]
        [InlineData(Culture.Create)]
        public void CultureEnumTest(Culture culture) 
        {
            //Assert Verification
            Assert.NotNull(culture.ToString());
        }

        #endregion

        #region EvaluationResultEnum

        [Theory]
        [InlineData(EvaluationResult.Applied)]
        [InlineData(EvaluationResult.FailedChecks)]
        [InlineData(EvaluationResult.NotQualified)]
        [InlineData(EvaluationResult.NotApplicable)]
        
        public void EvaluationEnumTest(EvaluationResult evaluation)
        {
            //Assert Verification
            Assert.NotNull(evaluation.ToString());
        }

        #endregion

        #region RuleTypeEnum

        [Theory]
        [InlineData(RuleType.AllZeros)]
        [InlineData(RuleType.AllLowScore)]
        public void RuleTypeEnumTest(RuleType ruleType)
        {
            //Assert Verification
            Assert.NotNull(ruleType.ToString());
        }

        #endregion
    }
}
