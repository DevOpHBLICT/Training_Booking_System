using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cascadingdropdownlist.Controllers
{
    public class SelectListItemHelper
    {
        public static IEnumerable<SelectListItem> GetHoursList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
  
            };
            return items;
        }


        public static IEnumerable<SelectListItem> GetMinutesList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                
            };
            return items;
        }

    }
    }
