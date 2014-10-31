using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DddSkeleton.Domain
{
    public class BusinessRule
    {
        public string Description { get; private set; }

        public BusinessRule(string description)
        {
            this.Description = description;
        }
    }
}
