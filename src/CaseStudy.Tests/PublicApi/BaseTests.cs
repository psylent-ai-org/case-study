using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using PublicApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseStudy.Tests.PUblicApi
{
    public class BaseTests
    {
        protected ScoresController scoresController;

        protected ScoringService scoringService;

        public BaseTests()
        {
            PopulateObjects();
        }

        private void PopulateObjects()
        {
            // Initialize Services.
            scoringService = new ScoringService();

            // Initialize Controllers.
            scoresController = new ScoresController(scoringService);
        }
    }
}
