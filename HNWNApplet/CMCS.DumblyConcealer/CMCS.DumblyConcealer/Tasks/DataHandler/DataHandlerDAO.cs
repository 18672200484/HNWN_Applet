using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using CMCS.Common;
using CMCS.Common.DAO;
using CMCS.Common.Entities.BaseInfo;
using CMCS.Common.Entities.CarTransport;
using CMCS.Common.Entities.Fuel;
using CMCS.Common.Entities.Inf;
using CMCS.Common.Entities.Sys;
using CMCS.DapperDber.Dbs.AccessDb;
using CMCS.DapperDber.Dbs.OracleDb;
using CMCS.DapperDber.Dbs.SqlServerDb;
using CMCS.DumblyConcealer.Enums;
using CMCS.DumblyConcealer.Tasks.CarSynchronous.Enums;
using CMCS.DapperDber.Util;
using HikISCApi.Core;
using CMCS.Common.Enums;

namespace CMCS.DumblyConcealer.Tasks.CarSynchronous
{
    /// <summary>
    /// 综合事件处理
    /// </summary>
    public class DataHandlerDAO
    {
        private static DataHandlerDAO instance;

        public static DataHandlerDAO GetInstance()
        {
            if (instance == null)
            {
                instance = new DataHandlerDAO();
            }
            return instance;
        }

        CommonDAO commonDAO = CommonDAO.GetInstance();

        private DataHandlerDAO()
        { }

        #region 全过程数据同步(第一版)

