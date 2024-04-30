using sadet.autodb.api.Models;

namespace sadet.autodb.api.Interfaces;

public interface IGamesRepository
{
    public bool Exists(long appid);
    public ICollection<Game> Get();
    public Game Get(long appid);
}