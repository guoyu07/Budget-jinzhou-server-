using Budget.DataAccess.AutoCreate.DAL;
using Budget.DataAccess.AutoCreate.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Budget.DataAccess.AutoCreate.BLL
{
    public static partial class RM_keshiyewufeiManager
    {
        public static RM_keshiyewufei AddRM_keshiyewufei(RM_keshiyewufei rM_keshiyewufei)
        {
            return RM_keshiyewufeiService.AddRM_keshiyewufei(rM_keshiyewufei);
        }

        public static bool DeleteRM_keshiyewufei(RM_keshiyewufei rM_keshiyewufei)
        {
            return RM_keshiyewufeiService.DeleteRM_keshiyewufei(rM_keshiyewufei);
        }

        public static bool DeleteRM_keshiyewufeiByID(int kSID)
        {
            return RM_keshiyewufeiService.DeleteRM_keshiyewufeiByKSID(kSID);
        }

        public static bool ModifyRM_keshiyewufei(RM_keshiyewufei rM_keshiyewufei)
        {
            return RM_keshiyewufeiService.ModifyRM_keshiyewufei(rM_keshiyewufei);
        }

        public static DataTable GetAllRM_keshiyewufei()
        {
            return RM_keshiyewufeiService.GetAllRM_keshiyewufei();
        }

        public static RM_keshiyewufei GetRM_keshiyewufeiByKSID(int kSID)
        {
            return RM_keshiyewufeiService.GetRM_keshiyewufeiByKSID(kSID);
        }

    }
}
