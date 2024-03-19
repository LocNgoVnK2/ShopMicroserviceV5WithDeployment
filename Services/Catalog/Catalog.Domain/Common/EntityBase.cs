using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Common
{
    public abstract class EntityBase
    {
        protected EntityBase()
        {
            CreatedBy = "System";
            CreatedDate = DateTime.UtcNow;
            LastModifiedBy = "system";
            LastModifiedDate = DateTime.UtcNow;
        }
        public int _Id { get; protected set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public void SetupForUnitTest(int id)
        {
            _Id = id;
            CreatedBy = "System";
            CreatedDate = DateTime.UtcNow;
            LastModifiedBy = "system";
            LastModifiedDate = DateTime.UtcNow;
        }

    }
}
