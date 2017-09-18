using System;
using System.Runtime.Serialization;

namespace FSX
{
    [Serializable]
    [DataContract(Namespace = "http://www.fsxtechnologies.com/iRule/WebService")]
    public sealed class Parameter : Member
    {
        [DataMember]
        public int Position { get; set; }
    }
}
