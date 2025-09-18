using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Web.Mvc;
using MongoDB.Bson.IO;


using MongoDB.Driver.Builders;
using MongoDB.Bson;
using System.Text.RegularExpressions;

using Prosol.Core.Interface;
using Prosol.Core.Model;
using MongoDB.Driver;

namespace Prosol.Core
{
    public partial class ValuestandardisationService : IValuestandardization
    {
        private readonly IRepository<Prosol_NounModifiers> _nounModifierRepository;
        private readonly IRepository<Prosol_Datamaster> _DatamasterRepository;
        private readonly IRepository<Prosol_Charateristics> _characteristicRepository;
        private readonly IRepository<Prosol_Vendor> _VendorRepository;
        private readonly IRepository<Prosol_Sequence> _SequenceRepository;
        private readonly IRepository<Prosol_UOMSettings> _UOMRepository;
        private readonly IRepository<Prosol_Users> _usersRepository;
        private readonly IRepository<Prosol_Abbrevate> _abbreivateRepository;
        private readonly CatalogueService _CatalogueService;




        public ValuestandardisationService(IRepository<Prosol_Datamaster> datamasterRepository,
            IRepository<Prosol_NounModifiers> nounModifierRepository,
            IRepository<Prosol_Charateristics> characteristicRepository, IRepository<Prosol_Vendor> vendorRepository,
            IRepository<Prosol_Sequence> seqRepository,
            IRepository<Prosol_UOMSettings> UOMRepository,
            IRepository<Prosol_Users> usersRepository,
            IRepository<Prosol_Abbrevate> abbreivateRepository, CatalogueService catalougeService)
        {
            this._DatamasterRepository = datamasterRepository;
            this._nounModifierRepository = nounModifierRepository;
            this._characteristicRepository = characteristicRepository;
            this._VendorRepository = vendorRepository;
            this._SequenceRepository = seqRepository;
            this._UOMRepository = UOMRepository;
            this._usersRepository = usersRepository;
            this._abbreivateRepository = abbreivateRepository;
            this._CatalogueService = catalougeService;
        }

        public IEnumerable<Prosol_NounModifiers> GetNoun()
        {
            //var Noun = _nounModifierRepository.FindAll().ToList();
            //return Noun;

            var sort = SortBy.Ascending("Noun");
            string[] incl = { "Noun" };
            var Flds = Fields.Include(incl).Exclude("_id");
            var Noun = _nounModifierRepository.FindAll(Flds, sort).ToList();
            return Noun;
        }

        public IEnumerable<Prosol_NounModifiers> GetModifier(string noun)
        {

            var query = Query.EQ("Noun", noun);
            var modifierlist = _nounModifierRepository.FindAll(query).ToList();
            return modifierlist;

        }
        

             public IEnumerable<Prosol_Charateristics> GetAttributes(string noun,string modifier)
        {
            var query = Query.And(Query.EQ("Noun", noun), Query.EQ("Modifier", modifier));
            var Attributeslist = _characteristicRepository.FindAll(query).ToList();
            return Attributeslist;

        }

        public IEnumerable<Prosol_Datamaster> load_values(string noun, string modifier, string attribute)
        {
            var query = Query.And(Query.EQ("Noun", noun), Query.EQ("Modifier", modifier), Query.EQ("Characteristics.Characteristic", attribute)); // ,Query.distinct("Characteristics.Values")
            var valuelist = _DatamasterRepository.FindAll(query).ToList();
            return valuelist;
        }

