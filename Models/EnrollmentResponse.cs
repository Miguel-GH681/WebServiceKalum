using System.Runtime.Serialization;

namespace FinalAdoSoap.Models{
    [DataContract]
    public class EnrollmentResponse{
        [DataMember]
        public int Codigo {get;set;}
        [DataMember]
        public string Respuesta {get;set;}
        [DataMember]
        public string Carne {get;set;}
    }
}