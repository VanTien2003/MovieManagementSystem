using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class BannerConverter
    {
        public BannerConverter(){ }
        public DataResponseBanner EntityToDTO(Banner banner)
        {
            return new DataResponseBanner()
            {
                ImageUrl = banner.ImageUrl,
                Title = banner.Title,
            };
        }
    }
}
