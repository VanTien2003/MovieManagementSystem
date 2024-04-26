using MovieManagementSystem.Entities;
using MovieManagementSystem.Payloads.DataResponses;

namespace MovieManagementSystem.Payloads.Converters
{
    public class GeneralSettingConverter
    {
        public GeneralSettingConverter() { }

        public DataResponseGeneralSetting EntityToDTO(GeneralSetting generalSetting)
        {
            return new DataResponseGeneralSetting()
            {
                BreakTime = generalSetting.BreakTime,
                BusinessHours = generalSetting.BusinessHours,
                CloseTime = generalSetting.CloseTime,
                FixedTicketPrice = generalSetting.FixedTicketPrice,
                PercentDay = generalSetting.PercentDay,
                PercentWeekend = generalSetting.PercentWeekend,
                TimeBeginToChange = generalSetting.TimeBeginToChange,
            };
        }
    }
}
