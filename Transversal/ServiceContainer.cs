using System;
using System.Collections.Generic;
using System.Text;
using Formulador.Data.DataAccess;

namespace Formulador.Transversal
{    
    public class ServiceContainer
    {
        protected readonly dynamic LocalDAO;
        protected readonly dynamic RemoteDAO;

        public ServiceContainer()
        {
            LocalDAO = new DataAccessSQLITE();
            RemoteDAO = new DataAccessMSSQL();
        }
    }
}
