using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Models;
using Oracle.DataAccess.Client;
using System.Configuration;
using System.Data;
using System.Globalization;
using Oracle.DataAccess.Types;

namespace DataAccess
{
    public interface IDecisionMakingDataAccess
    {

        int SubmitNewDecision(DecisionDocument decisionDocument);

        int SaveOptionsToDecision( DecisionOption option);

        void SaveAdvantageToOption( OptionAdvantage advantage);

        void SaveRiskToOption( OptionRisk risk);

        void AddNextStepToDecision( NextStep nextStep);

        List<DecisionDocument> GetDecisionDocumentsList();

        DecisionDocument GetDecisionDocument(int documentDecisionID);

        List<string> GetDepartments();

        IEnumerable<DecisionOption> GetDesicionOptions(int documentDecisionID);

        IEnumerable<OptionAdvantage> GetOptionAdvantages(int p);

        IEnumerable<OptionRisk> GetOptionRisks(int p);

        IEnumerable<NextStep> GetNextSteps(int documentDecisionID);

        IEnumerable<string> GetUserDetails();
    }

    public class DecisionMakingDataAccess : IDecisionMakingDataAccess
    {

        public int SubmitNewDecision(DecisionDocument decisionDocument)
        {
            DateTime openeddate;
            CultureInfo provider = new CultureInfo("he-IL");
            try
            {

                openeddate = decisionDocument.openeddate != null ? DateTime.ParseExact(decisionDocument.openeddate, "dd/MM/yyyy", provider) : DateTime.MinValue;

            }
            catch (Exception e)
            {

                throw new Exception("##Error to convert dates ##: " + e.Message);
            }
            int newDecisionID ;
            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {
                
                OracleDataAdapter da = new OracleDataAdapter();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = String.Format("INSERT INTO dm_decision_document ( openeddate, department, subject, requireddecision, currentstatedesc, recandreason, decision, comments)"
                 + " VALUES (TO_DATE('{0}','dd/mm/yyyy HH24:MI:SS') ,'{1}','{2}','{3}','{4}','{5}','{6}','{7}') returning id into :myOutputParameter",
                 decisionDocument.openeddate,
                 decisionDocument.Department,
                 decisionDocument.Subject,
                 decisionDocument.Requireddecision,
                 decisionDocument.Currentstatedesc,
                 decisionDocument.Recandreason,
                 decisionDocument.Decision,
                 decisionDocument.Comments);
                cmd.Connection.Open();
                cmd.Parameters.Add("myOutputParameter", OracleDbType.Decimal, ParameterDirection.ReturnValue);
                try
                {
                    cmd.ExecuteNonQuery();
                    var result = ((OracleDecimal)cmd.Parameters["myOutputParameter"].Value);
                  newDecisionID =   Convert.ToInt32(result.Value);
                    
                }
                catch (Exception)
                {

                    throw new Exception("##Error save to DB## : " + cmd.CommandText);
                }
                cmd.Connection.Close();

                return newDecisionID;
            }
            return -1;
        }


        public int SaveOptionsToDecision( DecisionOption option)
        {
            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {
                int newDecisionID;
                OracleDataAdapter da = new OracleDataAdapter();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = String.Format("INSERT INTO dm_options (Description,DecisionID) VALUES ('{0}','{1}') returning id into :myOutputParameter", option.Description, option.DecisionID);

                cmd.Parameters.Add("myOutputParameter", OracleDbType.Decimal, ParameterDirection.ReturnValue);
                cmd.Connection.Open();
                try
                {
                    cmd.ExecuteNonQuery();
                    var result = ((OracleDecimal)cmd.Parameters["myOutputParameter"].Value);
                    newDecisionID = Convert.ToInt32(result.Value);
                    
                }
                catch (Exception)
                {

                    throw new Exception("##Error save to DB## : " + cmd.CommandText);
                }
                cmd.Connection.Close();

                return newDecisionID;
            }
        }


