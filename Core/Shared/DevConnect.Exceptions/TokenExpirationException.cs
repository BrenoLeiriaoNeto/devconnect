namespace DevConnect.Exceptions;

public class TokenExpirationException(string message) : Exception(message);