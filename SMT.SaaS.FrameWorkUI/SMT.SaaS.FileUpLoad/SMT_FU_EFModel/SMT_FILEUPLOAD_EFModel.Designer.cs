﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.269
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

[assembly: global::System.Data.Objects.DataClasses.EdmSchemaAttribute()]

// 原始文件名:
// 生成日期: 2012/08/13 14:09:46
namespace SMT_FU_EFModel
{
    
    /// <summary>
    /// 架构中不存在 SMT_FILEUPLOAD_EFModelContext 的注释。
    /// </summary>
    public partial class SMT_FILEUPLOAD_EFModelContext : global::System.Data.Objects.ObjectContext
    {
        /// <summary>
        /// 请使用应用程序配置文件的“SMT_FILEUPLOAD_EFModelContext”部分中的连接字符串初始化新 SMT_FILEUPLOAD_EFModelContext 对象。
        /// </summary>
        public SMT_FILEUPLOAD_EFModelContext() : 
                base("name=SMT_FILEUPLOAD_EFModelContext", "SMT_FILEUPLOAD_EFModelContext")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// 初始化新的 SMT_FILEUPLOAD_EFModelContext 对象。
        /// </summary>
        public SMT_FILEUPLOAD_EFModelContext(string connectionString) : 
                base(connectionString, "SMT_FILEUPLOAD_EFModelContext")
        {
            this.OnContextCreated();
        }
        /// <summary>
        /// 初始化新的 SMT_FILEUPLOAD_EFModelContext 对象。
        /// </summary>
        public SMT_FILEUPLOAD_EFModelContext(global::System.Data.EntityClient.EntityConnection connection) : 
                base(connection, "SMT_FILEUPLOAD_EFModelContext")
        {
            this.OnContextCreated();
        }
        partial void OnContextCreated();
        /// <summary>
        /// 架构中不存在 SMT_FILELIST 的注释。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Data.Objects.ObjectQuery<SMT_FILELIST> SMT_FILELIST
        {
            get
            {
                if ((this._SMT_FILELIST == null))
                {
                    this._SMT_FILELIST = base.CreateQuery<SMT_FILELIST>("[SMT_FILELIST]");
                }
                return this._SMT_FILELIST;
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Data.Objects.ObjectQuery<SMT_FILELIST> _SMT_FILELIST;
        /// <summary>
        /// 架构中不存在 SMT_FILELIST 的注释。
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public void AddToSMT_FILELIST(SMT_FILELIST sMT_FILELIST)
        {
            base.AddObject("SMT_FILELIST", sMT_FILELIST);
        }
    }
    /// <summary>
    /// 架构中不存在 SMT_FILEUPLOAD_EFModel.SMT_FILELIST 的注释。
    /// </summary>
    /// <KeyProperties>
    /// SMTFILELISTID
    /// </KeyProperties>
    [global::System.Data.Objects.DataClasses.EdmEntityTypeAttribute(NamespaceName="SMT_FILEUPLOAD_EFModel", Name="SMT_FILELIST")]
    [global::System.Runtime.Serialization.DataContractAttribute(IsReference=true)]
    [global::System.Serializable()]
    public partial class SMT_FILELIST : global::System.Data.Objects.DataClasses.EntityObject
    {
        /// <summary>
        /// 创建新的 SMT_FILELIST 对象。
        /// </summary>
        /// <param name="sMTFILELISTID">SMTFILELISTID 的初始值。</param>
        /// <param name="fILENAME">FILENAME 的初始值。</param>
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public static SMT_FILELIST CreateSMT_FILELIST(string sMTFILELISTID, string fILENAME)
        {
            SMT_FILELIST sMT_FILELIST = new SMT_FILELIST();
            sMT_FILELIST.SMTFILELISTID = sMTFILELISTID;
            sMT_FILELIST.FILENAME = fILENAME;
            return sMT_FILELIST;
        }
        /// <summary>
        /// 架构中不存在属性 SMTFILELISTID 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string SMTFILELISTID
        {
            get
            {
                return this._SMTFILELISTID;
            }
            set
            {
                this.OnSMTFILELISTIDChanging(value);
                this.ReportPropertyChanging("SMTFILELISTID");
                this._SMTFILELISTID = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("SMTFILELISTID");
                this.OnSMTFILELISTIDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _SMTFILELISTID;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSMTFILELISTIDChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSMTFILELISTIDChanged();
        /// <summary>
        /// 架构中不存在属性 FILENAME 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute(IsNullable=false)]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string FILENAME
        {
            get
            {
                return this._FILENAME;
            }
            set
            {
                this.OnFILENAMEChanging(value);
                this.ReportPropertyChanging("FILENAME");
                this._FILENAME = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, false);
                this.ReportPropertyChanged("FILENAME");
                this.OnFILENAMEChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _FILENAME;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnFILENAMEChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnFILENAMEChanged();
        /// <summary>
        /// 架构中不存在属性 FILETYPE 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string FILETYPE
        {
            get
            {
                return this._FILETYPE;
            }
            set
            {
                this.OnFILETYPEChanging(value);
                this.ReportPropertyChanging("FILETYPE");
                this._FILETYPE = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("FILETYPE");
                this.OnFILETYPEChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _FILETYPE;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnFILETYPEChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnFILETYPEChanged();
        /// <summary>
        /// 架构中不存在属性 FILEURL 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string FILEURL
        {
            get
            {
                return this._FILEURL;
            }
            set
            {
                this.OnFILEURLChanging(value);
                this.ReportPropertyChanging("FILEURL");
                this._FILEURL = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("FILEURL");
                this.OnFILEURLChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _FILEURL;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnFILEURLChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnFILEURLChanged();
        /// <summary>
        /// 架构中不存在属性 FILESIZE 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Nullable<decimal> FILESIZE
        {
            get
            {
                return this._FILESIZE;
            }
            set
            {
                this.OnFILESIZEChanging(value);
                this.ReportPropertyChanging("FILESIZE");
                this._FILESIZE = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("FILESIZE");
                this.OnFILESIZEChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Nullable<decimal> _FILESIZE;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnFILESIZEChanging(global::System.Nullable<decimal> value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnFILESIZEChanged();
        /// <summary>
        /// 架构中不存在属性 COMPANYCODE 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string COMPANYCODE
        {
            get
            {
                return this._COMPANYCODE;
            }
            set
            {
                this.OnCOMPANYCODEChanging(value);
                this.ReportPropertyChanging("COMPANYCODE");
                this._COMPANYCODE = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("COMPANYCODE");
                this.OnCOMPANYCODEChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _COMPANYCODE;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCOMPANYCODEChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCOMPANYCODEChanged();
        /// <summary>
        /// 架构中不存在属性 COMPANYNAME 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string COMPANYNAME
        {
            get
            {
                return this._COMPANYNAME;
            }
            set
            {
                this.OnCOMPANYNAMEChanging(value);
                this.ReportPropertyChanging("COMPANYNAME");
                this._COMPANYNAME = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("COMPANYNAME");
                this.OnCOMPANYNAMEChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _COMPANYNAME;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCOMPANYNAMEChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCOMPANYNAMEChanged();
        /// <summary>
        /// 架构中不存在属性 SYSTEMCODE 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string SYSTEMCODE
        {
            get
            {
                return this._SYSTEMCODE;
            }
            set
            {
                this.OnSYSTEMCODEChanging(value);
                this.ReportPropertyChanging("SYSTEMCODE");
                this._SYSTEMCODE = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("SYSTEMCODE");
                this.OnSYSTEMCODEChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _SYSTEMCODE;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSYSTEMCODEChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnSYSTEMCODEChanged();
        /// <summary>
        /// 架构中不存在属性 MODELCODE 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string MODELCODE
        {
            get
            {
                return this._MODELCODE;
            }
            set
            {
                this.OnMODELCODEChanging(value);
                this.ReportPropertyChanging("MODELCODE");
                this._MODELCODE = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("MODELCODE");
                this.OnMODELCODEChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _MODELCODE;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnMODELCODEChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnMODELCODEChanged();
        /// <summary>
        /// 架构中不存在属性 APPLICATIONID 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string APPLICATIONID
        {
            get
            {
                return this._APPLICATIONID;
            }
            set
            {
                this.OnAPPLICATIONIDChanging(value);
                this.ReportPropertyChanging("APPLICATIONID");
                this._APPLICATIONID = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("APPLICATIONID");
                this.OnAPPLICATIONIDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _APPLICATIONID;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnAPPLICATIONIDChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnAPPLICATIONIDChanged();
        /// <summary>
        /// 架构中不存在属性 THUMBNAILURL 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string THUMBNAILURL
        {
            get
            {
                return this._THUMBNAILURL;
            }
            set
            {
                this.OnTHUMBNAILURLChanging(value);
                this.ReportPropertyChanging("THUMBNAILURL");
                this._THUMBNAILURL = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("THUMBNAILURL");
                this.OnTHUMBNAILURLChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _THUMBNAILURL;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnTHUMBNAILURLChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnTHUMBNAILURLChanged();
        /// <summary>
        /// 架构中不存在属性 INDEXL 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Nullable<decimal> INDEXL
        {
            get
            {
                return this._INDEXL;
            }
            set
            {
                this.OnINDEXLChanging(value);
                this.ReportPropertyChanging("INDEXL");
                this._INDEXL = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("INDEXL");
                this.OnINDEXLChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Nullable<decimal> _INDEXL;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnINDEXLChanging(global::System.Nullable<decimal> value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnINDEXLChanged();
        /// <summary>
        /// 架构中不存在属性 REMARK 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string REMARK
        {
            get
            {
                return this._REMARK;
            }
            set
            {
                this.OnREMARKChanging(value);
                this.ReportPropertyChanging("REMARK");
                this._REMARK = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("REMARK");
                this.OnREMARKChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _REMARK;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnREMARKChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnREMARKChanged();
        /// <summary>
        /// 架构中不存在属性 CREATETIME 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Nullable<global::System.DateTime> CREATETIME
        {
            get
            {
                return this._CREATETIME;
            }
            set
            {
                this.OnCREATETIMEChanging(value);
                this.ReportPropertyChanging("CREATETIME");
                this._CREATETIME = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("CREATETIME");
                this.OnCREATETIMEChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Nullable<global::System.DateTime> _CREATETIME;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCREATETIMEChanging(global::System.Nullable<global::System.DateTime> value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCREATETIMEChanged();
        /// <summary>
        /// 架构中不存在属性 CREATENAME 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string CREATENAME
        {
            get
            {
                return this._CREATENAME;
            }
            set
            {
                this.OnCREATENAMEChanging(value);
                this.ReportPropertyChanging("CREATENAME");
                this._CREATENAME = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("CREATENAME");
                this.OnCREATENAMEChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _CREATENAME;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCREATENAMEChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCREATENAMEChanged();
        /// <summary>
        /// 架构中不存在属性 UPDATETIME 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public global::System.Nullable<global::System.DateTime> UPDATETIME
        {
            get
            {
                return this._UPDATETIME;
            }
            set
            {
                this.OnUPDATETIMEChanging(value);
                this.ReportPropertyChanging("UPDATETIME");
                this._UPDATETIME = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value);
                this.ReportPropertyChanged("UPDATETIME");
                this.OnUPDATETIMEChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private global::System.Nullable<global::System.DateTime> _UPDATETIME;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnUPDATETIMEChanging(global::System.Nullable<global::System.DateTime> value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnUPDATETIMEChanged();
        /// <summary>
        /// 架构中不存在属性 UPDATENAME 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string UPDATENAME
        {
            get
            {
                return this._UPDATENAME;
            }
            set
            {
                this.OnUPDATENAMEChanging(value);
                this.ReportPropertyChanging("UPDATENAME");
                this._UPDATENAME = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("UPDATENAME");
                this.OnUPDATENAMEChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _UPDATENAME;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnUPDATENAMEChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnUPDATENAMEChanged();
        /// <summary>
        /// 架构中不存在属性 PASSWORD 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string PASSWORD
        {
            get
            {
                return this._PASSWORD;
            }
            set
            {
                this.OnPASSWORDChanging(value);
                this.ReportPropertyChanging("PASSWORD");
                this._PASSWORD = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("PASSWORD");
                this.OnPASSWORDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _PASSWORD;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnPASSWORDChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnPASSWORDChanged();
        /// <summary>
        /// 架构中不存在属性 OWNERID 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string OWNERID
        {
            get
            {
                return this._OWNERID;
            }
            set
            {
                this.OnOWNERIDChanging(value);
                this.ReportPropertyChanging("OWNERID");
                this._OWNERID = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("OWNERID");
                this.OnOWNERIDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _OWNERID;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOWNERIDChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOWNERIDChanged();
        /// <summary>
        /// 架构中不存在属性 OWNERCOMPANYID 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string OWNERCOMPANYID
        {
            get
            {
                return this._OWNERCOMPANYID;
            }
            set
            {
                this.OnOWNERCOMPANYIDChanging(value);
                this.ReportPropertyChanging("OWNERCOMPANYID");
                this._OWNERCOMPANYID = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("OWNERCOMPANYID");
                this.OnOWNERCOMPANYIDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _OWNERCOMPANYID;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOWNERCOMPANYIDChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOWNERCOMPANYIDChanged();
        /// <summary>
        /// 架构中不存在属性 OWNERDEPARTMENTID 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string OWNERDEPARTMENTID
        {
            get
            {
                return this._OWNERDEPARTMENTID;
            }
            set
            {
                this.OnOWNERDEPARTMENTIDChanging(value);
                this.ReportPropertyChanging("OWNERDEPARTMENTID");
                this._OWNERDEPARTMENTID = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("OWNERDEPARTMENTID");
                this.OnOWNERDEPARTMENTIDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _OWNERDEPARTMENTID;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOWNERDEPARTMENTIDChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOWNERDEPARTMENTIDChanged();
        /// <summary>
        /// 架构中不存在属性 OWNERPOSTID 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string OWNERPOSTID
        {
            get
            {
                return this._OWNERPOSTID;
            }
            set
            {
                this.OnOWNERPOSTIDChanging(value);
                this.ReportPropertyChanging("OWNERPOSTID");
                this._OWNERPOSTID = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("OWNERPOSTID");
                this.OnOWNERPOSTIDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _OWNERPOSTID;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOWNERPOSTIDChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnOWNERPOSTIDChanged();
        /// <summary>
        /// 架构中不存在属性 CREATEUSERID 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string CREATEUSERID
        {
            get
            {
                return this._CREATEUSERID;
            }
            set
            {
                this.OnCREATEUSERIDChanging(value);
                this.ReportPropertyChanging("CREATEUSERID");
                this._CREATEUSERID = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("CREATEUSERID");
                this.OnCREATEUSERIDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _CREATEUSERID;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCREATEUSERIDChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnCREATEUSERIDChanged();
        /// <summary>
        /// 架构中不存在属性 FORMID 的任何注释。
        /// </summary>
        [global::System.Data.Objects.DataClasses.EdmScalarPropertyAttribute()]
        [global::System.Runtime.Serialization.DataMemberAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        public string FORMID
        {
            get
            {
                return this._FORMID;
            }
            set
            {
                this.OnFORMIDChanging(value);
                this.ReportPropertyChanging("FORMID");
                this._FORMID = global::System.Data.Objects.DataClasses.StructuralObject.SetValidValue(value, true);
                this.ReportPropertyChanged("FORMID");
                this.OnFORMIDChanged();
            }
        }
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        private string _FORMID;
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnFORMIDChanging(string value);
        [global::System.CodeDom.Compiler.GeneratedCode("System.Data.Entity.Design.EntityClassGenerator", "4.0.0.0")]
        partial void OnFORMIDChanged();
    }
}