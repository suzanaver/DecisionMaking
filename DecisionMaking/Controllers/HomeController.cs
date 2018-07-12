using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using BLL;
using DecisionMaking.Models;
using Models;

namespace DecisionMaking.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDecisionMakingBLL decisionMakingBLL;

        public HomeController()
        {
            decisionMakingBLL = new DecisionMakingBLL();
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }


        public ViewResult CreateDecision(CreateDecisionViewModel viewModel)
        {
            viewModel.Departments = decisionMakingBLL.GetDepartments();
            viewModel.CreateDecision = new DecisionDocument();
            
            return View(viewModel);
        }

        public ViewResult EditDecision( int DecisionID)
        {
            CreateDecisionViewModel viewModel = new CreateDecisionViewModel();
            viewModel.CreateDecision = decisionMakingBLL.GetDecisionDocument(DecisionID);
            
            viewModel.Departments = decisionMakingBLL.GetDepartments();
            
            return View("CreateDecision",viewModel);
        }

        public ViewResult DisplayDocumentDecision(DisplayDocumentDecisionViewModel viewModel)
        {
            int documentDecisionID = viewModel.DecisionID;
           
           viewModel.DecisionDocument = decisionMakingBLL.GetDecisionDocument(documentDecisionID);

           return View(viewModel);
        }

        public JsonResult GetDecisions()
        {
            DisplayDecisionDocumentsViewModel viewModel = new DisplayDecisionDocumentsViewModel();
            viewModel.DecisionDocuments = decisionMakingBLL.GetDecisionDocumentsList();

            var result = new 
            {
                data = viewModel.DecisionDocuments

            };

            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult SubmitNewDecision(SubmitNewDecisionPostModel PostModel)
        {

            var decisionDocument = new DecisionDocument();

            decisionDocument = PostModel.DecisionDocument;

            decisionMakingBLL.SubmitNewDecision(decisionDocument);

            Object result = new { Message = "OK" };
            var jsonResult = Json(result, JsonRequestBehavior.AllowGet);

            return jsonResult;
         
        }

        public PartialViewResult GetOption(int lastOptionNumber)
        {
            ViewData["OptionNumber"] = lastOptionNumber++;
            return PartialView("~/Views/Shared/Partials/DecisionOption/_DecisionOption.cshtml",new DecisionOption());
        }


        public PartialViewResult GetNextStep(int nextStepNumber)
        {
            ViewData["nextStepNumber"] = nextStepNumber;
            return PartialView("~/Views/Shared/Partials/NextSteps/_NextStep.cshtml",new NextStep());
        }


        public PartialViewResult GetNewAdvantage()
        {
          
            return PartialView("~/Views/Shared/Partials/DecisionOption/_Advantage.cshtml", new OptionAdvantage());
        }

        public PartialViewResult GetNewRisk()
        {

            return PartialView("~/Views/Shared/Partials/DecisionOption/_Risk.cshtml", new OptionRisk());
        }
        
        
        public ViewResult DisplayDecisionDocuments()
        {
            DisplayDecisionDocumentsViewModel viewModel = new DisplayDecisionDocumentsViewModel();
            viewModel.DecisionDocuments = decisionMakingBLL.GetDecisionDocumentsList();
            return View(viewModel);
            
        }

        public ViewResult DisplayDocumentDecisionNew(DisplayDocumentDecisionViewModel viewModel)
        {
            int documentDecisionID = viewModel.DecisionID;

            viewModel.DecisionDocument = decisionMakingBLL.GetDecisionDocument(documentDecisionID);

            return View(viewModel);
        }

        public ViewResult CreateDecisionNew(CreateDecisionViewModel viewModel)
        {
            viewModel.Departments = decisionMakingBLL.GetDepartments();
            viewModel.CreateDecision = new DecisionDocument();
            viewModel.UserDetails = decisionMakingBLL.GetUserDetails();

            ViewBag.UserDetails = new JavaScriptSerializer().Serialize(viewModel.UserDetails);

            viewModel.CreateDecision.DeterminesTheDecision = User.Identity.Name.Split('\\')[1];
            return View(viewModel);
        }

          [HttpGet]
        public ViewResult EditDecisionNew(int DecisionID)
        {
            CreateDecisionViewModel viewModel = new CreateDecisionViewModel();
            viewModel.CreateDecision = decisionMakingBLL.GetDecisionDocument(DecisionID);

            viewModel.Departments = decisionMakingBLL.GetDepartments();

            return View("EditDecision", viewModel);
        }

       
     
    }
}
