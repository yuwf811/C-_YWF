using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YOrm.Provider
{
    public interface IDbQueryProvider : IQueryProvider
    {
        void SetConnection(string connStr);
    }
}
