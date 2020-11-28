using System;
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
        private string _content;
        private DateTime _time;
        private bool _isreaded=false;

        [DataMember]
        public string Sender
        {
            get { return _sender; }
            set { _sender = value; }
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
        bool SendFile(FileMessage fileMsg, Client receiver);

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
        void ReceiverFile(FileMessage fileMsg, Client receiver);

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
                            clients.Remove(key);
                            LogWrite(ex.ToString());
                            return false;
                        }

                    }

                }
                return true;
            } else { LogWrite("Client already exist.");  }
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


        public bool SendFile(FileMessage fileMsg, Client receiver)
        {
            foreach (Client rcvr in clients.Keys)
            {
                if (rcvr.Email == receiver.Email)
                {
                    Message msg = new Message();
                    msg.Sender = fileMsg.Sender;
                    msg.Content = "I'M SENDING FILE.. " + fileMsg.FileName;

                    IChatCallback rcvrCallback = clients[rcvr];
                    rcvrCallback.ReceiveWhisper(msg, receiver);
                    rcvrCallback.ReceiverFile(fileMsg, receiver);

                    foreach (Client sender in clients.Keys)
                    {
                        if (sender.Email == fileMsg.Sender)
                        {
                            IChatCallback sndrCallback = clients[sender];
                            sndrCallback.ReceiveWhisper(msg, receiver);
                            return true;
                        }
                    }
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
                        this.clients.Remove(c);
                        this.clientList.Remove(c);
                        foreach (IChatCallback callback in clients.Values)
                        {
                            callback.RefreshClients(this.clientList);
                            callback.UserLeave(client);
                        }
                    }
                    return;
                }
            }
        }

        private void LogWrite(string text)
        {
            File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "logs.txt", String.Format("{0}: {1}\n", DateTime.Now, text));
        }
        #endregion
    }
}
