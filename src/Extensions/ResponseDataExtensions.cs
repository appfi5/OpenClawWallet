using NetCorePal.Extensions.Dto;

namespace OpenClawWalletServer.Extensions;

public static class ResponseDataExtensions
{
    public static ResponseData<TData> AsSuccessResponseData<TData>(
        this TData data,
        string message = "",
        int code = 200,
        IEnumerable<object>? errorData = null)
    {
        return new ResponseData<TData>(data, true, message, code, errorData);
    }

    public static async Task<ResponseData> AsSuccessResponseData(
        this Task task,
        string message = "",
        int code = 200,
        IEnumerable<object>? errorData = null)
    {
        await task;
        return new ResponseData(true, message, code, errorData);
    }

    public static async Task<ResponseData<TData>> AsSuccessResponseData<TData>(
        this Task<TData> data,
        string message = "",
        int code = 200,
        IEnumerable<object>? errorData = null)
    {
        return new ResponseData<TData>(await data, true, message, code, errorData);
    }
}