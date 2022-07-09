using System.Runtime.Serialization;

namespace FinalAdoSoap.Models{
    [DataContract]
    public class EnrollmentRequest{
        [DataMember]
        public string NoExpediente {get;set;}
        [DataMember]
        public string Ciclo {get;set;}
        [DataMember]
        public string MesInicioPago {get;set;}
        [DataMember]
        public string CarreraId {get;set;}

    }
}