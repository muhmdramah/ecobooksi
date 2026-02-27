using ecobooksi.DataAccess.Interfaces;
using ecobooksi.Models.Models;
using ecobooksi.Models.View_Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ecobooksi.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICompanyRepository _companyRepository;

        public CompanyController(IUnitOfWork unitOfWork, ICompanyRepository companyRepository)
        {
            _unitOfWork = unitOfWork;
            _companyRepository = companyRepository;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            var companies = _unitOfWork.Company.GetAll();
            return View(companies);
        }

        public IActionResult Details(int companyId)
        {
            var company = _unitOfWork.Company.Get(company => company.CompanyId == companyId);
            return View(company);
        }

        [HttpGet]
        public IActionResult Upsert(int? companyId)
        {
            if (companyId is null || companyId == 0)
            {
                // Create new product
                return View(new Company());
            }
            else
            {
                // Update existing product

                var currentCompany = _unitOfWork.Company
                    .Get(company => company.CompanyId == companyId);

                return View(currentCompany);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.CompanyId == 0)
                    await _unitOfWork.Company.CreateAsync(company);
                else
                    _unitOfWork.Company.Update(company);

                _unitOfWork.Complete();

                TempData["success"] = "Company Created Successfully!";

                return RedirectToAction(nameof(Index));
            }

            return View(company);
        }

        #region API Calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var companies = _unitOfWork.Company.GetAll();
            return Json(new { data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int companyId)
        {
            var currentCompany = _unitOfWork.Company
                .Get(company => company.CompanyId == companyId);

            if (currentCompany is null)
                return Json(new { success = false, message = "Error while deleting" });


            _unitOfWork.Company.DeleteAsync(currentCompany);
            _unitOfWork.Complete();

            return Json(new { success = true, message = "Company Deleted Successfully!" });
        }
        #endregion
    }
}
