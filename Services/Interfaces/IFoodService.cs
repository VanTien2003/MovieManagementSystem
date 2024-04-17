﻿using MovieManagementSystem.Payloads.DataRequests;
using MovieManagementSystem.Payloads.DataResponses;
using MovieManagementSystem.Payloads.Responses;

namespace MovieManagementSystem.Services.Interfaces
{
    public interface IFoodService
    {
        ResponseObject<DataResponseFood> AddFood(Request_AddFood request);
        ResponseObject<DataResponseFood> EditFood(Request_EditFood request, int id);
        ResponseObject<DataResponseFood> DeleteFood(int id);
    }
}
