using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text.RegularExpressions;
using System.Threading;

namespace Service_Dagorlad
{
    [DataContract]
    public class Info
    {
        [DataMember]
        public string ip { get; set; }
        [DataMember]
        public string perhabs_email { get; set; }
        [DataMember]
        public string perhabs_fio { get; set; }
        [DataMember]
        public string username { get; set; }
        [DataMember]
        public DateTime enter_dt { get; set; }
        [DataMember]
        public DateTime update_dt { get; set; }
        [DataMember]
        public bool isanswered { get; set; }
    }

    [DataContract]
    public class Conversations
    {
        [DataMember]
        public string group { get; set; }
        [DataMember]
        public string from { get; set; }
        [DataMember]
        public string to { get; set; }
        [DataMember]
        public DateTime dt { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string email { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public string uri_images { get; set; }
        [DataMember]
        public bool isonline { get; set; }
        [DataMember]
        public bool isread { get; set; }
        [DataMember]
        public List<string> whoreaded_from = new List<string>();
        [DataMember]
        public Dictionary<string,string> files_uploaded = new Dictionary<string, string>();
    }
    [ServiceContract]
    public interface IService_Dagorlad
    {
        [OperationContract]
        bool TestConnection();
        [OperationContract]
        List<Info> Update(Info s);
        [OperationContract]
        void AddMessage(Conversations message);
        [OperationContract]
        List<Conversations> return_Conversation(string from, string to, string group);
        [OperationContract]
        List<Conversations> return_Conversation_Unreaded(string from, string to, string group);
        [OperationContract]
        void Set_Conversation_Readed(string from, string to, string group);
    }
    [ServiceBehavior]
    public class Main_Class : IService_Dagorlad
    {
        public static Timer timer = new Timer(TimerCallBack, null, 0, 1000);
        public static List<Info> list = new List<Info>();

        public List<Info> Update(Info s)
        {
            if (!list.Exists(x => x.ip == s.ip && x.username == s.username && x.enter_dt == s.enter_dt))
                list.Add(s);
            foreach (var o in list)
            {
                if (o.ip == s.ip && o.username == s.username && o.enter_dt == s.enter_dt)
                    o.update_dt = DateTime.Now;

            }
            return list;
        }
        public static void TimerCallBack(object state)
        {
            var temp_list = list.Where(x => (DateTime.Now - x.update_dt).TotalSeconds < 30).ToList();
            try
            {
                File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "logs\\" + DateTime.Now.Date.ToShortDateString().Replace(".", "_").Replace("/", "_").Replace(":", "_") + ".log",
                    DateTime.Now + " - (" + temp_list.Count() + ") \n");
            }
            catch (Exception ex) { File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "logs.log", ex.ToString()); }
            list = temp_list;
        }
        public bool TestConnection()
        {
            return true;
        }

        public static List<Conversations> list_conversations = new List<Conversations>();
        public void AddMessage(Conversations message)
        {
            try
            {
                list_conversations.Add(new Conversations
                {

                    dt = DateTime.Now,
                    name = message.name,
                    email = message.email,
                    message = message.message,
                    isonline = message.isonline,
                    uri_images = message.uri_images,
                    isread = false,
                    from = message.from,
                    to = message.to,
                    group = message.group,
                    files_uploaded=message.files_uploaded,
                });
            }
            catch (Exception ex) { File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "logs.log", ex.ToString()); }
        }
        public List<Conversations> return_Conversation(string from, string to, string group)
        {
            if (list_conversations != null && list_conversations.Count() > 0)
            {
                if (String.IsNullOrEmpty(group))
                {
                    return list_conversations.Where(x => (x.from == to && x.to == from) || (x.from == from && x.to == to)).ToList();
                }
                else
                {
                    return list_conversations.Where(x => x.group == group).ToList();
                }
            }
            return null;
        }

        public List<Conversations> return_Conversation_Unreaded(string from, string to, string group)
        {
            if (list_conversations != null && list_conversations.Count() > 0)
            {
                if (String.IsNullOrEmpty(group))
                {
                    return list_conversations.Where(x => x.from == to && x.to == from && !x.isread).ToList();
                }
                else
                {
                    return list_conversations.Where(x => x.to == to && !x.isread && x.group == group && !x.whoreaded_from.Exists(y => y == from)).ToList();
                }
            }
            return null;
        }
        public void Set_Conversation_Readed(string from, string to, string group)
        {
            if (list_conversations != null && list_conversations.Count() > 0)
            {
                foreach (var x in list_conversations)
                {
                    if (String.IsNullOrEmpty(group))
                    {
                        if (x.from == to && x.to == from)
                        {
                            x.isread = true;
                        }
                    }
                    else
                    {
                        if (x.group == group && !x.whoreaded_from.Exists(y => y == from))
                        {
                            x.whoreaded_from.Add(from);
                        }
                    }
                }
            }
        }
    }
}
