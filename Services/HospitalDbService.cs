using Kollokwium_1.DTOs.Responde;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Kollokwium_1.Services
{
    public class HospitalDbService : IHospitalDb
    { // IConfiguration?
        private readonly string connectionParametr = "Data Source=db-mssql;Initial Catalog=s19314;Integrated Security=True ";
        public ICollection<PrescriptionResponde> getPrescriptions(int idLeku) {
            try
            {
                using (var connection = new SqlConnection(connectionParametr))
                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    connection.Open();
                    command.CommandText = $" SELECT  Dose, Details, Date, DueDate  " +
                                           " FROM Prescription_Medicament " +
                                           " INNER JOIN Prescription " +
                                           " ON Prescription_Medicament.IdPrescription = Prescription.IdPrescription " +
                                           " WHERE idMedicament = @idLeku " +
                                           " ORDER BY Date DESC; ";
                    command.Parameters.AddWithValue("idLeku", idLeku);

                    var dataReader = command.ExecuteReader();
                    ICollection<PrescriptionResponde> responseList = new List<PrescriptionResponde>();
                    while (dataReader.Read())
                    {
                        var prescriptionResponde = new PrescriptionResponde();
                        prescriptionResponde.Date = dataReader["Date"].ToString();
                        prescriptionResponde.DueDate = dataReader["DueDate"].ToString();
                        prescriptionResponde.Dose = dataReader["Dose"].ToString();
                        prescriptionResponde.Details = dataReader["Details"].ToString();
                        responseList.Add(prescriptionResponde);
                    }
                    return responseList;
                }
            }
            catch (SqlException sqlException)
            {
                // sqlException.Message();
                return null;
            }
        }


        public int deletePatientAndInfo(int idPatient) {
            SqlTransaction transaction = null;
            try {
                
                using (var connection = new SqlConnection(connectionParametr))
                using (var command = new SqlCommand()) {
                    command.Connection = connection;
                    transaction = connection.BeginTransaction();

                    command.CommandText = $" SELECT IdPrescription FROM Prescription " +
                                            " WHERE IdPatient = @idPatient ; ";
                    command.Parameters.AddWithValue("idPatient", idPatient);
                    var dataReader = command.ExecuteReader();
                    List<string> idPrescriptions = new List<string>();
                    while (dataReader.Read()) 
                    {
                        idPrescriptions.Add( dataReader["IdPrescription"].ToString());
                    }

                    for(int i = 0; i < idPrescriptions.Count(); i++) 
                    {
                        command.CommandText = $" DELETE FROM Prescription_Medicament  " +
                                               " WHERE IdPrescription = @idPrescri ; ";
                        command.Parameters.AddWithValue("idPrescri", idPrescriptions.ElementAt(i));
                        if (command.ExecuteNonQuery() < 0) throw new Exception();

                        command.CommandText = $" DELETE FROM Prescription   " +
                                               " WHERE IdPatient = @idPatient ; ";
                        command.Parameters.AddWithValue("idPatient", idPatient);
                        if (command.ExecuteNonQuery() < 0) throw new Exception();


                    }

                    command.CommandText =  $" DELETE FROM Patient " +
                                        " WHERE IdPatient = @idPatient; ";
                    command.Parameters.AddWithValue("idPatient", idPatient);
                    if (command.ExecuteNonQuery() < 0) throw new Exception();
                    transaction.Commit();
                }
                return 200;
            }
            catch (SqlException sqlException) {
                if (transaction != null)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                return 400;
            }
            catch (Exception exception)
            {
                if (transaction != null)
                {
                    transaction.Rollback();
                    transaction.Dispose();
                }
                return 400;
            }
        }
    }
}
