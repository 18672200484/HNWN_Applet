using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMCS.Common.Entities.Sys;
using System.ComponentModel; 

namespace CMCS.Common.Entities.BaseInfo
{
    /// <summary>
    /// 其他物资计划表
    /// </summary>
    [Description("其他物资计划")]
    [CMCS.DapperDber.Attrs.DapperBind("CmcsTbGoodsPlan")]
    public class CmcsGoodsPlan : EntityBase1
    {
        private string _AutotruckId;
        /// <summary>
        /// 车辆
        /// </summary>
        public virtual string AutotruckId { get { return _AutotruckId; } set { _AutotruckId = value; } }

        private String _CarNumber;
        /// <summary>
        /// 车牌号
        /// </summary>
        [Description("车牌号")]
        public virtual String CarNumber { get { return _CarNumber; } set { _CarNumber = value; } }

        private String _SupplyUnitId;
        /// <summary>
        /// 发货单位
        /// </summary>
        public virtual String SupplyUnitId { get { return _SupplyUnitId; } set { _SupplyUnitId = value; } }

        private String _SupplyUnitName;
        /// <summary>
        /// 发货单位
        /// </summary>
        [Description("发货单位")]
        public virtual String SupplyUnitName { get { return _SupplyUnitName; } set { _SupplyUnitName = value; } }

        private String _ReceiveUnitId;
        /// <summary>
        /// 收货单位
        /// </summary>
        public virtual String ReceiveUnitId { get { return _ReceiveUnitId; } set { _ReceiveUnitId = value; } }

        private String _ReceiveUnitName;
        /// <summary>
        /// 收货单位
        /// </summary>
        [Description("收货单位")]
        public virtual String ReceiveUnitName { get { return _ReceiveUnitName; } set { _ReceiveUnitName = value; } }

        private String _GoodsTypeId;
        /// <summary>
        /// 物资类型
        /// </summary>
        public virtual String GoodsTypeId { get { return _GoodsTypeId; } set { _GoodsTypeId = value; } }

        private String _GoodsTypeName;
        /// <summary>
        /// 物资类型
        /// </summary>
        [Description("物资类型")]
        public virtual String GoodsTypeName { get { return _GoodsTypeName; } set { _GoodsTypeName = value; } }

        private string _Remark;
        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get { return _Remark; } set { _Remark = value; } }

    }
}
