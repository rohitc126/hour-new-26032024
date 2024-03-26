using BusinessLayer.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.DAL
{
    public class DAL_HOUR_METER_HMR_READING
    {
        public string INSERT_HOUR_METER_HMR_READING(string Comp_Code, string Branch_Code, string Emp_Code, HourMeterReport MeterReading)
        {
            string errorMsg = "";

            using (var connection = new SqlConnection(sqlConnection.GetConnectionString_SGX()))
            {
                connection.Open();
                SqlCommand command;
                SqlTransaction transactionScope = null;
                transactionScope = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    SqlParameter[] param =
                    { 
                      new SqlParameter("@ERRORSTR", SqlDbType.VarChar, 200)
                     ,new SqlParameter("@HMR_ID", SqlDbType.Decimal)  
                     ,new SqlParameter("@COMP_CODE", Comp_Code)
                     ,new SqlParameter("@BRANCH_CODE", Branch_Code)
                     ,new SqlParameter("@SHIFT_TIME", (MeterReading.SHIFT_Code == null)?(object)DBNull.Value : MeterReading.SHIFT_Code)    
                     ,new SqlParameter("@PRODUCTION_DATE", MeterReading.PRODUCTION_DATE)
                     ,new SqlParameter("@ADDED_BY", Emp_Code)  
                    };

                    param[0].Direction = ParameterDirection.Output;
                    param[1].Direction = ParameterDirection.Output;

                    new DataAccess().InsertWithTransaction("[AGG].[USP_INSERT_HOUR_METER_READING_HDR]", CommandType.StoredProcedure, out command, connection, transactionScope, param);
                    decimal HMR_ID = (decimal)command.Parameters["@HMR_ID"].Value;
                    string error_1 = (string)command.Parameters["@ERRORSTR"].Value;
                    if (HMR_ID == -1) { errorMsg = error_1; }

                    if (HMR_ID > 0)
                    {
                        if (MeterReading.HourMeterDtl_List != null)
                        {
                            foreach (HourMeterReport_Dtl item in MeterReading.HourMeterDtl_List)
                            {
                                if (!string.IsNullOrEmpty(item.Total))
                                {
                                    SqlParameter[] param2 =
                                    {
                                       new SqlParameter("@ERRORSTR", SqlDbType.VarChar, 200)
                                      ,new SqlParameter("@HMRR_ID", SqlDbType.Decimal)  
                                      ,new SqlParameter("@HMR_ID", HMR_ID)  
                                      ,new SqlParameter("@PM_ID", (item.PM_ID == null) ? 0  : item.PM_ID)
                                      ,new SqlParameter("@TOTAL_TIME", item.Total)
                                      ,new SqlParameter("@REMARKS",  (item.Remarks == null) ? ""  : item.Remarks)
                                    };
                                    param2[0].Direction = ParameterDirection.Output;
                                    param2[1].Direction = ParameterDirection.Output;
                                    new DataAccess().InsertWithTransaction("[AGG].[USP_INSERT_HOUR_METER_READING_dtl]", CommandType.StoredProcedure, out command, connection, transactionScope, param2);
                                    decimal HMRR_ID = (decimal)command.Parameters["@HMRR_ID"].Value;
                                    string error_2 = (string)command.Parameters["@ERRORSTR"].Value;
                                    if (HMRR_ID == -1) { errorMsg = error_2; break; }
                                }
                            }
                        }
                    }

                    if (errorMsg == "")
                    {
                        transactionScope.Commit();
                    }
                    else
                    {
                        transactionScope.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        transactionScope.Rollback();
                    }
                    catch (Exception ex1)
                    {
                        errorMsg = "Error: Exception occured. " + ex1.StackTrace.ToString();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return errorMsg;
        }

        public List<HourMeterReport_Dtl> GET_HOUR_METER_READING_DTLS(string Comp_Code, string Branch_Code, string Shift_Time, string ProductionDate)
        {
            SqlParameter[] param = { 
                                       new SqlParameter("@COMP_CODE", Comp_Code),
                                       new SqlParameter("@BRANCH_CODE", Branch_Code),
                                       new SqlParameter("@SHIFT_TIME", Shift_Time), 
                                       new SqlParameter("@PRODUCTION_DATE", ProductionDate),                                                         
                                   };

            DataTable dt = new DataAccess(sqlConnection.GetConnectionString_SGX()).GetDataTable("[AGG].[USP_SELECT_HOUR_METER_READING_DTLS]", CommandType.StoredProcedure, param);
            List<HourMeterReport_Dtl> list = new List<HourMeterReport_Dtl>();
            HourMeterReport_Dtl dtl = null;

            foreach (DataRow row in dt.Rows)
            {
                dtl = new HourMeterReport_Dtl();
                dtl.HMRR_ID = Convert.ToDecimal(row["HMRR_ID"] == DBNull.Value ? 0 : row["HMRR_ID"]);
                dtl.HMR_ID = Convert.ToDecimal(row["HMR_ID"] == DBNull.Value ? 0 : row["HMR_ID"]);
                dtl.PM_ID = Convert.ToInt32(row["PM_ID"] == DBNull.Value ? 0 : row["PM_ID"]);
                dtl.PM_NAME = Convert.ToString(row["PM_NAME"]);
                dtl.IS_LOCKED = Convert.ToInt32(row["IS_LOCKED"] == DBNull.Value ? 0 : row["IS_LOCKED"]);
                dtl.Total = Convert.ToString(row["TOTAL_TIME"]);
                dtl.Remarks = Convert.ToString(row["REMARKS"]);
                list.Add(dtl);
            }
            return list;
        }

        public string UPDATE_HOUR_METER_READING_DTLS(string Comp_Code, string Branch_Code, string Emp_Code, HourMeterReport MeterReading)
        {
            string errorMsg = "";
            using (var connection = new SqlConnection(sqlConnection.GetConnectionString_SGX()))
            {
                connection.Open();
                SqlCommand command;
                SqlTransaction transactionScope = null;
                transactionScope = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    decimal HMR_ID = MeterReading.HourMeterDtl_List[0].HMR_ID;
                    MeterReading.HMR_ID = HMR_ID;

                    if (MeterReading.HourMeterDtl_List != null)
                    {
                        foreach (HourMeterReport_Dtl item in MeterReading.HourMeterDtl_List)
                        {
                            if (item.HMRR_ID > 0)
                            {

                                SqlParameter[] param2 =
                                    {
                                       new SqlParameter("@ERRORSTR", SqlDbType.VarChar, 200) 
                                      ,new SqlParameter("@HMR_ID", HMR_ID) 
                                      ,new SqlParameter("@HMRR_ID", item.HMRR_ID) 
                                      ,new SqlParameter("@PM_ID", (item.PM_ID == null) ? 0  : item.PM_ID)
                                      ,new SqlParameter("@TOTAL_TIME",(item.Total == null) ? ""  : item.Total)
                                      ,new SqlParameter("@REMARKS",  (item.Remarks == null) ? ""  : item.Remarks)           
                                    };
                                param2[0].Direction = ParameterDirection.Output;
                                new DataAccess().InsertWithTransaction("[AGG].[USP_UPDATE_HOUR_METER_READING_DTLS]", CommandType.StoredProcedure, out command, connection, transactionScope, param2);
                                string error_2 = (string)command.Parameters["@ERRORSTR"].Value;
                                if (error_2 != "") { errorMsg = error_2; break; }
                            }
                            else if (!string.IsNullOrEmpty(item.Total) && item.HMRR_ID == 0)
                            {
                                SqlParameter[] param2 =
                                    {
                                       new SqlParameter("@ERRORSTR", SqlDbType.VarChar, 200) 
                                      ,new SqlParameter("@HMR_ID", HMR_ID) 
                                      ,new SqlParameter("@HMRR_ID", item.HMRR_ID) 
                                      ,new SqlParameter("@PM_ID", (item.PM_ID == null) ? 0  : item.PM_ID)
                                      ,new SqlParameter("@TOTAL_TIME",(item.Total == null) ? ""  : item.Total)
                                      ,new SqlParameter("@REMARKS",  (item.Remarks == null) ? ""  : item.Remarks)           
                                    };
                                param2[0].Direction = ParameterDirection.Output;
                                new DataAccess().InsertWithTransaction("[AGG].[USP_INSERT_HOUR_METER_READING_dtl]", CommandType.StoredProcedure, out command, connection, transactionScope, param2);
                                string error_2 = (string)command.Parameters["@ERRORSTR"].Value;
                                if (error_2 != "") { errorMsg = error_2; break; }
                            }
                        }
                    }

                    if (errorMsg == "")
                    {
                        transactionScope.Commit();
                    }
                    else
                    {
                        transactionScope.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        transactionScope.Rollback();
                    }
                    catch (Exception ex1)
                    {
                        errorMsg = "Error: Exception occured. " + ex1.StackTrace.ToString();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return errorMsg;
        }

        public string UPDATE_HOUR_METER_READING_DTLS_LOCK(string Emp_Code, decimal HMR_ID)
        {
            string errorMsg = "";

            using (var connection = new SqlConnection(sqlConnection.GetConnectionString_SGX()))
            {
                connection.Open();
                SqlCommand command;
                SqlTransaction transactionScope = null;
                transactionScope = connection.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {

                    SqlParameter[] param =
                    { 
                      new SqlParameter("@ERRORSTR", SqlDbType.VarChar, 200)
                     ,new SqlParameter("@HMR_ID", HMR_ID)   
                     ,new SqlParameter("@LOCKED_BY", Emp_Code)   
                    };

                    param[0].Direction = ParameterDirection.Output;
                    new DataAccess().InsertWithTransaction("[AGG].[USP_UPDATE_HOUR_METER_READING_LOCK]", CommandType.StoredProcedure, out command, connection, transactionScope, param);

                    string error_1 = (string)command.Parameters["@ERRORSTR"].Value;
                    if (error_1 != "") { errorMsg = error_1;  }
                    if (errorMsg == "")
                    {
                        transactionScope.Commit();
                    }
                    else
                    {
                        transactionScope.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        transactionScope.Rollback();
                    }
                    catch (Exception ex1)
                    {
                        errorMsg = "Error: Exception occured. " + ex1.StackTrace.ToString();
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
            return errorMsg;
        }
    }
}