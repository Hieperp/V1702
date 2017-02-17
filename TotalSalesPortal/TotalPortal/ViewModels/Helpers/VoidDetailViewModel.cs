using System;
using System.ComponentModel.DataAnnotations;

using TotalDTO.Commons;

namespace TotalPortal.ViewModels.Helpers
{
    public class VoidDetailViewModel
    {
        public int ID { get; set; }
        public int DetailID { get; set; }
        public bool InActivePartial { get; set; }

        public Nullable<int> VoidTypeID { get { return (this.VoidType != null ? this.VoidType.VoidTypeID : null); } }
        [UIHint("AutoCompletes/VoidType")]
        public VoidTypeBaseDTO VoidType { get; set; }

        public VoidDetailViewModel()
        {
            this.VoidType = new VoidTypeBaseDTO();
        }

    }
}