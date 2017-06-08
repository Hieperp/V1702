using System.Net;
using System.Web.Mvc;
using System.Text;

using AutoMapper;
using RequireJsNet;

using TotalBase.Enums;

using TotalModel.Models;

using TotalCore.Services.Accounts;
using TotalCore.Repositories.Commons;

using TotalDTO.Accounts;

using TotalPortal.Controllers;
using TotalPortal.Areas.Accounts.ViewModels;
using TotalPortal.Areas.Accounts.Builders;

namespace TotalPortal.Areas.Accounts.Controllers
{
    public class CreditNotesController : GenericSimpleController<CreditNote, CreditNoteDTO, CreditNotePrimitiveDTO, CreditNoteViewModel>
    {

        public CreditNotesController(ICreditNoteService creditNoteService, ICreditNoteViewModelSelectListBuilder creditNoteViewModelSelectListBuilder)
            : base(creditNoteService, creditNoteViewModelSelectListBuilder, true)
        {
        }

        public override void AddRequireJsOptions()
        {
            base.AddRequireJsOptions();

            StringBuilder commodityTypeIDList = new StringBuilder();
            commodityTypeIDList.Append((int)GlobalEnums.CommodityTypeID.CreditNote);

            RequireJsOptions.Add("commodityTypeIDList", commodityTypeIDList.ToString(), RequireJsOptionsScope.Page);
        }



    }

}