using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entity
{
    public class UniversalWayTypeConfigEntity
    {
        public UniversalWayTypeConfigEntity()
        {
        }
        
        public UniversalWayTypeConfigEntity( int Sequence, int PointID)
        {
            this._sequence = Sequence;
            this._PointID = PointID;
        }

        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { _sequence = value; }
        }

        //private UniversalCollectType _waytype;
        //public UniversalCollectType WayType
        //{
        //    get { return _waytype; }
        //    set { _waytype = value; }
        //}

        private int _PointID;
        public int PointID
        {
            get { return _PointID; }
            set { _PointID = value; }
        }

        private int _TerminalID;
        public int TerminalID
        {
            get { return _TerminalID; }
            set { _TerminalID = value; }
        }

        private DateTime _ModifyTime;
        public DateTime ModifyTime
        {
            get { return _ModifyTime; }
            set { _ModifyTime = value; }
        }
    }
}