        public int update_values(string noun, string modifier, string attribute, string value, string newvalue, string UOM, string newUOM)
        {
            var query = Query.And(Query.EQ("Noun", noun), Query.EQ("Modifier", modifier), Query.EQ("Characteristic", attribute));

           // Prosol_Charateristics chara_model = new Prosol_Charateristics();
            var chara_model=_characteristicRepository.FindAll(query).ToList();
            int seq;           
                seq = chara_model[0].Squence;
            


            var query_u = Query.And(Query.EQ("Noun", noun), Query.EQ("Modifier", modifier), Query.EQ("Characteristics.Characteristic", attribute),Query.EQ("Characteristics.Squence", seq), Query.EQ("Characteristics.Value", value));

            var upadate = Update.Set("Characteristics.$.Value", newvalue);
            var flg = UpdateFlags.Multi;
            var theresult = _DatamasterRepository.Update(query_u, upadate, flg);

            //NEW CODE FOR UOM


            var query_u1 = Query.And(Query.EQ("Noun", noun), Query.EQ("Modifier", modifier), Query.EQ("Characteristics.Characteristic", attribute), Query.EQ("Characteristics.Squence", seq), Query.EQ("Characteristics.Value", value), Query.EQ("Characteristics.UOM", UOM));

            var upadate1 = Update.Set("Characteristics.$.UOM", newUOM);
            var flg1 = UpdateFlags.Multi;
            var theresult1 = _DatamasterRepository.Update(query_u1, upadate1, flg1);

            ////

            var query_sony = Query.And(Query.EQ("Noun", noun), Query.EQ("Modifier", modifier), Query.EQ("Characteristics.Characteristic", attribute), Query.EQ("Characteristics.Squence", seq), Query.EQ("Characteristics.Value", newvalue), Query.EQ("Characteristics.UOM", newUOM));

            var fulldm = _DatamasterRepository.FindAll(query_sony).ToList();

            int countdm = fulldm.Count;

            // CatalogueService cs = new CatalogueService();

            bool result = true;

            foreach (Prosol_Datamaster pdm in fulldm)
            {
                string short_descccc = _CatalogueService.ShortDesc(pdm);
                string long_desc = _CatalogueService.LongDesc(pdm);

                var query_sl = Query.EQ("Itemcode", pdm.Itemcode);
                var update_sl = Update.Set("Shortdesc", short_descccc).Set("Longdesc", long_desc);
                var fllag = UpdateFlags.Upsert;

                var query_pre = _DatamasterRepository.Update(query_sl,update_sl,fllag);
                result = query_pre;
            }
            
            return fulldm.Count;
        }
        public int Delete_values(string noun, string modifier, string attribute, string value)
        {
            var query = Query.And(Query.EQ("Noun", noun), Query.EQ("Modifier", modifier), Query.EQ("Characteristic", attribute));
            var chara_model = _characteristicRepository.FindAll(query).ToList();
            int seq;
            seq = chara_model[0].Squence;
            var query_u = Query.And(Query.EQ("Noun", noun), Query.EQ("Modifier", modifier), Query.EQ("Characteristics.Characteristic", attribute), Query.EQ("Characteristics.Squence", seq), Query.EQ("Characteristics.Value", value));
            var fulldm = _DatamasterRepository.FindAll(query_u).ToList();
            var upadate = Update.Set("Characteristics.$.Value", "");
            var flg = UpdateFlags.Multi;
            var theresult = _DatamasterRepository.Update(query_u, upadate, flg);
            int countdm = fulldm.Count;
            bool result = true;
            foreach (Prosol_Datamaster pdm in fulldm)
            {
                var x = Query.EQ("Itemcode", pdm.Itemcode);
                var cat = _DatamasterRepository.FindOne(x);
                string short_descccc = _CatalogueService.ShortDesc(cat);
                string long_desc = _CatalogueService.LongDesc(cat);
                var query_sl = Query.EQ("Itemcode", pdm.Itemcode);
                var update_sl = Update.Set("Shortdesc", short_descccc).Set("Longdesc", long_desc);
                var fllag = UpdateFlags.Upsert;
                var query_pre = _DatamasterRepository.Update(query_sl, update_sl, fllag);
                result = query_pre;
            }

            return fulldm.Count;
        }
        //private string ShortDesc(Prosol_Datamaster cat)
        //{

        //    string mfrref = "";

