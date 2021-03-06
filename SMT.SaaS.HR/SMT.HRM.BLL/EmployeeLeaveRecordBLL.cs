﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SMT.HRM.DAL;
using SMT_HRM_EFModel;
using System.Linq.Dynamic;
using SMT.HRM.CustomModel;
using System.Data.Objects.DataClasses;
using SMT.HRM.BLL.Common;
using SMT.Foundation.Log;
using System.Threading;
namespace SMT.HRM.BLL
{
    public class EmployeeLeaveRecordBLL : BaseBll<T_HR_EMPLOYEELEAVERECORD>, ILookupEntity, IOperate
    {
        #region 获取数据
        /// <summary>
        /// 用于实体Grid中显示数据的分页查询
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="sort">排序字段</param>
        /// <param name="filterString">过滤条件</param>
        /// <param name="paras">过滤条件中的参数值</param>
        /// <param name="pageCount">返回总页数</param>
        /// <returns>查询结果集</returns>
        public IQueryable<V_EmpLeaveRdInfo> EmployeeLeaveRecordPaging(int pageIndex, int pageSize, string sort, string filterString, List<object> paras, ref int pageCount, string strCheckState, string strOwnerID, string recorderDate)
        {
            try
            {
                if (strCheckState != Convert.ToInt32(SMT.HRM.DAL.CheckStates.WaittingApproval).ToString())
                {
                    if (strCheckState == Convert.ToInt32(SMT.HRM.DAL.CheckStates.All).ToString())
                    {
                        strCheckState = string.Empty;
                    }

                    SetOrganizationFilter(ref filterString, ref paras, strOwnerID, "T_HR_EMPLOYEELEAVERECORD");
                }
                else
                {
                    string strCheckfilter = string.Copy(filterString);
                    SetFilterWithflow("LEAVERECORDID", "T_HR_EMPLOYEELEAVERECORD", strOwnerID, ref strCheckState, ref filterString, ref paras);
                    if (string.Compare(strCheckfilter, filterString) == 0)
                    {
                        return null;
                    }
                }

                if (!string.IsNullOrEmpty(strCheckState))
                {
                    int iIndex = 0;
                    if (!string.IsNullOrEmpty(filterString))
                    {
                        filterString += " AND";
                    }

                    if (paras.Count() > 0)
                    {
                        iIndex = paras.Count();
                    }

                    filterString += " CHECKSTATE == @" + iIndex.ToString();
                    paras.Add(strCheckState);
                }

                IQueryable<T_HR_EMPLOYEELEAVERECORD> ents = dal.GetObjects().Include("T_HR_LEAVETYPESET");
                if (!string.IsNullOrEmpty(filterString))
                {
                    ents = ents.Where(filterString, paras.ToArray());
                }
                //if (!string.IsNullOrEmpty(recorderDate))
                //{
                //    DateTime tmpDate = Convert.ToDateTime(recorderDate);
                //    ents = ents.Where(p => p.STARTDATETIME.Value.Year == tmpDate.Year && p.STARTDATETIME.Value.Month == tmpDate.Month);
                //}
                ents = ents.OrderBy(sort);

                var entrs = from e in ents
                            join vDepartment in dal.GetObjects<T_HR_DEPARTMENT>() on e.OWNERDEPARTMENTID equals vDepartment.DEPARTMENTID
                            select new V_EmpLeaveRdInfo
                            {
                                CHECKSTATE = e.CHECKSTATE,
                                OWNERCOMPANYID = e.OWNERCOMPANYID,
                                OWNERDEPARTMENTID = e.OWNERDEPARTMENTID,
                                DEPARTMENTNAME = vDepartment.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME,
                                OWNERPOSTID = e.OWNERPOSTID,
                                OWNERID = e.OWNERID,
                                EMPLOYEEID = e.EMPLOYEEID,
                                EMPLOYEECODE = e.EMPLOYEECODE,
                                EMPLOYEENAME = e.EMPLOYEENAME,
                                LEAVEDAYS = e.LEAVEDAYS,
                                LEAVEHOURS = e.LEAVEHOURS,
                                TOTALHOURS = e.TOTALHOURS,
                                STARTDATETIME = e.STARTDATETIME,
                                ENDDATETIME = e.ENDDATETIME,
                                LEAVERECORDID = e.LEAVERECORDID,
                                LEAVETYPENAME = e.T_HR_LEAVETYPESET.LEAVETYPENAME,
                                CREATECOMPANYID = e.CREATECOMPANYID,
                                CREATEDEPARTMENTID = e.CREATEDEPARTMENTID,
                                CREATEPOSTID = e.CREATEPOSTID,
                                CREATEUSERID = e.CREATEUSERID,
                                CREATEDATE = e.CREATEDATE,
                                UPDATEDATE = e.UPDATEDATE,
                                CANCELTOTALHOURS = e.T_HR_EMPLOYEECANCELLEAVE.Where(t => t.CHECKSTATE == "2").Sum(t => t.TOTALHOURS),                                
                                REASON = e.REASON
                            };
                entrs = entrs.OrderBy(sort);
                entrs = Utility.Pager<V_EmpLeaveRdInfo>(entrs, pageIndex, pageSize, ref pageCount);

                return entrs;
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("员工请假发生错误：EmployeeLeaveRecordPaging：" + ex.ToString());
                return null;
            }
        }
        /// <summary>
        /// 用于实体Grid中显示数据的分页查询
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="sort">排序字段</param>
        /// <param name="filterString">过滤条件</param>
        /// <param name="paras">过滤条件中的参数值</param>
        /// <param name="pageCount">返回总页数</param>
        /// <param name="strCheckState">审核状态</param>
        /// <param name="strOwnerID">所有者</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="recorderDate">截止日期</param>
        /// <returns>查询结果集</returns>
        public IQueryable<V_EmpLeaveRdInfo> EmployeeLeaveRecordPaged(int pageIndex, int pageSize, string sort, string filterString, List<object> paras, ref int pageCount, string strCheckState, string strOwnerID, string startDate, string recorderDate,string employeeID,string leaveTypeSetID)
        {
            try
            {
                string filter1 = string.Empty, filter2 = string.Empty, filter3 = string.Empty, filter4 = string.Empty;
                List<object> paraTemp1 = new List<object>();
                List<object> paraTemp2 = new List<object>();
                List<object> paraTemp3 = new List<object>();
                List<object> paraTemp4 = new List<object>();
                //开始月份在请假开始时间
                if (!string.IsNullOrEmpty(employeeID))
                {
                    if (!string.IsNullOrEmpty(filter1))
                    {
                        filter1 += " and ";
                    }
                    filter1 += "EMPLOYEEID==@" + paraTemp1.Count().ToString();
                    paraTemp1.Add(employeeID);
                }
                if (!string.IsNullOrEmpty(leaveTypeSetID))
                {
                    if (!string.IsNullOrEmpty(filter1))
                    {
                        filter1 += " and ";
                    }
                    filter1 += "T_HR_LEAVETYPESET.LEAVETYPESETID==@" + paraTemp1.Count().ToString();
                    paraTemp1.Add(leaveTypeSetID);
                }
                //开始月份在请假开始时间
                if (!string.IsNullOrEmpty(employeeID))
                {
                    if (!string.IsNullOrEmpty(filter2))
                    {
                        filter2 += " and ";
                    }
                    filter2 += "EMPLOYEEID==@" + paraTemp2.Count().ToString();
                    paraTemp2.Add(employeeID);
                }
                if (!string.IsNullOrEmpty(leaveTypeSetID))
                {
                    if (!string.IsNullOrEmpty(filter2))
                    {
                        filter2 += " and ";
                    }
                    filter2 += "T_HR_LEAVETYPESET.LEAVETYPESETID==@" + paraTemp2.Count().ToString();
                    paraTemp2.Add(leaveTypeSetID);
                }
                //截止月份在请假开始时间
                if (!string.IsNullOrEmpty(employeeID))
                {
                    if (!string.IsNullOrEmpty(filter3))
                    {
                        filter3 += " and ";
                    }
                    filter3 += "EMPLOYEEID==@" + paraTemp3.Count().ToString();
                    paraTemp3.Add(employeeID);
                }
                if (!string.IsNullOrEmpty(leaveTypeSetID))
                {
                    if (!string.IsNullOrEmpty(filter3))
                    {
                        filter3 += " and ";
                    }
                    filter3 += "T_HR_LEAVETYPESET.LEAVETYPESETID==@" + paraTemp3.Count().ToString();
                    paraTemp3.Add(leaveTypeSetID);
                }
                //开始月份在请假截止时间
                if (!string.IsNullOrEmpty(employeeID))
                {
                    if (!string.IsNullOrEmpty(filter4))
                    {
                        filter4 += " and ";
                    }
                    filter4 += "EMPLOYEEID==@" + paraTemp4.Count().ToString();
                    paraTemp4.Add(employeeID);
                }
                if (!string.IsNullOrEmpty(leaveTypeSetID))
                {
                    if (!string.IsNullOrEmpty(filter4))
                    {
                        filter4 += " and ";
                    }
                    filter4+= "T_HR_LEAVETYPESET.LEAVETYPESETID==@" + paraTemp4.Count().ToString();
                    paraTemp4.Add(leaveTypeSetID);
                }
                //处理跨月
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(recorderDate))
                {
                    //开始月份在请假开始时间
                    DateTime startFirstDay = new DateTime(Convert.ToDateTime(startDate).Year, Convert.ToDateTime(startDate).Month, 1);
                    DateTime startLastDay = new DateTime(Convert.ToDateTime(startDate).AddMonths(1).Year, Convert.ToDateTime(startDate).AddMonths(1).Month, 1).AddDays(-1);
                    if (!string.IsNullOrEmpty(filter1))
                    {
                        filter1 += " and ";
                    }
                    filter1 += " STARTDATETIME>=@" + paraTemp1.Count().ToString();
                    paraTemp1.Add(startFirstDay);
                    filter1 += " and STARTDATETIME<=@" + paraTemp1.Count().ToString();
                    paraTemp1.Add(startLastDay);
                    filter1 += " and ENDDATETIME>=@" + paraTemp1.Count().ToString();
                    paraTemp1.Add(startLastDay);
                    //截止月份在请假开始时间
                    if (!string.IsNullOrEmpty(filter4))
                    {
                        filter4 += " and ";
                    }
                    filter4 += " STARTDATETIME<@" + paraTemp4.Count().ToString();
                    paraTemp4.Add(startFirstDay);
                    filter4 += " and ENDDATETIME>=@" + paraTemp4.Count().ToString();
                    paraTemp4.Add(startFirstDay);
                    filter4 += " and ENDDATETIME<=@" + paraTemp4.Count().ToString();
                    paraTemp4.Add(startLastDay);
                    DateTime recorderFirstDay = new DateTime(Convert.ToDateTime(recorderDate).AddMonths(-1).Year, Convert.ToDateTime(recorderDate).AddMonths(-1).Month, 1);
                    DateTime recorderLastDay = new DateTime(Convert.ToDateTime(recorderDate).Year, Convert.ToDateTime(recorderDate).Month, 1).AddDays(-1);
                    //截止月份在请假截止时间
                    if (!string.IsNullOrEmpty(filter2))
                    {
                        filter2 += " and ";
                    }
                    filter2 += " STARTDATETIME<@" + paraTemp2.Count().ToString();
                    paraTemp2.Add(recorderFirstDay);
                    filter2 += " and ENDDATETIME>=@" + paraTemp2.Count().ToString();
                    paraTemp2.Add(recorderFirstDay);
                    filter2 += " and ENDDATETIME<=@" + paraTemp2.Count().ToString();
                    paraTemp2.Add(recorderLastDay);
                    //开始月份在请假截止时间
                    if (!string.IsNullOrEmpty(filter3))
                    {
                        filter3 += " and ";
                    }
                    filter3 += " STARTDATETIME>=@" + paraTemp3.Count().ToString();
                    paraTemp3.Add(recorderFirstDay);
                    filter3 += " and STARTDATETIME<=@" + paraTemp3.Count().ToString();
                    paraTemp3.Add(recorderLastDay);
                    filter3 += " and ENDDATETIME>@" + paraTemp3.Count().ToString();
                    paraTemp3.Add(recorderLastDay);
                }
                if (strCheckState != Convert.ToInt32(SMT.HRM.BLL.Common.CheckStates.WaittingApproval).ToString())
                {
                    if (strCheckState == Convert.ToInt32(SMT.HRM.BLL.Common.CheckStates.All).ToString())
                    {
                        strCheckState = string.Empty;
                    }
                    SetOrganizationFilter(ref filterString, ref paras, strOwnerID, "T_HR_EMPLOYEELEAVERECORD");
                    SetOrganizationFilter(ref filter1, ref paraTemp1, strOwnerID, "T_HR_EMPLOYEELEAVERECORD");
                    SetOrganizationFilter(ref filter2, ref paraTemp2, strOwnerID, "T_HR_EMPLOYEELEAVERECORD");
                    SetOrganizationFilter(ref filter3, ref paraTemp3, strOwnerID, "T_HR_EMPLOYEELEAVERECORD");
                    SetOrganizationFilter(ref filter4, ref paraTemp4, strOwnerID, "T_HR_EMPLOYEELEAVERECORD");
                }
                else
                {
                    string strCheckfilter = string.Copy(filterString);
                    SetFilterWithflow("LEAVERECORDID", "T_HR_EMPLOYEELEAVERECORD", strOwnerID, ref strCheckState, ref filterString, ref paras);
                    SetFilterWithflow("LEAVERECORDID", "T_HR_EMPLOYEELEAVERECORD", strOwnerID, ref strCheckState, ref filter1, ref paraTemp1);
                    SetFilterWithflow("LEAVERECORDID", "T_HR_EMPLOYEELEAVERECORD", strOwnerID, ref strCheckState, ref filter2, ref paraTemp2);
                    SetFilterWithflow("LEAVERECORDID", "T_HR_EMPLOYEELEAVERECORD", strOwnerID, ref strCheckState, ref filter3, ref paraTemp3);
                    SetFilterWithflow("LEAVERECORDID", "T_HR_EMPLOYEELEAVERECORD", strOwnerID, ref strCheckState, ref filter4, ref paraTemp4);
                    if (string.Compare(strCheckfilter, filterString) == 0)
                    {
                        return null;
                    }
                }

                if (!string.IsNullOrEmpty(strCheckState))
                {
                    int iIndex = 0;
                    if (!string.IsNullOrEmpty(filterString))
                    {
                        filterString += " AND";
                    }

                    if (paras.Count() > 0)
                    {
                        iIndex = paras.Count();
                    }

                    filterString += " CHECKSTATE == @" + iIndex.ToString();
                    paras.Add(strCheckState);
                }
                if (!string.IsNullOrEmpty(strCheckState))
                {
                    int iIndex1 = 0;
                    if (!string.IsNullOrEmpty(filter1))
                    {
                        filter1 += " AND";
                    }

                    if (paraTemp1.Count() > 0)
                    {
                        iIndex1 = paraTemp1.Count();
                    }

                    filter1 += " CHECKSTATE == @" + iIndex1.ToString();
                    paraTemp1.Add(strCheckState);
                }
                if (!string.IsNullOrEmpty(strCheckState))
                {
                    int iIndex2 = 0;
                    if (!string.IsNullOrEmpty(filter2))
                    {
                        filter2 += " AND";
                    }

                    if (paraTemp2.Count() > 0)
                    {
                        iIndex2 = paraTemp2.Count();
                    }

                    filter2 += " CHECKSTATE == @" + iIndex2.ToString();
                    paraTemp2.Add(strCheckState);
                }
                if (!string.IsNullOrEmpty(strCheckState))
                {
                    int iIndex3 = 0;
                    if (!string.IsNullOrEmpty(filter3))
                    {
                        filter3 += " AND";
                    }

                    if (paraTemp3.Count() > 0)
                    {
                        iIndex3 = paraTemp3.Count();
                    }

                    filter3 += " CHECKSTATE == @" + iIndex3.ToString();
                    paraTemp3.Add(strCheckState);
                }
                if (!string.IsNullOrEmpty(strCheckState))
                {
                    int iIndex4 = 0;
                    if (!string.IsNullOrEmpty(filter4))
                    {
                        filter4 += " AND";
                    }

                    if (paraTemp4.Count() > 0)
                    {
                        iIndex4 = paraTemp4.Count();
                    }

                    filter4 += " CHECKSTATE == @" + iIndex4.ToString();
                    paraTemp4.Add(strCheckState);
                }
                IQueryable<T_HR_EMPLOYEELEAVERECORD> ents = dal.GetObjects().Include("T_HR_LEAVETYPESET");
                IQueryable<T_HR_EMPLOYEELEAVERECORD> ents1 = null;
                IQueryable<T_HR_EMPLOYEELEAVERECORD> ents2 = null;
                IQueryable<T_HR_EMPLOYEELEAVERECORD> ents3 = null;
                IQueryable<T_HR_EMPLOYEELEAVERECORD> ents4 = null;
                if (!string.IsNullOrEmpty(filter1))
                {
                    ents1 = ents.Where(filter1, paraTemp1.ToArray());
                }
                if (!string.IsNullOrEmpty(filter2))
                {
                    ents2 = ents.Where(filter2, paraTemp2.ToArray());
                }
                if (!string.IsNullOrEmpty(filter3))
                {
                    ents3 = ents.Where(filter3, paraTemp3.ToArray());
                }
                if (!string.IsNullOrEmpty(filter4))
                {
                    ents4 = ents.Where(filter4, paraTemp4.ToArray());
                }
                if (!string.IsNullOrEmpty(filterString))
                {
                    ents = ents.Where(filterString, paras.ToArray()).Union(ents1).Union(ents2).Union(ents3).Union(ents4);
                }
                //if (!string.IsNullOrEmpty(recorderDate))
                //{
                //    DateTime tmpDate = Convert.ToDateTime(recorderDate);
                //    ents = ents.Where(p => p.STARTDATETIME.Value.Year == tmpDate.Year && p.STARTDATETIME.Value.Month == tmpDate.Month);
                //}
                //ents = ents.OrderBy(sort);

                var entrs = from e in ents
                            join vDepartment in dal.GetObjects<T_HR_DEPARTMENT>() on e.OWNERDEPARTMENTID equals vDepartment.DEPARTMENTID
                            select new V_EmpLeaveRdInfo
                            {
                                CHECKSTATE = e.CHECKSTATE,
                                OWNERCOMPANYID = e.OWNERCOMPANYID,
                                OWNERDEPARTMENTID = e.OWNERDEPARTMENTID,
                                DEPARTMENTNAME = vDepartment.T_HR_DEPARTMENTDICTIONARY.DEPARTMENTNAME,
                                OWNERPOSTID = e.OWNERPOSTID,
                                OWNERID = e.OWNERID,
                                EMPLOYEEID = e.EMPLOYEEID,
                                EMPLOYEECODE = e.EMPLOYEECODE,
                                EMPLOYEENAME = e.EMPLOYEENAME,
                                LEAVEDAYS = e.LEAVEDAYS,
                                LEAVEHOURS = e.LEAVEHOURS,
                                TOTALHOURS = e.TOTALHOURS,
                                STARTDATETIME = e.STARTDATETIME,
                                ENDDATETIME = e.ENDDATETIME,
                                LEAVERECORDID = e.LEAVERECORDID,
                                LEAVETYPENAME = e.T_HR_LEAVETYPESET.LEAVETYPENAME,
                                CREATECOMPANYID = e.CREATECOMPANYID,
                                CREATEDEPARTMENTID = e.CREATEDEPARTMENTID,
                                CREATEPOSTID = e.CREATEPOSTID,
                                CREATEUSERID = e.CREATEUSERID,
                                CREATEDATE = e.CREATEDATE,
                                UPDATEDATE = e.UPDATEDATE,
                                CANCELTOTALHOURS = e.T_HR_EMPLOYEECANCELLEAVE.Where(t => t.CHECKSTATE == "2").Sum(t => t.TOTALHOURS),
                                REASON = e.REASON
                            };
                entrs = entrs.OrderBy(sort);

                entrs = Utility.Pager<V_EmpLeaveRdInfo>(entrs, pageIndex, pageSize, ref pageCount);

                return entrs;
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("员工请假发生错误：EmployeeLeaveRecordPaging：" + ex.ToString());
                return null;
            }
        }
        #region 导出报表
        /// <summary>
        /// 导出请假报表
        /// </summary>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="sort">排序字段</param>
        /// <param name="filterString">过滤条件</param>
        /// <param name="paras">过滤条件中的参数值</param>
        /// <param name="pageCount">返回总页数</param>
        /// <param name="strCheckState">审核状态</param>
        /// <param name="strOwnerID">所有者</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="recorderDate">截止日期</param>
        /// <returns>查询结果集</returns>
        public byte[] ExportEmployeeLeaveRecordReports(int pageIndex, int pageSize, string sort, string filterString, List<object> paras, ref int pageCount, string strCheckState, string strOwnerID, string startDate, string recorderDate, string employeeID, string leaveTypeSetID)
        {

            byte[] result = null;
            try
            {
                List<V_EmpLeaveRdInfo> entlist = new List<V_EmpLeaveRdInfo>();
                IQueryable<V_EmpLeaveRdInfo> leaveRecordInfos = EmployeeLeaveRecordPaged(pageIndex, pageSize, sort, filterString, paras, ref  pageCount, strCheckState, strOwnerID, startDate, recorderDate,employeeID,leaveTypeSetID);
                if (leaveRecordInfos.Count() > 0)
                {
                    entlist = leaveRecordInfos.ToList();
                }
                result = EmployeeLeaveRecordStream(entlist);
            }
            catch (Exception ex)
            {
                SMT.Foundation.Log.Tracer.Debug("ExportEmployeeLeftOfficeConfirmReports:" + ex.Message);

            }
            return result;


        }

