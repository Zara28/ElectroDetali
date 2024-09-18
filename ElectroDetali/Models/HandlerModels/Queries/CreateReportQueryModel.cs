using ElectroDetali.Models.HelperModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElectroDetali.Models.HandlerModels.Queries
{
    public class CreateReportQueryModel : BaseQueryModel, IRequest<ResponseModel<string>>
    {
    }
}
