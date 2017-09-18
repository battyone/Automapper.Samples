using System;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace FSX
{
    [Serializable]
    [DataContract(Namespace = "http://www.fsxtechnologies.com/iRule/WebService")]
    public sealed class MemberInfo 
    {
        [DataMember]
        public Member Member { get; set; }

        [DataMember]
        public List<MemberInfo> Properties { get; set; }

        private object m_Value;
        [DataMember]
        public object Value
        {
            get
            {
                if (m_Value == null)
                {
                    return Labo.ServiceModel.Core.Utils.TypeUtils.GetDefaultValueOfType(Member.Type);
                }
                return m_Value;
                //return ConversionUtils.ChangeType(m_Value, Member.Type);
            }
            set
            {
                m_Value = value;
                //SetProperty(ref m_Value, value);
            }
        }
    }
}