        public static byte[] EmployeeLeaveRecordStream(List<V_EmpLeaveRdInfo> leaveRecordInfos)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Utility.GetHeader().ToString());
            sb.Append(GetEmployeeLeaveRecordBody(leaveRecordInfos).ToString());
            byte[] by = Encoding.UTF8.GetBytes(sb.ToString());

            return by;
        }

        public static StringBuilder GetEmployeeLeaveRecordBody(List<V_EmpLeaveRdInfo> Collects)
        {
            StringBuilder s = new StringBuilder();
            var tmp = new SaaS.BLLCommonServices.PermissionWS.PermissionServiceClient().GetSysDictionaryByCategoryList(new string[] { "CHECKSTATE" });
            string checkStateName = string.Empty;
            s.Append("<body>\n\r");
            s.Append("<table border=1 cellspacing=0 CELLPADDING=3 width=100% align=center>");
            s.Append("<tr>");
            s.Append("<td align=center class=\"title\" >员工编号</td>");
            s.Append("<td align=center class=\"title\" >员工姓名</td>");
            s.Append("<td align=center class=\"title\" >部门</td>");
            s.Append("<td align=center class=\"title\" >假期标准</td>");
            s.Append("<td align=center class=\"title\" >开始时间</td>");
            s.Append("<td align=center class=\"title\" >结束时间</td>");
            s.Append("<td align=center class=\"title\" >请假天数</td>");
            s.Append("<td align=center class=\"title\" >请假时长(小时)</td>");
            s.Append("<td align=center class=\"title\" >审批状态</td>");
            s.Append("<td align=center class=\"title\" >原因</td>");
            s.Append("<td align=center class=\"title\" >销假合计时长(小时)</td>");
            s.Append("</tr>");

            if (Collects.Count() > 0)
            {
                for (int i = 0; i < Collects.Count; i++)
                {
                    try
                    {
                        checkStateName = tmp.Where(e => e.DICTIONARYVALUE == decimal.Parse(Collects[i].CHECKSTATE.Trim())).FirstOrDefault().DICTIONARYNAME;
                    }
                    catch (Exception ex)
                    {
                        SMT.Foundation.Log.Tracer.Debug("GetEmployeeLeaveRecordBody:" + ex.Message);
                    }
                    s.Append("<tr>");
                    s.Append("<td class=\"x1282\">" + Collects[i].EMPLOYEECODE + "</td>");
                    s.Append("<td class=\"x1282\">" + Collects[i].EMPLOYEENAME + "</td>");
                    s.Append("<td class=\"x1282\">" + Collects[i].DEPARTMENTNAME + "</td>");
                    s.Append("<td class=\"x1282\">" + Collects[i].LEAVETYPENAME + "</td>");//应出勤天数，注释的为原来的
                    s.Append("<td class=\"x1282\">" + Collects[i].STARTDATETIME.Value.ToString("yyyy-MM-dd HH:mm") + "</td>");
                    s.Append("<td class=\"x1282\">" + Collects[i].ENDDATETIME.Value.ToString("yyyy-MM-dd HH:mm") + "</td>");
                    s.Append("<td class=\"x1282\">" + Collects[i].LEAVEDAYS + "</td>");
                    s.Append("<td class=\"x1282\">" + Collects[i].LEAVEHOURS + "</td>");
                    s.Append("<td class=\"x1282\">" + checkStateName + "</td>");
                    s.Append("<td class=\"x1282\">" + Collects[i].REASON + "</td>");
                    s.Append("<td class=\"x1282\">" + Collects[i].CANCELTOTALHOURS + "</td>");
                    s.Append("</tr>");
                }
            }
            s.Append("</table>");
            s.Append("</body></html>");
            return s;
        }



        #endregion
        /// <summary>
        /// 根据员工ID，请假的起止时间查询请假记录
        /// </summary>
        /// <param name="strEmployeeID"></param>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public IQueryable<T_HR_EMPLOYEELEAVERECORD> GetEmployeeLeaveRdListByEmployeeIDAndDate(string strEmployeeID, DateTime dtStart, DateTime dtEnd, string strCheckState)
        {
            if (string.IsNullOrEmpty(strEmployeeID))
            {
                return null;
            }

            StringBuilder strTemps = new StringBuilder();

            IQueryable<T_HR_EMPLOYEELEAVERECORD> ents = from e in dal.GetObjects().Include("T_HR_LEAVETYPESET")
                                                           where e.EMPLOYEEID == strEmployeeID                                                            
                                                           && e.CHECKSTATE == strCheckState
                                                           && (
                                                           (e.STARTDATETIME <= dtStart && e.ENDDATETIME >= dtStart)
                                                           || (e.STARTDATETIME <= dtEnd && e.ENDDATETIME >= dtEnd)
                                                           || (e.STARTDATETIME >= dtStart && e.ENDDATETIME <= dtEnd)
                                                           )
                                                           select e;

            if (ents.Count() > 0)
            {
                return ents;
            }
            return null;
            //IQueryable<T_HR_EMPLOYEELEAVERECORD> qEnds = from e in dal.GetObjects().Include("T_HR_LEAVETYPESET")
            //                                             where e.EMPLOYEEID == strEmployeeID && e.ENDDATETIME > dtStart && e.ENDDATETIME <= dtEnd && e.CHECKSTATE == strCheckState
            //                                             select e;

            //if (qEnds.Count() > 0)
            //{
            //    foreach (T_HR_EMPLOYEELEAVERECORD item in qEnds)
            //    {
            //        if (strTemps.ToString().Contains(item.LEAVERECORDID))
            //        {
            //            continue;
            //        }
            //        strTemps.Append(item.LEAVERECORDID + ",");
            //    }
            //}

            //IQueryable<T_HR_EMPLOYEELEAVERECORD> qCrosss = from e in dal.GetObjects().Include("T_HR_LEAVETYPESET")
            //                                               where e.EMPLOYEEID == strEmployeeID && e.STARTDATETIME < dtStart && e.ENDDATETIME > dtEnd && e.CHECKSTATE == strCheckState
            //                                               select e;

            //if (qCrosss.Count() > 0)
            //{
            //    foreach (T_HR_EMPLOYEELEAVERECORD item in qCrosss)
            //    {
            //        if (strTemps.ToString().Contains(item.LEAVERECORDID))
            //        {
            //            continue;
            //        }
            //        strTemps.Append(item.LEAVERECORDID + ",");
            //    }
            //}

            //IQueryable<T_HR_EMPLOYEELEAVERECORD> qCrosssSpec = from e in dal.GetObjects().Include("T_HR_LEAVETYPESET")
            //                                               where e.EMPLOYEEID == strEmployeeID && e.STARTDATETIME < dtEnd && e.ENDDATETIME > dtEnd && e.CHECKSTATE == strCheckState
            //                                               select e;

            //if (qCrosssSpec.Count() > 0)
            //{
            //    foreach (T_HR_EMPLOYEELEAVERECORD item in qCrosssSpec)
            //    {
            //        if (strTemps.ToString().Contains(item.LEAVERECORDID))
            //        {
            //            continue;
            //        }
            //        strTemps.Append(item.LEAVERECORDID + ",");
            //    }
            //}

            //string strLeaveIds = strTemps.ToString();

            //IQueryable<T_HR_EMPLOYEELEAVERECORD> ents = from e in dal.GetObjects().Include("T_HR_LEAVETYPESET")
            //                                            where e.LEAVERECORDID.Contains(strLeaveIds)
            //                                            select e;

        }

        /// <summary>
        /// 根据请假记录ID获取员工请假信息
        /// </summary>
        /// <param name="strID"></param>
        /// <returns></returns>
        public T_HR_EMPLOYEELEAVERECORD GetLeaveRecordByID(string strID)
        {
            if (string.IsNullOrWhiteSpace(strID))
            {
                return null;
            }

            IQueryable<T_HR_EMPLOYEELEAVERECORD> ents = from e in dal.GetObjects().Include("T_HR_LEAVETYPESET")
                                                        where e.LEAVERECORDID == strID
                                                        select e;

            if (ents.Count() == 0)
            {
                return null;
            }

            return ents.FirstOrDefault();
        }

        /// <summary>
        /// 根据请假记录ID、员工ID获取信息
        /// </summary>
        /// <param name="strID"></param>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public V_EMPLOYEELEAVERECORD GetEmployeeLeaveRecordByID(string strID)
        {
            V_EMPLOYEELEAVERECORD entity = new V_EMPLOYEELEAVERECORD();
            try
            {

                AttendanceSolutionAsignBLL asignBll = new AttendanceSolutionAsignBLL();
                FreeLeaveDaySetBLL bll = new FreeLeaveDaySetBLL();
                //根据请假记录ID获取请假记录信息
                entity.EmployeeLeaveRecord = dal.GetObjects().Include("T_HR_LEAVETYPESET").FirstOrDefault(s => s.LEAVERECORDID == strID);

                //根据请假记录ID获取请假调休记录信息
                var ent = from a in dal.GetObjects<T_HR_ADJUSTLEAVE>()
                          where a.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID == strID
                          select a;
                entity.AdjustLeave = ent.Count() > 0 ? ent.ToList() : null;
                if (entity.EmployeeLeaveRecord != null)
                {
                    entity.EmployeeLeave = bll.GetFreeLeaveDaySetByEmployeeID(entity.EmployeeLeaveRecord.EMPLOYEEID);
                    //每天工作时长
                    var temp = asignBll.GetAttendanceSolutionAsignByEmployeeID(entity.EmployeeLeaveRecord.EMPLOYEEID);
                    if (temp != null)
                    {
                        entity.WorkTimePerDay = temp.T_HR_ATTENDANCESOLUTION.WORKTIMEPERDAY.Value;
                    }
                    else
                    {
                        entity.WorkTimePerDay = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Tracer.Debug("GetEmployeeLeaveRecordByID异常："+ex.ToString());
            }
            return entity;
        }

        /// <summary>
        /// 获取员工同类型假本年已用天数，本月已用天数，冲减带薪假已用天数
        /// </summary>
        /// <param name="entEmployeeView">员工信息(主岗位)</param>
        /// <param name="strLeaveRecordId">当前请假记录ID</param>
        /// <param name="strLeaveTypeSetId">当前请假对应的请假标准ID</param>
        /// <param name="dtLeaveStartTime">请假起始时间</param>
        /// <param name="dtLeaveEndTime">请假结束时间</param>
        /// <param name="dLeaveYearDays">同类型假本年已用天数</param>
        /// <param name="dLeaveMonthDays">本月已用天数</param>
        /// <param name="dAdjLevPaidDays">冲减带薪假已用天数</param>
        public void GetLeaveDaysHistory(V_EMPLOYEEVIEW entEmployeeView, string strLeaveRecordId, string strLeaveTypeSetId, DateTime dtLeaveStartTime,
            DateTime dtLeaveEndTime, ref decimal dLeaveYearDays, ref decimal dLeaveMonthDays, ref decimal dAdjLevPaidDays)
        {
            if (entEmployeeView == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(strLeaveTypeSetId) || string.IsNullOrWhiteSpace(strLeaveRecordId) || string.IsNullOrWhiteSpace(entEmployeeView.EMPLOYEEID) || string.IsNullOrWhiteSpace(entEmployeeView.OWNERCOMPANYID))
            {
                return;
            }

            LeaveTypeSetBLL bllLeaveTypeSet = new LeaveTypeSetBLL();
            T_HR_LEAVETYPESET entLeaveTypeSet = bllLeaveTypeSet.GetLeaveTypeSetByID(strLeaveTypeSetId);

            if (entLeaveTypeSet == null)
            {
                return;
            }

            AttendanceSolutionAsignBLL bllAttendSolAsign = new AttendanceSolutionAsignBLL();
            T_HR_ATTENDANCESOLUTIONASIGN entAttendSolAsign = bllAttendSolAsign.GetAttendanceSolutionAsignByEmployeeIDAndDate(entEmployeeView.EMPLOYEEID, dtLeaveStartTime);
            if (entAttendSolAsign == null)
            {
                //当前员工没有分配考勤方案，无法提交请假申请
                return;
            }

            //获取考勤方案
            T_HR_ATTENDANCESOLUTION entAttendSol = entAttendSolAsign.T_HR_ATTENDANCESOLUTION;

            DateTime dtYearStart = new DateTime();
            DateTime dtMonthStart = new DateTime();
            string strCheckState = Convert.ToInt32(Common.CheckStates.Approved).ToString();
            DateTime.TryParse(dtLeaveStartTime.Year.ToString() + "-1-1", out dtYearStart);
            DateTime.TryParse(dtLeaveStartTime.ToString("yyyy-MM") + "-1", out dtMonthStart);

            var ey = from e in dal.GetObjects().Include("T_HR_LEAVETYPESET")
                     where e.LEAVERECORDID != strLeaveRecordId && e.T_HR_LEAVETYPESET.LEAVETYPESETID == strLeaveTypeSetId && e.OWNERCOMPANYID == entEmployeeView.OWNERCOMPANYID && e.EMPLOYEEID == entEmployeeView.EMPLOYEEID
                     && e.STARTDATETIME >= dtYearStart && e.ENDDATETIME <= dtLeaveEndTime && e.CHECKSTATE == strCheckState
                     select e;

            var em = from e in dal.GetObjects().Include("T_HR_LEAVETYPESET")
                     where e.LEAVERECORDID != strLeaveRecordId && e.T_HR_LEAVETYPESET.LEAVETYPESETID == strLeaveTypeSetId && e.OWNERCOMPANYID == entEmployeeView.OWNERCOMPANYID && e.EMPLOYEEID == entEmployeeView.EMPLOYEEID
                     && e.STARTDATETIME >= dtMonthStart && e.ENDDATETIME <= dtLeaveEndTime && e.CHECKSTATE == strCheckState
                     select e;

            var ec = from c in dal.GetObjects<T_HR_EMPLOYEECANCELLEAVE>().Include("T_HR_EMPLOYEELEAVERECORD")
                     join l in em on c.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID equals l.LEAVERECORDID
                     select c;

            if (ey.Count() == 0)
            {
                return;
            }

            decimal dCancelLeaveDays = 0;
            foreach (T_HR_EMPLOYEECANCELLEAVE item in ec)
            {
                dCancelLeaveDays += item.TOTALHOURS.Value;
            }

            foreach (T_HR_EMPLOYEELEAVERECORD item in ey)
            {
                if (item.TOTALHOURS != null)
                {
                    dLeaveYearDays += item.TOTALHOURS.Value;
                }
            }

            dLeaveYearDays = RoundOff((dLeaveYearDays - dCancelLeaveDays) / entAttendSol.WORKTIMEPERDAY.Value, "0.5", 1);

            if (entLeaveTypeSet.FINETYPE == (Convert.ToInt32(Common.LeaveFineType.Free) + 1).ToString())
            {
                dAdjLevPaidDays = dLeaveYearDays;
            }

            if (em.Count() == 0)
            {
                return;
            }

            foreach (T_HR_EMPLOYEELEAVERECORD item in ey)
            {
                dLeaveMonthDays += item.TOTALHOURS.Value;
            }

            dLeaveMonthDays = RoundOff((dLeaveMonthDays - dCancelLeaveDays) / entAttendSol.WORKTIMEPERDAY.Value, "0.5", 1);

            if (entLeaveTypeSet.FINETYPE == (Convert.ToInt32(Common.LeaveFineType.Free) + 1).ToString() || entLeaveTypeSet.FINETYPE == (Convert.ToInt32(Common.LeaveFineType.Deduct) + 1).ToString())
            {
                return;
            }

            var ep = from ad in dal.GetObjects<T_HR_ADJUSTLEAVE>().Include("T_HR_EMPLOYEELEAVERECORD")
                     join l in em on ad.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID equals l.LEAVERECORDID
                     select ad;

            foreach (T_HR_ADJUSTLEAVE item in ep)
            {
                if (item.ADJUSTLEAVEDAYS != null)
                {
                    dAdjLevPaidDays += item.ADJUSTLEAVEDAYS.Value;
                }
            }
        }

        /// <summary>
        /// 获取同类假期年度总次数，年度总时长，月度总次数，月度总次数，月度总时长
        /// </summary>
        /// <param name="strLeaveTypeSetId">假期标准Id</param>
        /// <param name="strLeaveRecordId">请假记录Id</param>
        /// <param name="strEmployeeID">员工Id</param>
        /// <param name="dtLeaveStartTime">请假起始时间</param>
        /// <param name="dtLeaveEndTime">请假截止时间</param>
        /// <param name="dLeaveYearTimes">同类假期年度总次数</param>
        /// <param name="dLeaveYearDays">同类假期年度总时长</param>
        /// <param name="dLeaveMonthTimes">同类假期月度总次数</param>
        /// <param name="dLeaveMonthDays">同类假期月度总时长</param>
        public void GetLeaveDaysHistory(string strLeaveTypeSetId, string strLeaveRecordId, string strEmployeeID,
            DateTime dtLeaveStartTime, DateTime dtLeaveEndTime, ref decimal dLeaveYearTimes,
            ref decimal dLeaveYearDays, ref decimal dLeaveMonthTimes, ref decimal dLeaveMonthDays, ref DateTime dLeaveFistDate,ref decimal dLeaveSYearTimes)
        {
            if (string.IsNullOrWhiteSpace(strLeaveTypeSetId) || string.IsNullOrWhiteSpace(strLeaveRecordId) || string.IsNullOrWhiteSpace(strEmployeeID))
            {
                return;
            }

            AttendanceSolutionAsignBLL bllAttendSolAsign = new AttendanceSolutionAsignBLL();
            T_HR_ATTENDANCESOLUTIONASIGN entAttendSolAsign = bllAttendSolAsign.GetAttendanceSolutionAsignByEmployeeIDAndDate(strEmployeeID, dtLeaveStartTime);
            if (entAttendSolAsign == null)
            {
                //当前员工没有分配考勤方案，无法提交请假申请
                return;
            }

            //获取考勤方案
            T_HR_ATTENDANCESOLUTION entAttendSol = entAttendSolAsign.T_HR_ATTENDANCESOLUTION;

            DateTime dtYearStart = new DateTime();
            DateTime dtMonthStart = new DateTime();
            string strCheckState = Convert.ToInt32(Common.CheckStates.Approved).ToString();
            DateTime.TryParse(dtLeaveStartTime.Year.ToString() + "-1-1", out dtYearStart);
            DateTime.TryParse(dtLeaveStartTime.ToString("yyyy-MM") + "-1", out dtMonthStart);

            //查询上一年请假
            DateTime dtSYearStart = new DateTime();
            DateTime.TryParse((dtLeaveStartTime.Year-1).ToString() + "-1-1", out dtSYearStart);
            var eu = from e in dal.GetObjects().Include("T_HR_LEAVETYPESET")
                     where e.LEAVERECORDID != strLeaveRecordId 
                     && e.T_HR_LEAVETYPESET.LEAVETYPESETID == strLeaveTypeSetId && e.EMPLOYEEID == strEmployeeID
                     && e.STARTDATETIME >= dtSYearStart && e.ENDDATETIME <= dtLeaveEndTime && e.CHECKSTATE == strCheckState
                     select e;
            dLeaveSYearTimes = eu.Count();
            //取得本年时间段内此请假类型，第一次的请假时间是什么时候
            if (eu.Count()!=0)
            {
                var ep = (from e in eu
                          orderby e.CREATEDATE ascending
                          select e).FirstOrDefault().STARTDATETIME;
                if (ep != null)
                {
                    dLeaveFistDate = DateTime.Parse(ep.ToString());
                }
            }
         
            var ey = from e in dal.GetObjects().Include("T_HR_LEAVETYPESET")
                     where e.LEAVERECORDID != strLeaveRecordId && e.T_HR_LEAVETYPESET.LEAVETYPESETID == strLeaveTypeSetId && e.EMPLOYEEID == strEmployeeID
                     && e.STARTDATETIME >= dtYearStart && e.ENDDATETIME <= dtLeaveEndTime && e.CHECKSTATE == strCheckState
                     select e;

            var em = from e in dal.GetObjects().Include("T_HR_LEAVETYPESET")
                     where e.LEAVERECORDID != strLeaveRecordId && e.T_HR_LEAVETYPESET.LEAVETYPESETID == strLeaveTypeSetId && e.EMPLOYEEID == strEmployeeID
                     && e.STARTDATETIME >= dtMonthStart && e.ENDDATETIME <= dtLeaveEndTime && e.CHECKSTATE == strCheckState
                     select e;

            if (ey.Count() == 0)
            {
                return;
            }
            
            //本年请假次数
            dLeaveYearTimes = ey.Count();
            foreach (T_HR_EMPLOYEELEAVERECORD item in ey)
            {
                if (item.TOTALHOURS != null)
                {
                    dLeaveYearDays += item.TOTALHOURS.Value;
                }
            }

            dLeaveYearDays = RoundOff(dLeaveYearDays / entAttendSol.WORKTIMEPERDAY.Value, "0.5", 1);

            if (em.Count() == 0)
            {
                return;
            }

            dLeaveMonthTimes = em.Count();
            foreach (T_HR_EMPLOYEELEAVERECORD item in em)
            {
                dLeaveMonthDays += item.TOTALHOURS.Value;
            }

            dLeaveMonthDays = RoundOff(dLeaveMonthDays / entAttendSol.WORKTIMEPERDAY.Value, "0.5", 1);
        }

        private void CalculateEmployeeLevelDayCount(string fingerprintids)
        {
            string[] arr = fingerprintids.Split(',');
            foreach (string s in arr)
            {
                if (s.Length > 0)
                {
                    var employee = from e in dal.GetObjects<T_HR_EMPLOYEE>()
                                             where e.FINGERPRINTID == s.Trim()
                                             select e;

                    if (employee == null)
                    {
                        return;
                    }
                    else
                    {
                        AttendanceSolutionAsignBLL bllAttSolAsign = new AttendanceSolutionAsignBLL();
                        T_HR_ATTENDANCESOLUTIONASIGN entAttSolAsign = bllAttSolAsign.GetAttendanceSolutionAsignByID("D65F9765-14BB-4712-A646-8E28125794DD");

                        EmployeeLevelDayCountBLL bllLevelDayCount = new EmployeeLevelDayCountBLL();
                        bllLevelDayCount.CalculateEmployeeLevelDayCount(entAttSolAsign, employee.FirstOrDefault(), "0"); 
                    }
                }
            }
        }

        /// <summary>
        /// 获取指定员工的实际请假天数(实际请假天数=请假天数-公休假天数-每周休息天数+休假调剂工作天数)，实际请假时长(按小时计，实际请假时长=非整天请假时长-当日作息间隙休息时间)
        /// </summary>
        /// <param name="strEmployeeID">员工ID</param>
        /// <param name="dtLeaveStartTime">请假起始时间</param>
        /// <param name="dtLeaveEndTime">请假截止时间</param>
        /// <param name="dLeaveDay">实际请假天数</param>
        /// <param name="dLeaveTime">实际请假时长</param>
        /// <param name="dLeaveTotalTime">实际请假时长</param>
        public string GetRealLeaveDayByEmployeeIdAndDate(string strLeaveRecordId, string strEmployeeID, DateTime dtLeaveStartTime,
            DateTime dtLeaveEndTime, ref decimal dLeaveDay, ref decimal dLeaveTime, ref decimal dLeaveTotalTime)
        {
            string strMsg = string.Empty;
            try
            {
                string[] strArr = strLeaveRecordId.Split('|');
                string LeaveTypeValue = string.Empty;//
                if (strArr.Count() >= 2)
                {
                    string strLeaveSetId = strArr[1];
                    strLeaveRecordId = strArr[0];
                    LeaveTypeSetBLL bllLeaveTypeSet = new LeaveTypeSetBLL();
                    T_HR_LEAVETYPESET entLeaveTypeSet = bllLeaveTypeSet.GetLeaveTypeSetByID(strLeaveSetId);
                    if (entLeaveTypeSet != null)
                    {
                        LeaveTypeValue = entLeaveTypeSet.LEAVETYPEVALUE;
                    }
                }

                #region 有数据情况则直接返回
                T_HR_EMPLOYEELEAVERECORD entLeaveRecord = GetLeaveRecordByID(strLeaveRecordId);
                bool flag = false; 
                //考勤方案生成带薪假测试结束
                if (entLeaveRecord != null)
                {
                    if (entLeaveRecord.STARTDATETIME == dtLeaveStartTime && entLeaveRecord.ENDDATETIME == dtLeaveEndTime)
                    {
                        if (entLeaveRecord.LEAVEDAYS == null)
                        {
                            dLeaveDay = 0;
                        }
                        else
                        {
                            dLeaveDay = entLeaveRecord.LEAVEDAYS.Value;
                        }

                        if (entLeaveRecord.LEAVEHOURS == null)
                        {
                            dLeaveTime = 0;
                        }
                        else
                        {
                            dLeaveTime = entLeaveRecord.LEAVEHOURS.Value;
                        }

                        if (entLeaveRecord.TOTALHOURS == null)
                        {
                            dLeaveTotalTime = 0;
                        }
                        else
                        {
                            dLeaveTotalTime = entLeaveRecord.TOTALHOURS.Value;
                        }

                        flag = true;
                    }
                }

                if (flag)
                {
                    return strMsg;
                }
                #endregion
                DateTime dtStart, dtEnd = new DateTime();
                decimal dTotalLeaveDay = 0;                         //起止时间的时长

                DateTime.TryParse(dtLeaveStartTime.ToString("yyyy-MM-dd"), out dtStart);        //获取请假起始日期
                DateTime.TryParse(dtLeaveEndTime.ToString("yyyy-MM-dd"), out dtEnd);            //获取请假截止日期

                AttendanceSolutionAsignBLL bllAttendSolAsign = new AttendanceSolutionAsignBLL();
                T_HR_ATTENDANCESOLUTIONASIGN entAttendSolAsign = bllAttendSolAsign.GetAttendanceSolutionAsignByEmployeeIDAndDate(strEmployeeID, dtStart);
                if (entAttendSolAsign == null)
                {
                    //当前员工没有分配考勤方案，无法提交请假申请
                    return "{NONEXISTASIGNEDATTENSOL}";
                }

                //获取考勤方案
                T_HR_ATTENDANCESOLUTION entAttendSol = entAttendSolAsign.T_HR_ATTENDANCESOLUTION;
                decimal dWorkTimePerDay = entAttendSol.WORKTIMEPERDAY.Value;
                decimal dWorkMode = entAttendSol.WORKMODE.Value;
                int iWorkMode = 0;
                int.TryParse(dWorkMode.ToString(), out iWorkMode);//获取工作制(工作天数/周)

                List<int> iWorkDays = new List<int>();
                Utility.GetWorkDays(iWorkMode, ref iWorkDays);//获取每周上班天数

                OutPlanDaysBLL bllOutPlanDays = new OutPlanDaysBLL();
                IQueryable<T_HR_OUTPLANDAYS> entOutPlanDays = bllOutPlanDays.GetOutPlanDaysRdListByEmployeeID(strEmployeeID);

                string strVacDayType = (Convert.ToInt32(Common.OutPlanDaysType.Vacation) + 1).ToString();
                string strWorkDayType = (Convert.ToInt32(Common.OutPlanDaysType.WorkDay) + 1).ToString();

                //节假日
                IQueryable<T_HR_OUTPLANDAYS> entVacDays = entOutPlanDays.Where(s => s.DAYTYPE == strVacDayType);
               
                //取出请假起始日年份和结束日年份的休假调剂工作天数
                DateTime startDate = Convert.ToDateTime(dtStart.Year + "-1-1");
                DateTime endDate = Convert.ToDateTime(dtEnd.Year + "-12-31");
                IQueryable<T_HR_OUTPLANDAYS> entWorkDays = entOutPlanDays.Where(s => s.DAYTYPE == strWorkDayType && s.STARTDATE >= startDate && s.ENDDATE <= endDate);

                if (LeaveTypeValue == "5" || LeaveTypeValue == "6")//5为产假，6为婚假，为产假和婚假时，是算自然日，不算上节假日
                {
                    entVacDays = entVacDays.Where(s => s.DAYTYPE == "0");
                    entWorkDays = entWorkDays.Where(s => s.DAYTYPE == "0");
                }

                SchedulingTemplateDetailBLL bllTemplateDetail = new SchedulingTemplateDetailBLL();
                IQueryable<T_HR_SCHEDULINGTEMPLATEDETAIL> entTemplateDetails = bllTemplateDetail.GetTemplateDetailRdListByAttendanceSolutionId(entAttendSol.ATTENDANCESOLUTIONID);
                T_HR_SCHEDULINGTEMPLATEMASTER entTemplateMaster = entTemplateDetails.FirstOrDefault().T_HR_SCHEDULINGTEMPLATEMASTER;

                TimeSpan ts = dtEnd.Subtract(dtStart);

                decimal dVacDay = 0, dWorkDay = 0;
                decimal dLeaveFirstDayTime = 0, dLeaveLastDayTime = 0, dLeaveFirstLastTime = 0;//请假第一天的时长，请假最后一天的时长，请假首尾两天合计时长
                if (dtLeaveStartTime != dtLeaveEndTime)
                {
                    CalculateNonWholeDayLeaveTime(dtLeaveStartTime, dtStart, entTemplateMaster, entTemplateDetails, entVacDays, entWorkDays, iWorkDays, "S", ref dLeaveFirstDayTime);
                    CalculateNonWholeDayLeaveTime(dtLeaveEndTime, dtEnd, entTemplateMaster, entTemplateDetails, entVacDays, entWorkDays, iWorkDays, "E", ref dLeaveLastDayTime);

                    dLeaveFirstLastTime = dLeaveFirstDayTime + dLeaveLastDayTime;

                    if (dtStart == dtEnd)
                    {
                        dLeaveFirstLastTime = dLeaveFirstLastTime - dWorkTimePerDay * 60;
                    }
                }
                else
                {
                    dLeaveFirstLastTime = dWorkTimePerDay * 60;
                }


                dTotalLeaveDay = ts.Days;
                if (ts.Days > 0)
                {
                    //取得总的请假天数(此天数扣除了首尾两天的时间,根据请假的情况，可能包含了公休假及周假天数,扣除首尾两天的计算只适合请三天以上的)
                    int iDays = ts.Days - 1;
                    dTotalLeaveDay = iDays;

                    for (int i = 0; i < iDays; i++)
                    {
                        int j = i + 1;
                        bool isVacDay = false;
                        DateTime dtCurDate = dtStart.AddDays(j);
                        
                        if (entVacDays.Count() > 0)
                        {             
                            //遍历节假日集合，取得节假日总天数
                            foreach (T_HR_OUTPLANDAYS item_Vac in entVacDays)
                            {
                                if (item_Vac.STARTDATE.Value <= dtCurDate && item_Vac.ENDDATE >= dtCurDate)
                                {
                                    isVacDay = true;
                                    break;
                                }
                            }
                        }

                        //如果是节假日,普通节假日总天数加1
                        if (isVacDay)
                        {
                            dVacDay += 1;
                        }
                        else//否则是周末，普通节假日总天数加1
                        {
                            if (iWorkDays.Contains(Convert.ToInt32(dtCurDate.DayOfWeek)) == false)
                            {
                                dVacDay += 1;
                            }
                        }

                        if (entWorkDays.Count() > 0)
                        {
                            //遍历休假调剂工作天数的集合，取得休假调剂工作天数
                            foreach (T_HR_OUTPLANDAYS item_Work in entWorkDays)
                            {
                                if (item_Work.STARTDATE.Value <= dtCurDate && item_Work.ENDDATE >= dtCurDate)
                                {
                                    dWorkDay += 1;
                                    break;
                                }                               
                            }
                        }
                    }
                }

                if (LeaveTypeValue == "5" || LeaveTypeValue == "6")//5为产假，6为婚假，为产假和婚假时，是算自然日，应该加上双休和节假日
                {
                    dLeaveDay = dTotalLeaveDay;
                }
                else
                {
                    dLeaveDay = dTotalLeaveDay - dVacDay + dWorkDay;    //请假天数 = 请假天数-首尾两天 - 总休假天数 + 休假调剂工作天数
                }
                decimal dTempTime = decimal.Round((dLeaveFirstLastTime) / 60, 1);
                if (dTempTime >= dWorkTimePerDay)
                {
                    decimal dTempDay = decimal.Round(dTempTime / dWorkTimePerDay, 2);
                    string[] strList = dTempDay.ToString().Split('.');
                    if (strList.Length == 2)
                    {
                        dLeaveDay += decimal.Parse(strList[0].ToString());
                        dLeaveTime = dTempTime - dWorkTimePerDay * decimal.Parse(strList[0].ToString());
                    }
                    else
                    {
                        dLeaveDay += dTempDay;
                    }
                }
                else if (dTempTime < dWorkTimePerDay)
                {
                    dLeaveTime = dTempTime;
                }

                dLeaveTotalTime = dLeaveDay * dWorkTimePerDay + dLeaveTime;

            }
            catch (Exception ex)
            {
                strMsg = ex.Message;
                Utility.SaveLog(ex.ToString());
            }

            return strMsg;
        }
        #endregion

        #region 操作
        ///// <summary>
        ///// 添加请假记录和请假调休记录
        ///// </summary>
        ///// <param name="LeaveRecord">请假记录实体</param>
        ///// <param name="AdjustLeave">请假调休记录实体</param>
        //public void EmployeeLeaveRecordAdd(T_HR_EMPLOYEELEAVERECORD LeaveRecord, List<V_ADJUSTLEAVE> AdjustLeaves)
        //{
        //    try
        //    {
        //        //添加请假记录
        //        T_HR_EMPLOYEELEAVERECORD ent = new T_HR_EMPLOYEELEAVERECORD();
        //        Utility.CloneEntity(LeaveRecord, ent);
        //        ent.T_HR_LEAVETYPESETReference.EntityKey =
        //            new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_LEAVETYPESET", "LEAVETYPESETID", LeaveRecord.T_HR_LEAVETYPESET.LEAVETYPESETID);

        //        //添加请假调休记录
        //        if (AdjustLeaves != null)
        //        {
        //            foreach (V_ADJUSTLEAVE item in AdjustLeaves)
        //            {
        //                T_HR_ADJUSTLEAVE entity = new T_HR_ADJUSTLEAVE();
        //                Utility.CloneEntity(item.T_HR_ADJUSTLEAVE, entity);
        //                //entity.T_HR_EMPLOYEELEAVERECORDReference.EntityKey =
        //                //    new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_EMPLOYEELEAVERECORD", "LEAVERECORDID", temp.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID);
        //                //DataContext.AddObject("T_HR_ADJUSTLEAVE", entity);
        //                ent.T_HR_ADJUSTLEAVE = new System.Data.Objects.DataClasses.EntityCollection<T_HR_ADJUSTLEAVE>();
        //                ent.T_HR_ADJUSTLEAVE.Add(entity);
        //            }
        //        }

        //        base.Add(ent);
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.SaveLog(ex.ToString());
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 添加请假记录和请假调休记录
        /// </summary>
        /// <param name="LeaveRecord">请假记录实体</param>
        /// <param name="AdjustLeave">请假调休记录实体</param>
        public string EmployeeLeaveRecordAdd(T_HR_EMPLOYEELEAVERECORD LeaveRecord, List<V_ADJUSTLEAVE> AdjustLeaves)
        {
            string strReturn = string.Empty;
            try
            {
                
                //添加请假记录
                T_HR_EMPLOYEELEAVERECORD ent = new T_HR_EMPLOYEELEAVERECORD();
                Utility.CloneEntity(LeaveRecord, ent);
                if (ent.STARTDATETIME < DateTime.Now)
                {
                    strReturn = "请假开始时间不能小于当前时间";
                    return strReturn;
                }
                #region 五四青年节
                try
                {
                    string strBirthDay = string.Empty;
                    EmployeeBLL employee = new EmployeeBLL();
                    T_HR_EMPLOYEE employeeInfo = employee.GetEmployeeByID(LeaveRecord.EMPLOYEEID);
                    if (employeeInfo == null)
                    {
                        strReturn = "获取员工信息为空";
                        return strReturn;
                    }
                    if (employeeInfo.BIRTHDAY == null)
                    {
                        strReturn = "没有获取到生日信息";
                        return strReturn;
                    }
                    strBirthDay = employeeInfo.BIRTHDAY.ToString();
                    //五四青年节
                    if (LeaveRecord.T_HR_LEAVETYPESET.LEAVETYPEVALUE == "12")
                    {
                        if (string.IsNullOrEmpty(strBirthDay))
                        {
                            strReturn = "没有获取到生日信息";
                            return strReturn;
                        }
                        DateTime dtBirthday = new DateTime();
                        DateTime dtYouth = new DateTime();
                        DateTime.TryParse(strBirthDay, out dtBirthday);
                        DateTime.TryParse(DateTime.Now.Year.ToString() + "-05-04", out dtYouth);
                        if (dtBirthday.AddYears(28) < dtYouth)
                        {
                            strReturn = "已超过五四假的设置条件，不能保存此假";
                            return strReturn;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Tracer.Debug("修改请假记录出现错误：" + ex.ToString());
                }

                #endregion
                ent.T_HR_LEAVETYPESETReference.EntityKey =
                    new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_LEAVETYPESET", "LEAVETYPESETID", LeaveRecord.T_HR_LEAVETYPESET.LEAVETYPESETID);

                //添加请假调休记录
                if (AdjustLeaves != null)
                {
                    foreach (V_ADJUSTLEAVE item in AdjustLeaves)
                    {
                        T_HR_ADJUSTLEAVE entity = new T_HR_ADJUSTLEAVE();
                        Utility.CloneEntity(item.T_HR_ADJUSTLEAVE, entity);
                        //entity.T_HR_EMPLOYEELEAVERECORDReference.EntityKey =
                        //    new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_EMPLOYEELEAVERECORD", "LEAVERECORDID", temp.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID);
                        //DataContext.AddObject("T_HR_ADJUSTLEAVE", entity);
                        ent.T_HR_ADJUSTLEAVE = new System.Data.Objects.DataClasses.EntityCollection<T_HR_ADJUSTLEAVE>();
                        ent.T_HR_ADJUSTLEAVE.Add(entity);
                    }
                }

                bool blSave = base.Add(ent);
                if (!blSave)
                {
                    strReturn = "保存失败";
                }
            }
            catch (Exception ex)
            {
                Utility.SaveLog(ex.ToString());
                throw ex;
            }
            return strReturn;
        }
        ///// <summary>
        ///// 修改请假记录和请假调休记录
        ///// </summary>
        ///// <param name="LeaveRecord"></param>
        ///// <param name="AdjustLeave"></param>
        //public void EmployeeLeaveRecordUpdate(T_HR_EMPLOYEELEAVERECORD LeaveRecord, List<V_ADJUSTLEAVE> AdjustLeaves)
        //{
        //    try
        //    {
        //        //修改请假记录
        //        var ent = dal.GetObjects().FirstOrDefault(s => s.LEAVERECORDID == LeaveRecord.LEAVERECORDID);
        //        if (ent == null)
        //        {
        //            return;
        //        }


        //        Utility.CloneEntity(LeaveRecord, ent);
        //        ent.T_HR_LEAVETYPESETReference.EntityKey =
        //            new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_LEAVETYPESET", "LEAVETYPESETID", LeaveRecord.T_HR_LEAVETYPESET.LEAVETYPESETID);
        //        dal.UpdateFromContext(ent);
        //        dal.SaveContextChanges();
        //        SaveMyRecord(ent);

        //        if (AdjustLeaves != null)
        //        {
        //            foreach (var temp in AdjustLeaves)
        //            {
        //                var entity = dal.GetObjects<T_HR_ADJUSTLEAVE>().FirstOrDefault(s => s.ADJUSTLEAVEID == temp.T_HR_ADJUSTLEAVE.ADJUSTLEAVEID);
        //                //如果找到就修改,反之就添加
        //                if (entity != null)
        //                {
        //                    Utility.CloneEntity(temp.T_HR_ADJUSTLEAVE, entity);
        //                    if (entity.T_HR_EMPLOYEELEAVERECORD != null)
        //                    {
        //                        entity.T_HR_EMPLOYEELEAVERECORDReference.EntityKey =
        //                             new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_EMPLOYEELEAVERECORD", "LEAVERECORDID", temp.T_HR_ADJUSTLEAVE.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID);
        //                    }
        //                }
        //                else
        //                {
        //                    entity = new T_HR_ADJUSTLEAVE();
        //                    Utility.CloneEntity(temp.T_HR_ADJUSTLEAVE, entity);
        //                    entity.T_HR_EMPLOYEELEAVERECORDReference.EntityKey =
        //                        new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_EMPLOYEELEAVERECORD", "LEAVERECORDID", temp.T_HR_ADJUSTLEAVE.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID);
        //                    //DataContext.AddObject("T_HR_ADJUSTLEAVE", entity);
        //                    dal.AddToContext(entity);
        //                }
        //            }
        //        }
        //        dal.SaveContextChanges();
        //    }
        //    catch (Exception ex)
        //    {
        //        Utility.SaveLog(ex.ToString());
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// 修改请假记录和请假调休记录
        /// </summary>
        /// <param name="LeaveRecord"></param>
        /// <param name="AdjustLeave"></param>
        public string EmployeeLeaveRecordUpdate(T_HR_EMPLOYEELEAVERECORD LeaveRecord, List<V_ADJUSTLEAVE> AdjustLeaves)
        {
            string strReturn = string.Empty;
            try
            {
                //修改请假记录
                var ent = dal.GetObjects().FirstOrDefault(s => s.LEAVERECORDID == LeaveRecord.LEAVERECORDID);
                if (ent == null)
                {
                    strReturn = "请假记录不存在";
                    return strReturn;
                }
                #region 五四青年节
                try
                {
                    string strBirthDay = string.Empty;
                    EmployeeBLL employee = new EmployeeBLL();
                    T_HR_EMPLOYEE employeeInfo = employee.GetEmployeeByID(LeaveRecord.EMPLOYEEID);
                    if (employeeInfo == null)
                    {
                        strReturn = "获取员工信息为空";
                        return strReturn;
                    }
                    if (employeeInfo.BIRTHDAY == null)
                    {
                        strReturn = "没有获取到生日信息";
                        return strReturn;
                    }
                    strBirthDay = employeeInfo.BIRTHDAY.ToString();
                    //五四青年节
                    if (LeaveRecord.T_HR_LEAVETYPESET.LEAVETYPEVALUE == "12")
                    {
                        if (string.IsNullOrEmpty(strBirthDay))
                        {
                            strReturn = "没有获取到生日信息";
                            return strReturn;
                        }
                        DateTime dtBirthday = new DateTime();
                        DateTime dtYouth = new DateTime();
                        DateTime.TryParse(strBirthDay, out dtBirthday);
                        DateTime.TryParse(DateTime.Now.Year.ToString() + "-05-04", out dtYouth);
                        if (dtBirthday.AddYears(28) < dtYouth)
                        {
                            strReturn = "已超过五四假的设置条件，不能保存此假";
                            return strReturn;
                        }
                    }
                }
                catch(Exception ex)
                {
                    Tracer.Debug("修改请假记录出现错误："+ex.ToString());
                }

                #endregion
                if (LeaveRecord.STARTDATETIME < DateTime.Now)
                {
                    strReturn = "请假开始时间不能小于当前时间";
                    return strReturn;
                }
                Utility.CloneEntity(LeaveRecord, ent);
                
                ent.T_HR_LEAVETYPESETReference.EntityKey =
                    new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_LEAVETYPESET", "LEAVETYPESETID", LeaveRecord.T_HR_LEAVETYPESET.LEAVETYPESETID);
                dal.UpdateFromContext(ent);
                dal.SaveContextChanges();
                SaveMyRecord(ent);

                if (AdjustLeaves != null)
                {
                    foreach (var temp in AdjustLeaves)
                    {
                        var entity = dal.GetObjects<T_HR_ADJUSTLEAVE>().FirstOrDefault(s => s.ADJUSTLEAVEID == temp.T_HR_ADJUSTLEAVE.ADJUSTLEAVEID);
                        //如果找到就修改,反之就添加
                        if (entity != null)
                        {
                            Utility.CloneEntity(temp.T_HR_ADJUSTLEAVE, entity);
                            if (entity.T_HR_EMPLOYEELEAVERECORD != null)
                            {
                                entity.T_HR_EMPLOYEELEAVERECORDReference.EntityKey =
                                     new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_EMPLOYEELEAVERECORD", "LEAVERECORDID", temp.T_HR_ADJUSTLEAVE.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID);
                            }
                        }
                        else
                        {
                            entity = new T_HR_ADJUSTLEAVE();
                            Utility.CloneEntity(temp.T_HR_ADJUSTLEAVE, entity);
                            entity.T_HR_EMPLOYEELEAVERECORDReference.EntityKey =
                                new System.Data.EntityKey(qualifiedEntitySetName + "T_HR_EMPLOYEELEAVERECORD", "LEAVERECORDID", temp.T_HR_ADJUSTLEAVE.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID);
                            //DataContext.AddObject("T_HR_ADJUSTLEAVE", entity);
                            dal.AddToContext(entity);
                        }
                    }
                }
                dal.SaveContextChanges();
                
            }
            catch (Exception ex)
            {
                Utility.SaveLog(ex.ToString());
                throw ex;
            }
            return strReturn;
        }
        /// <summary>
        /// 删除请假记录组
        /// </summary>
        /// <param name="leaveRecordIDs">请假记录ID组</param>
        /// <returns>返回受影响的行数</returns>
        public int EmployeeLeaveRecordDelete(string[] leaveRecordIDs)
        {
            try
            {
                foreach (var id in leaveRecordIDs)
                {
                    //先删除请假调休记录,再删除请假记录
                    var entity = from a in dal.GetObjects<T_HR_ADJUSTLEAVE>().Include("T_HR_EMPLOYEELEAVERECORD")
                                 where a.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID == id
                                 select a;
                    if (entity.Count() > 0)
                    {
                        foreach (var temp in entity)
                        {
                            var tempEnt = dal.GetObjects<T_HR_ADJUSTLEAVE>().FirstOrDefault(s => s.ADJUSTLEAVEID == temp.ADJUSTLEAVEID);
                            //DataContext.DeleteObject(tempEnt);
                            dal.DeleteFromContext(tempEnt);
                        }
                    }

                    var ent = dal.GetObjects().FirstOrDefault(s => s.LEAVERECORDID == id);
                    if (ent != null)
                    {
                        dal.DeleteFromContext(ent);
                        DeleteMyRecord(ent);
                    }
                }
                return dal.SaveContextChanges();
            }
            catch (Exception ex)
            {
                Utility.SaveLog(ex.ToString());
                return 0;
            }
        }

        /// <summary>
        /// 审核请假申请
        /// </summary>
        /// <param name="strLeaveRecordID">请假申请主键ID</param>
        /// <param name="AdjustLeaves">低假记录</param>
        /// <param name="strCheckState">审核状态</param>
        /// <returns>返回处理消息</returns>
        public string AuditLeaveRecord(string strLeaveRecordID, List<V_ADJUSTLEAVE> AdjustLeaves, string strCheckState)
        {
            string strMsg = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(strLeaveRecordID) || string.IsNullOrEmpty(strCheckState))
                {
                    return "{NOTFOUND}";
                }

                //修改请假记录
                T_HR_EMPLOYEELEAVERECORD ent = dal.GetObjects().FirstOrDefault(s => s.LEAVERECORDID == strLeaveRecordID);
                if (ent == null)
                {
                    return "{NOTFOUND}";
                }

                //已审核通过的记录禁止再次提交审核
                //if (ent.CHECKSTATE == Convert.ToInt32(CheckStates.Approved).ToString())
                //{
                //    return "{REPEATAUDITERROR}";
                //}

                //审核状态变为审核通过时，生成对应的员工考勤记录(应用的员工范围，视应用对象而定)
                if (strCheckState == Convert.ToInt32(SMT.HRM.DAL.CheckStates.Approved).ToString())
                {
                    if (ent.STARTDATETIME == null || ent.ENDDATETIME == null)
                    {
                        return "{REQUIREDFIELDS}";
                    }

                    ModifyAdjustLeavesByAudit(AdjustLeaves);

                    DateTime dtCheck = new DateTime();
                    DateTime dtStart = new DateTime();
                    DateTime dtEnd = new DateTime();

                    DateTime.TryParse(ent.STARTDATETIME.Value.ToString("yyyy-MM-dd"), out dtStart);
                    DateTime.TryParse(ent.ENDDATETIME.Value.ToString("yyyy-MM-dd"), out dtEnd);

                    if (dtStart <= dtCheck || dtEnd <= dtCheck)
                    {
                        return "{REQUIREDFIELDS}";
                    }

                    #region  启动处理考勤异常的线程

                    string attState = (Convert.ToInt32(Common.AttendanceState.Leave) + 1).ToString();
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    d.Add("EMPLOYEEID", ent.EMPLOYEEID);
                    d.Add("STARTDATETIME", ent.STARTDATETIME.Value);
                    d.Add("ENDDATETIME", ent.ENDDATETIME.Value);
                    d.Add("ATTSTATE", attState);
                    Thread thread = new Thread(dealAttend);
                    thread.Start(d);

                    Tracer.Debug("请假启动消除异常的线程，出差开始时间:" + ent.STARTDATETIME.Value.ToString("yyyy-MM-dd HH:mm:ss")
                            + " 结束时间：" + ent.ENDDATETIME.Value.ToString("yyyy-MM-dd HH:mm:ss") + "员工id：" + ent.EMPLOYEEID);

                    #endregion
                }
                ent.CHECKSTATE = strCheckState;
                ent.UPDATEDATE = DateTime.Now;
                Update(ent);
                strMsg = "{SAVESUCCESSED}";
            }
            catch (Exception ex)
            {
                Utility.SaveLog(ex.ToString());
            }

            return strMsg;
        }

        private void dealAttend(object obj)
        {
            Dictionary<string, object> parameterDic = (Dictionary<string, object>)obj;
            string employeeid = parameterDic["EMPLOYEEID"].ToString();
            DateTime STARTDATETIME = (DateTime)parameterDic["STARTDATETIME"];
            DateTime ENDDATETIME = (DateTime)parameterDic["ENDDATETIME"];
            string attState = parameterDic["ATTSTATE"].ToString();

            using (AbnormRecordBLL bll = new AbnormRecordBLL())
            {
                //请假消除异常
                bll.DealEmployeeAbnormRecord(employeeid, STARTDATETIME, ENDDATETIME, attState);
            }
        }


        public void updateAllLeve()
        {
            DateTime dtStar=new DateTime(2013,4,1);
            DateTime dtend=new DateTime(2013,5,1);
            var q = from ent in dal.GetObjects<T_HR_EMPLOYEELEAVERECORD>()
                    where ent.STARTDATETIME >= dtStar
                    && ent.ENDDATETIME <= dtend
                    && ent.CHECKSTATE=="2"
                    select ent;
            if (q.Count() > 0)
            {
                foreach (var item in q.ToList())
                {
                    try
                    {
                        AuditLeaveRecord(item.LEAVERECORDID, null, "2");
                        SMT.Foundation.Log.Tracer.Debug(item.EMPLOYEENAME + item.STARTDATETIME + item.ENDDATETIME + " 成功");
                    }
                    catch (Exception ex)
                    {
                        SMT.Foundation.Log.Tracer.Debug(item.EMPLOYEENAME+item.STARTDATETIME+item.ENDDATETIME+ex.ToString());
                        continue;
                    }
                }
            }
        }

        public int UpdateCheckState(string strEntityName, string EntityKeyName, string EntityKeyValue, string CheckState)
        {
            try
            {
                int i = 0;
                string strMsg = string.Empty;
                strMsg = AuditLeaveRecord(EntityKeyValue, null, CheckState);
                if (strMsg == "{SAVESUCCESSED}")
                {
                    i = 1;
                }
                return i;
            }
            catch (Exception e)
            {
                SMT.Foundation.Log.Tracer.Debug("FormID:" + EntityKeyValue + " UpdateCheckState:" + e.Message);
                return 0;
            }
        }
        private void ModifyAdjustLeavesByAudit(List<V_ADJUSTLEAVE> AdjustLeaves)
        {
            try
            {
                if (AdjustLeaves == null)
                {
                    return;
                }

                if (AdjustLeaves.Count() == 0)
                {
                    return;
                }

                foreach (V_ADJUSTLEAVE item in AdjustLeaves)
                {
                    string strEmployeeID = item.T_HR_ADJUSTLEAVE.EMPLOYEEID;
                    string strVacType = item.VacationType;
                    DateTime currDate = DateTime.Now;

                    EmployeeLevelDayCountBLL bllLevelCount = new EmployeeLevelDayCountBLL();
                    T_HR_EMPLOYEELEVELDAYCOUNT ent = bllLevelCount.GetCurLevelDayCountByEmployeeID(strEmployeeID, strVacType, currDate);

                    if (ent == null)
                    {
                        continue;
                    }

                    decimal dDays = ent.DAYS.Value;
                    decimal dLeaveDays = item.T_HR_ADJUSTLEAVE.OFFSETDAYS.Value;
                    ent.DAYS = dDays - dLeaveDays;
                    bllLevelCount.ModifyLevelDayCount(ent);

                    T_HR_EMPLOYEELEVELDAYDETAILS entDetails = new T_HR_EMPLOYEELEVELDAYDETAILS();
                    entDetails.T_HR_EMPLOYEELEVELDAYCOUNT = ent;
                    entDetails.EMPLOYEEID = strEmployeeID;
                    entDetails.EMPLOYEENAME = item.T_HR_ADJUSTLEAVE.EMPLOYEENAME;
                    entDetails.EMPLOYEECODE = item.T_HR_ADJUSTLEAVE.EMPLOYEECODE;
                    entDetails.VACATIONTYPE = strVacType;
                    entDetails.DAYS = (0 - dLeaveDays);
                    entDetails.EFFICDATE = currDate;
                    entDetails.REMARK = string.Empty;
                    entDetails.OWNERPOSTID = item.T_HR_ADJUSTLEAVE.OWNERPOSTID;
                    entDetails.OWNERDEPARTMENTID = item.T_HR_ADJUSTLEAVE.OWNERDEPARTMENTID;
                    entDetails.OWNERCOMPANYID = item.T_HR_ADJUSTLEAVE.OWNERCOMPANYID;
                    entDetails.OWNERID = item.T_HR_ADJUSTLEAVE.OWNERID;
                    entDetails.CREATEPOSTID = item.T_HR_ADJUSTLEAVE.CREATEPOSTID;
                    entDetails.CREATEDEPARTMENTID = item.T_HR_ADJUSTLEAVE.CREATEDEPARTMENTID;
                    entDetails.CREATECOMPANYID = item.T_HR_ADJUSTLEAVE.CREATECOMPANYID;
                    entDetails.CREATEUSERID = item.T_HR_ADJUSTLEAVE.CREATEUSERID;
                    entDetails.CREATEDATE = item.T_HR_ADJUSTLEAVE.CREATEDATE;
                    entDetails.UPDATEUSERID = item.T_HR_ADJUSTLEAVE.UPDATEUSERID;
                    entDetails.UPDATEDATE = item.T_HR_ADJUSTLEAVE.UPDATEDATE;

                    //DataContext.AddObject("T_HR_EMPLOYEELEVELDAYDETAILS", entDetails);
                    dal.AddToContext(entDetails);
                }
                dal.SaveContextChanges();
            }
            catch (Exception ex)
            {
                Utility.SaveLog(ex.ToString());
            }

        }
        #endregion

        #region 私有方法

        /// <summary>
        /// 四舍五入浮点数
        /// </summary>
        /// <param name="dValue">浮点数</param>
        /// <param name="strNumOfDec">小数位数值比较值</param>
        /// <param name="ilength"></param>
        /// <returns></returns>
        private decimal RoundOff(decimal dValue, string strNumOfDec, int ilength)
        {
            decimal dRes = 0;
            try
            {
                dRes = decimal.Round(dValue, ilength);

                if (!string.IsNullOrEmpty(strNumOfDec))
                {
                    decimal dNumOfDec = 0, dCheck = 0;
                    decimal.TryParse(strNumOfDec, out dNumOfDec);

                    string[] strlist = dRes.ToString().Split('.');
                    if (strlist.Length == 2)
                    {
                        decimal.TryParse("0." + strlist[1].ToString(), out dCheck);

                        if (dCheck > dNumOfDec)
                        {
                            dRes = decimal.Parse(strlist[0]) + 1;
                        }
                        else
                        {
                            dRes = decimal.Parse(strlist[0]) + dNumOfDec;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Utility.SaveLog(ex.ToString());
            }

            return dRes;
        }

        /// <summary>
        /// 获取非整天的请假时长(按天计)
        /// </summary>
        /// <param name="dtRealDateTime">请假日期时间</param>
        /// <param name="dtRealDate">请假日期</param>
        /// <param name="entTemplateMaster">作息方案</param>
        /// <param name="entTemplateDetails">作息方案明细</param>
        /// <param name="entVacDays">公休假</param>
        /// <param name="strDayFlag">请假日属于:"S",请假第一天；"E",请假最后一天</param>
        /// <param name="dLeaveDayTime">请假时长(按分钟计)</param>
        public void CalculateNonWholeDayLeaveTime(DateTime dtRealDateTime, DateTime dtRealDate, T_HR_SCHEDULINGTEMPLATEMASTER entTemplateMaster,
            IQueryable<T_HR_SCHEDULINGTEMPLATEDETAIL> entTemplateDetails, IQueryable<T_HR_OUTPLANDAYS> entVacDays, IQueryable<T_HR_OUTPLANDAYS> entWorkDays,
            List<int> iWorkDays, string strDayFlag, ref decimal dLeaveDayTime)
        {
            bool bCalculate = false;

            //检查请假时间是否为节假日
            if (entVacDays.Count() > 0)
            {
                foreach (T_HR_OUTPLANDAYS item_Vac in entVacDays)
                {
                    if (item_Vac.STARTDATE.Value <= dtRealDate && item_Vac.ENDDATE >= dtRealDate)
                    {
                        dLeaveDayTime = 0;
                        return;
                    }
                }
            }

            if (entWorkDays.Count() > 0)
            {
                foreach (T_HR_OUTPLANDAYS item_Work in entWorkDays)
                {
                    if (item_Work.STARTDATE.Value <= dtRealDate && item_Work.ENDDATE >= dtRealDate)
                    {
                        bCalculate = true;
                        break;
                    }
                }
            }

            if (!bCalculate && iWorkDays.Contains(Convert.ToInt32(dtRealDate.DayOfWeek)) == false && (entVacDays.Any() || entWorkDays.Any()))
            {
                dLeaveDayTime = 0;
                return;
            }

            DateTime dtCurStartDate = DateTime.Parse(dtRealDate.ToString("yyyy-MM") + "-1");
            DateTime dtCurEndDate = DateTime.Parse(dtRealDate.ToString("yyyy-MM") + "-1").AddMonths(1).AddDays(-1);
            TimeSpan ts = dtCurEndDate.Subtract(dtCurStartDate);
            int iTotalDay = ts.Days;

            int iCircleDay = 0;
            if (entTemplateMaster.SCHEDULINGCIRCLETYPE == (Convert.ToInt32(Common.SchedulingCircleType.Month) + 1).ToString())
            {
                iCircleDay = 31;
            }
            else if (entTemplateMaster.SCHEDULINGCIRCLETYPE == (Convert.ToInt32(Common.SchedulingCircleType.Week) + 1).ToString())
            {
                iCircleDay = 7;
            }

            int iPeriod = iTotalDay / iCircleDay;
            if (iTotalDay % iCircleDay > 0)
            {
                iPeriod += 1;
            }

            bool flag = false;

            for (int i = 0; i < iPeriod; i++)
            {
                for (int j = 0; j < iCircleDay; j++)
                {
                    int m = i * iCircleDay + j;
                    string strSchedulingDate = (j + 1).ToString();
                    DateTime dtCurDate = dtRealDate.AddDays(m);
                    T_HR_SCHEDULINGTEMPLATEDETAIL item = entTemplateDetails.Where(c => c.SCHEDULINGDATE == strSchedulingDate).FirstOrDefault();
                    T_HR_SHIFTDEFINE entShiftDefine = item.T_HR_SHIFTDEFINE;                   

                    //检查是否为全天请假
                    if (dtRealDateTime == dtCurDate)
                    {
                        dLeaveDayTime = entShiftDefine.WORKTIME.Value * 60;
                        flag = true;
                        break;
                    }

                    //检查请假日期时间
                    if (strDayFlag == "S")
                    {
                        DateTime dtCheckStart = new DateTime();
                        dtCheckStart = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.FIRSTSTARTTIME).ToString("HH:mm"));
                        if (dtRealDateTime <= dtCheckStart)
                        {
                            dLeaveDayTime = entShiftDefine.WORKTIME.Value * 60;
                            flag = true;
                            break;
                        }
                    }
                    else if (strDayFlag == "E")
                    {
                        DateTime dtCheck = new DateTime();
                        DateTime dtCheckEnd = new DateTime();
                        GetWorkEndDate(dtRealDateTime, entShiftDefine, ref dtCheckEnd);

                        if (dtCheckEnd != dtCheck)
                        {
                            if (dtRealDateTime >= dtCheckEnd)
                            {
                                dLeaveDayTime = entShiftDefine.WORKTIME.Value * 60;
                                flag = true;
                                break;
                            }
                        }
                    }

                    if (entShiftDefine.FIRSTSTARTTIME != null && entShiftDefine.FIRSTENDTIME != null)
                    {
                        DateTime dtFirstStart = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.FIRSTSTARTTIME).ToString("HH:mm"));
                        DateTime dtFirstEnd = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.FIRSTENDTIME).ToString("HH:mm"));

                        if (strDayFlag == "S")
                        {
                            if (dtFirstEnd <= dtRealDateTime)
                            {
                                dLeaveDayTime = 0;
                            }
                            else
                            {
                                if (dtFirstStart < dtRealDateTime)
                                {
                                    TimeSpan tsFirst = dtFirstEnd.Subtract(dtRealDateTime);
                                    dLeaveDayTime = tsFirst.Hours * 60 + tsFirst.Minutes;
                                }
                                else
                                {
                                    TimeSpan tsFirst = dtFirstEnd.Subtract(dtFirstStart);
                                    dLeaveDayTime = tsFirst.Hours * 60 + tsFirst.Minutes;
                                }
                            }
                        }
                        else if (strDayFlag == "E")
                        {
                            if (dtFirstEnd <= dtRealDateTime)
                            {
                                TimeSpan tsFirst = dtFirstEnd.Subtract(dtFirstStart);
                                dLeaveDayTime = tsFirst.Hours * 60 + tsFirst.Minutes;
                            }
                            else
                            {
                                if (dtFirstStart < dtRealDateTime)
                                {
                                    TimeSpan tsFirst = dtRealDateTime.Subtract(dtFirstStart);
                                    dLeaveDayTime = tsFirst.Hours * 60 + tsFirst.Minutes;
                                }
                                else
                                {
                                    dLeaveDayTime = 0;
                                }
                            }
                        }
                    }

                    if (entShiftDefine.SECONDSTARTTIME != null && entShiftDefine.SECONDENDTIME != null)
                    {
                        DateTime dtSecondStart = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.SECONDSTARTTIME).ToString("HH:mm"));
                        DateTime dtSecondEnd = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.SECONDENDTIME).ToString("HH:mm"));

                        if (strDayFlag == "S")
                        {
                            if (dtSecondEnd <= dtRealDateTime)
                            {
                                dLeaveDayTime += 0;
                            }
                            else
                            {
                                if (dtSecondStart < dtRealDateTime)
                                {
                                    TimeSpan tsSecond = dtSecondEnd.Subtract(dtRealDateTime);
                                    dLeaveDayTime += tsSecond.Hours * 60 + tsSecond.Minutes;
                                }
                                else
                                {
                                    TimeSpan tsSecond = dtSecondEnd.Subtract(dtSecondStart);
                                    dLeaveDayTime += tsSecond.Hours * 60 + tsSecond.Minutes;
                                }
                            }
                        }
                        else if (strDayFlag == "E")
                        {
                            if (dtSecondEnd <= dtRealDateTime)
                            {
                                TimeSpan tsSecond = dtSecondEnd.Subtract(dtSecondStart);
                                dLeaveDayTime = tsSecond.Hours * 60 + tsSecond.Minutes;
                            }
                            else
                            {
                                if (dtSecondStart < dtRealDateTime)
                                {
                                    TimeSpan tsSecond = dtRealDateTime.Subtract(dtSecondStart);
                                    dLeaveDayTime += tsSecond.Hours * 60 + tsSecond.Minutes;
                                }
                                else
                                {
                                    dLeaveDayTime += 0;
                                }
                            }
                        }
                    }

                    if (entShiftDefine.THIRDSTARTTIME != null && entShiftDefine.THIRDENDTIME != null)
                    {
                        DateTime dtThirdStart = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.THIRDSTARTTIME).ToString("HH:mm"));
                        DateTime dtThirdEnd = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.THIRDENDTIME).ToString("HH:mm"));

                        if (strDayFlag == "S")
                        {
                            if (dtThirdEnd <= dtRealDateTime)
                            {
                                dLeaveDayTime += 0;
                            }
                            else
                            {
                                if (dtThirdStart < dtRealDateTime)
                                {
                                    TimeSpan tsThird = dtThirdEnd.Subtract(dtRealDateTime);
                                    dLeaveDayTime += tsThird.Hours * 60 + tsThird.Minutes;
                                }
                                else
                                {
                                    TimeSpan tsThird = dtThirdEnd.Subtract(dtThirdStart);
                                    dLeaveDayTime += tsThird.Hours * 60 + tsThird.Minutes;
                                }
                            }
                        }
                        else if (strDayFlag == "E")
                        {
                            if (dtThirdEnd <= dtRealDateTime)
                            {
                                TimeSpan tsThird = dtThirdEnd.Subtract(dtThirdStart);
                                dLeaveDayTime = tsThird.Hours * 60 + tsThird.Minutes;
                            }
                            else
                            {
                                if (dtThirdStart < dtRealDateTime)
                                {
                                    TimeSpan tsThird = dtRealDateTime.Subtract(dtThirdStart);
                                    dLeaveDayTime += tsThird.Hours * 60 + tsThird.Minutes;
                                }
                                else
                                {
                                    dLeaveDayTime += 0;
                                }
                            }
                        }
                    }

                    if (entShiftDefine.FOURTHSTARTTIME != null && entShiftDefine.FOURTHENDTIME != null)
                    {
                        DateTime dtFourthStart = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.FOURTHSTARTTIME).ToString("HH:mm"));
                        DateTime dtFourthEnd = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.FOURTHENDTIME).ToString("HH:mm"));

                        if (strDayFlag == "S")
                        {
                            if (dtFourthEnd <= dtRealDateTime)
                            {
                                dLeaveDayTime += 0;
                            }
                            else
                            {
                                if (dtFourthStart < dtRealDateTime)
                                {
                                    TimeSpan tsFourth = dtFourthEnd.Subtract(dtRealDateTime);
                                    dLeaveDayTime += tsFourth.Hours * 60 + tsFourth.Minutes;
                                }
                                else
                                {
                                    TimeSpan tsFourth = dtFourthEnd.Subtract(dtFourthStart);
                                    dLeaveDayTime += tsFourth.Hours * 60 + tsFourth.Minutes;
                                }
                            }
                        }
                        else if (strDayFlag == "E")
                        {
                            if (dtFourthEnd <= dtRealDateTime)
                            {
                                TimeSpan tsFourth = dtFourthEnd.Subtract(dtFourthStart);
                                dLeaveDayTime = tsFourth.Hours * 60 + tsFourth.Minutes;
                            }
                            else
                            {
                                if (dtFourthStart < dtRealDateTime)
                                {
                                    TimeSpan tsFourth = dtRealDateTime.Subtract(dtFourthStart);
                                    dLeaveDayTime += tsFourth.Hours * 60 + tsFourth.Minutes;
                                }
                                else
                                {
                                    dLeaveDayTime += 0;
                                }
                            }
                        }
                    }

                    if (dLeaveDayTime > 0)
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 获取下班时间
        /// </summary>
        /// <param name="dtRealDateTime"></param>
        /// <param name="entShiftDefine"></param>
        /// <param name="dtRes"></param>
        private static void GetWorkEndDate(DateTime dtRealDateTime, T_HR_SHIFTDEFINE entShiftDefine, ref DateTime dtRes)
        {
            if (entShiftDefine.FOURTHENDTIME != null)
            {
                dtRes = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.FOURTHENDTIME).ToString("HH:mm"));
                return;
            }

            if (entShiftDefine.THIRDENDTIME != null)
            {
                dtRes = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.THIRDENDTIME).ToString("HH:mm"));
                return;
            }

            if (entShiftDefine.SECONDENDTIME != null)
            {
                dtRes = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.SECONDENDTIME).ToString("HH:mm"));
                return;
            }

            if (entShiftDefine.FIRSTENDTIME != null)
            {
                dtRes = DateTime.Parse(dtRealDateTime.ToString("yyyy-MM-dd") + " " + DateTime.Parse(entShiftDefine.FIRSTENDTIME).ToString("HH:mm"));
                return;
            }
        }
        #endregion

        #region ILookupEntity 成员

        public EntityObject[] GetLookupData(int pageIndex, int pageSize, string sort, string filterString, IList<object> paras, ref int pageCount, string userID)
        {

            List<object> queryParas = new List<object>();
            queryParas.AddRange(paras);

            SetOrganizationFilter(ref filterString, ref queryParas, userID, "T_HR_EMPLOYEELEAVERECORD");

            IQueryable<T_HR_EMPLOYEELEAVERECORD> ents = dal.GetObjects().Include("T_HR_LEAVETYPESET").Include("T_HR_EMPLOYEECANCELLEAVE");

            if (!string.IsNullOrEmpty(filterString))
            {
                ents = ents.Where(filterString, queryParas.ToArray());
            }
            EmployeeSalaryRecordBLL bll = new EmployeeSalaryRecordBLL();
            DateTime? date = bll.GetLastSalaryDateByEemployeeID(userID, "2");
            if (date != null)
            {
                ents = ents.Where(t => t.ENDDATETIME > date);
            }
            //var d = from a in dal.GetObjects<T_HR_EMPLOYEECANCELLEAVE>().Include("T_HR_EMPLOYEELEAVERECORD")
            //        where a.EMPLOYEEID == userID && (a.CHECKSTATE == "0" || a.CHECKSTATE == "1" || a.CHECKSTATE == "2")
            //        select a.T_HR_EMPLOYEELEAVERECORD;
            //ents = ents.Except(d);

            var dv = from a in dal.GetObjects<T_HR_EMPLOYEECANCELLEAVE>()
                     where a.EMPLOYEEID == userID && (a.CHECKSTATE == "2" || a.CHECKSTATE == "1")
                     group a by a.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID into g
                     select new
                     {
                         id = g.Key,
                         totalHours = g.Sum(p => p.TOTALHOURS)
                     };
            var ex = from e in dal.GetObjects<T_HR_EMPLOYEECANCELLEAVE>()
                     join c in dv on e.T_HR_EMPLOYEELEAVERECORD.LEAVERECORDID equals c.id
                     where e.T_HR_EMPLOYEELEAVERECORD.TOTALHOURS <= c.totalHours
                     select e.T_HR_EMPLOYEELEAVERECORD;
            ents = ents.Except(ex);
            ents = from ent in ents.ToList().AsQueryable()
                   orderby  ent.STARTDATETIME descending
                   select ent;

            ents = Utility.Pager<T_HR_EMPLOYEELEAVERECORD>(ents, pageIndex, pageSize, ref pageCount);
            return ents.Count()>0 ? ents.ToArray() : null;
        }

        #endregion

    }
}
