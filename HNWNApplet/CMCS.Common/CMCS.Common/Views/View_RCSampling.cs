using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities;
using CMCS.Common.Entities.Sys; 

namespace CMCS.Common.Views
{
    /// <summary>
    /// 视图-入厂煤运输记录视图
    /// </summary>
    [CMCS.DapperDber.Attrs.DapperBind("View_RCSampling")]
    public class View_RCSampling : EntityBase2
    {
        private DateTime _SamplingDate;
        private String _SampleCode;
        private String _SamplingType;
        private String _Batch;
        private String _BatchId;
        private DateTime _FactarriveDate;
        private String _BatchType;
        private String _SupplierName;
        private String _MineName;
        private String _FuelName;
        private String _StationName;

        /// <summary>
        /// 采样码
        /// </summary>
        public String SampleCode
        {
            get { return _SampleCode; }
            set { _SampleCode = value; }
        }

        /// <summary>
        /// 采样方式
        /// </summary>
        public String SamplingType
        {
            get { return _SamplingType; }
            set { _SamplingType = value; }
        }

        /// <summary>
        /// 入厂批次号
        /// </summary>
        public String Batch
        {
            get { return _Batch; }
            set { _Batch = value; }
        }

        /// <summary>
        /// 入厂批次ID
        /// </summary>
        public String BatchId
        {
            get { return _BatchId; }
            set { _BatchId = value; }
        }

        /// <summary>
        /// 实际到厂时间
        /// </summary>
        public DateTime FactarriveDate
        {
            get { return _FactarriveDate; }
            set { _FactarriveDate = value; }
        }

        /// <summary>
        /// 火车 汽车 船运
        /// </summary>
        public String BatchType
        {
            get { return _BatchType; }
            set { _BatchType = value; }
        }

        /// <summary>
        /// 供应商名称
        /// </summary>
        public String SupplierName
        {
            get { return _SupplierName; }
            set { _SupplierName = value; }
        }

        /// <summary>
        /// 矿点名称
        /// </summary>
        public String MineName
        {
            get { return _MineName; }
            set { _MineName = value; }
        }

        /// <summary>
        /// 煤种名称
        /// </summary>
        public String FuelName
        {
            get { return _FuelName; }
            set { _FuelName = value; }
        }

        /// <summary>
        /// 发站名称
        /// </summary>
        public String StationName
        {
            get { return _StationName; }
            set { _StationName = value; }
        }

        /// <summary>
        /// 采样日期
        /// </summary>
        public DateTime SamplingDate
        {
            get { return _SamplingDate; }
            set { _SamplingDate = value; }
        }
    }
}
