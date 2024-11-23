namespace BackendServer;

public static class Server {
    private static string _name;

    public static string Name {
        get => _name;
        set =>
            _name = _name is null
                ? value
                : throw new Exception("Server name already set.");
    }
}