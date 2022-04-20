using BidsManager.Database.Models;
using BidsManager.Database.Modules;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using YandexDirectBidsManager.Extensions;
using YandexDirectBidsManager.Memory;
using YandexDirectBidsManager.ViewModels;

namespace YandexDirectBidsManager.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IWebHostEnvironment _appEnvironment;
        public DashboardController(IWebHostEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        [Route("Dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            var vm = await InitVM();
            return View("Views/Dashboard/Index.cshtml", vm);
        }


        #region Аккаунты Yandex.Direct

        [HttpPost]
        [Route("Dashboard/CreateYandexDirectAccount")]
        public async Task<IActionResult> CreateYandexDirectAccount([FromBody]YandexDirectAccount account)
        {
            var user = await this.GetAuthorizedUser();
            await YandexDirectAccountsModule.CreateAccount(user, account);
            if(user.UserData.SelectedYandexDirectAccount == null)
            {
               // user.UserData.SelectedYandexDirectAccount = user.YandexDirectAccounts.FirstOrDefault();
            }
            return await Dashboard();
        }
        [HttpDelete]
        [Route("Dashboard/DeleteYandexDirectAccount")]
        public async Task<IActionResult> DeleteYandexDirectAccount(int accountId)
        {
            var user = await this.GetAuthorizedUser();
            await YandexDirectAccountsModule.DeleteAccount(user, accountId);
            return await Dashboard();
        }
        [HttpPut]
        [Route("Dashboard/SelectActiveYaDirectAcc")]
        public async Task<IActionResult> SelectActiveYaDirectAcc(int accountId)
        {
            var user = await this.GetAuthorizedUser();
            await YandexDirectAccountsModule.SetActiveAccount(user, accountId);
            return await Dashboard();
        }


        #endregion

        #region CRUD для правил

        [HttpPut]
        [Route("Dashboard/AddCampaignRule")]
        public async Task<IActionResult> AddCampaignRule()
        {
            var user = await this.GetAuthorizedUser();
            var token = user.UserData.SelectedYandexDirectAccount;
            if (token == null) return new JsonResult(0);

            int id = await CampaignRulesModule.CreateCampaignRule(token, new CampaignRule());
            return new JsonResult(id);
        }
        [HttpDelete]
        [Route("Dashboard/DeleteCampaignRule")]
        public async Task DeleteCampaignRule(int id)
        {
            var user = await this.GetAuthorizedUser();
            var token = user.UserData.SelectedYandexDirectAccount;
            if (token == null) return;

            await CampaignRulesModule.DeleteCampaignRule(token, token.CampaignRules.FirstOrDefault(o => o.Id == id));
        }
        [HttpPost]
        [Route("Dashboard/UpdateCampaignRule")]
        public async Task UpdateCampaignRule([FromBody] CampaignRule model)
        {
            var user = await this.GetAuthorizedUser();
            model.Owner = user.UserData.SelectedYandexDirectAccount;

            var found = user.UserData.SelectedYandexDirectAccount.CampaignRules.FirstOrDefault(o => o.Id == model.Id);
            found.IdealShowingsPerHour = model.IdealShowingsPerHour;
            found.KeyPhraseId = model.KeyPhraseId;
            found.CampaignId = model.CampaignId;
            await CampaignRulesModule.UpdateCampaignRule(found);
        }

        #endregion

        #region Excel

        public IActionResult GetCampaignRulesExcelSample()
        {
            return File("/files/campaignRules.xlsx", "application/xlsx", "campaignRules.xlsx");
        }
        [HttpPost]
        public async Task<IActionResult> LoadCampaignRulesFromExcel()
        {
            var attachment = Request.Form.Files.FirstOrDefault();
            var filePath = "/uploads/" + Guid.NewGuid().ToString()+".xlsx";

            var physicalPath = _appEnvironment.WebRootPath + filePath;
            List<CampaignRule> rules = new List<CampaignRule>();

            if (attachment != null)
            {
                using (var fileStream = new FileStream(physicalPath, FileMode.Create))
                {
                    await attachment.CopyToAsync(fileStream);
                }
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage(physicalPath);
                ExcelWorksheet sheet = excel.Workbook.Worksheets.FirstOrDefault(); 

                int rows = sheet.Dimension.End.Row;
                for(int i = 2; i < rows + 1; i++)
                {
                    try
                    {
                        CampaignRule rule = new CampaignRule
                        {
                            CampaignId = Convert.ToInt64(sheet.Cells[i, 1].Value),
                            KeyPhraseId = Convert.ToInt64(sheet.Cells[i, 2].Value),
                            IdealShowingsPerHour = Convert.ToInt32(sheet.Cells[i, 3].Value),
                        };
                        rules.Add(rule);
                    }
                    catch (Exception ex) { }
                }

                var user = await this.GetAuthorizedUser();
                await CampaignRulesModule.ClearCampaignRules(user.UserData.SelectedYandexDirectAccount);
                await CampaignRulesModule.CreateCampaignRules(user.UserData.SelectedYandexDirectAccount,rules);

            }

            return RedirectToAction("Dashboard", "Dashboard");
        }

        #endregion

        private async Task<BaseDashboardVM> InitVM()
        {
            var vm  = new BaseDashboardVM();
            vm.User = await this.GetAuthorizedUser();
            return vm;
        }
    }
}
