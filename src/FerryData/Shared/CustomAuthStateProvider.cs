using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Serilog;


//описание как сделать здесь
//https://docs.microsoft.com/ru-ru/aspnet/core/blazor/security/?view=aspnetcore-3.1

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private bool _isLoggedIn = false;
    public bool IsLoggedIn 
    {
        get 
        {
            //_logger.Information("get IsLoggedIn");
            return _isLoggedIn; 
        }
        set 
        {
            //_logger.Information("set IsLoggedIn");
            _isLoggedIn = value;
            GetAuthenticationStateAsync();
        } 
    }


    //public static CustomAuthStateProvider MyInstance;
    public Serilog.ILogger _logger { get; set; }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //надо, чтобы вот тут он смотрел, залогинен или нет, и в зав от этого возвращал значение или нулл

        ClaimsIdentity identity;
        Task<AuthenticationState> rez;

        _logger.Information($"GetAuthStateCalled, IsLoggedIn={IsLoggedIn}");
       
        if (IsLoggedIn)
        {
            identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "FerryUser01"),
                }, "Fake authentication type");
        }
        else
        {
            identity = new ClaimsIdentity();
        }
        var user = new ClaimsPrincipal(identity);

        rez = Task.FromResult(new AuthenticationState(user));

        //важная строчка, которая уведомляет компоненты о том, что логин стейт изменился
        NotifyAuthenticationStateChanged(rez);

        return rez;

    }

    public static CustomAuthStateProvider GetMyInstance(Serilog.ILogger logger=null)
    {
        /*
        if (MyInstance == null) MyInstance = new CustomAuthStateProvider();
        return MyInstance;
        */
        CustomAuthStateProvider rez= new CustomAuthStateProvider();

        rez._logger = logger;

        return rez;
    }
}
