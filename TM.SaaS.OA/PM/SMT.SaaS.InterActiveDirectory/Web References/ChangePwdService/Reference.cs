﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.225
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.225 版自动生成。
// 
#pragma warning disable 1591

namespace InterActiveDirectory.ChangePwdService {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.ComponentModel;
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ChangePWDSoap", Namespace="http://tempuri.org/")]
    public partial class ChangePWD : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback GetConnStringOperationCompleted;
        
        private System.Threading.SendOrPostCallback Exchange_PassWord_hlsOperationCompleted;
        
        private System.Threading.SendOrPostCallback Exchange_PassWord_erpOperationCompleted;
        
        private System.Threading.SendOrPostCallback Exchange_PassWord_kmoaOperationCompleted;
        
        private System.Threading.SendOrPostCallback Exchange_PassWord_RTXOperationCompleted;
        
        private System.Threading.SendOrPostCallback HelloWorldOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public ChangePWD() {
            this.Url = global::InterActiveDirectory.Properties.Settings.Default.InterActiveDirectory_com_sinomaster_cttest_ChangePWD;
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event GetConnStringCompletedEventHandler GetConnStringCompleted;
        
        /// <remarks/>
        public event Exchange_PassWord_hlsCompletedEventHandler Exchange_PassWord_hlsCompleted;
        
        /// <remarks/>
        public event Exchange_PassWord_erpCompletedEventHandler Exchange_PassWord_erpCompleted;
        
        /// <remarks/>
        public event Exchange_PassWord_kmoaCompletedEventHandler Exchange_PassWord_kmoaCompleted;
        
        /// <remarks/>
        public event Exchange_PassWord_RTXCompletedEventHandler Exchange_PassWord_RTXCompleted;
        
        /// <remarks/>
        public event HelloWorldCompletedEventHandler HelloWorldCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/GetConnString", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string GetConnString() {
            object[] results = this.Invoke("GetConnString", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginGetConnString(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("GetConnString", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public string EndGetConnString(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void GetConnStringAsync() {
            this.GetConnStringAsync(null);
        }
        
        /// <remarks/>
        public void GetConnStringAsync(object userState) {
            if ((this.GetConnStringOperationCompleted == null)) {
                this.GetConnStringOperationCompleted = new System.Threading.SendOrPostCallback(this.OnGetConnStringOperationCompleted);
            }
            this.InvokeAsync("GetConnString", new object[0], this.GetConnStringOperationCompleted, userState);
        }
        
        private void OnGetConnStringOperationCompleted(object arg) {
            if ((this.GetConnStringCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.GetConnStringCompleted(this, new GetConnStringCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Exchange_PassWord_hls", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool Exchange_PassWord_hls(string userid, string NewPassWord, string OldPassWord, out string err) {
            object[] results = this.Invoke("Exchange_PassWord_hls", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord});
            err = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginExchange_PassWord_hls(string userid, string NewPassWord, string OldPassWord, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Exchange_PassWord_hls", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndExchange_PassWord_hls(System.IAsyncResult asyncResult, out string err) {
            object[] results = this.EndInvoke(asyncResult);
            err = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void Exchange_PassWord_hlsAsync(string userid, string NewPassWord, string OldPassWord) {
            this.Exchange_PassWord_hlsAsync(userid, NewPassWord, OldPassWord, null);
        }
        
        /// <remarks/>
        public void Exchange_PassWord_hlsAsync(string userid, string NewPassWord, string OldPassWord, object userState) {
            if ((this.Exchange_PassWord_hlsOperationCompleted == null)) {
                this.Exchange_PassWord_hlsOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExchange_PassWord_hlsOperationCompleted);
            }
            this.InvokeAsync("Exchange_PassWord_hls", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord}, this.Exchange_PassWord_hlsOperationCompleted, userState);
        }
        
        private void OnExchange_PassWord_hlsOperationCompleted(object arg) {
            if ((this.Exchange_PassWord_hlsCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Exchange_PassWord_hlsCompleted(this, new Exchange_PassWord_hlsCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Exchange_PassWord_erp", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool Exchange_PassWord_erp(string userid, string NewPassWord, string OldPassWord, out string err) {
            object[] results = this.Invoke("Exchange_PassWord_erp", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord});
            err = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginExchange_PassWord_erp(string userid, string NewPassWord, string OldPassWord, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Exchange_PassWord_erp", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndExchange_PassWord_erp(System.IAsyncResult asyncResult, out string err) {
            object[] results = this.EndInvoke(asyncResult);
            err = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void Exchange_PassWord_erpAsync(string userid, string NewPassWord, string OldPassWord) {
            this.Exchange_PassWord_erpAsync(userid, NewPassWord, OldPassWord, null);
        }
        
        /// <remarks/>
        public void Exchange_PassWord_erpAsync(string userid, string NewPassWord, string OldPassWord, object userState) {
            if ((this.Exchange_PassWord_erpOperationCompleted == null)) {
                this.Exchange_PassWord_erpOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExchange_PassWord_erpOperationCompleted);
            }
            this.InvokeAsync("Exchange_PassWord_erp", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord}, this.Exchange_PassWord_erpOperationCompleted, userState);
        }
        
        private void OnExchange_PassWord_erpOperationCompleted(object arg) {
            if ((this.Exchange_PassWord_erpCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Exchange_PassWord_erpCompleted(this, new Exchange_PassWord_erpCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Exchange_PassWord_kmoa", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public bool Exchange_PassWord_kmoa(string userid, string NewPassWord, string OldPassWord, out string err) {
            object[] results = this.Invoke("Exchange_PassWord_kmoa", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord});
            err = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginExchange_PassWord_kmoa(string userid, string NewPassWord, string OldPassWord, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Exchange_PassWord_kmoa", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord}, callback, asyncState);
        }
        
        /// <remarks/>
        public bool EndExchange_PassWord_kmoa(System.IAsyncResult asyncResult, out string err) {
            object[] results = this.EndInvoke(asyncResult);
            err = ((string)(results[1]));
            return ((bool)(results[0]));
        }
        
        /// <remarks/>
        public void Exchange_PassWord_kmoaAsync(string userid, string NewPassWord, string OldPassWord) {
            this.Exchange_PassWord_kmoaAsync(userid, NewPassWord, OldPassWord, null);
        }
        
        /// <remarks/>
        public void Exchange_PassWord_kmoaAsync(string userid, string NewPassWord, string OldPassWord, object userState) {
            if ((this.Exchange_PassWord_kmoaOperationCompleted == null)) {
                this.Exchange_PassWord_kmoaOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExchange_PassWord_kmoaOperationCompleted);
            }
            this.InvokeAsync("Exchange_PassWord_kmoa", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord}, this.Exchange_PassWord_kmoaOperationCompleted, userState);
        }
        
        private void OnExchange_PassWord_kmoaOperationCompleted(object arg) {
            if ((this.Exchange_PassWord_kmoaCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Exchange_PassWord_kmoaCompleted(this, new Exchange_PassWord_kmoaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/Exchange_PassWord_RTX", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Exchange_PassWord_RTX(string userid, string NewPassWord, string OldPassWord) {
            object[] results = this.Invoke("Exchange_PassWord_RTX", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginExchange_PassWord_RTX(string userid, string NewPassWord, string OldPassWord, System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("Exchange_PassWord_RTX", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord}, callback, asyncState);
        }
        
        /// <remarks/>
        public string EndExchange_PassWord_RTX(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void Exchange_PassWord_RTXAsync(string userid, string NewPassWord, string OldPassWord) {
            this.Exchange_PassWord_RTXAsync(userid, NewPassWord, OldPassWord, null);
        }
        
        /// <remarks/>
        public void Exchange_PassWord_RTXAsync(string userid, string NewPassWord, string OldPassWord, object userState) {
            if ((this.Exchange_PassWord_RTXOperationCompleted == null)) {
                this.Exchange_PassWord_RTXOperationCompleted = new System.Threading.SendOrPostCallback(this.OnExchange_PassWord_RTXOperationCompleted);
            }
            this.InvokeAsync("Exchange_PassWord_RTX", new object[] {
                        userid,
                        NewPassWord,
                        OldPassWord}, this.Exchange_PassWord_RTXOperationCompleted, userState);
        }
        
        private void OnExchange_PassWord_RTXOperationCompleted(object arg) {
            if ((this.Exchange_PassWord_RTXCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.Exchange_PassWord_RTXCompleted(this, new Exchange_PassWord_RTXCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/HelloWorld", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string HelloWorld() {
            object[] results = this.Invoke("HelloWorld", new object[0]);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public System.IAsyncResult BeginHelloWorld(System.AsyncCallback callback, object asyncState) {
            return this.BeginInvoke("HelloWorld", new object[0], callback, asyncState);
        }
        
        /// <remarks/>
        public string EndHelloWorld(System.IAsyncResult asyncResult) {
            object[] results = this.EndInvoke(asyncResult);
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void HelloWorldAsync() {
            this.HelloWorldAsync(null);
        }
        
        /// <remarks/>
        public void HelloWorldAsync(object userState) {
            if ((this.HelloWorldOperationCompleted == null)) {
                this.HelloWorldOperationCompleted = new System.Threading.SendOrPostCallback(this.OnHelloWorldOperationCompleted);
            }
            this.InvokeAsync("HelloWorld", new object[0], this.HelloWorldOperationCompleted, userState);
        }
        
        private void OnHelloWorldOperationCompleted(object arg) {
            if ((this.HelloWorldCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.HelloWorldCompleted(this, new HelloWorldCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void GetConnStringCompletedEventHandler(object sender, GetConnStringCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class GetConnStringCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal GetConnStringCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void Exchange_PassWord_hlsCompletedEventHandler(object sender, Exchange_PassWord_hlsCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Exchange_PassWord_hlsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Exchange_PassWord_hlsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string err {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void Exchange_PassWord_erpCompletedEventHandler(object sender, Exchange_PassWord_erpCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Exchange_PassWord_erpCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Exchange_PassWord_erpCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string err {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void Exchange_PassWord_kmoaCompletedEventHandler(object sender, Exchange_PassWord_kmoaCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Exchange_PassWord_kmoaCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Exchange_PassWord_kmoaCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public bool Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((bool)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public string err {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void Exchange_PassWord_RTXCompletedEventHandler(object sender, Exchange_PassWord_RTXCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class Exchange_PassWord_RTXCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal Exchange_PassWord_RTXCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    public delegate void HelloWorldCompletedEventHandler(object sender, HelloWorldCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class HelloWorldCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal HelloWorldCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591