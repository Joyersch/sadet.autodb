using System.Collections;
using sadet.autodb.api.Context;
using sadet.autodb.api.Interfaces;
using sadet.autodb.api.Models;

namespace sadet.autodb.api.Repository;

public class GamesRepository : IGamesRepository
{
    private readonly ApplicationDbContext _context;

    public GamesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public bool Exists(long appid)
    {
        return _context.Games.Any(g => g.Appid == appid);
    }

    public ICollection<Game> Get()
    {
        return _context.Games.ToArray();
    }

    public Game Get(long appid)
    {
        return _context.Games.First(g => g.Appid == appid);
    }
}