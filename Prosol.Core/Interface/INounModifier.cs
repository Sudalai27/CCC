using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prosol.Core.Model;
using System.Web;

namespace Prosol.Core.Interface
{
    public partial interface INounModifier
    {
       
       // void DeleteNounModifier(Prosol_NounModifiers NM);   

        bool Create(Prosol_NounModifiers NM, HttpPostedFileBase file);
        int BulkNounModifier(HttpPostedFileBase file);
        string[] AutoSearchNoun(string term);
        string[] AutoSearchModifier(string term, string Noun);

        List<Prosol_NounModifiers> Getformat(string Noun, string Modifier);
        Prosol_NounModifiers GetNounModifier(string Noun, string Modifier);
        IEnumerable<Prosol_NounModifiers> GetNounList();
        IEnumerable<Prosol_NounModifiers> GetModifierList(string Noun);
        IEnumerable<Prosol_NounModifiers> GetNounModifierList();
        Prosol_NounModifiers GetNounDetail(string Noun);

        IEnumerable<Prosol_NounModifiers> GetNMinfo(string info);
        IEnumerable<Prosol_Nounscrub> GetScrubNMinfo();
        //codesave

        bool codesave(Prosol_CodeLogic data);
        Prosol_CodeLogic showcode();

        List<Prosol_UOMMODEL> getuomlist(string Noun, string Modifier);
        IEnumerable<Prosol_NounModifiers> GetAssetNounList();
        string[] AutoSearchAssetNoun(string term);
        bool AssetCreate(Prosol_NounModifiers NM, HttpPostedFileBase file);
        IEnumerable<Prosol_NounModifiers> GetAssetModifierList(string Noun);
        string[] AutoSearchAssetValues(string term);
    }
}
