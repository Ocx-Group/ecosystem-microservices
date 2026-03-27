namespace Ecosystem.WalletService.Domain.Responses;

public class ResultResponse<T>
{
    public bool IsSuccess { get; set; }
    public T? Value { get; set; }
    public string? Error { get; set; }

    private ResultResponse(bool isSuccess, T? value, string? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }
    
    public static ResultResponse<T> Ok(T value)
    => new (true, value, null);

    public static ResultResponse<T> Fail(string error)
        => new (false, default, error);
}