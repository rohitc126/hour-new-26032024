using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BusinessLayer.Entity
{
    public class HourMeterReport
    {

        public decimal HMR_ID { get; set; }
        public string hdnShiftTime { get; set; }

        [Required(ErrorMessage = "Select Shift")]
        public int? SHIFT_Code { get; set; }
        public SelectList SHIFT_LIST { get; set; }
        public SelectList Employee_List { get; set; }
        public string PRODUCTION_DATE { get; set; }

        public HourMeterReport()
        {
            SHIFT_LIST = new SelectList(new List<SelectListItem>
                                    { 
                                        new SelectListItem { Text = "Morning", Value = "1" },
                                        new SelectListItem { Text = "Evening", Value = "2" } 
                                    }, "Value", "Text");
        }

        public List<HourMeterReport_Dtl> HourMeterDtl_List { get; set; }
    }

    public class HourMeterReport_Dtl
    {
        public decimal HMRR_ID { get; set; }
        public decimal HMR_ID { get; set; }
        public int? PM_ID { get; set; }
        public string PM_NAME { get; set; }
        public string Total { get; set; }
        public string Remarks { get; set; }
        public string BRANCH_CODE { get; set; }
        public string COMP_CODE { get; set; }
        public int? IS_LOCKED { get; set; }
        public string ShiftTime { get; set; }

    }
}
