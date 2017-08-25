﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace eae_coolkatz
{
    public class XmlManager<T>
    {
        public Type Type;

        public XmlManager()
        {
            Type = typeof(T);
        }
        public T Load(string path)
        {
            T instance;
            using (TextReader reader = new StreamReader(path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                instance = (T)xml.Deserialize(reader);
            }
            return instance;
        }

        public void Save(string path, object obj)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                xml.Serialize(writer, obj);
            }
        }
    }
}
