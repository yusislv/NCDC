﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

// 
// 此源代码是由 Microsoft.VSDesigner 4.0.30319.42000 版自动生成。
// 
#pragma warning disable 1591

namespace LinkWS.WebR_NCSB {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="yhjyptSoap", Namespace="http://www.yinhai.com/yhjypt/yhjyremotept")]
    public partial class yhjypt : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback yhcallOperationCompleted;
        
        private System.Threading.SendOrPostCallback yhconfirmOperationCompleted;
        
        private System.Threading.SendOrPostCallback yhcancelOperationCompleted;
        
        private System.Threading.SendOrPostCallback tempOperationCompleted;
        
        private System.Threading.SendOrPostCallback cancelOperationCompleted;
        
        private System.Threading.SendOrPostCallback confirmOperationCompleted;
        
        private System.Threading.SendOrPostCallback jylxOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public yhjypt() {
            this.Url = global::LinkWS.Properties.Settings.Default.LinkWS_攀枝花社保服务_WebR_MSSB_InsuranceBusiYHDSService;
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
        public event yhcallCompletedEventHandler yhcallCompleted;
        
        /// <remarks/>
        public event yhconfirmCompletedEventHandler yhconfirmCompleted;
        
        /// <remarks/>
        public event yhcancelCompletedEventHandler yhcancelCompleted;
        
        /// <remarks/>
        public event tempCompletedEventHandler tempCompleted;
        
        /// <remarks/>
        public event cancelCompletedEventHandler cancelCompleted;
        
        /// <remarks/>
        public event confirmCompletedEventHandler confirmCompleted;
        
        /// <remarks/>
        public event jylxCompletedEventHandler jylxCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.yinhai.com/yhjypt/yhjyremotept/yhcall", RequestNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", ResponseNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void yhcall(string astr_jyh, string astr_jylsh, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] byte[] astr_jysj, string astr_lydz, string astr_ydz, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] ref byte[] astr_jyjg, ref int aint_clbz, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] ref byte[] astr_clxx) {
            object[] results = this.Invoke("yhcall", new object[] {
                        astr_jyh,
                        astr_jylsh,
                        astr_jysj,
                        astr_lydz,
                        astr_ydz,
                        astr_jyjg,
                        aint_clbz,
                        astr_clxx});
            astr_jyjg = ((byte[])(results[0]));
            aint_clbz = ((int)(results[1]));
            astr_clxx = ((byte[])(results[2]));
        }
        
        /// <remarks/>
        public void yhcallAsync(string astr_jyh, string astr_jylsh, byte[] astr_jysj, string astr_lydz, string astr_ydz, byte[] astr_jyjg, int aint_clbz, byte[] astr_clxx) {
            this.yhcallAsync(astr_jyh, astr_jylsh, astr_jysj, astr_lydz, astr_ydz, astr_jyjg, aint_clbz, astr_clxx, null);
        }
        
        /// <remarks/>
        public void yhcallAsync(string astr_jyh, string astr_jylsh, byte[] astr_jysj, string astr_lydz, string astr_ydz, byte[] astr_jyjg, int aint_clbz, byte[] astr_clxx, object userState) {
            if ((this.yhcallOperationCompleted == null)) {
                this.yhcallOperationCompleted = new System.Threading.SendOrPostCallback(this.OnyhcallOperationCompleted);
            }
            this.InvokeAsync("yhcall", new object[] {
                        astr_jyh,
                        astr_jylsh,
                        astr_jysj,
                        astr_lydz,
                        astr_ydz,
                        astr_jyjg,
                        aint_clbz,
                        astr_clxx}, this.yhcallOperationCompleted, userState);
        }
        
        private void OnyhcallOperationCompleted(object arg) {
            if ((this.yhcallCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.yhcallCompleted(this, new yhcallCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.yinhai.com/yhjypt/yhjyremotept/yhconfirm", RequestNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", ResponseNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void yhconfirm(string astr_jylsh, string astr_lydz, string astr_ydz, ref int aint_clbz, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] ref byte[] astr_clxx) {
            object[] results = this.Invoke("yhconfirm", new object[] {
                        astr_jylsh,
                        astr_lydz,
                        astr_ydz,
                        aint_clbz,
                        astr_clxx});
            aint_clbz = ((int)(results[0]));
            astr_clxx = ((byte[])(results[1]));
        }
        
        /// <remarks/>
        public void yhconfirmAsync(string astr_jylsh, string astr_lydz, string astr_ydz, int aint_clbz, byte[] astr_clxx) {
            this.yhconfirmAsync(astr_jylsh, astr_lydz, astr_ydz, aint_clbz, astr_clxx, null);
        }
        
        /// <remarks/>
        public void yhconfirmAsync(string astr_jylsh, string astr_lydz, string astr_ydz, int aint_clbz, byte[] astr_clxx, object userState) {
            if ((this.yhconfirmOperationCompleted == null)) {
                this.yhconfirmOperationCompleted = new System.Threading.SendOrPostCallback(this.OnyhconfirmOperationCompleted);
            }
            this.InvokeAsync("yhconfirm", new object[] {
                        astr_jylsh,
                        astr_lydz,
                        astr_ydz,
                        aint_clbz,
                        astr_clxx}, this.yhconfirmOperationCompleted, userState);
        }
        
        private void OnyhconfirmOperationCompleted(object arg) {
            if ((this.yhconfirmCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.yhconfirmCompleted(this, new yhconfirmCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.yinhai.com/yhjypt/yhjyremotept/yhcancel", RequestNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", ResponseNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public void yhcancel(string astr_jylsh, string astr_lydz, string astr_ydz, ref int aint_clbz, [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")] ref byte[] astr_clxx) {
            object[] results = this.Invoke("yhcancel", new object[] {
                        astr_jylsh,
                        astr_lydz,
                        astr_ydz,
                        aint_clbz,
                        astr_clxx});
            aint_clbz = ((int)(results[0]));
            astr_clxx = ((byte[])(results[1]));
        }
        
        /// <remarks/>
        public void yhcancelAsync(string astr_jylsh, string astr_lydz, string astr_ydz, int aint_clbz, byte[] astr_clxx) {
            this.yhcancelAsync(astr_jylsh, astr_lydz, astr_ydz, aint_clbz, astr_clxx, null);
        }
        
        /// <remarks/>
        public void yhcancelAsync(string astr_jylsh, string astr_lydz, string astr_ydz, int aint_clbz, byte[] astr_clxx, object userState) {
            if ((this.yhcancelOperationCompleted == null)) {
                this.yhcancelOperationCompleted = new System.Threading.SendOrPostCallback(this.OnyhcancelOperationCompleted);
            }
            this.InvokeAsync("yhcancel", new object[] {
                        astr_jylsh,
                        astr_lydz,
                        astr_ydz,
                        aint_clbz,
                        astr_clxx}, this.yhcancelOperationCompleted, userState);
        }
        
        private void OnyhcancelOperationCompleted(object arg) {
            if ((this.yhcancelCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.yhcancelCompleted(this, new yhcancelCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.yinhai.com/yhjypt/yhjyremotept/temp", RequestNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", ResponseNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public object temp(string astr_jyh, string astr_jylsh, string astr_jysj) {
            object[] results = this.Invoke("temp", new object[] {
                        astr_jyh,
                        astr_jylsh,
                        astr_jysj});
            return ((object)(results[0]));
        }
        
        /// <remarks/>
        public void tempAsync(string astr_jyh, string astr_jylsh, string astr_jysj) {
            this.tempAsync(astr_jyh, astr_jylsh, astr_jysj, null);
        }
        
        /// <remarks/>
        public void tempAsync(string astr_jyh, string astr_jylsh, string astr_jysj, object userState) {
            if ((this.tempOperationCompleted == null)) {
                this.tempOperationCompleted = new System.Threading.SendOrPostCallback(this.OntempOperationCompleted);
            }
            this.InvokeAsync("temp", new object[] {
                        astr_jyh,
                        astr_jylsh,
                        astr_jysj}, this.tempOperationCompleted, userState);
        }
        
        private void OntempOperationCompleted(object arg) {
            if ((this.tempCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.tempCompleted(this, new tempCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.yinhai.com/yhjypt/yhjyremotept/cancel", RequestNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", ResponseNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public object cancel(string astr_jylsh) {
            object[] results = this.Invoke("cancel", new object[] {
                        astr_jylsh});
            return ((object)(results[0]));
        }
        
        /// <remarks/>
        public void cancelAsync(string astr_jylsh) {
            this.cancelAsync(astr_jylsh, null);
        }
        
        /// <remarks/>
        public void cancelAsync(string astr_jylsh, object userState) {
            if ((this.cancelOperationCompleted == null)) {
                this.cancelOperationCompleted = new System.Threading.SendOrPostCallback(this.OncancelOperationCompleted);
            }
            this.InvokeAsync("cancel", new object[] {
                        astr_jylsh}, this.cancelOperationCompleted, userState);
        }
        
        private void OncancelOperationCompleted(object arg) {
            if ((this.cancelCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.cancelCompleted(this, new cancelCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.yinhai.com/yhjypt/yhjyremotept/confirm", RequestNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", ResponseNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public object confirm(string astr_jylsh) {
            object[] results = this.Invoke("confirm", new object[] {
                        astr_jylsh});
            return ((object)(results[0]));
        }
        
        /// <remarks/>
        public void confirmAsync(string astr_jylsh) {
            this.confirmAsync(astr_jylsh, null);
        }
        
        /// <remarks/>
        public void confirmAsync(string astr_jylsh, object userState) {
            if ((this.confirmOperationCompleted == null)) {
                this.confirmOperationCompleted = new System.Threading.SendOrPostCallback(this.OnconfirmOperationCompleted);
            }
            this.InvokeAsync("confirm", new object[] {
                        astr_jylsh}, this.confirmOperationCompleted, userState);
        }
        
        private void OnconfirmOperationCompleted(object arg) {
            if ((this.confirmCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.confirmCompleted(this, new confirmCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://www.yinhai.com/yhjypt/yhjyremotept/jylx", RequestNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", ResponseNamespace="http://www.yinhai.com/yhjypt/yhjyremotept", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public object jylx(string astr_jyh) {
            object[] results = this.Invoke("jylx", new object[] {
                        astr_jyh});
            return ((object)(results[0]));
        }
        
        /// <remarks/>
        public void jylxAsync(string astr_jyh) {
            this.jylxAsync(astr_jyh, null);
        }
        
        /// <remarks/>
        public void jylxAsync(string astr_jyh, object userState) {
            if ((this.jylxOperationCompleted == null)) {
                this.jylxOperationCompleted = new System.Threading.SendOrPostCallback(this.OnjylxOperationCompleted);
            }
            this.InvokeAsync("jylx", new object[] {
                        astr_jyh}, this.jylxOperationCompleted, userState);
        }
        
        private void OnjylxOperationCompleted(object arg) {
            if ((this.jylxCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.jylxCompleted(this, new jylxCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void yhcallCompletedEventHandler(object sender, yhcallCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class yhcallCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal yhcallCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public byte[] astr_jyjg {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[0]));
            }
        }
        
        /// <remarks/>
        public int aint_clbz {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[1]));
            }
        }
        
        /// <remarks/>
        public byte[] astr_clxx {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[2]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void yhconfirmCompletedEventHandler(object sender, yhconfirmCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class yhconfirmCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal yhconfirmCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int aint_clbz {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public byte[] astr_clxx {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void yhcancelCompletedEventHandler(object sender, yhcancelCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class yhcancelCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal yhcancelCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public int aint_clbz {
            get {
                this.RaiseExceptionIfNecessary();
                return ((int)(this.results[0]));
            }
        }
        
        /// <remarks/>
        public byte[] astr_clxx {
            get {
                this.RaiseExceptionIfNecessary();
                return ((byte[])(this.results[1]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void tempCompletedEventHandler(object sender, tempCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class tempCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal tempCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void cancelCompletedEventHandler(object sender, cancelCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class cancelCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal cancelCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void confirmCompletedEventHandler(object sender, confirmCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class confirmCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal confirmCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    public delegate void jylxCompletedEventHandler(object sender, jylxCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.8.3752.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class jylxCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal jylxCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public object Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((object)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591