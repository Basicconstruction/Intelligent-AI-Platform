﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using OpenAI;

namespace Intelligent_AI_Platform.Model.platform.app.GenericChat.chat
{
    public class SessionGroup
    {

        private List<Session> _group = new List<Session>();

        public SessionGroup()
        {
            
        }

        public List<Session> Group
        {
            set => _group = value;
            get => _group;
        }

        public void RemoveSession(int index)
        {
            _group.RemoveAt(index);
        }

        public void RemoveSession(Session session)
        {
            if (_group.Contains(session))
            {
                _group.Remove(session);
            }
        }

        public void InsertNewSession()
        {
            _group.Insert(0,new Session(){Theme = Session.DefaultTheme});
        }

        public static void Serialize(SessionGroup sessionGroup,string location)
        {
            var app = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var dataPath = app + "//" + location;
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            var data = dataPath + "//" + "app.sg";
            if (!File.Exists(data))
            {
                using (File.Create(data))
                {
                    
                }
            }

            var json = JsonConvert.SerializeObject(sessionGroup);
            File.WriteAllText(data,json);
        }

        public static SessionGroup LoadSessionGroup(string location)
        {
            var app = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var dataPath = app + "//" + location;
            if (!Directory.Exists(dataPath))
            {
                return null;
            }

            var data = dataPath + "//" + "app.sg";
            if (!File.Exists(data))
            {
                return null;
            }

            try
            {
                var res = JsonConvert.DeserializeObject<SessionGroup>(File.ReadAllText(data));
                if (res!=null&&res.Group.Count < 1)
                {
                    res.Group.Add(new Session(){Theme = Session.DefaultTheme});
                }
                return res;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}









