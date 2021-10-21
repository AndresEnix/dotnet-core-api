using System;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnet_core_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecretController : ControllerBase
    {
        private readonly ILogger<SecretController> _logger;

        public SecretController(ILogger<SecretController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Secret Get()
        {
            string secretPath = Environment.GetEnvironmentVariable("APP_SECRET_NAME");

            Secret appSecret = new Secret();
            appSecret.Name = secretPath;

            try
            {
                var config = new AmazonSecretsManagerConfig();
                var client = new AmazonSecretsManagerClient(config);
                var request = new GetSecretValueRequest { SecretId = secretPath };

                GetSecretValueResponse response = null;
                response = Task.Run(async () => await client.GetSecretValueAsync(request)).Result;
                appSecret.Value = response?.SecretString;
            }
            catch (ResourceNotFoundException)
            {
                appSecret.Value = "The requested secret " + secretPath + " was not found";
            }
            catch (InvalidRequestException e)
            {
                appSecret.Value = "The request was invalid due to: " + e.Message;
            }
            catch (InvalidParameterException e)
            {
                appSecret.Value = "The request had invalid params: " + e.Message;
            }
            catch (Exception e)
            {
                appSecret.Value = "Fatal error: " + e.Message;
            }

            return appSecret;
        }
    }
}
