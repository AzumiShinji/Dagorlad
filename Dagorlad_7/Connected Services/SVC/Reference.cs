﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dagorlad_7.SVC {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Client", Namespace="http://schemas.datacontract.org/2004/07/Service_Chat_Dagorlad")]
    [System.SerializableAttribute()]
    public partial class Client : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Nullable<int> CountUnreadedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string DirectionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LastMessageField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string NameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StatusField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SystemInformationField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TimeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Nullable<int> CountUnreaded {
            get {
                return this.CountUnreadedField;
            }
            set {
                if ((this.CountUnreadedField.Equals(value) != true)) {
                    this.CountUnreadedField = value;
                    this.RaisePropertyChanged("CountUnreaded");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Direction {
            get {
                return this.DirectionField;
            }
            set {
                if ((object.ReferenceEquals(this.DirectionField, value) != true)) {
                    this.DirectionField = value;
                    this.RaisePropertyChanged("Direction");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Email {
            get {
                return this.EmailField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailField, value) != true)) {
                    this.EmailField = value;
                    this.RaisePropertyChanged("Email");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LastMessage {
            get {
                return this.LastMessageField;
            }
            set {
                if ((object.ReferenceEquals(this.LastMessageField, value) != true)) {
                    this.LastMessageField = value;
                    this.RaisePropertyChanged("LastMessage");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Status {
            get {
                return this.StatusField;
            }
            set {
                if ((object.ReferenceEquals(this.StatusField, value) != true)) {
                    this.StatusField = value;
                    this.RaisePropertyChanged("Status");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SystemInformation {
            get {
                return this.SystemInformationField;
            }
            set {
                if ((object.ReferenceEquals(this.SystemInformationField, value) != true)) {
                    this.SystemInformationField = value;
                    this.RaisePropertyChanged("SystemInformation");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Time {
            get {
                return this.TimeField;
            }
            set {
                if ((this.TimeField.Equals(value) != true)) {
                    this.TimeField = value;
                    this.RaisePropertyChanged("Time");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Message", Namespace="http://schemas.datacontract.org/2004/07/Service_Chat_Dagorlad")]
    [System.SerializableAttribute()]
    public partial class Message : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ContentField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.Collections.Generic.Dictionary<string, string> FileLinksField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsFileField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsReadedField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsStickerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LinkStickerField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SenderField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SenderNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TimeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Content {
            get {
                return this.ContentField;
            }
            set {
                if ((object.ReferenceEquals(this.ContentField, value) != true)) {
                    this.ContentField = value;
                    this.RaisePropertyChanged("Content");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.Dictionary<string, string> FileLinks {
            get {
                return this.FileLinksField;
            }
            set {
                if ((object.ReferenceEquals(this.FileLinksField, value) != true)) {
                    this.FileLinksField = value;
                    this.RaisePropertyChanged("FileLinks");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsFile {
            get {
                return this.IsFileField;
            }
            set {
                if ((this.IsFileField.Equals(value) != true)) {
                    this.IsFileField = value;
                    this.RaisePropertyChanged("IsFile");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsReaded {
            get {
                return this.IsReadedField;
            }
            set {
                if ((this.IsReadedField.Equals(value) != true)) {
                    this.IsReadedField = value;
                    this.RaisePropertyChanged("IsReaded");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsSticker {
            get {
                return this.IsStickerField;
            }
            set {
                if ((this.IsStickerField.Equals(value) != true)) {
                    this.IsStickerField = value;
                    this.RaisePropertyChanged("IsSticker");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LinkSticker {
            get {
                return this.LinkStickerField;
            }
            set {
                if ((object.ReferenceEquals(this.LinkStickerField, value) != true)) {
                    this.LinkStickerField = value;
                    this.RaisePropertyChanged("LinkSticker");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Sender {
            get {
                return this.SenderField;
            }
            set {
                if ((object.ReferenceEquals(this.SenderField, value) != true)) {
                    this.SenderField = value;
                    this.RaisePropertyChanged("Sender");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string SenderName {
            get {
                return this.SenderNameField;
            }
            set {
                if ((object.ReferenceEquals(this.SenderNameField, value) != true)) {
                    this.SenderNameField = value;
                    this.RaisePropertyChanged("SenderName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Time {
            get {
                return this.TimeField;
            }
            set {
                if ((this.TimeField.Equals(value) != true)) {
                    this.TimeField = value;
                    this.RaisePropertyChanged("Time");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="FileMessage", Namespace="http://schemas.datacontract.org/2004/07/Service_Chat_Dagorlad")]
    [System.SerializableAttribute()]
    public partial class FileMessage : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private byte[] DataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string FileNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string SenderField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private System.DateTime TimeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public byte[] Data {
            get {
                return this.DataField;
            }
            set {
                if ((object.ReferenceEquals(this.DataField, value) != true)) {
                    this.DataField = value;
                    this.RaisePropertyChanged("Data");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string FileName {
            get {
                return this.FileNameField;
            }
            set {
                if ((object.ReferenceEquals(this.FileNameField, value) != true)) {
                    this.FileNameField = value;
                    this.RaisePropertyChanged("FileName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Sender {
            get {
                return this.SenderField;
            }
            set {
                if ((object.ReferenceEquals(this.SenderField, value) != true)) {
                    this.SenderField = value;
                    this.RaisePropertyChanged("Sender");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.DateTime Time {
            get {
                return this.TimeField;
            }
            set {
                if ((this.TimeField.Equals(value) != true)) {
                    this.TimeField = value;
                    this.RaisePropertyChanged("Time");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="SVC.IChat", CallbackContract=typeof(Dagorlad_7.SVC.IChatCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IChat {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Connect", ReplyAction="http://tempuri.org/IChat/ConnectResponse")]
        bool Connect(Dagorlad_7.SVC.Client client);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/Connect", ReplyAction="http://tempuri.org/IChat/ConnectResponse")]
        System.Threading.Tasks.Task<bool> ConnectAsync(Dagorlad_7.SVC.Client client);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/Say")]
        void Say(Dagorlad_7.SVC.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/Say")]
        System.Threading.Tasks.Task SayAsync(Dagorlad_7.SVC.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/Whisper")]
        void Whisper(Dagorlad_7.SVC.Message msg, Dagorlad_7.SVC.Client receiver);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/Whisper")]
        System.Threading.Tasks.Task WhisperAsync(Dagorlad_7.SVC.Message msg, Dagorlad_7.SVC.Client receiver);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/IsWriting")]
        void IsWriting(Dagorlad_7.SVC.Client client);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/IsWriting")]
        System.Threading.Tasks.Task IsWritingAsync(Dagorlad_7.SVC.Client client);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/SendFile", ReplyAction="http://tempuri.org/IChat/SendFileResponse")]
        bool SendFile(Dagorlad_7.SVC.FileMessage fileMsg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/SendFile", ReplyAction="http://tempuri.org/IChat/SendFileResponse")]
        System.Threading.Tasks.Task<bool> SendFileAsync(Dagorlad_7.SVC.FileMessage fileMsg);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/SendFileWhisper", ReplyAction="http://tempuri.org/IChat/SendFileWhisperResponse")]
        bool SendFileWhisper(Dagorlad_7.SVC.FileMessage fileMsg, Dagorlad_7.SVC.Client receiver);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChat/SendFileWhisper", ReplyAction="http://tempuri.org/IChat/SendFileWhisperResponse")]
        System.Threading.Tasks.Task<bool> SendFileWhisperAsync(Dagorlad_7.SVC.FileMessage fileMsg, Dagorlad_7.SVC.Client receiver);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, IsTerminating=true, Action="http://tempuri.org/IChat/Disconnect")]
        void Disconnect(Dagorlad_7.SVC.Client client);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, IsTerminating=true, Action="http://tempuri.org/IChat/Disconnect")]
        System.Threading.Tasks.Task DisconnectAsync(Dagorlad_7.SVC.Client client);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/RefreshClients")]
        void RefreshClients(Dagorlad_7.SVC.Client[] clients);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/Receive")]
        void Receive(Dagorlad_7.SVC.Message msg);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/ReceiveWhisper")]
        void ReceiveWhisper(Dagorlad_7.SVC.Message msg, Dagorlad_7.SVC.Client receiver);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/IsWritingCallback")]
        void IsWritingCallback(Dagorlad_7.SVC.Client client);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/ReceiverFile")]
        void ReceiverFile(Dagorlad_7.SVC.FileMessage fileMsg);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/ReceiverFileWhisper")]
        void ReceiverFileWhisper(Dagorlad_7.SVC.FileMessage fileMsg, Dagorlad_7.SVC.Client receiver);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/UserJoin")]
        void UserJoin(Dagorlad_7.SVC.Client client);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChat/UserLeave")]
        void UserLeave(Dagorlad_7.SVC.Client client);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatChannel : Dagorlad_7.SVC.IChat, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ChatClient : System.ServiceModel.DuplexClientBase<Dagorlad_7.SVC.IChat>, Dagorlad_7.SVC.IChat {
        
        public ChatClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ChatClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ChatClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public bool Connect(Dagorlad_7.SVC.Client client) {
            return base.Channel.Connect(client);
        }
        
        public System.Threading.Tasks.Task<bool> ConnectAsync(Dagorlad_7.SVC.Client client) {
            return base.Channel.ConnectAsync(client);
        }
        
        public void Say(Dagorlad_7.SVC.Message msg) {
            base.Channel.Say(msg);
        }
        
        public System.Threading.Tasks.Task SayAsync(Dagorlad_7.SVC.Message msg) {
            return base.Channel.SayAsync(msg);
        }
        
        public void Whisper(Dagorlad_7.SVC.Message msg, Dagorlad_7.SVC.Client receiver) {
            base.Channel.Whisper(msg, receiver);
        }
        
        public System.Threading.Tasks.Task WhisperAsync(Dagorlad_7.SVC.Message msg, Dagorlad_7.SVC.Client receiver) {
            return base.Channel.WhisperAsync(msg, receiver);
        }
        
        public void IsWriting(Dagorlad_7.SVC.Client client) {
            base.Channel.IsWriting(client);
        }
        
        public System.Threading.Tasks.Task IsWritingAsync(Dagorlad_7.SVC.Client client) {
            return base.Channel.IsWritingAsync(client);
        }
        
        public bool SendFile(Dagorlad_7.SVC.FileMessage fileMsg) {
            return base.Channel.SendFile(fileMsg);
        }
        
        public System.Threading.Tasks.Task<bool> SendFileAsync(Dagorlad_7.SVC.FileMessage fileMsg) {
            return base.Channel.SendFileAsync(fileMsg);
        }
        
        public bool SendFileWhisper(Dagorlad_7.SVC.FileMessage fileMsg, Dagorlad_7.SVC.Client receiver) {
            return base.Channel.SendFileWhisper(fileMsg, receiver);
        }
        
        public System.Threading.Tasks.Task<bool> SendFileWhisperAsync(Dagorlad_7.SVC.FileMessage fileMsg, Dagorlad_7.SVC.Client receiver) {
            return base.Channel.SendFileWhisperAsync(fileMsg, receiver);
        }
        
        public void Disconnect(Dagorlad_7.SVC.Client client) {
            base.Channel.Disconnect(client);
        }
        
        public System.Threading.Tasks.Task DisconnectAsync(Dagorlad_7.SVC.Client client) {
            return base.Channel.DisconnectAsync(client);
        }
    }
}
