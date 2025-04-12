using Npgsql;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.QueryModel;

namespace PCParts.Application.Services.QueryBuilderService;

public class QueryBuilderService : IQueryBuilderService
{
    public UpdateQuery BuildComponentUpdateQuery(UpdateComponentCommand command)
    {
        var parameters = new List<object>();
        parameters.Add(new NpgsqlParameter("@Id", command.Id));
        parameters.Add(new NpgsqlParameter("@Name", command.Name ?? (object)DBNull.Value));
        parameters.Add(new NpgsqlParameter("@CategoryId", command.CategoryId ?? (object)DBNull.Value));

        var query =
            @"UPDATE ""Component"" 
                SET 
                    ""Name""=COALESCE(@Name, ""Name""),
                    ""CategoryId""=COALESCE(@CategoryId, ""CategoryId"")
                WHERE ""Id"" =@Id;";
        var updateQuery = new UpdateQuery
        {
            Id = command.Id,
            Parameters = parameters.ToArray(),
            Query = query
        };

        return updateQuery;
    }

    public UpdateQuery BuildSpecificationUpdateQuery(UpdateSpecificationCommand command)
    {
        var parameters = new List<object>();
        parameters.Add(new NpgsqlParameter("@Id", command.Id));
        parameters.Add(new NpgsqlParameter("@Name", command.Name ?? (object)DBNull.Value));
        parameters.Add(new NpgsqlParameter("@Type", command.Type.HasValue ? (int)command.Type.Value : DBNull.Value));

        var query =
            @"UPDATE ""Specification"" 
                SET 
                    ""Name""=COALESCE(@Name, ""Name""),
                    ""DataType""=COALESCE(@Type, ""DataType"")
                WHERE ""Id"" =@Id;";
        var updateQuery = new UpdateQuery
        {
            Id = command.Id,
            Parameters = parameters.ToArray(),
            Query = query
        };

        return updateQuery;
    }

    public UpdateQuery BuildSpecificationValueUpdateQuery(UpdateSpecificationValueCommand command)
    {
        var parameters = new List<object>();
        parameters.Add(new NpgsqlParameter("@Id", command.Id));
        parameters.Add(new NpgsqlParameter("@Value", command.Value ?? (object)DBNull.Value));

        var query =
            @"UPDATE ""SpecificationValue"" 
                SET 
                    ""Value""=COALESCE(@Value, ""Value"")
                WHERE ""Id"" =@Id;";
        var updateQuery = new UpdateQuery
        {
            Id = command.Id,
            Parameters = parameters.ToArray(),
            Query = query
        };

        return updateQuery;
    }
}