using System.Linq;
using System.Collections.Generic;

using TotalModel;

namespace TotalDTO
{
    public abstract class BaseWithDetailDTO<TDtoDetail> : BaseDTO
        where TDtoDetail : IBaseModel
    {
        //USING IEnumerable<TDtoDetail> TO ALLOW override RETURN TYPE OF DtoDetails(), WHILE ICOLLECTION<TDtoDetail> DOES NOT ALLOWED. BUT IEnumerable<TDtoDetail> DOES NOT SUPPORT Add OR Remove ITEM -> CAN NOT USING THIS INTERFACE IN SERVICE LAYER TO SAVE THE ENTITY
        //This IEnumerable<TDtoDetail> DtoDetails(): is a read-only list of details (IEnumerable<T> does not allow to add of remove). This IEnumerable<T> DtoDetails() is used by only the DTO itself only TO CALL Validate(ValidationContext validationContext) OR PerformPresaveRule()
        protected virtual IEnumerable<TDtoDetail> DtoDetails() { return new List<TDtoDetail>(); }

        public override void PerformPresaveRule()
        {
            base.PerformPresaveRule();

            this.DtoDetails().ToList().ForEach(e => { e.LocationID = this.LocationID; e.EntryDate = this.EntryDate; e.Approved = this.Approved; e.ApprovedDate = this.ApprovedDate; e.InActive = this.InActive; e.InActiveDate = this.InActiveDate; });
        }
    }
}
