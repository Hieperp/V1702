using System.Web.Mvc;
using System.Collections.Generic;

using TotalModel;
using TotalDTO;

using TotalCore.Repositories.Commons;
using TotalPortal.Areas.Commons.Builders;

using TotalPortal.ViewModels.Helpers;


namespace TotalPortal.Builders
{
    public interface IViewModelSelectListBuilder<TBaseViewModel>
        where TBaseViewModel : ISimpleViewModel
    {
        void BuildSelectLists(TBaseViewModel baseViewModel);
    }

    public interface IA01SimpleViewModel : IAccessControlAttribute, ISimpleViewModel
    {
        IEnumerable<SelectListItem> AspNetUserSelectList { get; set; }
    }

    public interface IA02SimpleViewModel : IA01SimpleViewModel
    {
        IEnumerable<SelectListItem> PaymentTermSelectList { get; set; }
    }

    public class A01ViewModelSelectListBuilder<TA01BaseViewModel> : IViewModelSelectListBuilder<TA01BaseViewModel>
        where TA01BaseViewModel : IA01SimpleViewModel
    {
        private readonly IAspNetUserRepository aspNetUserRepository;
        private readonly IAspNetUserSelectListBuilder aspNetUserSelectListBuilder;

        public A01ViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository)
        {
            this.aspNetUserRepository = aspNetUserRepository;
            this.aspNetUserSelectListBuilder = aspNetUserSelectListBuilder;
        }

        public virtual void BuildSelectLists(TA01BaseViewModel a01BaseViewModel)
        {
            a01BaseViewModel.AspNetUserSelectList = this.aspNetUserSelectListBuilder.BuildSelectListItemsForAspNetUsers(this.aspNetUserRepository.GetAllAspNetUsers(), a01BaseViewModel.UserID);
        }
    }

    public class A02ViewModelSelectListBuilder<TA02BaseViewModel> : A01ViewModelSelectListBuilder<TA02BaseViewModel>
        where TA02BaseViewModel : IA02SimpleViewModel
    {
        private readonly IPaymentTermSelectListBuilder paymentTermSelectListBuilder;
        private readonly IPaymentTermRepository paymentTermRepository;

        public A02ViewModelSelectListBuilder(IAspNetUserSelectListBuilder aspNetUserSelectListBuilder, IAspNetUserRepository aspNetUserRepository, IPaymentTermSelectListBuilder paymentTermSelectListBuilder, IPaymentTermRepository paymentTermRepository)
            : base(aspNetUserSelectListBuilder, aspNetUserRepository)
        {
            this.paymentTermSelectListBuilder = paymentTermSelectListBuilder;
            this.paymentTermRepository = paymentTermRepository;
        }

        public override void BuildSelectLists(TA02BaseViewModel a02BaseViewModel)
        {
            base.BuildSelectLists(a02BaseViewModel);
            a02BaseViewModel.PaymentTermSelectList = this.paymentTermSelectListBuilder.BuildSelectListItemsForPaymentTerms(this.paymentTermRepository.GetAllPaymentTerms());
        }

    }
}
