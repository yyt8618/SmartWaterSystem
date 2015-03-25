using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class UniversalWayTypeEntity
    {
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _level;
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        private int _parentID;
        public int ParentID
        {
            get { return _parentID; }
            set { _parentID = value; }
        }

        private UniversalCollectType _waytype;
        public UniversalCollectType WayType
        {
            get { return _waytype; }
            set { _waytype = value; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private int _FrameWidth = 2;
        /// <summary>
        /// 帧中数据宽度,默认2字节
        /// </summary>
        public int FrameWidth
        {
            get { return _FrameWidth; }
            set { _FrameWidth = value; }
        }

        private int _Sequence;
        /// <summary>
        /// 帧中顺序(序号从1开始)
        /// </summary>
        public int Sequence
        {
            get { return _Sequence; }
            set { _Sequence = value; }
        }

        private float _MaxMeasureRange;
        public float MaxMeasureRange
        {
            get { return _MaxMeasureRange; }
            set { _MaxMeasureRange = value; }
        }

        private float _ManMeasureRangeFlag;
        public float ManMeasureRangeFlag
        {
            get { return _ManMeasureRangeFlag; }
            set { _ManMeasureRangeFlag = value; }
        }

        private float _Precision;
        public float Precision
        {
            get { return _Precision; }
            set { _Precision = value; }
        }

        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set { _Unit = value; }
        }

        private int _SyncState = 0;
        /// <summary>
        /// 0:已同步,1:新增未同步,-1:删除未同步
        /// </summary>
        public int SyncState
        {
            get { return _SyncState; }
            set { _SyncState = value; }
        }

        private DateTime _ModifyTime;
        public DateTime ModifyTime
        {
            get { return _ModifyTime; }
            set { _ModifyTime = value; }
        }
    }
}
