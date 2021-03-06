﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SMT.SaaS.OA.UI;
using SMT.SaaS.FrameworkUI;
using SMT.SaaS.OA.UI.Class;
using SMT.Saas.Tools.PermissionWS;
using SMT.SaaS.OA.UI.SmtOAPersonOfficeService;
using System.Collections.ObjectModel;
using SMT.SaaS.FrameworkUI.ChildWidow;
using SMT.SAAS.Main.CurrentContext;
using SMT.SaaS.FrameworkUI.SelectCityControl;
using SMT.SaaS.FrameworkUI.SelectPostLevel;
using SMT.Saas.Tools.OrganizationWS;
using SMT.SaaS.OA.UI.UserControls.Travelmanagement;

namespace SMT.SaaS.OA.UI.Views.Travelmanagement
{
    public partial class SolutionManagement : BaseForm, IEntityEditor
    {

        #region 初始化参数
        private string AAA = "";
        FormTypes action;
        RefreshedTypes saveType;

        SmtOAPersonOfficeClient client = new SmtOAPersonOfficeClient();
        PermissionServiceClient permissionclient = new PermissionServiceClient();
        OrganizationServiceClient OrgClient = new OrganizationServiceClient();
        //交通工具标准
        private ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT> StandardList = new ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT>();
        //private ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT> EditStandardList = new ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT>();


        //记录旧的交通工具信息  用来和新的做比较如果不一样 则修改标志为true
        //private List<T_OA_TAKETHESTANDARDTRANSPORT> OldStandardList = new List<T_OA_TAKETHESTANDARDTRANSPORT>();
        private T_OA_TAKETHESTANDARDTRANSPORT StandardObj = new T_OA_TAKETHESTANDARDTRANSPORT();
        //飞机路线列表

        private int ToolBarSolution = 0;
        //private bool IsAddStandard = false;//添加交通工具
        private bool IsAddPlaneLine = false;//添加飞机路线
        private T_OA_TRAVELSOLUTIONS travelObj = new T_OA_TRAVELSOLUTIONS();
        private bool EditFlag = false;//修改标志 修改的时候如果做了改动则传列表 否则只修改解决方案
        private bool isChange = false; //用于只加载第一个获取的出差方案 true指enable了cmbSolution的控制
        private bool isDefaultSolution = false;

        ObservableCollection<T_OA_CANTAKETHEPLANELINE> RefPlaneList = new ObservableCollection<T_OA_CANTAKETHEPLANELINE>();
        ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT> RefvechileList = new ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT>();


        /// <summary>
        /// 用来记录交通类型的等级信息
        /// </summary>
        List<T_SYS_DICTIONARY> ListVechileLevel = new List<T_SYS_DICTIONARY>();
        //记录岗位级别
        List<T_SYS_DICTIONARY> ListPost = new List<T_SYS_DICTIONARY>();
        #region 方案设置
        List<T_HR_COMPANY> ListCompany = new List<T_HR_COMPANY>();
        List<T_HR_COMPANY> ListFirstCompany = new List<T_HR_COMPANY>(); List<T_OA_PROGRAMAPPLICATIONS> ListAppSolution = new List<T_OA_PROGRAMAPPLICATIONS>();
        private ObservableCollection<string> companyids = new ObservableCollection<string>();
        List<T_OA_PROGRAMAPPLICATIONS> ListAddAppSolution = new List<T_OA_PROGRAMAPPLICATIONS>();
        SMTLoading loadbar = new SMTLoading();//用于全局的刷新
        #endregion
        #endregion

        #region 构造函数
        public SolutionManagement(FormTypes Action, T_OA_TRAVELSOLUTIONS SolutionObj)
        {
            loadbar.Stop();
            LayoutRoot.Children.Add(loadbar);
            action = Action;
            this.BtnSave.IsEnabled = false;//默认禁用保存按钮
            /*
            InitEvent();
            */
            //this.Loaded += new RoutedEventHandler(SolutionForms_Loaded);
            if (action == FormTypes.Edit || action == FormTypes.Browse)
            {
                travelObj = SolutionObj;
            }
            else if (action == FormTypes.New)
            {
                //IsAddStandard = true;
            }
            //Utility.DisplayGridToolBarButtonUI();
            //OldStandardList.Clear();
            InitializeComponent();
            #region 新增
            this.Loaded += (o, e) =>
            {
                InitEvent();
                InitToobar();
            };
            #endregion
            /*
            InitToobar();
             */
        }

        public SolutionManagement()
        {

            InitializeComponent();
            this.BtnSave.IsEnabled = false;//默认禁用保存按钮
            this.cmbSolution.IsEnabled = false;//默认禁用选择方案cmbox
            this.Loaded += new RoutedEventHandler(SolutionForms_Loaded);
            /*
            Utility.DisplayGridToolBarButtonUI(ToolBar_Solution, "T_OA_TRAVELSOLUTIONS", true);
            */
            //SolutionManagement aa = new SolutionManagement();
            //EntityBrowser browser = new EntityBrowser(aa);
            //browser.Show<string>(DialogMode.Default, Common.ParentLayoutRoot, "", (result) => { }, true);
        }
        void client_GetTravelSolutionFlowCompleted(object sender, GetTravelSolutionFlowCompletedEventArgs e)
        {

            try
            {
                if (e.Result != null)
                {
                    isChange = false;
                    BindDataGrid(e.Result.ToList(), e.pageCount);
                }
                else
                {
                    BindDataGrid(null, 0);
                }


            }
            catch (Exception ex)
            {
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), ex.ToString());
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
        }

