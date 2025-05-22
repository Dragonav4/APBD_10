using APBD_s31722_11.DataLayer.Models;

namespace APBD_s31722_11.Interfaces
{
    public interface IPrescriptionService
    {
        Task<int> AddPrescriptionAsync(PrescriptionRequestDto dto);
        Task<PatientWithPrescriptionsDto> GetPatientWithPrescriptionsAsync(int id);
    }
}