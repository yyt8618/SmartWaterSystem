
namespace Entity
{
    public class SyncIdsEntity
    {
        public SyncIdsEntity(string localid, long serverid)
        {
            this.localid = localid;
            this.serverid = serverid;
        }
        public string localid;
        public long serverid;
    }
}
