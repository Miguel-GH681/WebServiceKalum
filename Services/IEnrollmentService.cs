using System.ServiceModel;
using FinalAdoSoap.Models;

namespace FinalAdoSoap.Services{
    [ServiceContract]
    public interface IEnrollmentService{
        [OperationContract]
        string Test(string s);
        [OperationContract]
        EnrollmentResponse EnrollmentProcess(EnrollmentRequest er);
    }
}