        public void SaveAdvantageToOption( OptionAdvantage advantage)
        {
            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {
                int result;
                OracleDataAdapter da = new OracleDataAdapter();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = String.Format("INSERT INTO dm_option_advantage (Description,OptionID) VALUES ('{0}','{1}')", advantage.Description, advantage.OptionID);

                cmd.Connection.Open();
                try
                {
                    result = cmd.ExecuteNonQuery();

                }
                catch (Exception)
                {

                    throw new Exception("##Error save to DB## : " + cmd.CommandText);
                }
                cmd.Connection.Close();

                
            }
        }

        public void SaveRiskToOption( OptionRisk risk)
        {
            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {
                int result;
                OracleDataAdapter da = new OracleDataAdapter();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = String.Format("INSERT INTO dm_option_risks (Description,OptionID) VALUES ('{0}','{1}')", risk.Description, risk.OptionID);

                cmd.Connection.Open();
                try
                {
                    result = cmd.ExecuteNonQuery();

                }
                catch (Exception)
                {

                    throw new Exception("##Error save to DB## : " + cmd.CommandText);
                }
                cmd.Connection.Close();


            }
        }


        public void AddNextStepToDecision( NextStep nextStep)
        {
            DateTime due_date, date_postponed;
            CultureInfo provider = new CultureInfo("he-IL");
            try
            {
                due_date = nextStep.Date_postponed != null ? DateTime.ParseExact(nextStep.Date_postponed, "dd/MM/yyyy", provider) : DateTime.MinValue;
                date_postponed = nextStep.Due_date != null ? DateTime.ParseExact(nextStep.Due_date, "dd/MM/yyyy", provider) : DateTime.MinValue;
            }
            catch(Exception e)
            {
                 throw new Exception("##Error to convert dates ##: " + e.Message);
            }

            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {
                int result;
                OracleDataAdapter da = new OracleDataAdapter();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = String.Format("INSERT INTO dm_next_steps (mission,responsibility,due_date,date_postponed,decisionid)" + 
                    " VALUES ('{0}','{1}',TO_DATE('{2}','dd/mm/yyyy HH24:MI:SS'),TO_DATE('{3}','dd/mm/yyyy HH24:MI:SS'),'{4}')"
                    , nextStep.Mission, nextStep.Responsibility, due_date, date_postponed, nextStep.DecisionID);
                try
                {
                    cmd.Connection.Open();
                    result = cmd.ExecuteNonQuery();
                    cmd.Connection.Close();

                }
                catch (Exception)
                {

                    throw new Exception("##Error save to DB## : " + cmd.CommandText);
                }
            }
        }


        public List<DecisionDocument> GetDecisionDocumentsList()
        {

            List<DecisionDocument> data = new List<DecisionDocument>();

                using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
                {
                    cn.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = cn;
                    cmd.InitialLONGFetchSize = 1000;

                    cmd.CommandText = "select * from dm_decision_document t";
                    cmd.CommandType = CommandType.Text;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var newElement = new DecisionDocument();

                        newElement.ID = Convert.ToInt32(reader["id"]);
                        newElement.openeddate = Convert.ToString(reader["openeddate"]);
                        newElement.Department = Convert.ToString(reader["department"]);
                        newElement.Subject = Convert.ToString(reader["subject"]);
                        newElement.Requireddecision = Convert.ToString(reader["requireddecision"]);
                        newElement.Currentstatedesc = Convert.ToString(reader["currentstatedesc"]);

                        newElement.Recandreason = Convert.ToString(reader["recandreason"]);
                        newElement.Decision = Convert.ToString(reader["decision"]);

                        newElement.Comments = Convert.ToString(reader["comments"]);
                        newElement.Status = Convert.ToString(reader["status"]);
                       
                       

                        //   newElement.MANDATORY = Convert.ToString(reader["MANDATORY"]);



                        //      newElement.WEIGHT = Convert.ToString(reader["WEIGHT"]);
                        //    newElement.expresion = Convert.ToString(reader["expresion"]);
                        //    newElement.expresionfeild = Convert.ToString(reader["expresionfeild"]);
                        //     newElement.expresion = "";//should be new implementation
                        //     newElement.expresionfeild = "";  //should be new implementation


                        data.Add(newElement);

                    }
                    cn.Close();

                }
              
