using MovieManagementSystem.DataContext;
using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.Converters;
using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;
using MovieManagementSystem.Services.Interfaces;

namespace MovieManagementSystem.Services.Implements
{
    public class GeneralSettingService : IGeneralSettingService
    {
        private readonly AppDbContext _context;
        private readonly ResponseObject<DataResponseGeneralSetting> _responseObject;
        private readonly GeneralSettingConverter _converter;

        public GeneralSettingService(AppDbContext context, ResponseObject<DataResponseGeneralSetting> responseObject, GeneralSettingConverter converter)
        {
            _context = context;
            _responseObject = responseObject;
            _converter = converter;
        }

        public ResponseObject<DataResponseGeneralSetting> AddGeneralSetting(Request_GeneralSetting request)
        {
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }

            GeneralSetting generalSetting = new GeneralSetting();
            generalSetting.BreakTime = request.BreakTime;
            generalSetting.BusinessHours = request.BusinessHours;
            generalSetting.FixedTicketPrice = request.FixedTicketPrice;
            generalSetting.PercentDay = request.PercentDay;
            generalSetting.PercentWeekend = request.PercentWeekend;
            generalSetting.CloseTime = request.CloseTime;
            generalSetting.TimeBeginToChange = request.TimeBeginToChange;

            _context.generalSettings.Add(generalSetting);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Add general setting successfully!", _converter.EntityToDTO(generalSetting));
        }

        public ResponseObject<DataResponseGeneralSetting> EditGeneralSetting(Request_GeneralSetting request, int id)
        {           
            if (request == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "Request cannot be blank", null);
            }
            var generalSetting = _context.generalSettings.SingleOrDefault(x => x.Id == id);
            if (generalSetting == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The general setting doestn't exist", null);
            }

            generalSetting.BreakTime = request.BreakTime;
            generalSetting.BusinessHours = request.BusinessHours;
            generalSetting.FixedTicketPrice = request.FixedTicketPrice;
            generalSetting.PercentDay = request.PercentDay;
            generalSetting.PercentWeekend = request.PercentWeekend;
            generalSetting.CloseTime = request.CloseTime;
            generalSetting.TimeBeginToChange = request.TimeBeginToChange;

            _context.generalSettings.Update(generalSetting);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("Edit general setting successfully!", _converter.EntityToDTO(generalSetting));
        }

        public ResponseObject<DataResponseGeneralSetting> DeleteGeneralSetting(int id)
        {
            var existingGeneralSetting = _context.generalSettings.SingleOrDefault(x => x.Id == id);
            if (existingGeneralSetting == null)
            {
                return _responseObject.ResponseError(StatusCodes.Status400BadRequest, "The general setting is not found. Please check again!", null);
            }

            _context.generalSettings.Remove(existingGeneralSetting);
            _context.SaveChanges();
            return _responseObject.ResponseSuccess("The general setting has been deleted successfully!", null);
        }
    }
}
