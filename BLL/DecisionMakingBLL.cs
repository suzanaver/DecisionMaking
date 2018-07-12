using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using Models;

namespace BLL
{
    public interface IDecisionMakingBLL
    {

        void SubmitNewDecision(DecisionDocument decisionDocument);

        DecisionDocument GetDecisionDocument(int documentDecisionID);

        IEnumerable<DecisionDocument> GetDecisionDocumentsList();

        IEnumerable<string> GetDepartments();

        IEnumerable<string> GetUserDetails();
    }

    public class DecisionMakingBLL : IDecisionMakingBLL
    {
        private readonly IDecisionMakingDataAccess decisionMakingDataAccess;

        public DecisionMakingBLL()
        {
            decisionMakingDataAccess = new DecisionMakingDataAccess();
        }

        public void SubmitNewDecision(DecisionDocument decisionDocument)
        {
            var decisionDocumentID = decisionMakingDataAccess.SubmitNewDecision(decisionDocument);
            decisionDocument.ID = decisionDocumentID;

            foreach (var option in decisionDocument.DecisionOptions)
            {
                option.DecisionID = decisionDocument.ID;
                var newOptionID = SaveOptionsToDecision(option);
                option.ID = newOptionID;
                foreach (var advantage in option.OptionAdvantages)
                {
                    advantage.OptionID = option.ID;
                    SaveAdvantageToOption(advantage);
                }

                foreach (var risk in option.OptionRisks)
                {
                    risk.OptionID = option.ID;
                    SaveRiskToOption(risk);
                }

            }

            foreach (var nextStep in decisionDocument.NextSteps)
            {
                nextStep.DecisionID = decisionDocumentID;
                AddNextStepToDecision(nextStep);
            }
        }

        private void AddNextStepToDecision(NextStep nextStep)
        {
            decisionMakingDataAccess.AddNextStepToDecision(nextStep);
        }

        private void SaveRiskToOption(OptionRisk risk)
        {
            decisionMakingDataAccess.SaveRiskToOption(risk);
        }

        private void SaveAdvantageToOption(OptionAdvantage advantage)
        {
            decisionMakingDataAccess.SaveAdvantageToOption(advantage);
        }

        private int SaveOptionsToDecision(DecisionOption option)
        {
            var newOptionID = decisionMakingDataAccess.SaveOptionsToDecision(option);

            return newOptionID;
        }

        public DecisionDocument GetDecisionDocument(int documentDecisionID)
        {
            DecisionDocument result = decisionMakingDataAccess.GetDecisionDocument(documentDecisionID);

            result.DecisionOptions = decisionMakingDataAccess.GetDesicionOptions(documentDecisionID);

            result.NextSteps = decisionMakingDataAccess.GetNextSteps(documentDecisionID);

            foreach (var option in  result.DecisionOptions)
            {
                option.OptionAdvantages = decisionMakingDataAccess.GetOptionAdvantages(option.ID);
                option.OptionRisks = decisionMakingDataAccess.GetOptionRisks(option.ID);
                
            }


            return result;
        }

        public IEnumerable<DecisionDocument> GetDecisionDocumentsList()
        {
            var result = decisionMakingDataAccess.GetDecisionDocumentsList();

            return result;
        }

        public IEnumerable<string> GetDepartments()
        {
            var result = decisionMakingDataAccess.GetDepartments();

            return result;
        }

        IEnumerable<string> IDecisionMakingBLL.GetUserDetails()
        {
            var result = decisionMakingDataAccess.GetUserDetails();
            return result;
        }
    }
}
