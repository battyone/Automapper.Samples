using System;
using System.Runtime.Serialization;

namespace FSX
{
    [Serializable]
    [DataContract(Namespace = "http://www.fsxtechnologies.com/iRule/WebService")]
    public class Instance
    {
        private Type _type;
        public Type Type
        {
            get { return _type; }
            set
            {
                if (_type == value) return;
                _type = value;
                //if (value != null)
                //    SerializableType = new SerializableType(value);
            }
        }
        
        //private SerializableType _serializableType;
        //[DataMember]
        //public SerializableType SerializableType
        //{
        //    get { return _serializableType; }
        //    set { _serializableType = value; }
        //}

        [DataMember]
        public Instance Definition { get; set; }

        public bool IsClass => Definition is Class;

        public Class GetClass()
        {
            return Definition as Class;
        }
    }
}