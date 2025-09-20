using Elk.Interface;

namespace Elk.Service
{
    public class SchaduleService : ISchaduleService
    {
        private readonly ILogger<SchaduleService> _logger;
        public SchaduleService(ILogger<SchaduleService> logger) { 
        
        
            this._logger = logger;
        }
        public async Task CreateError()
        {
            try
            {
                for (int i = 0; i < 100; i++)
                {
                    try
                    {

                        throw new InvalidOperationException("Elk Project"+ ":::::This is a test exception for logging::::::");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Elk Project::::::" + ex.Message?.ToString() + "An exception occurred while executing CreateError method.");
                    }


                }

                for (int i = 0; i < 100; i++)
                {

                    _logger.LogInformation("Test ");

                }
                
            }
            catch (Exception ex) {

                _logger.LogError(ex.Message.ToString() + "An exception occurred while executing CreateError method.");
            }
        }
    }
}
