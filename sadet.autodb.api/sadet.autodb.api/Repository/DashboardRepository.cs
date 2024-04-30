using Microsoft.EntityFrameworkCore;
using sadet.autodb.api.Context;
using sadet.autodb.api.Interfaces;
using sadet.autodb.api.Models;

public class DashboardRepository : IDashboardRepository
{
    private readonly ApplicationDbContext _context;

    public DashboardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public TableRow[] GetData()
    {
        // GPT-Generated
        var subQuery = from data in _context.Data
            group data by data.Appid
            into grouped
            select new
            {
                Appid = grouped.Key,
                MaxCompletion = grouped.Max(g => g.Completion),
                LatestCompletion = grouped.OrderByDescending(g => g.CreateDate).FirstOrDefault().Completion,
                LatestCreateDate = grouped.Max(g => g.CreateDate)
            };

        var query = from game in _context.Games
            join sub in subQuery on game.Appid equals sub.Appid into gameData
            from gd in gameData.DefaultIfEmpty()
            select new TableRow
            {
                Appid = game.Appid,
                Name = game.Name,
                Dropped = gd != null && gd.MaxCompletion > gd.LatestCompletion,
                Percent = gd == null ? 0 : gd.LatestCompletion
            };

        return query.ToArray();
    }

    public TableRow[] GetDataFast()
    {
        // GPT-Generated
        var optimizedSubQuery =
            from data in _context.Data
            join latest in (
                from d in _context.Data
                where _context.Data.Any(dd => dd.Appid == d.Appid && dd.CreateDate > d.CreateDate) == false
                select new { d.Appid, d.Completion, d.CreateDate }
            ) on data.Appid equals latest.Appid
            group data by new { data.Appid, latest.Completion, latest.CreateDate }
            into grouped
            select new
            {
                Appid = grouped.Key.Appid,
                MaxCompletion = grouped.Max(g => g.Completion),
                LatestCompletion = grouped.Key.Completion,
                LatestCreateDate = grouped.Key.CreateDate
            };

        var optimizedQuery =
            from game in _context.Games
            join sub in optimizedSubQuery on game.Appid equals sub.Appid into gameData
            from gd in gameData.DefaultIfEmpty()
            select new TableRow
            {
                Appid = game.Appid,
                Name = game.Name,
                Dropped = gd != null && gd.MaxCompletion > gd.LatestCompletion,
                Percent = gd == null ? 0 : gd.LatestCompletion
            };

        return optimizedQuery.ToArray();
    }
}