        /// <summary>
        /// 同步全过程基础信息(车辆基本信息)
        /// </summary>
        public void SyncBaseInfoForCHLGL(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_rlgl_chlgl_chlgl qgc_chlgl in sqlDapperDber.Entities<View_rlgl_chlgl_chlgl>())
                {
                    if (commonDAO.SelfDber.Get<View_rlgl_chlgl_chlgl>(qgc_chlgl.Dbid.ToString()) != null)
                        commonDAO.SelfDber.Update<View_rlgl_chlgl_chlgl>(qgc_chlgl);
                    else
                        commonDAO.SelfDber.Insert<View_rlgl_chlgl_chlgl>(qgc_chlgl);

                    CmcsAutotruck cmcsAutotruck = commonDAO.SelfDber.Entity<CmcsAutotruck>("where CarNumber=:CarNumber", new { CarNumber = qgc_chlgl.Chph });
                    if (cmcsAutotruck == null)
                    {
                        commonDAO.SelfDber.Insert(new CmcsAutotruck()
                        {
                            CarNumber = qgc_chlgl.Chph,
                            EPCCardId = qgc_chlgl.Rfid_xlh,
                            CarMaxWeight = qgc_chlgl.Hzl,
                            Is_hmd = qgc_chlgl.Is_hmd,
                            BX_EndDate = qgc_chlgl.Bx_enddate,
                            Xshzh_EndDate = qgc_chlgl.Xshzh_enddate,
                            Zgzh_EndDate = qgc_chlgl.Zgzh_enddate,
                            IsUse = 1,
                            CarType = "入厂煤"
                        });
                    }
                    else
                    {
                        cmcsAutotruck.CarNumber = qgc_chlgl.Chph;
                        cmcsAutotruck.EPCCardId = qgc_chlgl.Rfid_xlh;
                        cmcsAutotruck.CarMaxWeight = qgc_chlgl.Hzl;
                        cmcsAutotruck.Is_hmd = qgc_chlgl.Is_hmd;
                        cmcsAutotruck.BX_EndDate = qgc_chlgl.Bx_enddate;
                        cmcsAutotruck.Xshzh_EndDate = qgc_chlgl.Xshzh_enddate;
                        cmcsAutotruck.Zgzh_EndDate = qgc_chlgl.Zgzh_enddate;
                        cmcsAutotruck.IsUse = 1;
                        cmcsAutotruck.CarType = "入厂煤";
                        commonDAO.SelfDber.Update(cmcsAutotruck);
                    }

                    res++;
                }
            }
            catch (Exception ex)
            {
                output("同步车辆基本信息报错，" + ex.Message, eOutputType.Error);
            }
            output(string.Format("同步车辆基本信息{0}条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步全过程基础信息(车辆日调运计划)
        /// </summary>
        public void SyncBaseInfoForDYJH(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_rlgl_dygl_dyjh qgc_dyjh in sqlDapperDber.Entities<View_rlgl_dygl_dyjh>())
                {
                    if (commonDAO.SelfDber.Get<View_rlgl_dygl_dyjh>(qgc_dyjh.Dbid.ToString()) != null)
                        commonDAO.SelfDber.Update<View_rlgl_dygl_dyjh>(qgc_dyjh);
                    else
                        commonDAO.SelfDber.Insert<View_rlgl_dygl_dyjh>(qgc_dyjh);

                    res++;
                }
            }
            catch (Exception ex)
            {
                output("同步车辆日调运计划报错，" + ex.Message, eOutputType.Error);
            }

            output(string.Format("同步车辆日调运计划{0}条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步全过程基础信息(公司信息)
        /// </summary>
        public void SyncBaseInfoForGS(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_rlgl_dygl_gs qgc_gs in sqlDapperDber.Entities<View_rlgl_dygl_gs>())
                {
                    if (commonDAO.SelfDber.Get<View_rlgl_dygl_gs>(qgc_gs.Dbid.ToString()) != null)
                        commonDAO.SelfDber.Update<View_rlgl_dygl_gs>(qgc_gs);
                    else
                        commonDAO.SelfDber.Insert<View_rlgl_dygl_gs>(qgc_gs);

                    CmcsSupplier cmcsSupplier = commonDAO.SelfDber.Entity<CmcsSupplier>("where PkId=:PkId", new { PkId = qgc_gs.Dbid });
                    if (cmcsSupplier == null)
                    {
                        commonDAO.SelfDber.Insert(new CmcsSupplier()
                        {
                            PkId = qgc_gs.Dbid,
                            TaxregCode = qgc_gs.Taxnumber,
                            Name = qgc_gs.Corpname,
                            ShortName = qgc_gs.Corpshortname,
                            Code = qgc_gs.Gys,
                            Is_fhdw = qgc_gs.Is_fhdw,
                            is_shhdw = qgc_gs.Is_shhdw
                        });
                    }
                    else
                    {
                        cmcsSupplier.PkId = qgc_gs.Dbid;
                        cmcsSupplier.TaxregCode = qgc_gs.Taxnumber;
                        cmcsSupplier.Name = qgc_gs.Corpname;
                        cmcsSupplier.ShortName = qgc_gs.Corpshortname;
                        cmcsSupplier.Code = qgc_gs.Gys;
                        cmcsSupplier.Is_fhdw = qgc_gs.Is_fhdw;
                        cmcsSupplier.is_shhdw = qgc_gs.Is_shhdw;
                        commonDAO.SelfDber.Update(cmcsSupplier);
                    }

                    res++;
                }
            }
            catch (Exception ex)
            {
                output("同步公司信息报错，" + ex.Message, eOutputType.Error);
            }
            output(string.Format("同步公司信息{0}条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步全过程基础信息(承运商信息)
        /// </summary>
        public void SyncBaseInfoForCHYSH(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_rlgl_dygl_chysh qgc_chysh in sqlDapperDber.Entities<View_rlgl_dygl_chysh>())
                {
                    if (commonDAO.SelfDber.Get<View_rlgl_dygl_chysh>(qgc_chysh.Dbid.ToString()) != null)
                        commonDAO.SelfDber.Update<View_rlgl_dygl_chysh>(qgc_chysh);
                    else
                        commonDAO.SelfDber.Insert<View_rlgl_dygl_chysh>(qgc_chysh);

                    CmcsTransportCompany cmcsTransportCompany = commonDAO.SelfDber.Entity<CmcsTransportCompany>("where PkId=:PkId", new { PkId = qgc_chysh.Dbid });
                    if (cmcsTransportCompany == null)
                    {
                        commonDAO.SelfDber.Insert(new CmcsTransportCompany()
                        {
                            PkId = qgc_chysh.Dbid,
                            ShipperTaxNumber = qgc_chysh.Shippertaxnumber,
                            Name = qgc_chysh.Shipername,
                            PlanNo = qgc_chysh.Planno,
                            Code = qgc_chysh.Gys
                        });
                    }
                    else
                    {
                        cmcsTransportCompany.PkId = qgc_chysh.Dbid;
                        cmcsTransportCompany.ShipperTaxNumber = qgc_chysh.Shippertaxnumber;
                        cmcsTransportCompany.Name = qgc_chysh.Shipername;
                        cmcsTransportCompany.PlanNo = qgc_chysh.Planno;
                        cmcsTransportCompany.Code = qgc_chysh.Gys;
                        commonDAO.SelfDber.Update(cmcsTransportCompany);
                    }

                    res++;
                }
            }
            catch (Exception ex)
            {
                output("同步承运商信息报错，" + ex.Message, eOutputType.Error);
            }
            output(string.Format("同步承运商信息{0}条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步全过程基础信息(货物信息)
        /// </summary>
        public void SyncBaseInfoForHW(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_rlgl_dygl_hw qgc_hw in sqlDapperDber.Entities<View_rlgl_dygl_hw>())
                {
                    if (commonDAO.SelfDber.Get<View_rlgl_dygl_hw>(qgc_hw.Dbid.ToString()) != null)
                        commonDAO.SelfDber.Update<View_rlgl_dygl_hw>(qgc_hw);
                    else
                        commonDAO.SelfDber.Insert<View_rlgl_dygl_hw>(qgc_hw);

                    CmcsFuelKind cmcsFuelKind = commonDAO.SelfDber.Entity<CmcsFuelKind>("where PkId=:PkId", new { PkId = qgc_hw.Dbid });
                    if (cmcsFuelKind == null)
                    {
                        commonDAO.SelfDber.Insert(new CmcsFuelKind()
                        {
                            ParentId = "-1",
                            PkId = qgc_hw.Dbid,
                            Code = qgc_hw.Productcode,
                            Name = qgc_hw.Productname,
                            Mzbh = qgc_hw.Mzbh,
                            WLType = qgc_hw.Wltype
                        });
                    }
                    else
                    {
                        cmcsFuelKind.PkId = qgc_hw.Dbid;
                        cmcsFuelKind.Code = qgc_hw.Productcode;
                        cmcsFuelKind.Name = qgc_hw.Productname;
                        cmcsFuelKind.Mzbh = qgc_hw.Mzbh;
                        cmcsFuelKind.WLType = qgc_hw.Wltype;
                        commonDAO.SelfDber.Update(cmcsFuelKind);
                    }

                    res++;
                }
            }
            catch (Exception ex)
            {
                output("同步货物信息报错，" + ex.Message, eOutputType.Error);
            }
            output(string.Format("同步货物信息{0}条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步全过程基础信息(验票反馈表)
        /// </summary>
        public void SyncBaseInfoForYAPFK(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_rlgl_chlgl_yapfk qgc_dyjh in commonDAO.SelfDber.Entities<View_rlgl_chlgl_yapfk>("where issync=0"))
                {
                    if (sqlDapperDber.Insert(new View_rlgl_chlgl_yapfk_QGC()
                      {
                          Yptime = qgc_dyjh.Yptime,
                          Chph = qgc_dyjh.Chph,
                          Cyjbm = qgc_dyjh.Cyjbm,
                          Kfl = qgc_dyjh.Kfl,
                          Dybh = qgc_dyjh.Dybh,
                          Rfid_xlh = qgc_dyjh.Rfid_xlh
                      }) > 0)
                    {
                        res++;

                        //更新同步状态
                        qgc_dyjh.IsSync = 1;
                        commonDAO.SelfDber.Update(qgc_dyjh);
                    }
                }
            }
            catch (Exception ex)
            {
                output("同步验票反馈报错，" + ex.Message, eOutputType.Error);
            }

            output(string.Format("同步验票反馈{0}条", res), eOutputType.Normal);
        }

        #endregion

        #region 全过程数据同步(第二版)

        /// <summary>
        /// 同步全过程基础信息(供应商信息)
        /// </summary>
        public void SyncBaseInfoForGYS(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_gys qgc_gys in sqlDapperDber.Entities<View_gys>())
                {
                    if (commonDAO.SelfDber.Get<View_gys>(qgc_gys.Dbid.ToString()) != null)
                        commonDAO.SelfDber.Update<View_gys>(qgc_gys);
                    else
                        commonDAO.SelfDber.Insert<View_gys>(qgc_gys);

                    CmcsSupplier cmcsSupplier = commonDAO.SelfDber.Entity<CmcsSupplier>("where PkId=:PkId", new { PkId = qgc_gys.Dbid });
                    if (cmcsSupplier == null)
                    {
                        commonDAO.SelfDber.Insert(new CmcsSupplier()
                        {
                            PkId = qgc_gys.Dbid,
                            Name = qgc_gys.Gysqc,
                            ShortName = qgc_gys.Gysjc,
                            Code = qgc_gys.Gysbm,
                            IsStop = int.Parse(qgc_gys.Is_valid)
                        });
                    }
                    else
                    {
                        cmcsSupplier.PkId = qgc_gys.Dbid;
                        cmcsSupplier.Name = qgc_gys.Gysqc;
                        cmcsSupplier.ShortName = qgc_gys.Gysjc;
                        cmcsSupplier.Code = qgc_gys.Gysbm;
                        cmcsSupplier.IsStop = int.Parse(qgc_gys.Is_valid);
                        commonDAO.SelfDber.Update(cmcsSupplier);
                    }

                    CmcsTransportCompany cmcsTransportCompany = commonDAO.SelfDber.Entity<CmcsTransportCompany>("where PkId=:PkId", new { PkId = qgc_gys.Dbid });
                    if (cmcsTransportCompany == null)
                    {
                        commonDAO.SelfDber.Insert(new CmcsTransportCompany()
                        {
                            PkId = qgc_gys.Dbid,
                            Name = qgc_gys.Gysqc,
                            IsStop = int.Parse(qgc_gys.Is_valid),
                            Code = qgc_gys.Gysbm
                        });
                    }
                    else
                    {
                        cmcsTransportCompany.PkId = qgc_gys.Dbid;
                        cmcsTransportCompany.Name = qgc_gys.Gysqc;
                        cmcsTransportCompany.IsStop = int.Parse(qgc_gys.Is_valid);
                        cmcsTransportCompany.Code = qgc_gys.Gysbm;
                        commonDAO.SelfDber.Update(cmcsTransportCompany);
                    }

                    res++;
                }
            }
            catch (Exception ex)
            {
                output("同步供应商信息报错，" + ex.Message, eOutputType.Error);
            }
            output(string.Format("同步供应商信息{0}条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步全过程基础信息(矿点信息)
        /// </summary>
        public void SyncBaseInfoForKB(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_kb qgc_kb in sqlDapperDber.Entities<View_kb>())
                {
                    if (commonDAO.SelfDber.Get<View_kb>(qgc_kb.Dbid.ToString()) != null)
                        commonDAO.SelfDber.Update<View_kb>(qgc_kb);
                    else
                        commonDAO.SelfDber.Insert<View_kb>(qgc_kb);

                    CmcsMine cmcsMine = commonDAO.SelfDber.Entity<CmcsMine>("where PkId=:PkId", new { PkId = qgc_kb.Dbid });
                    if (cmcsMine == null)
                    {
                        commonDAO.SelfDber.Insert(new CmcsMine()
                        {
                            ParentId = "-1",
                            PkId = qgc_kb.Dbid,
                            Code = qgc_kb.Kbbm,
                            Name = qgc_kb.Kbmc,
                            IsStop = int.Parse(qgc_kb.Is_valid)
                        });
                    }
                    else
                    {
                        cmcsMine.ParentId = "-1";
                        cmcsMine.PkId = qgc_kb.Dbid;
                        cmcsMine.Code = qgc_kb.Kbbm;
                        cmcsMine.Name = qgc_kb.Kbmc;
                        cmcsMine.IsStop = int.Parse(qgc_kb.Is_valid);
                        commonDAO.SelfDber.Update(cmcsMine);
                    }

                    res++;
                }
            }
            catch (Exception ex)
            {
                output("同步矿点信息报错，" + ex.Message, eOutputType.Error);
            }
            output(string.Format("同步矿点信息{0}条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步全过程基础信息(煤种信息)
        /// </summary>
        public void SyncBaseInfoForMZ(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_mz qgc_mz in sqlDapperDber.Entities<View_mz>())
                {
                    if (commonDAO.SelfDber.Get<View_mz>(qgc_mz.Dbid.ToString()) != null)
                        commonDAO.SelfDber.Update<View_mz>(qgc_mz);
                    else
                        commonDAO.SelfDber.Insert<View_mz>(qgc_mz);

                    CmcsFuelKind cmcsFuelKind = commonDAO.SelfDber.Entity<CmcsFuelKind>("where PkId=:PkId", new { PkId = qgc_mz.Dbid });
                    if (cmcsFuelKind == null)
                    {
                        commonDAO.SelfDber.Insert(new CmcsFuelKind()
                        {
                            ParentId = "-1",
                            PkId = qgc_mz.Dbid,
                            Code = qgc_mz.Mzbm,
                            Name = qgc_mz.Mzmc,
                            IsStop = int.Parse(qgc_mz.Is_valid)
                        });
                    }
                    else
                    {
                        cmcsFuelKind.ParentId = "-1";
                        cmcsFuelKind.PkId = qgc_mz.Dbid;
                        cmcsFuelKind.Code = qgc_mz.Mzbm;
                        cmcsFuelKind.Name = qgc_mz.Mzmc;
                        cmcsFuelKind.IsStop = int.Parse(qgc_mz.Is_valid);
                        commonDAO.SelfDber.Update(cmcsFuelKind);
                    }

                    res++;
                }
            }
            catch (Exception ex)
            {
                output("同步煤种信息报错，" + ex.Message, eOutputType.Error);
            }
            output(string.Format("同步煤种信息{0}条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步全过程基础信息(调运计划卡)
        /// </summary>
        public void SyncBaseInfoForDYJHK(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_dyjhk qgc_dyjh in sqlDapperDber.Entities<View_dyjhk>())
                {
                    if (commonDAO.SelfDber.Get<View_dyjhk>(qgc_dyjh.Dbid.ToString()) != null)
                        commonDAO.SelfDber.Update<View_dyjhk>(qgc_dyjh);
                    else
                        commonDAO.SelfDber.Insert<View_dyjhk>(qgc_dyjh);

                    res++;
                }
            }
            catch (Exception ex)
            {
                output("同步调运计划卡报错，" + ex.Message, eOutputType.Error);
            }

            output(string.Format("同步调运计划卡{0}条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步全过程基础信息(汽车来煤预归批)
        /// </summary>
        public void SyncBaseInfoForQCLMYGP(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_rlgl_cygl_qclmygp qgc_qclmygp in sqlDapperDber.Entities<View_rlgl_cygl_qclmygp>())
                {
                    if (commonDAO.SelfDber.Get<View_rlgl_cygl_qclmygp>(qgc_qclmygp.Dbid.ToString()) != null)
                        commonDAO.SelfDber.Update<View_rlgl_cygl_qclmygp>(qgc_qclmygp);
                    else
                        commonDAO.SelfDber.Insert<View_rlgl_cygl_qclmygp>(qgc_qclmygp);

                    res++;
                }
            }
            catch (Exception ex)
            {
                output("同步汽车来煤预归批报错，" + ex.Message, eOutputType.Error);
            }

            output(string.Format("同步汽车来煤预归批{0}条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步全过程基础信息(重车计量反馈)
        /// </summary>
        public void SyncBaseInfoForJLMZ(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_rlgl_jlgl_mz qgc_mz in commonDAO.SelfDber.Entities<View_rlgl_jlgl_mz>("where issync=0"))
                {
                    if (sqlDapperDber.Insert(new View_rlgl_jlgl_mz_QGC()
                    {
                        Chph = qgc_mz.Chph,
                        Mpph = qgc_mz.Mpph,
                        Mztime = qgc_mz.Mztime,
                        Rfid_xlh = qgc_mz.Rfid_xlh,
                        Mz = qgc_mz.Mz
                    }) > 0)
                    {
                        res++;

                        //更新同步状态
                        qgc_mz.Issync = 1;
                        commonDAO.SelfDber.Update(qgc_mz);
                    }
                }
            }
            catch (Exception ex)
            {
                output("同步重车计量反馈报错，" + ex.Message, eOutputType.Error);
            }

            output(string.Format("同步重车计量反馈{0}条", res), eOutputType.Normal);
        }

        /// <summary>
        /// 同步全过程基础信息(轻车计量反馈)
        /// </summary>
        public void SyncBaseInfoForJLPZ(Action<string, eOutputType> output, SqlServerDapperDber sqlDapperDber)
        {
            int res = 0;

            try
            {
                foreach (View_rlgl_jlgl_pz qgc_pz in commonDAO.SelfDber.Entities<View_rlgl_jlgl_pz>("where issync=0"))
                {
                    if (sqlDapperDber.Insert(new View_rlgl_jlgl_pz_QGC()
                    {
                        Chph = qgc_pz.Chph,
                        Mpph = qgc_pz.Mpph,
                        Mztime = qgc_pz.Mztime,
                        Rfid_xlh = qgc_pz.Rfid_xlh,
                        Pz = qgc_pz.Pz
                    }) > 0)
                    {
                        res++;

                        //更新同步状态
                        qgc_pz.Issync = 1;
                        commonDAO.SelfDber.Update(qgc_pz);
                    }
                }
            }
            catch (Exception ex)
            {
                output("同步轻车计量反馈报错，" + ex.Message, eOutputType.Error);
            }

            output(string.Format("同步轻车计量反馈{0}条", res), eOutputType.Normal);
        }

        #endregion

        #region 同步门禁数据

        /// <summary>
        /// 同步门禁数据
        /// </summary>
        /// <param name="output"></param>
        public void SyncDoorData(Action<string, eOutputType> output, AccessDapperDber doorDapperDber)
        {
            int res = 0;

            string sql = " select * from acc_monitor_log where time>#2018-08-08#";
            sql += " order by time";
            DataTable dtNum = doorDapperDber.ExecuteDataTable(sql);

            if (dtNum.Rows.Count > 0)
            {
                for (int i = 0; i < dtNum.Rows.Count; i++)
                {
                    string id = dtNum.Rows[i]["id"].ToString();
                    string userId = dtNum.Rows[i]["pin"].ToString();
                    string deviceId = dtNum.Rows[i]["device_id"].ToString();

                    string consumerName = GetConsumer(userId, doorDapperDber);
                    string doorName = GetMachine(deviceId, doorDapperDber);

                    if (string.IsNullOrEmpty(consumerName)) continue;
                    if (string.IsNullOrEmpty(doorName)) continue;

                    CmcsGuardInfo entity = Dbers.GetInstance().SelfDber.Entity<CmcsGuardInfo>("where NId=:NId", new { NId = id });
                    if (entity == null)
                    {
                        entity = new CmcsGuardInfo()
                        {
                            DataFrom = "智能化",
                            F_ConsumerId = userId,
                            F_ConsumerName = consumerName,
                            F_InOut = "1",
                            F_ReaderId = deviceId,
                            F_ReaderName = doorName,
                            NId = id,
                            F_ReadDate = DateTime.Parse(dtNum.Rows[i]["time"].ToString())
                        };

                        res += Dbers.GetInstance().SelfDber.Insert(entity);
                    }
                }
            }
            output(string.Format("同步门禁数据{0}条", res), eOutputType.Normal);
        }

        //根据用户id得到用户名
        private string GetConsumer(string UserId, AccessDapperDber doorDapperDber)
        {
            string sql = " select name from userinfo where Badgenumber='" + UserId + "'";
            DataTable dt = doorDapperDber.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            return "";
        }

        private string GetMachine(string MachineId, AccessDapperDber doorDapperDber)
        {
            string sql = " select MachineAlias from Machines where id=" + MachineId + "";
            DataTable dt = doorDapperDber.ExecuteDataTable(sql);
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][0].ToString();
            }
            return "";
        }

        #endregion

        #region 同步门禁数据ByHik Api

        /// <summary>
        /// 事件类型字典表
        /// </summary>
        Dictionary<int, string> dicEventType = new Dictionary<int, string>() 
        {
         {196893,"人脸认证通过"},
         {197127,"指纹比对通过"},
         {198914,"合法卡比对通过"},
         {197162,"人证比对通过"},
         {196883,"多重认证成功"},
         {196874,"首卡比对通过"},
         {196885,"刷卡+指纹认证通过"},
         {196887,"指纹+密码认证通过"},
         {198915,"刷卡+密码认证通过"},
         {196886,"刷卡+指纹+密码通过"},
         {196897,"工号+密码认证通过"},
         {196888,"人脸+指纹认证通过"},
         {196889,"人脸+密码认证通过"},
         {196890,"人脸+刷卡认证通过"},
         {196891,"人脸+密码+指纹认证通过"},
         {196892,"人脸+刷卡+指纹认证通过"}
        };

        public void SyncDoorEventData(Action<string, eOutputType> output)
        {
            int res = 0;

            try
            {
                List<Dictionary<string, object>> acsEventList = new List<Dictionary<string, object>>();
                List<string> cameraNameList = new List<string>();

                DoorEventRequest doorEventRequest = new DoorEventRequest()
                {
                    startTime = DateTime.Now.AddDays(-10),
                    endTime = DateTime.Now,
                    pageNo = 1,
                    pageSize = 1000
                };

                string uri = "/artemis/api/acs/v1/door/events";
                string body = JsonHelper.SerializeObject(doorEventRequest);

                byte[] resultByte = HttpUtillib.HttpPost(uri, body, 20);
                string resultStr = Encoding.UTF8.GetString(resultByte);

                Result result = JsonHelper.DeserializeJsonToObject<Result>(resultStr);

                if (result.Code.Equals("0"))
                {
                    ResultData resultData = JsonHelper.DeserializeJsonToObject<ResultData>(result.Data.ToString());

                    acsEventList = JsonHelper.DeserializeJsonToObject<List<Dictionary<string, object>>>(resultData.List.ToString());

                    foreach (Dictionary<string, object> doorEvents in acsEventList)
                    {
                        int eventType = Convert.ToInt32(doorEvents["eventType"]);
                        string personName = doorEvents["personName"].ToString();

                        //只处理成功的事件
                        if (dicEventType.ContainsKey(eventType) && !string.IsNullOrWhiteSpace(personName))
                        {
                            string eventId = doorEvents["eventId"].ToString();
                            DateTime eventTime = DateTime.Parse(doorEvents["eventTime"].ToString());
                            string personId = doorEvents["personId"].ToString();
                            string doorIndexCode = doorEvents["doorIndexCode"].ToString();
                            string doorName = doorEvents["doorName"].ToString();

                            CmcsGuardInfo entity = Dbers.GetInstance().SelfDber.Entity<CmcsGuardInfo>("where NId=:NId", new { NId = eventId });
                            if (entity == null)
                            {
                                entity = new CmcsGuardInfo()
                                {
                                    DataFrom = "智能化",
                                    F_ConsumerId = personId,
                                    F_ConsumerName = personName,
                                    F_InOut = "1",
                                    F_ReaderId = doorIndexCode,
                                    F_ReaderName = doorName,
                                    NId = eventId,
                                    F_ReadDate = eventTime,
                                    Remark = dicEventType[eventType]
                                };

                                res += Dbers.GetInstance().SelfDber.Insert(entity);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                output("同步门禁数据异常" + ex.Message, eOutputType.Error);
                return;
            }

            output(string.Format("同步门禁数据{0}条", res), eOutputType.Normal);
        }

        public class DoorEventRequest
        {
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
            public int pageNo { get; set; }
            public int pageSize { get; set; }
        }

        #endregion

        #region 处理集控首页数据

        /// <summary>
        /// 同步处理集控首页数据
        /// </summary>
        public void HandleHomePageData(Action<string, eOutputType> output)
        {
            try
            {
                //今日有效入厂煤运输记录
                List<CmcsBuyFuelTransport> list = commonDAO.SelfDber.Entities<CmcsBuyFuelTransport>("where IsUse=1 and InFactoryTime>=:dtStart and InFactoryTime<:dtEnd order by InFactoryTime desc", new { dtStart = DateTime.Now.Date, dtEnd = DateTime.Now.Date.AddDays(1) });

                if (list.Count > 0)
                {
                    commonDAO.SetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日入厂车数", list.Count.ToString());
                    commonDAO.SetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日待卸车数", list.Where(a => a.StepName == eTruckInFactoryStep.重车.ToString()).Count().ToString());
                    commonDAO.SetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日出厂车数", list.Where(a => a.StepName == eTruckInFactoryStep.出厂.ToString()).Count().ToString());
                    commonDAO.SetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日已卸车数", list.Where(a => a.StepName == eTruckInFactoryStep.轻车.ToString()).Count().ToString());
                    commonDAO.SetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日进煤量", list.Where(a => a.StepName == eTruckInFactoryStep.轻车.ToString()).Sum(a => a.SuttleWeight).ToString());
                }
                else
                {
                    commonDAO.SetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日入厂车数", "");
                    commonDAO.SetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日待卸车数", "");
                    commonDAO.SetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日出厂车数", "");
                    commonDAO.SetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日已卸车数", "");
                    commonDAO.SetSignalDataValue(GlobalVars.MachineCode_HomePage_1, "今日进煤量", "");
                }
            }
            catch (Exception ex)
            {
                output("同步处理集控首页数据异常" + ex.Message, eOutputType.Error);
                return;
            }
            output("成功同步处理集控首页数据", eOutputType.Normal);
        }

        #endregion
    }
}
