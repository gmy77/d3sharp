
// Calculates the ServiceHash for the given string
static uint GetServiceHash(string name)
{
    var bytes = Encoding.ASCII.GetBytes(name);

    uint result = 0x811C9DC5;

    for (var i = 0; i < bytes.Length; ++i)
    {
        result = 0x1000193 * (bytes[i] ^ result);
    }
    return result;
}

// e.g.
var hash = GetServiceHash("bnet.protocol.authentication.AuthenticationClient");
