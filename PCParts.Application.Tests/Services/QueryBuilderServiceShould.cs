﻿using FluentAssertions;
using Npgsql;
using PCParts.Application.Command;
using PCParts.Application.Model.Enum;
using PCParts.Application.Model.QueryModel;
using PCParts.Application.Services.QueryBuilderService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PCParts.Application.Tests.Services;

public class QueryBuilderServiceShould
{
    private readonly QueryBuilderService _sut;

    public QueryBuilderServiceShould()
    {
        _sut = new QueryBuilderService();
    }

    [Fact]
    public async Task ReturnComponentUpdateQuery()
    {
        var componentId = Guid.Parse("86bc4fa7-7c86-4685-9131-8436220a8dba");
        var command =
            new UpdateComponentCommand(componentId, "title", Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a"));
        var parameters = new List<object>();
        parameters.Add(new NpgsqlParameter("@Id", command.Id));
        parameters.Add(new NpgsqlParameter("@Name", command.Name ?? (object)DBNull.Value));
        parameters.Add(new NpgsqlParameter("@CategoryId", command.CategoryId ?? (object)DBNull.Value));
        var sql =
            @"UPDATE ""Component"" 
                SET 
                    ""Name""=COALESCE(@Name, ""Name""),
                    ""CategoryId""=COALESCE(@CategoryId, ""CategoryId"")
                WHERE ""Id"" =@Id;";
        var query = new UpdateQuery { Id = componentId, Parameters = parameters.ToArray(), Query = sql };

        var result = _sut.BuildComponentUpdateQuery(command);
        result.Should().BeEquivalentTo(query);
    }

    [Fact]
    public async Task ReturnSpecificationUpdateQuery()
    {
        var specificationId = Guid.Parse("86bc4fa7-7c86-4685-9131-8436220a8dba");
        var command = new UpdateSpecificationCommand(specificationId, "title", SpecificationDataType.STRING);
        var parameters = new List<object>();
        parameters.Add(new NpgsqlParameter("@Id", command.Id));
        parameters.Add(new NpgsqlParameter("@Name", command.Name ?? (object)DBNull.Value));
        parameters.Add(new NpgsqlParameter("@Type", command.Type.HasValue ? (int)command.Type.Value : DBNull.Value));

        var sql =
            @"UPDATE ""Specification"" 
                SET 
                    ""Name""=COALESCE(@Name, ""Name""),
                    ""DataType""=COALESCE(@Type, ""DataType"")
                WHERE ""Id"" =@Id;";
        var query = new UpdateQuery { Id = specificationId, Parameters = parameters.ToArray(), Query = sql };

        var result = _sut.BuildSpecificationUpdateQuery(command);
        result.Should().BeEquivalentTo(query);
    }

    [Fact]
    public async Task ReturnSpecificationValueUpdateQuery()
    {
        var specificationValueId = Guid.Parse("86bc4fa7-7c86-4685-9131-8436220a8dba");
        var command = new UpdateSpecificationValueCommand(specificationValueId, "title");
        var parameters = new List<object>();
        parameters.Add(new NpgsqlParameter("@Id", command.Id));
        parameters.Add(new NpgsqlParameter("@Value", command.Value ?? (object)DBNull.Value));

        var sql =
            @"UPDATE ""SpecificationValue"" 
                SET 
                    ""Value""=COALESCE(@Value, ""Value"")
                WHERE ""Id"" =@Id;";
        var query = new UpdateQuery { Id = specificationValueId, Parameters = parameters.ToArray(), Query = sql };

        var result = _sut.BuildSpecificationValueUpdateQuery(command);
        result.Should().BeEquivalentTo(query);
    }
}