﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.IO;
using System.Windows.Media.Imaging;

namespace Service_Chat_Dagorlad
{

    [DataContract]
    public class Client
    {
        private string _email;
        private string _name;
        private string _direction;
        private DateTime _time;
        private int? _countunreaded;
        private string _lastmessage;

        [DataMember]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        [DataMember]
        public string Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        [DataMember]
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
        [DataMember]
        public int? CountUnreaded
        {
            get { return _countunreaded; }
            set { _countunreaded = value; }
        }
        [DataMember]
        public string LastMessage
        {
            get { return _lastmessage; }
            set { _lastmessage = value; }
        }
    }

    [DataContract]
    public class Message
    {
        private string _sender;
        private string _sendername;
        private string _content;
        private DateTime _time;
        private bool _isreaded=false;
        private bool _isfile = false;
        private Dictionary<string, string> _filelinks;
        private bool _issticker = false;
        private string _linksticker;
        [DataMember]
        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }
        [DataMember]
        public string SenderName
        {
            get { return _sendername; }
            set { _sendername = value; }
        }
        [DataMember]
        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }
        [DataMember]
        public DateTime Time
        {
            get { return _time; }
            set { _time = value; }
        }
        [DataMember]
        public bool IsReaded
        {
            get { return _isreaded; }
            set { _isreaded = value; }
        }
        [DataMember]
        public bool IsFile
        {
            get { return _isfile; }
            set { _isfile = value; }
        }
        [DataMember]
        public Dictionary<string,string> FileLinks
        {
            get { return _filelinks; }
            set { _filelinks = value; }
        }
        [DataMember]
        public bool IsSticker
        {
            get { return _issticker; }
            set { _issticker = value; }
        }
        [DataMember]
        public string LinkSticker
        {
            get { return _linksticker; }
            set { _linksticker = value; }
        }
    }

    [DataContract]
    public class FileMessage
    {
        private string sender;
        private string fileName;
        private byte[] data;
        private DateTime time;

        [DataMember]
        public string Sender 
        {
            get { return sender; }
            set { sender = value; }
        }

        [DataMember]
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        [DataMember]
        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        [DataMember]
        public DateTime Time
        {
            get { return time; }
            set { time = value; }
        }
    }

    [ServiceContract(CallbackContract = typeof(IChatCallback), SessionMode = SessionMode.Required)]
    public interface IChat
    {
        [OperationContract(IsInitiating = true)]
        bool Connect(Client client);

        [OperationContract(IsOneWay = true)]
        void Say(Message msg);

        [OperationContract(IsOneWay = true)]
        void Whisper(Message msg, Client receiver);

        [OperationContract(IsOneWay = true)]
        void IsWriting(Client client);

        [OperationContract(IsOneWay = false)]
        bool SendFile(FileMessage fileMsg);

        [OperationContract(IsOneWay = false)]
        bool SendFileWhisper(FileMessage fileMsg, Client receiver);

        [OperationContract(IsOneWay = true, IsTerminating = true)]
        void Disconnect(Client client);
    }

    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void RefreshClients(List<Client> clients);

        [OperationContract(IsOneWay = true)]
        void Receive(Message msg);

        [OperationContract(IsOneWay = true)]
        void ReceiveWhisper(Message msg, Client receiver);

        [OperationContract(IsOneWay = true)]
        void IsWritingCallback(Client client);

        [OperationContract(IsOneWay = true)]
        void ReceiverFile(FileMessage fileMsg);

        [OperationContract(IsOneWay = true)]
        void ReceiverFileWhisper(FileMessage fileMsg, Client receiver);

        [OperationContract(IsOneWay = true)]
        void UserJoin(Client client);

        [OperationContract(IsOneWay = true)]
        void UserLeave(Client client);
    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single,
    ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false)]
    public class ChatService : IChat
    {
        Dictionary<Client, IChatCallback> clients = new Dictionary<Client, IChatCallback>();

        List<Client> clientList = new List<Client>();

        public IChatCallback CurrentCallback
        {
            get
            {
                return OperationContext.Current.GetCallbackChannel<IChatCallback>();

            }
        }

        object syncObj = new object();

        private bool IsUniqueClient(string Email)
        {
            foreach (Client c in clients.Keys)
            {
                if (c.Email == Email)
                {
                    return false;
                }
            }
            return true;
        }



        #region IChat Members

        public bool Connect(Client client)
        {
            if (!clients.ContainsValue(CurrentCallback) && IsUniqueClient(client.Email))
            {
                lock (syncObj)
                {
                    clients.Add(client, CurrentCallback);
                    clientList.Add(client);

                    foreach (Client key in clients.Keys)
                    {
                        IChatCallback callback = clients[key];
                        try
                        {
                            callback.RefreshClients(clientList);
                            callback.UserJoin(client);
                        }
                        catch (Exception ex)
                        {
                            LogWrite(ex.ToString());
                            LogWrite("Try remove \"" + client.Email + "\"");
                            clients.Remove(key);
                            return false;
                        }
                    }

                }
                return true;
            }
            else
            {
                LogWrite("Client \""+ client.Email + "\" already exist.");
            }
            return false;
        }

        public void Say(Message msg)
        {
            lock (syncObj)
            {
                foreach (IChatCallback callback in clients.Values)
                {
                    callback.Receive(msg);
                }
            }
        }

        public void Whisper(Message msg, Client receiver)
        {
            foreach (Client rec in clients.Keys)
            {
                if (rec.Email == receiver.Email)
                {
                    IChatCallback callback = clients[rec];
                    callback.ReceiveWhisper(msg, rec);

                    foreach (Client sender in clients.Keys)
                    {
                        if (sender.Email == msg.Sender)
                        {
                            IChatCallback senderCallback = clients[sender];
                            senderCallback.ReceiveWhisper(msg, rec);
                            return;
                        }
                    }
                }
            }
        }

        public bool SendFile(FileMessage fileMsg)
        {
            foreach (Client rcvr in clients.Keys)
            {
                IChatCallback rcvrCallback = clients[rcvr];
                rcvrCallback.ReceiverFile(fileMsg);
                return true;
            }
            return false;
        }
        public bool SendFileWhisper(FileMessage fileMsg, Client receiver)
        {
            foreach (Client rcvr in clients.Keys)
            {
                if (rcvr.Email == receiver.Email)
                {
                    IChatCallback rcvrCallback = clients[rcvr];
                    rcvrCallback.ReceiverFileWhisper(fileMsg, receiver);
                    return true;
                }
            }
            return false;
        }

        public void IsWriting(Client client)
        {
            lock (syncObj)
            {
                foreach (IChatCallback callback in clients.Values)
                {
                    callback.IsWritingCallback(client);
                }
            }
        }

        public void Disconnect(Client client)
        {
            foreach (Client c in clients.Keys)
            {
                if (client.Email == c.Email)
                {
                    lock (syncObj)
                    {
                        try
                        {
                            this.clients.Remove(c);
                            this.clientList.Remove(c);
                            foreach (IChatCallback callback in clients.Values)
                            {
                                callback.RefreshClients(this.clientList);
                                callback.UserLeave(client);
                            }
                            LogWrite(client.Email+" - has been disconnected successfully.");
                        }
                        catch (Exception ex) { LogWrite("Trying disconnecting error: " + ex.ToString()); }
                    }
                    return;
                }
            }
        }

        private void LogWrite(string text)
        {
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "ServerLog.txt", String.Format("{0}: {1}\n", DateTime.Now, text));
        }
        #endregion
    }
}
