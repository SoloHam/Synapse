﻿using Synapse.Core.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synapse.Core.Keys
{
    [Serializable]
    internal struct Parameter
    {
        public ConfigurationBase parameterConfig;
        public string parameterValue;

        public Parameter(ConfigurationBase parameterConfig, string parameterValue)
        {
            this.parameterConfig = parameterConfig;
            this.parameterValue = parameterValue;
        }
    }
}