        //    var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
        //    var NMList = _nounModifierRepository.FindAll(FormattedQuery).ToList();

        //    var sort = SortBy.Ascending("Seq").Ascending("Description");
        //    var query = Query.EQ("Description", "Short_OEM");
        //    string ShortStr = "", strNM = "";

        //    var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    var UOMSet = _UOMRepository.FindOne();

        //    var AbbrList = _abbreivateRepository.FindAll();
        //    //Short_Generic
        //    List<shortFrame> lst = new List<shortFrame>();
        //    foreach (Prosol_Sequence sq in seqList)
        //    {
        //        if (sq.Required == "Yes")
        //        {
        //            switch (sq.CatId)
        //            {
        //                case 101:

        //                    if (NMList[0].Formatted == 1)
        //                    {
        //                        //var abbObj = (from Abb in AbbrList where Abb.Value.Equals(cat.Noun, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                        //if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                        //    ShortStr += abbObj.Abbrevated + sq.Separator;
        //                        //else ShortStr += cat.Noun + sq.Separator;


        //                        if (NMList[0].Nounabv != null && NMList[0].Nounabv != "")
        //                            ShortStr += NMList[0].Nounabv + sq.Separator;
        //                        else ShortStr += cat.Noun + sq.Separator;
        //                    }
        //                    else
        //                    {
        //                        if (cat.Characteristics != null)
        //                        {
        //                            var sObj = cat.Characteristics.OrderBy(x => x.Squence).FirstOrDefault();
        //                            var abbObj = (from Abb in AbbrList where Abb.Value.Equals(sObj.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                            if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                ShortStr += abbObj.Abbrevated + sq.Separator;
        //                            else ShortStr += sObj.Value + sq.Separator;
        //                        }
        //                    }
        //                    strNM = ShortStr;
        //                    //var nounabbr = (from Abb in AbbrList where Abb.Value.Equals(cat.Noun, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                    //if (nounabbr != null && (nounabbr.Abbrevated != null && nounabbr.Abbrevated != ""))
        //                    //{
        //                    //    ShortStr += nounabbr.Abbrevated + sq.Separator;
        //                    //}
        //                    //else ShortStr += cat.Noun + sq.Separator;

        //                    break;
        //                case 102:
        //                    if (cat.Modifier != "NO DEFINER" && cat.Modifier != "NO MODIFIER")
        //                    {
        //                        if (NMList[0].Formatted == 1)
        //                        {

        //                            if (NMList[0].Modifierabv != null && NMList[0].Modifierabv != "")
        //                                ShortStr += NMList[0].Modifierabv + sq.Separator;
        //                            else ShortStr += cat.Modifier + sq.Separator;



        //                            strNM = ShortStr;
        //                        }
        //                    }
        //                    break;
        //                case 103:
        //                    int flg = 0;


        //                    //  int[] arrPos= new int[cat.Characteristics.Count];
        //                    //  string[] arrVal = new string[cat.Characteristics.Count];
        //                    int i = 0;
        //                    if (cat.Characteristics != null)
        //                    {
        //                        foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.ShortSquence))
        //                        {
        //                            if (NMList[0].Formatted == 1 || flg == 1)
        //                            {

        //                                if (chM.Value != null && chM.Value != "")
        //                                {
        //                                    string strValue = "";
        //                                    var frmMdl = new shortFrame();
        //                                    frmMdl.position = chM.Squence;


