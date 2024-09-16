using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Commands
{
    public class CreateBusketRowCommandModel : IRequest<ResponseModel<string>>
    {
        public int? Goodid { get; set; }

        public int? Userid { get; set; }

        public DateTime? Datecreate { get; set; }

        public DateTime? Datedelivery { get; set; }

        public int Pointid { get; set; }
    }
}
