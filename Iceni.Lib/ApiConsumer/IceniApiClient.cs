using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using RestSharp;

namespace Iceni.Lib.ApiConsumer;

/// <summary>
///     Root client for accessing the api scoped to controller
/// </summary>
public class IceniApiClient
{
    /// <summary>
    /// The unauthorised event handler.
    /// </summary>
    public event EventHandler? OnUnauthorisedEventHandler;

    /// <summary>
    /// The RestSharp client.
    /// </summary>
    public readonly RestClient Client;

    private string? _jwtToken;
    
    /// <summary>
    ///     The current users Claims Identity
    /// </summary>
    public ClaimsIdentity? Identity { get; set; }

    /// <summary>
    ///     The current users JwtToken
    ///     On Set -> updates the Jwt value and Identity from the JwtToken
    /// </summary>
    public string? JwtToken
    {
        get => _jwtToken;
        set
        {
            _jwtToken = value;
            if (value == null)
            {
                Jwt = null;
                Identity = null;
                return;
            }
            Jwt = new JwtSecurityTokenHandler().ReadJwtToken(value);
            Identity = new ClaimsIdentity(Jwt.Claims);
        }
    }

    /// <summary>
    ///     The current users Jwt parsed to a JwtSecurity token for machine readability
    /// </summary>
    public JwtSecurityToken? Jwt { get; set; }

    /// <summary>
    ///     ctr 
    /// </summary>
    public IceniApiClient(RestClient client)
    {
        Client = client;

        Auth = new AuthApiClient(Client, this);
        Pupils = new PupilApiClient(Client, this);
        Lessons = new LessonApiClient(Client, this);
        InternalUsers = new UserApiClient(Client, this);
        Payments = new PaymentApiClient(Client, this);
    }
    
    /// <summary>
    ///     Client for performing auth request
    /// </summary>
    public AuthApiClient Auth { get; set; }
    
    /// <summary>
    ///     Client for CRUDing Pupils
    /// </summary>
    public PupilApiClient Pupils { get; set; }
    
    /// <summary>
    ///     Client for CRUDing Lessons
    /// </summary>
    public LessonApiClient Lessons { get; set; }
    
    /// <summary>
    ///     Client for managing internal users
    /// </summary>
    public UserApiClient InternalUsers { get; set; }
    
    /// <summary>
    ///     Client for managing payments
    /// </summary>
    public PaymentApiClient Payments { get; set; }
    
    /// <summary>
    /// Invoke the unauthorised event handler.
    /// </summary>
    public void InvokeOnUnauthorisedEvent()
    {
        OnUnauthorisedEventHandler?.Invoke(this, EventArgs.Empty);
    }
}