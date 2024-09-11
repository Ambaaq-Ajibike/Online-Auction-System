using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Bidding
{
    public class BiddingDto(Guid id, string productName, decimal productPrice, decimal biddingAmount, Guid bidderId);
}
