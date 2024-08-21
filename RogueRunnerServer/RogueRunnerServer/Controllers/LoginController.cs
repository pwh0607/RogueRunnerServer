using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace RogueRunnerServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController
    {
    }
}
public class LoginRequest
{
    public string Id;
    public string Password;
}