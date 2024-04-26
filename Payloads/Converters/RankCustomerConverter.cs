using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class RankCustomerConverter
    {
        public RankCustomerConverter() { }  
        public DataResponseRankCustomer EntityToDTO(RankCustomer rankCustomer)
        {
            return new DataResponseRankCustomer()
            {
                RankPoint = rankCustomer.Point,
                Description = rankCustomer.Description,
                RankName = rankCustomer.Name,
                IsASctive = rankCustomer.IsActive
            };
        }
    }
}
