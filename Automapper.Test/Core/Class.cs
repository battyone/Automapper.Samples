using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FSX
{
    [Serializable]
    [DataContract(Namespace = "http://www.fsxtechnologies.com/iRule/WebService")]
    public sealed class Class : Instance
    {
        //private IList<Property> m_Properties;
        [DataMember]
        public IList<Member> Properties { get; set; }
        //{
        //    get
        //    {
        //        return m_Properties ?? (m_Properties = new List<Property>());
        //    }
        //}
    }
}