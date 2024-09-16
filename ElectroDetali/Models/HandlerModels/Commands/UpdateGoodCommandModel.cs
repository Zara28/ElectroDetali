﻿using ElectroDetali.Models.HelperModels;
using MediatR;

namespace ElectroDetali.Models.HandlerModels.Commands
{
    public class UpdateGoodCommandModel : IRequest<ResponseModel<string>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }
    }
}
