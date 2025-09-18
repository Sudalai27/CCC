using Prosol.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Prosol.Core.DashboardService;

namespace Prosol.Core.Interface
{
    public interface IDashboard
    {
        List<Prosol_Dashboard> BindTotalItem(string[] plants);
        List<Prosol_Dashboard> BindReleaser(string[] plants, List<TargetExn> UserRole, string userId);
        List<Prosol_Dashboard> BindReview(string[] plants, List<TargetExn> UserRole, string userId);
        List<Prosol_Dashboard> BindCatalogue(string[] plants, List<TargetExn> UserRole, string userId);
        //    List<Prosol_Dashboard> BindApprove(string[] plants, List<TargetExn> UserRole, string userId);
        //   List<Prosol_Dashboard> BindRequest(string[] plants, List<TargetExn> UserRole, string userId);

        List<Prosol_Dashboard> bindReviewTarget(string[] plants, string userId);
        List<Prosol_Dashboard> bindCatalogueTarget(string[] plants, string userId);
        //   List<Prosol_Dashboard> bindApproveTarget(string[] plants, string userId);
        //    List<Prosol_Dashboard> bindRequestTarget(string[] plants, string userId);

        List<Prosol_ActionHistory> BindItemHistory(string[] plants);

        //List<Prosol_ActionHistory> BindItemHistory1(string[] plants);

        ////ItemHistoryGO
        //wrote by saikat chowdhury 07/01/2019

        List<Prosol_Request> BindRequest(DateTime dte, DateTime dte1, string UserId);
        List<Prosol_ActionHistory> BindItemHistory2(DateTime dte, DateTime dte1);
        List<list_details> BindOverAll(DateTime dte, DateTime dte1);
        List<Prosol_Plants> getRepPurg();
        List<Prosol_Datamaster> getReqItems();
        List<Prosol_Datamaster> getMasterItems();
        List<Prosol_Datamaster> getSKUItems();
        List<Prosol_Request> getRlsdReqItems();
        List<Prosol_Datamaster> getRlsdMasItems();
        List<Prosol_Datamaster> getPendItems();
        List<Prosol_Request> getNewItems();
        List<Prosol_Request> getModItems();
    }
}
