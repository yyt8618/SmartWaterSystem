using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class GetGroupsRespEntity
    {
        /// <summary>
        /// 调用结果编码,-1:失败,1:成功
        /// </summary>
        public int code = -1;
        /// <summary>
        /// 调用结果编码为-1时，返回详细信息
        /// </summary>
        public string msg = "";
        /// <summary>
        /// 组信息列表
        /// </summary>
        public List<GroupsData> groupsdata = new List<GroupsData>();
        /// <summary>
        /// 终端信息列表
        /// </summary>
        public List<TersData> tersdata = new List<TersData>();
    }

    public class GroupsData
    {
        /// <summary>
        /// 组ID
        /// </summary>
        public string groupid = "";
        /// <summary>
        /// 组名
        /// </summary>
        public string groupname = "";
        /// <summary>
        /// 组备注
        /// </summary>
        public string remark = "";
    }

    public class TersData
    {
        /// <summary>
        /// 所属组ID(未分组时为空)
        /// </summary>
        public string groupid = "";
        /// <summary>
        /// 终端ID
        /// </summary>
        public string terid = "";
        /// <summary>
        /// 终端分组情况(0:未分组,1:已分组)
        /// </summary>
        public int groupstate;
        /// <summary>
        /// 终端备注
        /// </summary>
        public string remark = "";
    }
}
