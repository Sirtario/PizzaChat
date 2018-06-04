namespace PIZZA.Chat.Core
{
    public enum ChatConnectReturncode
    {
        ACCEPTED,
        DeniedIncorrectUserPass,
        DeniedIncorrectProtocollVersion,
        DeniedServerInavailable,
        DeniedBadConnection,
        DeniedBadIP,
        DeniedTooManyClients,
        DeniedMISC
    }
}