using sadet.autodb.api.Models;

namespace sadet.autodb.api.Interfaces;

public interface IDataRepository
{
    public bool Exists(long appid);
    public DataSet GetData(long appId);
    public DataSet GetData(long appId, TimeSpan limit);
}