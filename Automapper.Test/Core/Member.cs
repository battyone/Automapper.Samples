using System;
using System.Runtime.Serialization;

namespace FSX
{
    [Serializable]
    [DataContract(Namespace = "http://www.fsxtechnologies.com/iRule/WebService")]
    public abstract class Member : Instance
    {
        [DataMember]
        public string Name { get; set; }

        //private object m_Value;
        //[DataMember]
        //public object Value
        //{
        //    get
        //    {
        //        if (m_Value == null)
        //        {
        //            return TypeUtils.GetDefaultValueOfType(Type);
        //        }
        //        return m_Value;
        //    }
        //    set
        //    {
        //        m_Value = value;
        //    }
        //}
    }
}