        //                                    if (chM.Value.Contains(','))
        //                                    {
        //                                        string tmpstr = "";
        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                        }
        //                                        else
        //                                        {
        //                                            string[] strsplt = chM.Value.Split(',');
        //                                            foreach (string str in strsplt)
        //                                            {
        //                                                abbObj = (from Abb in AbbrList where Abb.Value.Equals(str, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                                if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                                    tmpstr += abbObj.Abbrevated.TrimStart().TrimEnd() + ',';
        //                                                else tmpstr += str.TrimStart().TrimEnd() + ',';
        //                                            }
        //                                        }
        //                                        tmpstr = tmpstr.TrimEnd(',');
        //                                        if (UOMSet.Short_space == "with space")
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + " " + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        else
        //                                        {
        //                                            if (chM.UOM != null && chM.UOM != "")
        //                                                strValue += tmpstr + chM.UOM + sq.Separator;
        //                                            else strValue += tmpstr + sq.Separator;
        //                                        }
        //                                        frmMdl.values = strValue;
        //                                    }
        //                                    else
        //                                    {
        //                                        var abbObj = (from Abb in AbbrList where Abb.Value.Equals(chM.Value, StringComparison.OrdinalIgnoreCase) select Abb).FirstOrDefault();
        //                                        if (abbObj != null && (abbObj.Abbrevated != null && abbObj.Abbrevated != ""))
        //                                        {
        //                                            // Abbreivated
        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += abbObj.Abbrevated.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }
        //                                        else
        //                                        {
        //                                            // Abbreivated not exist

        //                                            if (UOMSet.Short_space == "with space")
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + " " + chM.UOM + sq.Separator;
        //                                                else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            else
        //                                            {
        //                                                if (chM.UOM != null && chM.UOM != "")
        //                                                    strValue += chM.Value.TrimStart().TrimEnd() + chM.UOM + sq.Separator;
        //                                                else strValue += chM.Value.TrimStart().TrimEnd() + sq.Separator;
        //                                            }
        //                                            frmMdl.values = strValue;
        //                                        }
        //                                    }

        //                                    lst.Add(frmMdl);

        //                                    ShortStr = strNM;
        //                                    string pattern = " X ";
        //                                    foreach (shortFrame sMdl in lst)
        //                                    {
        //                                        foreach (Match match in Regex.Matches(sMdl.values, pattern))
        //                                        {
        //                                            sMdl.values = Regex.Replace(sMdl.values, pattern, match.Value.TrimEnd().TrimStart());
        //                                        }
        //                                        // ShortStr += sMdl.values;
        //                                        string pattern1 = " x ";
        //                                        foreach (Match match in Regex.Matches(sMdl.values, pattern1))
        //                                        {
        //                                            sMdl.values = Regex.Replace(sMdl.values, pattern1, match.Value.TrimEnd().TrimStart());
        //                                        }
        //                                        ShortStr += sMdl.values;
        //                                    }

        //                                    if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                    {
        //                                        ShortStr = ShortStr.Trim();
        //                                        char[] chr = sq.Separator.ToCharArray();
        //                                        ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                        while (ShortStr.Length > seqList[0].ShortLength)
        //                                        {
        //                                            int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                            if (lstIndx > -1)
        //                                            {
        //                                                if (lst.Count > 0)
        //                                                {

        //                                                    if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    {
        //                                                        if (ShortStr.Substring(lstIndx).TrimStart(chr[0]) == lst[lst.Count - 1].values.TrimEnd(chr[0]))
        //                                                        {
        //                                                            lst.RemoveAt(lst.Count - 1);
        //                                                        }
        //                                                        else
        //                                                        {
        //                                                            int indx = lst[lst.Count - 1].values.TrimEnd(chr[0]).LastIndexOf(chr[0]);
        //                                                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx) + chr[0];
        //                                                        }
        //                                                    }
        //                                                }
        //                                                ShortStr = ShortStr.Remove(lstIndx);

        //                                            }
        //                                            else
        //                                            {
        //                                                lstIndx = ShortStr.LastIndexOf(' ');
        //                                                ShortStr = ShortStr.Remove(lstIndx);
        //                                                if (lst.Count > 0)
        //                                                {
        //                                                    int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                    lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                                }
        //                                            }

        //                                        }
        //                                        ShortStr = ShortStr + chr[0];
        //                                    }
        //                                    i++;
        //                                }
        //                            }
        //                            else flg = 1;
        //                        }
        //                        ShortStr = strNM;
        //                        foreach (shortFrame sMdl in lst.OrderBy(x => x.position))
        //                        {
        //                            ShortStr += sMdl.values;
        //                        }
        //                    }

        //                    break;

        //                case 104:

        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.s == 1)
        //                        {
        //                            if (vs.Name != null && vs.Name != "")
        //                            {
        //                                var querry = Query.EQ("Name", vs.Name);
        //                                var shtmfr = _VendorRepository.FindOne(querry);
        //                                if (shtmfr != null)
        //                                {
        //                                    if (shtmfr.ShortDescName != null && shtmfr.ShortDescName != "")
        //                                    {
        //                                        vs.shortmfr = shtmfr.ShortDescName;
        //                                    }
        //                                }
        //                            }
        //                            else
        //                            {
        //                                vs.shortmfr = vs.Name;
        //                            }

        //                            if (vs.shortmfr != null && vs.shortmfr != "" && vs.shortmfr != "undefined")
        //                            {
        //                                mfrref = vs.Name;
        //                                var frmMdl = new shortFrame();
        //                                frmMdl.position = 101;
        //                                frmMdl.values = vs.shortmfr + sq.Separator;
        //                                lst.Add(frmMdl);
        //                                ShortStr += vs.shortmfr + sq.Separator;
        //                                if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                                {
        //                                    ShortStr = ShortStr.Trim();
        //                                    char[] chr = sq.Separator.ToCharArray();
        //                                    ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                    while (ShortStr.Length > seqList[0].ShortLength)
        //                                    {
        //                                        int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                        if (lstIndx > -1)
        //                                        {
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                    lst.RemoveAt(lst.Count - 1);
        //                                            }
        //                                            ShortStr = ShortStr.Remove(lstIndx);

        //                                        }
        //                                        else
        //                                        {
        //                                            lstIndx = ShortStr.LastIndexOf(' ');
        //                                            ShortStr = ShortStr.Remove(lstIndx);
        //                                            if (lst.Count > 0)
        //                                            {
        //                                                int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                                lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                            }
        //                                        }
        //                                    }
        //                                    ShortStr = ShortStr + chr[0];
        //                                }
        //                                break;
        //                            }

        //                        }
        //                    }
        //                    break;
        //                case 105:


        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.s == 1 && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined")
        //                        {
        //                            var frmMdl = new shortFrame();
        //                            frmMdl.position = 100;
        //                            frmMdl.values = vs.RefNo.Trim() + sq.Separator;
        //                            lst.Add(frmMdl);
        //                            // ShortStr = strNM;

        //                            ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                            if (!checkLength(ShortStr, seqList[0].ShortLength))
        //                            {
        //                                ShortStr = ShortStr.Trim();
        //                                char[] chr = sq.Separator.ToCharArray();
        //                                ShortStr = ShortStr.TrimEnd(chr[0]);
        //                                while (ShortStr.Length > seqList[0].ShortLength)
        //                                {
        //                                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                                    if (lstIndx > -1)
        //                                    {
        //                                        if (lst.Count > 0)
        //                                        {
        //                                            if (ShortStr.Substring(lstIndx).Length > 1)
        //                                                lst.RemoveAt(lst.Count - 1);
        //                                        }
        //                                        ShortStr = ShortStr.Remove(lstIndx);

        //                                    }
        //                                    else
        //                                    {
        //                                        lstIndx = ShortStr.LastIndexOf(' ');
        //                                        ShortStr = ShortStr.Remove(lstIndx);
        //                                        if (lst.Count > 0)
        //                                        {
        //                                            int indx = lst[lst.Count - 1].values.LastIndexOf(' ');
        //                                            lst[lst.Count - 1].values = lst[lst.Count - 1].values.Remove(indx);
        //                                        }
        //                                    }

        //                                }
        //                                ShortStr = ShortStr + chr[0];
        //                            }
        //                            break;
        //                        }

        //                    }
        //                    break;
        //                case 106:
        //                    if (cat.Application != null && cat.Application != "")
        //                        ShortStr += cat.Application + sq.Separator;
        //                    break;
        //                case 107:
        //                    //if (cat.Drawingno != null && cat.Drawingno != "")
        //                    //    ShortStr += cat.Drawingno + sq.Separator;


        //                    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    {
        //                        if (vs.Refflag.ToUpper() == "DRAWING & POSITION NUMBER" && vs.RefNo != "" && vs.RefNo != null && vs.RefNo != "undefined" && vs.Name == mfrref && vs.Name != "" && vs.Name != null)
        //                        {
        //                            ShortStr += vs.RefNo.Trim() + sq.Separator;
        //                            break;
        //                        }
        //                    }
        //                    break;
        //                case 108:
        //                    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                        ShortStr += cat.Equipment.Name + sq.Separator;
        //                    break;
        //                case 109:
        //                    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                        ShortStr += cat.Equipment.Manufacturer + sq.Separator;
        //                    break;
        //                case 110:
        //                    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                        ShortStr += cat.Equipment.Modelno + sq.Separator;
        //                    break;
        //                case 111:
        //                    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                        ShortStr += cat.Equipment.Tagno + sq.Separator;
        //                    break;
        //                case 112:
        //                    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                        ShortStr += cat.Equipment.Serialno + sq.Separator;
        //                    break;
        //                case 113:
        //                    if (cat.Referenceno != null && cat.Referenceno != "")
        //                        ShortStr += cat.Referenceno + sq.Separator;
        //                    break;
        //                case 114:
        //                    if (cat.Additionalinfo != null && cat.Additionalinfo != "")
        //                        ShortStr += cat.Additionalinfo + sq.Separator;
        //                    break;
        //                case 115:
        //                    if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
        //                        ShortStr += cat.Equipment.Additionalinfo + sq.Separator;
        //                    break;


        //            }
        //            if (!checkLength(ShortStr, seqList[0].ShortLength))
        //            {
        //                ShortStr = ShortStr.Trim();
        //                char[] chr = sq.Separator.ToCharArray();
        //                ShortStr = ShortStr.TrimEnd(chr[0]);
        //                while (ShortStr.Length > seqList[0].ShortLength)
        //                {
        //                    int lstIndx = ShortStr.LastIndexOf(chr[0]);
        //                    ShortStr = ShortStr.Remove(lstIndx);
        //                }
        //                break;
        //            }
        //        }
        //    }
        //    if (ShortStr.Length != seqList[0].ShortLength)
        //    {
        //        ShortStr = ShortStr.Trim();
        //        int lsIndx = ShortStr.Length;
        //        string str = ShortStr.Substring(lsIndx - 1, 1);
        //        if (str == seqList[0].Separator.Trim())
        //        {
        //            ShortStr = ShortStr.Remove(lsIndx - 1);
        //        }
        //    }
        //    return ShortStr.ToUpper();
        //}

        //private string LongDesc(Prosol_Datamaster cat)
        //{

        //    // var vendortype = _VendortypeRepository.FindAll().ToList();
        //    var FormattedQuery = Query.And(Query.EQ("Noun", cat.Noun), Query.EQ("Modifier", cat.Modifier));
        //    var NMList = _nounModifierRepository.FindAll(FormattedQuery).ToList();
        //    var sort = SortBy.Ascending("Seq").Ascending("Description");
        //    var query = Query.EQ("Description", "Long");
        //    string LongStr = "";
        //    var seqList = _SequenceRepository.FindAll(query, sort).ToList();
        //    var UOMSet = _UOMRepository.FindOne();
        //    //Short_Generic
        //    foreach (Prosol_Sequence sq in seqList)
        //    {
        //        if (sq.Required == "Yes")
        //        {
        //            switch (sq.CatId)
        //            {
        //                case 101:
        //                    if (NMList[0].Formatted == 1)
        //                        LongStr += cat.Noun + sq.Separator;
        //                    else LongStr += cat.Noun + " ";
        //                    break;
        //                case 102:
        //                    if (cat.Modifier != "NO DEFINER" && cat.Modifier != "NO MODIFIER")
        //                        LongStr += cat.Modifier + sq.Separator;
        //                    break;
        //                case 103:
        //                    if (cat.Characteristics != null)
        //                    {
        //                        foreach (Prosol_AttributeList chM in cat.Characteristics.OrderBy(x => x.Squence))
        //                        {
        //                            if (chM.Value != null && chM.Value != "")
        //                            {
        //                                if (UOMSet.Long_space == "with space")
        //                                {
        //                                    if (chM.UOM != null && chM.UOM != "")
        //                                        LongStr += chM.Characteristic + ":" + chM.Value + " " + chM.UOM + sq.Separator;
        //                                    else LongStr += chM.Characteristic + ":" + chM.Value + sq.Separator;
        //                                }
        //                                else
        //                                {
        //                                    if (chM.UOM != null && chM.UOM != "")
        //                                        LongStr += chM.Characteristic + ":" + chM.Value + chM.UOM + sq.Separator;
        //                                    else LongStr += chM.Characteristic + ":" + chM.Value + sq.Separator;
        //                                }

        //                            }
        //                        }
        //                    }
        //                    break;

        //                case 104:
        //                    //if (cat.Manufacturer != null && cat.Manufacturer != "")
        //                    //    LongStr += "Manufacturer:" + cat.Manufacturer + sq.Separator;
        //                    //break;
        //                    if (cat.Vendorsuppliers != null)
        //                    {

        //                        int g = 0;
        //                        foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        {

        //                            if (vs.l == 1 && vs.Name != null && vs.Name != "")
        //                            {
        //                                LongStr += vs.Type + ":" + vs.Name + sq.Separator;

        //                            }
        //                            if (vs.l == 1 && vs.RefNo != null && vs.RefNo != "")
        //                            {
        //                                LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
        //                            }
        //                            //else
        //                            //{
        //                            //    if (vs.l == 1 && vs.RefNo != null && vs.RefNo != "")
        //                            //    {
        //                            //        LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
        //                            //    }
        //                            //}
        //                        }


        //                        //string[] mfrnames = new string[cat.Vendorsuppliers.Count];
        //                        //int iii = 0;
        //                        //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        //{
        //                        //    if (!mfrnames.Contains(vs.Name))
        //                        //    {
        //                        //        mfrnames[iii] = vs.Name;
        //                        //        iii++;
        //                        //    }
        //                        //}
        //                        //foreach (string names in mfrnames)
        //                        //{
        //                        //    int g = 0;
        //                        //    foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                        //    {

        //                        //        if (vs.l == 1 && g == 0 && vs.Name == names && vs.Name != null && vs.Name != "")
        //                        //        {
        //                        //            LongStr += vs.Type + ":" + vs.Name + sq.Separator;
        //                        //            g = 1;
        //                        //        }
        //                        //        if (vs.l == 1 && vs.Name == names && vs.RefNo != null && vs.RefNo != "")
        //                        //        {
        //                        //            LongStr += vs.Refflag + ":" + vs.RefNo + sq.Separator;
        //                        //        }
        //                        //    }

        //                    }
        //                    break;

        //                case 105:
        //                    //if (cat.Partno != null && cat.Partno != "")
        //                    //    LongStr += "Partno:" + cat.Partno + sq.Separator;
        //                    //break;


        //                    //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    //{




        //                    //    if (vs.l == '1' && vs.Refflag == "PART NUMBER")
        //                    //    {
        //                    //        LongStr += vs.Type.ToUpper()+" "+ "PART NUMBER" + ":" + vs.RefNo + sq.Separator;

        //                    //    }


        //                    //}
        //                    break;

        //                case 106:
        //                    if (cat.Application != null && cat.Application != "")
        //                        LongStr += "Application:" + cat.Application + sq.Separator;
        //                    break;
        //                case 107:
        //                    //if (cat.Drawingno != null && cat.Drawingno != "")
        //                    //    LongStr += "Drawing NO.:" + cat.Drawingno + sq.Separator;
        //                    //break;


        //                    //foreach (Vendorsuppliers vs in cat.Vendorsuppliers)
        //                    //{
        //                    //    if (vs.l == '1' && vs.Refflag == "DRAWING & POSITION NUMBER")
        //                    //    {
        //                    //        LongStr += vs.Type.ToUpper() + " DRAWING & POSITION NUMBER:" + vs.RefNo + sq.Separator;

        //                    //    }
        //                    //    if (vs.l == '1' && vs.Refflag == "MODEL NUMBER")
        //                    //    {
        //                    //        LongStr += vs.Type.ToUpper() + " MODEL NUMBER:" + vs.RefNo + sq.Separator;

        //                    //    }
        //                    //    if (vs.l == '1' && vs.Refflag == "REFERENCE NUMBER")
        //                    //    {
        //                    //        LongStr += vs.Type.ToUpper() + " REFERENCE NUMBER:" + vs.RefNo + sq.Separator;

        //                    //    }
        //                    //}

        //                    break;


        //                case 108:
        //                    if (cat.Equipment != null && cat.Equipment.Name != null && cat.Equipment.Name != "")
        //                        LongStr += "Equipment Name:" + cat.Equipment.Name + sq.Separator;
        //                    break;
        //                case 109:
        //                    if (cat.Equipment != null && cat.Equipment.Manufacturer != null && cat.Equipment.Manufacturer != "")
        //                        LongStr += "Equipment Manufacturer:" + cat.Equipment.Manufacturer + sq.Separator;
        //                    break;
        //                case 110:
        //                    if (cat.Equipment != null && cat.Equipment.Modelno != null && cat.Equipment.Modelno != "")
        //                        LongStr += "Equipment Modelno:" + cat.Equipment.Modelno + sq.Separator;
        //                    break;
        //                case 111:
        //                    if (cat.Equipment != null && cat.Equipment.Tagno != null && cat.Equipment.Tagno != "")
        //                        LongStr += "Equipment Tagno:" + cat.Equipment.Tagno + sq.Separator;
        //                    break;
        //                case 112:
        //                    if (cat.Equipment != null && cat.Equipment.Serialno != null && cat.Equipment.Serialno != "")
        //                        LongStr += "Equipment Serialno:" + cat.Equipment.Serialno + sq.Separator;
        //                    break;
        //                case 113:
        //                    if (cat.Referenceno != null && cat.Referenceno != "")
        //                        LongStr += "Position No.:" + cat.Referenceno + sq.Separator;
        //                    break;
        //                case 114:
        //                    if (cat.Additionalinfo != null && cat.Additionalinfo != "")
        //                        LongStr += "Additional Information:" + cat.Additionalinfo + sq.Separator;
        //                    break;
        //                case 115:
        //                    if (cat.Equipment != null && cat.Equipment.Additionalinfo != null && cat.Equipment.Additionalinfo != "")
        //                        LongStr += "Additional Information(Equipment):" + cat.Equipment.Additionalinfo + sq.Separator;
        //                    break;


        //            }
        //        }
        //    }
        //    LongStr = LongStr.Trim();
        //    int lstIndx = LongStr.Length;
        //    LongStr = LongStr.Remove(lstIndx - 1, 1);
        //    return LongStr.ToUpper();
        //}

        protected bool checkLength(string str, int len)
        {
            str = str.Trim();
            int lstIndx = str.Length;
            str = str.Remove(lstIndx - 1, 1);
            if (str.Length < len)
            {
                return true;
            }
            else return false;

        }


    }
}
