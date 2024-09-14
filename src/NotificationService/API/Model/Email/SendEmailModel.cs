namespace API.Model.Email
{
    public record SendEmailModel<T>(string recieverName, string recieverEmail, T data);
}
