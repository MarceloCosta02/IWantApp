namespace IWantApp.Domain.Request;

public record ClientRequest(string Email, string Password, string Name, string Cpf);