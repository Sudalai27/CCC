using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Prosol.Core.Interface
{
    public partial interface ILogic
    {
        //attributerelasionship
        IEnumerable<Prosol_Attribute> getattributelist();
        IEnumerable<Prosol_UOM> getunit();
        bool insertdata(Prosol_Logic data, int update);
        IEnumerable<Prosol_Logic> ATTRRELLIST();
        bool deleteattribute(string id);
        //IEnumerable<Prosol_Abbrevate> getunitname();
        IEnumerable<Prosol_Attribute> getunitname(string AttributeName1);
        IEnumerable<Prosol_Abbrevate> GetValue1();

        //nmrelasionship
        IEnumerable<Prosol_NounModifiers> getnoun();
        IEnumerable<Prosol_NounModifiers> getmodifiers(string Noun);
        bool savenmattribute(Prosol_NMAttributeRelationship catList, int update);
        IEnumerable<Prosol_NMAttributeRelationship> getnmList();
        bool DeleteNMAttrRel(string id);
        IEnumerable<Prosol_NMAttributeRelationship> GetTableListforkeyatt(string Noun, string Modifier, string KeyAttribute, string KeyValue);

        IEnumerable<Prosol_Logic> GetLogicList(string srchtxt);
        IEnumerable<Prosol_Logic> GetLogicList();
        IEnumerable<Prosol_NMAttributeRelationship> GetNMList(string srchtxt);
        IEnumerable<Prosol_NMAttributeRelationship> GetNMList();
        int BulkNounModifierAttribute(HttpPostedFileBase file);

        IEnumerable<Prosol_NMAttributeRelationship>  FetchNMRelation(string Noun,string Modifier);

        IEnumerable<Prosol_Logic> FetchATTRelation(string Noun, string Modifier);
    }
}
