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

                        ///_logger.LogError("Elk Project::::::" + ex.Message?.ToString() + "An exception occurred while executing CreateError method.");
                        _logger.LogError(ex, "Elk Project: {Test} - An exception occurred in CreateError method. Iteration: {Iteration}",
                                        "Elk Project", i);

                    }


                }

                for (int i = 0; i < 100; i++)
                {

                    //// _logger.LogInformation("Test ");
                    _logger.LogInformation("Elk Project: Test message. Iteration: {Iteration}", i);


                }

            }
            catch (Exception ex) {

                _logger.LogError(ex, "Elk Project: {Test} - An unexpected exception occurred in CreateError method",
                                     "Elk Project");
            }
        }
    }
}
