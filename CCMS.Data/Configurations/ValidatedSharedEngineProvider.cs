using NHibernate.Validator.Cfg;
using NHibernate.Validator.Engine;

namespace CCMS.Data.Configurations
{
    internal class ValidatorSharedEngineProvider : ISharedEngineProvider
    {
        private readonly ValidatorEngine _validatorEngine;

        public ValidatorSharedEngineProvider(ValidatorEngine validationEngine)
        {
            _validatorEngine = validationEngine;
        }

        public ValidatorEngine GetEngine()
        {
            return _validatorEngine;
        }

        public void UseMe()
        {
            Environment.SharedEngineProvider = this;
        }
    }
}
