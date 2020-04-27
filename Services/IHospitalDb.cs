using Kollokwium_1.DTOs.Responde;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kollokwium_1.Services
{
    public interface IHospitalDb
    {
        public ICollection<PrescriptionResponde> getPrescriptions(int idLeku);
        public int deletePatientAndInfo(int idPatient);
    }
}
