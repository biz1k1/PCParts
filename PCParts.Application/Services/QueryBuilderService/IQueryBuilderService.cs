using PCParts.Application.Model.Command;
using PCParts.Application.Model.QueryModel;

namespace PCParts.Application.Services.QueryBuilderService;

public interface IQueryBuilderService
{
    UpdateQuery BuildComponentUpdateQuery(UpdateComponentCommand command);
    UpdateQuery BuildSpecificationUpdateQuery(UpdateSpecificationCommand command);
    UpdateQuery BuildSpecificationValueUpdateQuery(UpdateSpecificationValueCommand command);
}