using System.Data.EntityClient;

namespace BABollettari.Web.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Data;
    using System.Linq;
    using System.ServiceModel.DomainServices.EntityFramework;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using BABollettari.Web.Models;



    

    


    //The entity type 'BABollettari.Web.Models.v_Bollettario' is exposed by multiple DomainService types. Entity types cannot be shared across DomainServices

    public partial class BollettarioDomainService : LinqToEntitiesDomainService<BollettariEntities>
    {


        //public System.Data.Objects.ObjectResult<SP_Result> LoginUsrPwd(string Usr, string pwd)
        //{
        //    System.Data.Objects.ObjectResult<SP_Result> result =  SP_Result.CreateSP_Result (  GetLoginUsrPwd(Usr, pwd);
        //    return result;
        //}


        

        

        public IQueryable<v_Bollettario> GetV_BollettarioSorted()
        {

            return this.ObjectContext.v_Bollettario.OrderByDescending(b => b.dt_INS);
        }



        public IQueryable<v_Bollettario> GetV_BollettarioByIdBollettario(int IdBollettario)
        {
            if (IdBollettario == 0)
                return GetV_BollettarioSorted();

            return this.ObjectContext.v_Bollettario.Where(c => c.IdBollettario == IdBollettario).OrderBy(b => b.dt_INS);
        }

        public IQueryable<v_Bollettario> GetV_BollettarioByProgTipografo(string progTipografo)
        {
            if (progTipografo == "")
                return GetV_BollettarioSorted();

            
                return this.ObjectContext.v_Bollettario.Where(c => c.progTipografo == progTipografo).OrderBy(b => b.dt_INS);
        }




        public IQueryable<v_Bollette> GetV_BolletteByIdBollettario(int IdBollettario)
        {

            if (IdBollettario == 0)
                return this.ObjectContext.v_Bollette ;

            return this.ObjectContext.v_Bollette.Where(c => c.IdBollettario == IdBollettario);
        }


        public IQueryable<v_Provincie> GetV_ProvincieSorted()
        {
            return this.ObjectContext.v_Provincie.OrderBy(b => b.provincia);
        }

        public IQueryable<v_Comuni> GetV_ComuniByProvincia(string Provincia)
        {
            return this.ObjectContext.v_Comuni.Where(c => c.provincia == Provincia).OrderBy(b => b.dComune);
        }
        public IQueryable<v_Comuni> GetV_ComuniBycComune(int cComune)
        {
            return this.ObjectContext.v_Comuni.Where(c => c.cComune == cComune);
        }

        public IQueryable<v_Cimiteri> GetV_CimiteriByComune(int cComune)
        {
            return this.ObjectContext.v_Cimiteri.Where(c => c.codComune == cComune).OrderBy(b => b.dCimitero);
        }
        public IQueryable<v_Cimiteri> GetV_CimiteroBycCimitero(int cCimitero)
        {
            return this.ObjectContext.v_Cimiteri.Where(c => c.cCimitero == cCimitero);
        }

        public IQueryable<v_Categoria> GetV_CategoriaBycCimitero(int cCimitero)
        {
            return this.ObjectContext.v_Categoria.Where(c => c.cCimitero == cCimitero);
        }



        


        public Int32   GetLoginUsrPwd(string Usr, string pwd)
        {
            Int32 retValue = 0;
            try
            {

                using (EntityConnection conn = new EntityConnection(ObjectContext.Connection.ConnectionString))
                {
                    conn.Open();
                    
                    // Create an EntityCommand.
                    using (EntityCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "BollettariEntities.CheckLogin";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        EntityParameter param = new EntityParameter("user",System.Data.DbType.String );
                        param.Value = Usr;
                        cmd.Parameters.Add(param);
                        param = new EntityParameter("pwd",System.Data.DbType.String   );
                        param.Value = pwd;
                        cmd.Parameters.Add(param);
                        param = new EntityParameter("ret", System.Data.DbType.Int32   );
                        param.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(param);
                    
                        cmd.ExecuteNonQuery();
                        retValue = Convert.ToInt32(cmd.Parameters[2].Value.ToString());

                        //retValue = Int32.Parse(cmd.Parameters["@LastNameCount"].Value.ToString());
                    
                        //using (EntityDataReader rdr =cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        //{
                        //    retValue =Convert.ToInt32 (  cmd.Parameters[2]);  
                        //}
                    }
                    conn.Close();
                    
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("GetLoginUsrPwd "+ex.Message );
            }


            //The data reader is incompatible with the specified 'BollettariModel.SP_Result'. 
            //A member of the type, 'ret', does not have a corresponding column in the data reader with the same name.
            
            return retValue;

        }


        public Int32 GeneraBollettario(string progTipografo, Int32 NumBollette, Int32 DaBolletta, string cOP)
        {
            Int32 retValue = 0;
            try
            {

                using (EntityConnection conn = new EntityConnection(ObjectContext.Connection.ConnectionString))
                {
                    conn.Open();

                    // Create an EntityCommand.
                    using (EntityCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "BollettariEntities.GeneraBoll";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Clear();
                        EntityParameter param = new EntityParameter("progTipografo", System.Data.DbType.String);
                        param.Value = progTipografo;
                        cmd.Parameters.Add(param);

                        param = new EntityParameter("NumBollette", System.Data.DbType.Int32 );
                        param.Value = NumBollette;
                        cmd.Parameters.Add(param);

                        param = new EntityParameter("DaBolletta", System.Data.DbType.Int32);
                        param.Value = DaBolletta;
                        cmd.Parameters.Add(param);

                        param = new EntityParameter("cOP", System.Data.DbType.String);
                        param.Value = cOP;
                        cmd.Parameters.Add(param);


                        param = new EntityParameter("ret", System.Data.DbType.Int32);
                        param.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(param);

                        cmd.ExecuteNonQuery();
                        retValue = Convert.ToInt32(cmd.Parameters[4].Value.ToString());

                        //retValue = Int32.Parse(cmd.Parameters["@LastNameCount"].Value.ToString());

                        //using (EntityDataReader rdr =cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        //{
                        //    retValue =Convert.ToInt32 (  cmd.Parameters[2]);  
                        //}
                    }
                    conn.Close();

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("GeneraBollettario " + ex.Message);
            }


            //The data reader is incompatible with the specified 'BollettariModel.SP_Result'. 
            //A member of the type, 'ret', does not have a corresponding column in the data reader with the same name.

            return retValue;

        }




        public Int32 EmettiBollettario(Int32 idBollettario, Int32 cComune, Int32 cCimitero, Int32 cCategoria, string decorrenza, string cOP)
        {
            Int32 retValue = 0;
            try
            {

                using (EntityConnection conn = new EntityConnection(ObjectContext.Connection.ConnectionString))
                {
                    conn.Open();

                    // Create an EntityCommand.
                    using (EntityCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "BollettariEntities.EmetteBoll";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Clear();
                        EntityParameter param = new EntityParameter("idBollettario", System.Data.DbType.Int32);
                        param.Value = idBollettario;
                        cmd.Parameters.Add(param);

                        param = new EntityParameter("cComune", System.Data.DbType.Int32);
                        param.Value = cComune;
                        cmd.Parameters.Add(param);

                        param = new EntityParameter("cCimitero", System.Data.DbType.Int32);
                        param.Value = cCimitero;
                        cmd.Parameters.Add(param);

                        param = new EntityParameter("cCategoria", System.Data.DbType.Int32);
                        param.Value = cCategoria;
                        cmd.Parameters.Add(param);

                        param = new EntityParameter("decorrenza", System.Data.DbType.String);
                        param.Value = decorrenza;
                        cmd.Parameters.Add(param);

                        param = new EntityParameter("cOP", System.Data.DbType.String);
                        param.Value = cOP;
                        cmd.Parameters.Add(param);

                        param = new EntityParameter("idMAXCimitero", System.Data.DbType.Int32);
                        param.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(param);

                        param = new EntityParameter("ret", System.Data.DbType.Int32);
                        param.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(param);

                        cmd.ExecuteNonQuery();
                        retValue = Convert.ToInt32(cmd.Parameters[6].Value.ToString());

                        //retValue = Int32.Parse(cmd.Parameters["@LastNameCount"].Value.ToString());

                        //using (EntityDataReader rdr =cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        //{
                        //    retValue =Convert.ToInt32 (  cmd.Parameters[2]);  
                        //}
                    }
                    conn.Close();

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("EmettiBollettario " + ex.Message);
            }


            //The data reader is incompatible with the specified 'BollettariModel.SP_Result'. 
            //A member of the type, 'ret', does not have a corresponding column in the data reader with the same name.

            return retValue;

        }


        
        public Int32 checkBollette(Int32 NumBolletta, Int32 QuanteBollette)
        {
            Int32 retValue = 0;
            try
            {

                using (EntityConnection conn = new EntityConnection(ObjectContext.Connection.ConnectionString))
                {
                    conn.Open();

                    // Create an EntityCommand.
                    using (EntityCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = "BollettariEntities.checkEsBollette";
                        cmd.CommandType = CommandType.StoredProcedure;
                        //retValue = 2;

                        cmd.Parameters.Clear();
                        //retValue = 3;
                        EntityParameter param = new EntityParameter("NumBolletta", System.Data.DbType.Int32);
                        param.Value = NumBolletta;
                        cmd.Parameters.Add(param);
                        //retValue = 4;
                        param = new EntityParameter("QuanteBollette", System.Data.DbType.Int32);
                        param.Value = QuanteBollette;
                        cmd.Parameters.Add(param);
                        //retValue = 5;

                        param = new EntityParameter("ret", System.Data.DbType.Int32);
                        param.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(param);
                        //retValue = 6;
                        cmd.ExecuteNonQuery();
                        //retValue = 7;
                        retValue =Convert.ToInt32(cmd.Parameters[2].Value.ToString()) ;
                        //retValue = 8;
                        //retValue = Int32.Parse(cmd.Parameters["@LastNameCount"].Value.ToString());

                        //using (EntityDataReader rdr =cmd.ExecuteReader(CommandBehavior.SequentialAccess))
                        //{
                        //    retValue =Convert.ToInt32 (  cmd.Parameters[2]);  
                        //}
                    }
                    conn.Close();

                }
            }
            catch (Exception ex)
            {
                //retValue = 1000;
                //throw new ApplicationException("GeneraBollettario " + ex.Message);
            }


            //The data reader is incompatible with the specified 'BollettariModel.SP_Result'. 
            //A member of the type, 'ret', does not have a corresponding column in the data reader with the same name.

            return retValue;

        }

        
        protected override void OnError(DomainServiceErrorInfo errorInfo)
        {
            //Log exception errorInfo.Error
        }
    }




}