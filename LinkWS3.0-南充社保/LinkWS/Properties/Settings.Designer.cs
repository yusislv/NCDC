﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace LinkWS.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "16.4.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://10.49.21.119:7003/web/services/InsuranceBusiYHDS")]
        public string LinkWS_眉山社保社保服务_WebR_MSSB_InsuranceBusiYHDSService {
            get {
                return ((string)(this["LinkWS_眉山社保社保服务_WebR_MSSB_InsuranceBusiYHDSService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://10.49.21.206/yhjypt/yhjypt.asmx")]
        public string LinkWS_攀枝花社保服务_WebR_MSSB_InsuranceBusiYHDSService {
            get {
                return ((string)(this["LinkWS_攀枝花社保服务_WebR_MSSB_InsuranceBusiYHDSService"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://10.49.21.119:7003/web/services/InsuranceBusiYHDS")]
        public string LinkWS_南充社保社保服务_WebR_MSSB_InsuranceBusiYHDSService {
            get {
                return ((string)(this["LinkWS_南充社保社保服务_WebR_MSSB_InsuranceBusiYHDSService"]));
            }
            set {
                this["LinkWS_南充社保社保服务_WebR_MSSB_InsuranceBusiYHDSService"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("http://10.49.21.206/yhjypt/yhjypt.asmx")]
        public string LinkWS_南充社保服务_WebR_MSSB_InsuranceBusiYHDSService {
            get {
                return ((string)(this["LinkWS_南充社保服务_WebR_MSSB_InsuranceBusiYHDSService"]));
            }
            set {
                this["LinkWS_南充社保服务_WebR_MSSB_InsuranceBusiYHDSService"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://localhost:8080/javaEncrypt/serviceEncryptPort")]
        public string LinkWS_南充社保二期服务_javaEncrypt_serviceEncryptService {
            get {
                return ((string)(this["LinkWS_南充社保二期服务_javaEncrypt_serviceEncryptService"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.SpecialSettingAttribute(global::System.Configuration.SpecialSetting.WebServiceUrl)]
        [global::System.Configuration.DefaultSettingValueAttribute("http://10.49.21.206:7003/ysptsz/services/szWebService")]
        public string LinkWS_南充社保二期服务_WebR_NCDC_SYConnWebService {
            get {
                return ((string)(this["LinkWS_南充社保二期服务_WebR_NCDC_SYConnWebService"]));
            }
        }
    }
}
