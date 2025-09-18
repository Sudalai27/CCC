using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prosol.Core.Interface
{
    public interface ISearch
    {

        List<Prosol_Datamaster> SearchDesc(string term, string sBy);
        List<Prosol_Datamaster> Search(string term);
        List<Prosol_Datamaster> Get(string noun, string Modifier);
        List<Prosol_Datamaster> getattri(Prosol_Datamaster cat, List<Prosol_AttributeList> lst);
        List<Prosol_Datamaster> GetCodalogic(string cat);
        List<Prosol_UNSPSC> GetSegment();
        List<Prosol_UNSPSC> GetFamily(string Family);
        List<Prosol_UNSPSC> GetClass(string Class);
        List<Prosol_UNSPSC> GetCommodity(string Commodity);
        List<Prosol_Datamaster> GetUnspsclogic(string value);
        List<Prosol_Datamaster> GetEquip(string value);
        List<Prosol_Datamaster> Getmanu(string value);
        List<Prosol_Datamaster> SearchRef(string value);

        Prosol_Datamaster getItemInfo(string Itemcode);
        Prosol_ERPInfo getItemERP(string Itemcode);
        List<Prosol_Datamaster> getDataforES();
        bool CataloguerCheck(string UserName);
        //mapduplicate
        List<Prosol_Datamaster> Mapduplicate(string Itemcode);
        bool savemapduplicate(string new_code, string existing_code);
        bool savemapduplicate(string new_code, string existing_code, string userid, string username);
        bool unmapcode(string Itemcode);
        string Pushtosap(string code, string refcode);
        List<Prosol_AssetMaster> AssetSearchDesc(string term, string sBy);
        Prosol_AssetAttributes getAssetAttributes(string Itemcode);
    }
}
