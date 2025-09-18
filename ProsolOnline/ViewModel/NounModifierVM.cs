using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProsolOnline.Models;
namespace ProsolOnline.ViewModel
{
    public class NounModifierVM
    {
        public int Id { get; set; }
        public NounModifierModel One_NounModifier { get; set; }
        public List<NounModifierModel> All_NounModifier { get; set; }
        public List<NM_AttributesModel> ALL_NM_Attributes { get; set; }
        
        
    }
}