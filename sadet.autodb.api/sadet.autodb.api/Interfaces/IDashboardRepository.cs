using sadet.autodb.api.Models;

namespace sadet.autodb.api.Interfaces;

public interface IDashboardRepository
{
    public TableRow[] GetData();
    public TableRow[] GetDataFast();
}