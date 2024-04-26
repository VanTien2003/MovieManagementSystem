using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IGeneralSettingService
    {
        ResponseObject<DataResponseGeneralSetting> AddGeneralSetting(Request_GeneralSetting request);
        ResponseObject<DataResponseGeneralSetting> EditGeneralSetting(Request_GeneralSetting request, int id);
        ResponseObject<DataResponseGeneralSetting> DeleteGeneralSetting(int id);
    }
}