            return data;
        }

        public DecisionDocument GetDecisionDocument(int documentDecisionID)
        {
            DecisionDocument result = new DecisionDocument();
            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {
                cn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;

                cmd.CommandText = String.Format("select * from dm_decision_document t WHERE id = '{0}'", documentDecisionID) ;
                cmd.CommandType = CommandType.Text;
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    result.ID = Convert.ToInt32(reader["id"]);
                    result.openeddate = Convert.ToString(reader["openeddate"]);
                    result.Department = Convert.ToString(reader["department"]);
                    result.Subject = Convert.ToString(reader["subject"]);
                    result.Requireddecision = Convert.ToString(reader["requireddecision"]);
                    result.Currentstatedesc = Convert.ToString(reader["currentstatedesc"]);

                    result.Recandreason = Convert.ToString(reader["recandreason"]);
                    result.Decision = Convert.ToString(reader["decision"]);

                    result.Comments = Convert.ToString(reader["comments"]);
                    result.Status = Convert.ToString(reader["status"]);



                    //   newElement.MANDATORY = Convert.ToString(reader["MANDATORY"]);



                    //      newElement.WEIGHT = Convert.ToString(reader["WEIGHT"]);
                    //    newElement.expresion = Convert.ToString(reader["expresion"]);
                    //    newElement.expresionfeild = Convert.ToString(reader["expresionfeild"]);
                    //     newElement.expresion = "";//should be new implementation
                    //     newElement.expresionfeild = "";  //should be new implementation
                }
                else
                {
                    throw new Exception("No result");
                }
                cn.Close();

            }
            return result;
              
        }


        public List<string> GetDepartments()
        {
            List<string> departments = new List<string>();

            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {

                cn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;

                cmd.CommandText = "select Distinct t.department from users_details_new t";
                cmd.CommandType = CommandType.Text;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var newElement = new DecisionDocument();

                    string department = Convert.ToString(reader["department"]);

                    departments.Add(department);

                }
                cn.Close();

            }

            return departments;
        }


        public IEnumerable<DecisionOption> GetDesicionOptions(int documentDecisionID)
        {

            List<DecisionOption> data = new List<DecisionOption>();


            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {

                cn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;

                cmd.CommandText = String.Format("select * from dm_options t WHERE decisionid = '{0}'", documentDecisionID);
                cmd.CommandType = CommandType.Text;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var option = new DecisionOption();

                    option.ID = Convert.ToInt32(reader["ID"]);
                    option.Description = Convert.ToString(reader["Description"]);
                    option.DecisionID = Convert.ToInt32(reader["DecisionID"]);
                    option.Status = Convert.ToString(reader["Status"]);

                    //   newElement.MANDATORY = Convert.ToString(reader["MANDATORY"]);
                    //      newElement.WEIGHT = Convert.ToString(reader["WEIGHT"]);
                    //    newElement.expresion = Convert.ToString(reader["expresion"]);
                    //    newElement.expresionfeild = Convert.ToString(reader["expresionfeild"]);
                    //     newElement.expresion = "";//should be new implementation
                    //     newElement.expresionfeild = "";  //should be new implementation


                    data.Add(option);

                }
                cn.Close();

            }

            return data;
        }


        public IEnumerable<OptionAdvantage> GetOptionAdvantages(int p)
        {
            List<OptionAdvantage> data = new List<OptionAdvantage>();


            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {

                cn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;

                cmd.CommandText = String.Format("select * from dm_option_advantage t WHERE optionId = '{0}'", p);
                cmd.CommandType = CommandType.Text;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var option = new OptionAdvantage();


                    option.ID = Convert.ToInt32(reader["ID"]);
                    option.Description = Convert.ToString(reader["Description"]);
                    option.OptionID = Convert.ToInt32(reader["OptionId"]);
                    option.Status = Convert.ToString(reader["Status"]);

                    //    newElement.MANDATORY = Convert.ToString(reader["MANDATORY"]);
                    //    newElement.WEIGHT = Convert.ToString(reader["WEIGHT"]);
                    //    newElement.expresion = Convert.ToString(reader["expresion"]);
                    //    newElement.expresionfeild = Convert.ToString(reader["expresionfeild"]);
                    //    newElement.expresion = "";//should be new implementation
                    //    newElement.expresionfeild = "";  //should be new implementation

                    data.Add(option);

                }
                cn.Close();

            }

            return data;
        }

        public IEnumerable<OptionRisk> GetOptionRisks(int p)
        {
            List<OptionRisk> data = new List<OptionRisk>();

            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {
                cn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;

                cmd.CommandText = String.Format("select * from dm_option_risks t WHERE optionId = '{0}'", p);
                cmd.CommandType = CommandType.Text;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var option = new OptionRisk();
                    option.ID = Convert.ToInt32(reader["ID"]);
                    option.Description = Convert.ToString(reader["Description"]);
                    option.OptionID = Convert.ToInt32(reader["OptionId"]);
                    option.Status = Convert.ToString(reader["Status"]);

                    //   newElement.MANDATORY = Convert.ToString(reader["MANDATORY"]);
                    //      newElement.WEIGHT = Convert.ToString(reader["WEIGHT"]);
                    //    newElement.expresion = Convert.ToString(reader["expresion"]);
                    //    newElement.expresionfeild = Convert.ToString(reader["expresionfeild"]);
                    //     newElement.expresion = "";//should be new implementation
                    //     newElement.expresionfeild = "";  //should be new implementation


                    data.Add(option);

                }
                cn.Close();

            }

            return data;
        }


        public IEnumerable<NextStep> GetNextSteps(int documentDecisionID)
        {
            List<NextStep> data = new List<NextStep>();

            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {
                cn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;

                cmd.CommandText = String.Format("select * from dm_next_steps t WHERE decisionid = '{0}'", documentDecisionID);
                cmd.CommandType = CommandType.Text;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var step = new NextStep();


                    step.ID = Convert.ToInt32(reader["ID"]);
                    step.Mission = Convert.ToString(reader["mission"]);
                    step.Responsibility = Convert.ToString(reader["responsibility"]);
                    step.Due_date = Convert.ToString(reader["due_date"]);
                    step.Date_postponed = Convert.ToString(reader["date_postponed"]);
                    step.DecisionID = Convert.ToInt32(reader["decisionid"]);

                    //   newElement.MANDATORY = Convert.ToString(reader["MANDATORY"]);
                    //      newElement.WEIGHT = Convert.ToString(reader["WEIGHT"]);
                    //    newElement.expresion = Convert.ToString(reader["expresion"]);
                    //    newElement.expresionfeild = Convert.ToString(reader["expresionfeild"]);
                    //     newElement.expresion = "";//should be new implementation
                    //     newElement.expresionfeild = "";  //should be new implementation


                    data.Add(step);

                }
                cn.Close();

            }

            return data;
        }


        public IEnumerable<string> GetUserDetails()
        {
            List<string> displaynames = new List<string>();


            using (OracleConnection cn = new OracleConnection(ConfigurationManager.ConnectionStrings["OracleDatabase"].ConnectionString))
            {
                cn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = cn;
                cmd.InitialLONGFetchSize = 1000;

                cmd.CommandText = "select Distinct t.displayname from users_details_new t";
                cmd.CommandType = CommandType.Text;
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var newElement = new DecisionDocument();
                    string displayname = Convert.ToString(reader["displayname"]);
                    displaynames.Add(displayname);

                }
                cn.Close();

            }

            return displaynames;
        }
    }
}