        private void BindDataGrid(List<T_OA_TRAVELSOLUTIONS> obj, int pageCount)
        {

            if (obj == null || obj.Count < 1)
            {
                this.cmbSolution.ItemsSource = null;
                return;
            }
            cmbSolution.ItemsSource = obj;
            cmbSolution.DisplayMemberPath = "PROGRAMMENAME";
            //用于计算最后的的该显示的出差方案及其使用公司
            int NumSolutions = cmbSolution.Items.Count();
            int CountSolutions = 0;

            foreach (T_OA_TRAVELSOLUTIONS Region in cmbSolution.Items)
            {
                CountSolutions++;
                if (Region !=null)//== Common.CurrentLoginUserInfo.UserPosts[0].CompanyID)
                {
                    if (CountSolutions == NumSolutions) isDefaultSolution = true;
                    cmbSolution.SelectedItem = Region;
                }
                else
                {
                    cmbSolution.SelectedIndex = 0;
                }
            }
        }
        private void DetailSolutionInfo(T_OA_TRAVELSOLUTIONS obj)
        {
            txtSolutionName.Text = string.IsNullOrEmpty(obj.PROGRAMMENAME) ? "" : obj.PROGRAMMENAME;
            nuHalfDay.Value = System.Convert.ToInt32(obj.CUSTOMHALFDAY);

            //区间天数
            nuqujiaomindays.Value = System.Convert.ToInt32(obj.MINIMUMINTERVALDAYS);
            nuqujianmax.Value = System.Convert.ToInt32(obj.MAXIMUMRANGEDAYS);
            nuqujianbili.Value = System.Convert.ToInt32(obj.INTERVALRATIO);
            //最大天数
            //numaxdays.Value = System.Convert.ToInt32(obj.MAXIMUMDAYS);
            nubaoxiaomindays.Value = System.Convert.ToInt32(obj.RANGEDAYS);


            if (!string.IsNullOrEmpty(obj.RANGEPOSTLEVEL.ToString()))
            {
                foreach (T_SYS_DICTIONARY Region in cbxpostlevel.Items)
                {
                    if (Region.DICTIONARYVALUE.ToString() == obj.RANGEPOSTLEVEL.ToString())
                    {
                        cbxpostlevel.SelectedItem = Region;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(obj.NOALLOWANCEPOSTLEVEL.ToString()))
            {
                foreach (T_SYS_DICTIONARY Region in cbxNoAllowancePostlevel.Items)
                {
                    if (Region.DICTIONARYVALUE.ToString() == obj.NOALLOWANCEPOSTLEVEL.ToString())
                    {
                        cbxNoAllowancePostlevel.SelectedItem = Region;
                        break;
                    }
                }
            }
            RefreshUI(RefreshedTypes.HideProgressBar);

        }
        /// <summary>
        /// 禁用控件
        /// </summary>
        private void SetEnabled()
        {
            //this.txtHour.IsEnabled = false;
            this.nuHalfDay.IsEnabled = false;
            this.nuqujianbili.IsEnabled = false;
            this.nuqujianmax.IsEnabled = false;
            this.nuqujiaomindays.IsEnabled = false;
            this.txtSolutionName.IsEnabled = false;

            this.DGVechileStandard.IsReadOnly = true;

            ToolBar_Vechile.IsEnabled = false;
            this.nubaoxiaomindays.IsEnabled = false;
            this.cbxpostlevel.IsEnabled = false;
            this.cbxNoAllowancePostlevel.IsEnabled = false;
            this.DelAllBtn.IsEnabled = false;
            this.AddBtn.IsEnabled = false;
            this.AddAllBtn.IsEnabled = false;
            this.DelBtn.IsEnabled = false;
        }
        #endregion

        #region 公共调用函数
        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            client.AddTravleSolutionCompleted += new EventHandler<AddTravleSolutionCompletedEventArgs>(client_AddTravleSolutionCompleted);
            client.GetTravelSolutionFlowCompleted += new EventHandler<GetTravelSolutionFlowCompletedEventArgs>(client_GetTravelSolutionFlowCompleted);
            client.GetVechileStandardAndPlaneLineCompleted += new EventHandler<GetVechileStandardAndPlaneLineCompletedEventArgs>(client_GetVechileStandardAndPlaneLineCompleted);
            client.UpdateTravleSolutionCompleted += new EventHandler<UpdateTravleSolutionCompletedEventArgs>(client_UpdateTravleSolutionCompleted);
            client.DeleteTravleSolutionCompleted += new EventHandler<DeleteTravleSolutionCompletedEventArgs>(client_DeleteTravleSolutionCompleted);
            //client.GetTravelSolutionFlowAsync(0,100, "CREATEDATE descending", filter, paras, pageCount, loginUserInfo);
            //LoadSolutionInfos();
            OrgClient.GetCompanyActivedCompleted += new EventHandler<GetCompanyActivedCompletedEventArgs>(OrgClient_GetCompanyActivedCompleted);
            OrgClient.GetCompanyActivedAsync(Common.CurrentLoginUserInfo.EmployeeID);//获取公司信息后才加载出差方案数据
            client.GetTravleSolutionSetBySolutionIDCompleted += new EventHandler<GetTravleSolutionSetBySolutionIDCompletedEventArgs>(client_GetTravleSolutionSetBySolutionIDCompleted);

            //方案应用
            client.AddTravleSolutionSetCompleted += new EventHandler<AddTravleSolutionSetCompletedEventArgs>(client_AddTravleSolutionSetCompleted);
            //initToolbarSolution();
        }

        void client_DeleteTravleSolutionCompleted(object sender, DeleteTravleSolutionCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            try
            {
                if (e.Result == false)
                {
                    //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"));
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                }
                else
                {
                    //Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("MODIFYSUCCESSED"));
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("DELETESUCCESS"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                    action = FormTypes.Edit;
                    //cmbSolution.SelectedIndex = 0;
                    RefreshUI(RefreshedTypes.All);
                }
                BtnSave.IsEnabled = false;
                cmbSolution.IsEnabled = false;
            }
            catch (Exception ex)
            {
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"));
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                //throw (ex);
            }
            OrgClient.GetCompanyActivedAsync(Common.CurrentLoginUserInfo.EmployeeID);
            //LoadSolutionInfos();
        }


        #region  复制加载数据
        void LoadSolutionInfos()
        {
            int pageCount = 0;
            string filter = "";    //查询过滤条件

            ObservableCollection<object> paras = new System.Collections.ObjectModel.ObservableCollection<object>();   //参数值

            SMT.SaaS.OA.UI.SmtOAPersonOfficeService.LoginUserInfo loginUserInfo = new SMT.SaaS.OA.UI.SmtOAPersonOfficeService.LoginUserInfo();
            loginUserInfo.companyID = Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
            loginUserInfo.userID = Common.CurrentLoginUserInfo.EmployeeID;
            RefreshUI(RefreshedTypes.ShowProgressBar);
            client.GetTravelSolutionFlowAsync(0, 100, "CREATEDATE descending", filter, paras, pageCount, loginUserInfo);

        }
        #endregion
        /// <summary>
        /// 初始化 出差方案toolbar
        /// </summary>
        private void initToolbarSolution()
        {

            ToolBar_Solution.btnAudit.Visibility = Visibility.Collapsed;
            ToolBar_Solution.BtnView.Visibility = Visibility.Collapsed;
            ToolBar_Solution.btnRefresh.Visibility = Visibility.Collapsed;
            ToolBar_Solution.btnEdit.Click += new RoutedEventHandler(btnEdit_Click);
            ToolBar_Solution.stpCheckState.Visibility = Visibility.Collapsed;
            ToolBar_Solution.btnNew.Click += new RoutedEventHandler(ToolBarSolution_btnNew_Click);
            ToolBar_Solution.btnDelete.Click += new RoutedEventHandler(ToolBar_SolutionbtnDelete_Click);
            ToolBar_Solution.ShowRect();
        }

        void browser_ReloadDataEvent()
        {

        }
        /// <summary>
        /// 添加新的解决方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolBarSolution_btnNew_Click(object sender, RoutedEventArgs e)
        {
            action = FormTypes.New;
            this.cbxpostlevel.SelectedIndex = 0;
            this.cbxNoAllowancePostlevel.SelectedIndex = 0;
            ToolBarSolution += 1;

            //IsAddStandard = true;
            travelObj.TRAVELSOLUTIONSID = System.Guid.NewGuid().ToString();
            travelObj.CREATECOMPANYID = Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
            travelObj.CREATEDEPARTMENTID = Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
            travelObj.CREATEPOSTID = Common.CurrentLoginUserInfo.UserPosts[0].PostID;
            travelObj.CREATEUSERID = Common.CurrentLoginUserInfo.EmployeeID;
            travelObj.CREATEUSERNAME = Common.CurrentLoginUserInfo.EmployeeName;
            travelObj.OWNERCOMPANYID = Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
            travelObj.OWNERDEPARTMENTID = Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
            travelObj.OWNERPOSTID = Common.CurrentLoginUserInfo.UserPosts[0].PostID;
            InitSolutionPara();
            this.SetEnabledIsTrue();
            //在新建时不可以选择其他的出差方案，否则会造成错误
            cmbSolution.IsEnabled = false;
            ToolBar_Solution.btnEdit.IsEnabled = false;

            SecondCompany.Items.Clear();
            //OldStandardList.Clear();
            this.StandardList.Clear();
            NewStandardDetail();
            //新建后出来复制操作
            if (ToolBarSolution == 1)
            {
                ImageButton _ImageBtnCopy = new ImageButton();
                _ImageBtnCopy.AddButtonAction("/SMT.SaaS.FrameworkUI;Component/Images/Tool/copy.png", Utility.GetResourceStr("复制")).Click += new RoutedEventHandler(SolutionManagement_Click);
                ToolBar_Solution.stpOtherAction.Children.Add(_ImageBtnCopy);
            }
        }
        /// <summary>
        /// 复制按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void SolutionManagement_Click(object sender, RoutedEventArgs e)
        {
            ReplicationProgram AddWin = new ReplicationProgram();
            AddWin.SelectSolutionComplete += AddWin_SelectSolutionComplete;
            EntityBrowser browser = new EntityBrowser(AddWin);
            browser.MinWidth = 500;
            browser.MinHeight = 350;
            browser.ReloadDataEvent += new EntityBrowser.refreshGridView(browser_ReloadDataEvent);
            browser.Show<string>(DialogMode.Default, Common.ParentLayoutRoot, "", (result) => { });
            //if (browser.ParentWindow != null)
            //{
            //    browser.ParentWindow.Closed += (sender2, args) =>
            //    {

            //       //LoadSolutionInfos();
                  
            //    };
            //}
        }
        //复制选择方案完成事件
        bool isCopy = false;
        void AddWin_SelectSolutionComplete(object sender, ReplicationProgram.SelectSolutionEventArgs e)
        {
            isCopy = true;
            travelObj = e.solution;
            travelObj.PROGRAMMENAME = "";
            DetailSolutionInfo(travelObj);
            client.GetVechileStandardAndPlaneLineAsync(travelObj.TRAVELSOLUTIONSID, RefPlaneList, RefvechileList, "DefaultSolution");
           
        }

        private void InitSolutionPara()
        {
            this.nuHalfDay.Value = 1;
            this.nuqujianbili.Value = 1;
            this.nuqujianmax.Value = 1;
            this.nuqujiaomindays.Value = 1;
            this.txtSolutionName.Text = "";
            this.nubaoxiaomindays.Value = 1;
        }
        /// <summary>
        /// 删除 出差方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolBar_SolutionbtnDelete_Click(object sender, RoutedEventArgs e)
        {
            travelObj = cmbSolution.SelectedItem as T_OA_TRAVELSOLUTIONS;
            if (travelObj == null)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "出差方案不能为空，请选择！",
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);


            }
            else
            {
                string Result = "";

                ComfirmWindow com = new ComfirmWindow();
                com.OnSelectionBoxClosed += (obj, result) =>
                {
                    string SoltionID = travelObj.TRAVELSOLUTIONSID;
                    isChange = true;
                    client.DeleteTravleSolutionAsync(SoltionID);
                    //TravelSolution = null;

                };
                com.SelectionBox(Utility.GetResourceStr("DELETECONFIRM"), Utility.GetResourceStr("DELETEALTER"), ComfirmWindow.titlename, Result);
            }
        }
        /// <summary>
        /// 修改出差方案
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            action = FormTypes.Edit;
            travelObj = cmbSolution.SelectedItem as T_OA_TRAVELSOLUTIONS;
            if (travelObj == null)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "出差方案不能为空，请选择！",
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                return;

            }
            else
            {
                isChange = true;
                SetEnabledIsTrue();
            }
        }

        /// <summary>
        /// 禁用控件
        /// </summary>
        private void SetEnabledIsTrue()
        {
            this.nuHalfDay.IsEnabled = true;
            this.nuqujianbili.IsEnabled = true;
            this.nuqujianmax.IsEnabled = true;
            this.nuqujiaomindays.IsEnabled = true;
            this.txtSolutionName.IsEnabled = true;
            this.DGVechileStandard.IsReadOnly = false;

            ToolBar_Vechile.IsEnabled = true;
            this.nubaoxiaomindays.IsEnabled = true;
            this.cbxpostlevel.IsEnabled = true;
            this.cbxNoAllowancePostlevel.IsEnabled = true;

            this.DelAllBtn.IsEnabled = true;
            this.AddBtn.IsEnabled = true;
            this.AddAllBtn.IsEnabled = true;
            this.DelBtn.IsEnabled = true;
            this.BtnSave.IsEnabled = true;
            this.cmbSolution.IsEnabled = true;

            if (action == FormTypes.New)
            {
                NewStandardDetail();
            }
            else
            {
                //DGVechileStandard.ItemsSource = null;
                //StandardList.Clear();
                //if (IsAddStandard)
                //else
                //    client.GetVechileStandardAndPlaneLineAsync(travelObj.TRAVELSOLUTIONSID, f, RefvechileList);
            }
        }

        void client_UpdateTravleSolutionCompleted(object sender, UpdateTravleSolutionCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            BtnSave.IsEnabled = true;
            try
            {
                if(e.Error==null)
                { 
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("MODIFYSUCCESSED"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                }else
                {
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"),e.Error.Message,
                                       Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            action = FormTypes.Edit;
            this.BtnSave.IsEnabled = false;
            this.cmbSolution.IsEnabled = false;//禁用选择出差方案控件
            RefreshUI(RefreshedTypes.All);
            isChange = false;
            //LoadSolutionInfos();
        }

        void client_GetVechileStandardAndPlaneLineCompleted(object sender, GetVechileStandardAndPlaneLineCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            //OldStandardList.Clear();
            try
            {
                if (!e.Cancelled)
                {
                    if (e.Error == null)
                    {
                        if (e.VechileStandardList.Count() > 0 && (e.UserState=="DefaultSolution" || isChange==true))
                        {
                            //OldStandardList = e.VechileStandardList.ToList();
                            //e.VechileStandardList.OrderBy(c => c.TYPEOFTRAVELTOOLS).ThenBy(c => c.TAKETHETOOLLEVEL).ToList().ForEach(item =>
                            //{
                            //    T_OA_TAKETHESTANDARDTRANSPORT Sport = new T_OA_TAKETHESTANDARDTRANSPORT();
                            //    Sport.TAKETHESTANDARDTRANSPORTID = item.TAKETHESTANDARDTRANSPORTID;
                            //    Sport.T_OA_TRAVELSOLUTIONS = item.T_OA_TRAVELSOLUTIONS;
                            //    Sport.TAKETHETOOLLEVEL = item.TAKETHETOOLLEVEL;
                            //    Sport.TYPEOFTRAVELTOOLS = item.TYPEOFTRAVELTOOLS;
                            //    Sport.ENDPOSTLEVEL = item.ENDPOSTLEVEL;
                            //    Sport.CREATEDATE = item.CREATEDATE;
                            //    Sport.CREATEUSERID = item.CREATEUSERID;
                            //    Sport.UPDATEDATE = item.UPDATEDATE;
                            //    Sport.UPDATEUSERID = item.UPDATEUSERID;
                            //    Sport.EntityKey = item.EntityKey;
                            //    if (OldStandardList.Count() > 0)
                            //    {
                            //        var ents = from ent in OldStandardList
                            //                   where ent.ENDPOSTLEVEL == item.ENDPOSTLEVEL && ent.TAKETHETOOLLEVEL == item.TAKETHETOOLLEVEL
                            //                   && ent.TYPEOFTRAVELTOOLS == item.TYPEOFTRAVELTOOLS && ent.T_OA_TRAVELSOLUTIONS.TRAVELSOLUTIONSID == item.T_OA_TRAVELSOLUTIONS.TRAVELSOLUTIONSID
                            //                   && ent.TAKETHESTANDARDTRANSPORTID == item.TAKETHESTANDARDTRANSPORTID
                            //                   select ent;
                            //        if (ents.Count() == 0)
                            //            OldStandardList.Add(Sport);
                            //    }
                            //    else
                            //    {
                            //        OldStandardList.Add(Sport);
                            //    }
                            //});

                            var listALl=e.VechileStandardList.OrderBy(c => c.TYPEOFTRAVELTOOLS).ThenBy(c => c.TAKETHETOOLLEVEL).ToList();
                            ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT> tObjectStruct = new ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT>();

                            listALl.ForEach(p => tObjectStruct.Add(p));

                            StandardList = tObjectStruct;

                            this.DGVechileStandard.ItemsSource = StandardList;
                        }
                        else
                        {
                            DGVechileStandard.ItemsSource = null;
                            StandardList.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }



        void client_AddTravleSolutionCompleted(object sender, AddTravleSolutionCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);
            //BtnSave.IsEnabled = true;
            try
            {
                if (e.Result != "")
                {
                    //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Result));
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Result),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                }
                else
                {
                    //Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("ADDDATASUCCESSED"));
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("ADDDATASUCCESSED"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                    action = FormTypes.Edit;
                    this.BtnSave.IsEnabled = false;//禁用保存按钮
                    this.cmbSolution.IsEnabled = false;//禁用选择出差方案控件
                    isChange = false;
                    RefreshUI(RefreshedTypes.All);
                    //}
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            LoadSolutionInfos();
        }


        /// <summary>
        /// 初始化toolbar
        /// </summary>
        private void InitToobar()
        {
            InitVechileStandardToolBar();


            //GetVechileLevelInfos();
            if (action != FormTypes.New && action != FormTypes.Edit)
            {
                NewStandardDetail();

            }
            //GetVechileLevelInfos();
        }

        void SolutionForms_Loaded(object sender, RoutedEventArgs e)
        {
            #region 新增
            Utility.DisplayGridToolBarButtonUI(ToolBar_Solution, "T_OA_TRAVELSOLUTIONS", true);
            #endregion

            InitEvent();
            SetEnabled();
            InitToobar();
            RefreshUI(RefreshedTypes.ShowProgressBar);
            GetVechileLevelInfos();
            initToolbarSolution();
            if (action == FormTypes.New)
            {
                NewStandardDetail();
            }
            switch (action)
            {
                case FormTypes.New:
                    travelObj.TRAVELSOLUTIONSID = System.Guid.NewGuid().ToString();
                    travelObj.CREATECOMPANYID = Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
                    travelObj.CREATEDEPARTMENTID = Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
                    travelObj.CREATEPOSTID = Common.CurrentLoginUserInfo.UserPosts[0].PostID;
                    travelObj.CREATEUSERID = Common.CurrentLoginUserInfo.EmployeeID;
                    travelObj.CREATEUSERNAME = Common.CurrentLoginUserInfo.EmployeeName;
                    travelObj.OWNERCOMPANYID = Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
                    travelObj.OWNERDEPARTMENTID = Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
                    travelObj.OWNERPOSTID = Common.CurrentLoginUserInfo.UserPosts[0].PostID;
                    this.cbxpostlevel.SelectedIndex = -1;
                    cbxNoAllowancePostlevel.SelectedIndex = -1;
                    break;
                case FormTypes.Edit:


                    client.GetVechileStandardAndPlaneLineAsync(travelObj.TRAVELSOLUTIONSID, RefPlaneList, RefvechileList);
                    DetailSolutionInfo(travelObj);
                    break;
                case FormTypes.Browse:

                    client.GetVechileStandardAndPlaneLineAsync(travelObj.TRAVELSOLUTIONSID, RefPlaneList, RefvechileList);
                    DetailSolutionInfo(travelObj);
                    SetEnabled();
                    break;
            }
            SecondCompany.DisplayMemberPath = "CNAME";
            FirstComany.DisplayMemberPath = "CNAME";
        }


        /// <summary>
        /// 获取交通工具的级别
        /// </summary>
        void GetVechileLevelInfos()
        {
            List<T_SYS_DICTIONARY> dicts = Application.Current.Resources["SYS_DICTIONARY"] as List<T_SYS_DICTIONARY>;
            var objs = from d in dicts
                       where d.DICTIONCATEGORY == "VICHILELEVEL"
                       orderby d.DICTIONARYVALUE
                       select d;
            var objposts = from ent in dicts
                           where ent.DICTIONCATEGORY == "POSTLEVEL"
                           orderby ent.DICTIONARYVALUE
                           select ent;

            ListVechileLevel = objs.ToList();
            ListPost = objposts.ToList();
        }
        /// <summary>
        /// 初始化交通工具标准toolbar
        /// </summary>
        private void InitVechileStandardToolBar()
        {
            ToolBar_Vechile.btnAudit.Visibility = Visibility.Collapsed;
            ToolBar_Vechile.BtnView.Visibility = Visibility.Collapsed;
            ToolBar_Vechile.btnEdit.Visibility = Visibility.Collapsed;
            ToolBar_Vechile.btnRefresh.Visibility = Visibility.Collapsed;
            ToolBar_Vechile.stpCheckState.Visibility = Visibility.Collapsed;
            ToolBar_Vechile.btnNew.Click += new RoutedEventHandler(ToolBar_VechilebtnNew_Click);
            ToolBar_Vechile.btnDelete.Click += new RoutedEventHandler(ToolBar_VechilebtnDelete_Click);
            ToolBar_Vechile.ShowRect();
        }
        /// <summary>
        /// 删除交通工具标准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolBar_VechilebtnDelete_Click(object sender, RoutedEventArgs e)
        {

            if (DGVechileStandard.SelectedItems.Count > 0)
            {
                for (int i = 0; i < DGVechileStandard.SelectedItems.Count; i++)
                {
                    T_OA_TAKETHESTANDARDTRANSPORT ent = DGVechileStandard.SelectedItems[i] as T_OA_TAKETHESTANDARDTRANSPORT;
                    StandardList.Remove(ent);
                }
                this.DGVechileStandard.ItemsSource = StandardList;
            }
            else
            {
                ComfirmWindow.ConfirmationBox(Utility.GetResourceStr("CONFIRMINFO"), Utility.GetResourceStr("SELECTERROR", "DELETE"), Utility.GetResourceStr("CONFIRMBUTTON"));
            }
        }
        /// <summary>
        /// 增加交通工具标准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ToolBar_VechilebtnNew_Click(object sender, RoutedEventArgs e)
        {
            NewStandardDetail();
            }
        private void NewStandardDetail()
        {
            T_OA_TAKETHESTANDARDTRANSPORT AddStandardObj = new T_OA_TAKETHESTANDARDTRANSPORT();
            AddStandardObj.TAKETHESTANDARDTRANSPORTID = Guid.NewGuid().ToString();
            AddStandardObj.T_OA_TRAVELSOLUTIONS = travelObj;
            AddStandardObj.CREATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            AddStandardObj.UPDATEUSERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            AddStandardObj.CREATEDATE = DateTime.Now;
            AddStandardObj.UPDATEDATE = DateTime.Now;
            AddStandardObj.OWNERID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.EmployeeID;
            AddStandardObj.OWNERPOSTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].PostID;
            AddStandardObj.OWNERDEPARTMENTID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
            AddStandardObj.OWNERCOMPANYID = SMT.SAAS.Main.CurrentContext.Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
            this.StandardList.Add(AddStandardObj);
            DGVechileStandard.ItemsSource = StandardList;  
        }
        #endregion
        
        #region 保存
        private void Save()
        {
            try
            {
                ToolBar_Solution.btnEdit.IsEnabled = true;
                if (Check())
                {
                    if (ChxOnday.IsChecked == true)
                    {
                        travelObj.ANDFROMTHATDAY = "1";
                    }
                    else
                    {
                        travelObj.ANDFROMTHATDAY = "0";
                    }
                    if (cbxpostlevel.SelectedIndex == 0)
                    {
                        ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), "岗位级别不能为空!", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        T_SYS_DICTIONARY Dict = cbxpostlevel.SelectedItem as T_SYS_DICTIONARY;
                        travelObj.RANGEPOSTLEVEL = Dict.DICTIONARYVALUE.ToString();
                    }

                    if (cbxNoAllowancePostlevel.SelectedIndex == 0)
                    {
                        ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), "必须指定小于多少岗位级别无出差各项补贴!", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                        return;
                    }
                    else
                    {
                        T_SYS_DICTIONARY Dict = cbxNoAllowancePostlevel.SelectedItem as T_SYS_DICTIONARY;
                        travelObj.NOALLOWANCEPOSTLEVEL = Dict.DICTIONARYVALUE.ToString();
                    }
                    travelObj.PROGRAMMENAME = this.txtSolutionName.Text;
                    //最小天数                    
                    travelObj.RANGEDAYS = nubaoxiaomindays.Value.ToString();
                    //区间天数
                    travelObj.MINIMUMINTERVALDAYS = nuqujiaomindays.Value.ToString();
                    travelObj.MAXIMUMRANGEDAYS = nuqujianmax.Value.ToString();
                    travelObj.INTERVALRATIO = nuqujianbili.Value.ToString();
                    //最大天数
                    if (nuqujianmax.Value <= nuqujiaomindays.Value)
                    {
                        ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("TIPS"), "报销区间最大数不能小于等于最小天数!", Utility.GetResourceStr("CONFIRM"), MessageIcon.Exclamation);
                        return;
                    }
                    travelObj.CUSTOMHALFDAY = nuHalfDay.Value.ToString();
                    RefreshUI(RefreshedTypes.ShowProgressBar);
                    BtnSave.IsEnabled = false;
                    //return;

                    if (action == FormTypes.New)
                    {
                        travelObj.TRAVELSOLUTIONSID = System.Guid.NewGuid().ToString();
                        travelObj.CREATECOMPANYID = Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
                        travelObj.CREATEDEPARTMENTID = Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
                        travelObj.CREATEPOSTID = Common.CurrentLoginUserInfo.UserPosts[0].PostID;
                        travelObj.CREATEUSERID = Common.CurrentLoginUserInfo.EmployeeID;
                        travelObj.CREATEUSERNAME = Common.CurrentLoginUserInfo.EmployeeName;
                        travelObj.OWNERCOMPANYID = Common.CurrentLoginUserInfo.UserPosts[0].CompanyID;
                        travelObj.OWNERDEPARTMENTID = Common.CurrentLoginUserInfo.UserPosts[0].DepartmentID;
                        travelObj.OWNERPOSTID = Common.CurrentLoginUserInfo.UserPosts[0].PostID;
                        //添加出差方案设置
                        AddSetSolution();
                        ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT> tObjectStruct = new ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT>();

                        StandardList.ForEach(p => tObjectStruct.Add(p));

                        client.AddTravleSolutionAsync(travelObj, tObjectStruct, companyids);
                    }
                    else
                    {
                        //AddVechileStandard();
                        //添加出差方案设置
                        AddSetSolution();

                        ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT> tObjectStruct = new ObservableCollection<T_OA_TAKETHESTANDARDTRANSPORT>();

                        StandardList.ForEach(p => tObjectStruct.Add(p));
                        client.UpdateTravleSolutionAsync(travelObj, tObjectStruct, companyids, EditFlag);
                    }

                }
            }
            catch (Exception ex)
            {
                RefreshUI(RefreshedTypes.HideProgressBar);
            }
        }
        /// <summary>
        /// 添加交通工具标准
        /// </summary>


        private bool Check()
        {

            //工具标准检查
            if (DGVechileStandard.ItemsSource != null)
            {

                foreach (Object obj in DGVechileStandard.ItemsSource)
                {
                    //SelectBox = Utility.FindChildControl<CheckBox>(DaGr, "SelectAll");
                    //如果DGVechileStandard.Columns[1].GetCellContent(obj)为null 不进行校验
                    if (DGVechileStandard.Columns[1].GetCellContent(obj) == null)
                        break;
                    TravelDictionaryComboBox ComVechile = DGVechileStandard.Columns[1].GetCellContent(obj).FindName("ComVechileType") as TravelDictionaryComboBox;
                    //TravelDictionaryComboBox ComVechile = Utility.FindChildControl<TravelDictionaryComboBox>(DGVechileStandard, DGVechileStandard.Columns[1].GetCellContent(obj).FindName("ComVechileType").ToString());// DGVechileStandard.Columns[1].GetCellContent(obj).FindName("ComVechileType") as TravelDictionaryComboBox;
                    TravelDictionaryComboBox ComLevel = DGVechileStandard.Columns[2].GetCellContent(obj).FindName("ComVechileTypeLeve") as TravelDictionaryComboBox;
                    SelectPost EndComPost = DGVechileStandard.Columns[3].GetCellContent(obj).FindName("txtSelectPost") as SelectPost;


                    if (EndComPost != null)
                    {
                        if (string.IsNullOrEmpty(EndComPost.TxtSelectedPost.Text))
                        {
                            //Utility.ShowCustomMessage(MessageTypes.Caution, "CAUTION", "岗位不能为空");
                            ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "岗位不能为空",
                        Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                            return false;
                        }
                    }
                    if (ComVechile != null)
                    {
                        if (ComVechile.SelectedIndex == 0)
                        {
                            //Utility.ShowCustomMessage(MessageTypes.Caution, "CAUTION", "交通工具类型不能为空");
                            ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), "交通工具类型不能为空！",
                        Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                            return false;
                        }
                    }

                }
            }



            List<SMT.SaaS.FrameworkUI.Validator.ValidatorBase> validators = Group1.ValidateAll();
            if (validators.Count > 0)
            {
                foreach (var h in validators)
                {
                    //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(h.ErrorMessage));
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(h.ErrorMessage),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
                    return false;
                }
                RefreshUI(RefreshedTypes.HideProgressBar);
            }
            return true;
        }
        #endregion

        #region IEntityEditor
        public string GetTitle()
        {

            if (action == FormTypes.New)
            {
                return "添加出差方案";
                //return Utility.GetResourceStr("ADDTITLE", "GRADENAME");
            }
            else if (action == FormTypes.Edit)
            {
                return "修改出差方案";
                //return Utility.GetResourceStr("EDITTITLE", "GRADENAME");
            }
            else
            {
                return "查看出差方案";
                //return Utility.GetResourceStr("VIEWTITLE", "GRADENAME");
            }
        }

        public string GetStatus()
        {
            return "";
        }

        public void DoAction(string actionType)
        {
            switch (actionType)
            {
                case "0":
                    saveType = RefreshedTypes.LeftMenu;
                    Save();
                    break;
                case "1":
                    saveType = RefreshedTypes.CloseAndReloadData;
                    Save();
                    break;

            }
        }

        public List<NavigateItem> GetLeftMenuItems()
        {
            List<NavigateItem> items = new List<NavigateItem>();
            NavigateItem item = new NavigateItem
            {
                Title = "详细信息",
                Tooltip = "详细信息"
            };
            items.Add(item);
            return items;
        }
        public List<ToolbarItem> GetToolBarItems()
        {
            List<ToolbarItem> items = new List<ToolbarItem>();
            if (action != FormTypes.Browse)
            {
                ToolbarItem item = new ToolbarItem
                {
                    DisplayType = ToolbarItemDisplayTypes.Image,
                    Key = "1",
                    Title = Utility.GetResourceStr("SAVEANDCLOSE"),
                    ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_saveClose.png"
                };
                items.Add(item);
                item = new ToolbarItem
                {
                    DisplayType = ToolbarItemDisplayTypes.Image,
                    Key = "0",
                    Title = Utility.GetResourceStr("SAVE"),
                    ImageUrl = "/SMT.SaaS.FrameworkUI;Component/Images/ToolBar/16_save.png"

                };
                items.Add(item);
            }

            return items;
        }

        private void Close()
        {
            RefreshUI(saveType);
        }

        public event UIRefreshedHandler OnUIRefreshed;
        public void RefreshUI(RefreshedTypes type)
        {
            if (OnUIRefreshed != null)
            {
                UIRefreshedEventArgs args = new UIRefreshedEventArgs();
                args.RefreshedType = type;
                OnUIRefreshed(this, args);
            }
        }
        #endregion


        #region 出差方案设置


        /// <summary>
        /// 添加所选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (FirstComany.SelectedItems.Count > 0)
            {
                T_HR_COMPANY selectcompany = new T_HR_COMPANY();
                selectcompany = FirstComany.SelectedItem as T_HR_COMPANY;
                if (SecondCompany.Items.Count > 0)
                {
                    if (!(SecondCompany.Items.Contains(selectcompany)))
                    {
                        SecondCompany.Items.Add(selectcompany);
                    }
                }
                else
                {
                    SecondCompany.Items.Add(selectcompany);
                }

                FirstComany.Items.Remove(selectcompany);

            }
            else
            {
                //Utility.ShowCustomMessage(MessageTypes.Caution, "CAUTION", "请选择公司");
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "请选择公司",
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            //FirstComany.ItemsSource = null;
            //FirstComany.ItemsSource = ListFirstCompany;
            if (action == FormTypes.Edit)
                EditFlag = true;
            SecondCompany.DisplayMemberPath = "CNAME";
        }
        /// <summary>
        /// 添加所有项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAllBtn_Click(object sender, RoutedEventArgs e)
        {
            //SecondCompany.Items.Clear();
            if (FirstComany.Items.Count > 0)
            {
                foreach (var obj in FirstComany.Items)
                {
                    T_HR_COMPANY selectcompany = new T_HR_COMPANY();
                    selectcompany = obj as T_HR_COMPANY;
                    if (!(SecondCompany.Items.Contains(selectcompany)))        //zl
                    {
                        SecondCompany.Items.Add(selectcompany);
                    }


                    //FirstComany.Items.Remove(selectcompany);

                }
                FirstComany.Items.Clear();
            }
            if (action == FormTypes.Edit)
                EditFlag = true;
        }
        /// <summary>
        /// 删除选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SecondCompany.SelectedItems.Count > 0)
            {

                T_HR_COMPANY selectcompany = new T_HR_COMPANY();
                selectcompany = SecondCompany.SelectedItem as T_HR_COMPANY;

                SecondCompany.Items.Remove(selectcompany);
                //FirstComany.Items.Add(selectcompany);

                if (!(FirstComany.Items.Contains(selectcompany)))    //zl
                {
                    FirstComany.Items.Add(selectcompany);
                }


            }
            else
            {
                //Utility.ShowCustomMessage(MessageTypes.Caution, "CAUTION", "请选择公司");
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), "请选择公司",
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);
            }
            if (action == FormTypes.Edit)
                EditFlag = true;
            SecondCompany.DisplayMemberPath = "CNAME";
        }
        /// <summary>
        /// 删除所有项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DelAllBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var obj in SecondCompany.Items)
            {
                T_HR_COMPANY selectcompany = new T_HR_COMPANY();
                selectcompany = obj as T_HR_COMPANY;

                //FirstComany.Items.Add(selectcompany);
                if (!(FirstComany.Items.Contains(selectcompany)))    //zl
                {
                    FirstComany.Items.Add(selectcompany);
                }

            }
            if (action == FormTypes.Edit)
                EditFlag = true;
            SecondCompany.Items.Clear();
            this.FirstComany.DisplayMemberPath = "CNAME";

        }

        void OrgClient_GetCompanyActivedCompleted(object sender, GetCompanyActivedCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                //this.FirstComany.ItemsSource = e.Result;
                FirstComany.Items.Clear();
                foreach (var obj in e.Result)
                {
                    FirstComany.Items.Add(obj as T_HR_COMPANY);
                }
                ListCompany = e.Result.ToList();
                LoadSolutionInfos();
            }
        }

        void client_AddTravleSolutionSetCompleted(object sender, AddTravleSolutionSetCompletedEventArgs e)
        {
            RefreshUI(RefreshedTypes.HideProgressBar);

            try
            {
                if (e.Result == "")
                {
                    //Utility.ShowCustomMessage(MessageTypes.Message, Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("ADDDATASUCCESSED"));
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("SUCCESSED"), Utility.GetResourceStr("ADDDATASUCCESSED"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
                    action = FormTypes.Edit;
                    //cmbSolution.SelectedIndex = 0;
                }
                else
                {
                    //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), e.Error.Message);
                    ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr(e.Error.Message),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Error);

                }


            }
            catch (Exception ex)
            {
                //Utility.ShowCustomMessage(MessageTypes.Error, Utility.GetResourceStr("ERROR"), ex.ToString());
                ComfirmWindow.ConfirmationBoxs(Utility.GetResourceStr("ERROR"), Utility.GetResourceStr("ERROR"),
                    Utility.GetResourceStr("CONFIRM"), MessageIcon.Information);
            }
        }

        void client_GetTravleSolutionSetBySolutionIDCompleted(object sender, GetTravleSolutionSetBySolutionIDCompletedEventArgs e)
        {
            loadbar.Stop();
            if (!e.Cancelled)
            {
                if (e.Error == null)
                {
                    if (e.UserState == "DefaultSolution" || isChange)
                        SecondCompany.Items.Clear();            
                    if (e.Result.Count() > 0)
                    {
                        ListAppSolution = e.Result.ToList();               
                        foreach (var h in ListAppSolution)
                        {
                            var ents = from ent in ListCompany
                                       where ent.COMPANYID == h.COMPANYID
                                       select ent;
                            if (ents.Count() > 0)
                            {
                                if (e.UserState == "DefaultSolution" || isChange)
                                {
                                    SecondCompany.Items.Add(ents.FirstOrDefault());
                                }
                                FirstComany.Items.Remove(ents.FirstOrDefault());
                            }
                        }

                    }
                }
            }
        }

        private void AddSetSolution()
        {
            companyids.Clear();
            if (SecondCompany.Items.Count > 0)
            {
                foreach (var obj in SecondCompany.Items)
                {
                    string companyid = "";
                    companyid = (obj as T_HR_COMPANY).COMPANYID;
                    if (companyids.Count() > 0)
                    {
                        if (!(companyids.IndexOf(companyid) > -1))
                            companyids.Add(companyid);
                    }
                    else
                    {
                        companyids.Add(companyid);
                    }
                }
            }
        }
        #endregion

        #region 保存按钮
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Save();
        }
        #endregion

        //private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        //{
        //    loadbar.Start();
        //    client.GetTravleSolutionSetBySolutionIDAsync(travelObj.TRAVELSOLUTIONSID);
        //}

    }
}
